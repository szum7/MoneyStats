import { Component, OnInit } from '@angular/core';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { ExcelReader } from 'src/app/models/component-models/excel-reader';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { ReadInBankRowsMerger } from 'src/app/models/component-models/read-in-bank-rows-merger';
import { BankType } from 'src/app/models/service-models/bank-type.enum';
import { Wizard, WizardStep } from '../wizard-navigator/wizard-navigator.component';

class ReadInFilesStep {

    public readFiles: any[];
    private reader: ExcelReader;
    private wizard: Wizard;

    constructor(wizard: Wizard, mapper: ExcelBankRowMapper) {
        this.readFiles = [];
        this.reader = new ExcelReader(mapper);
        this.wizard = wizard;
    }

    start(): void {
        this.readFiles = [];
        this.setInitAlerts();
    }

    private setInitAlerts(): void {
        this.wizard.clearAlerts();
        this.wizard.addCriteria("No files are selected.");
    }

    onChangeFilesSelected(event: any, callback: (response: ReadInBankRow[][]) => void): void {
        // Get the selected files
        let files = event.target.files;
    
        // Save filenames
        this.readFiles = files;
        
        if (files == null || files.length === 0) {
            console.error("No files were selected.");
            return;
        }

        // Read files
        let mappedExcelMatrix: ReadInBankRow[][] = this.reader.getBankRowMatrix(files);
    
        // Wait for reader to read files
        var self = this;
        //this.loadingScreen.start(); // TODO
        var finishedReadingInterval = setInterval(function () {    
            if (self.reader.isReadingFinished()) {
                clearInterval(finishedReadingInterval);

                // Evaluate read files
                if (mappedExcelMatrix == null || mappedExcelMatrix.length == 0) {
                    console.log("No read files/rows to work with.");
                }

                //self.loadingScreen.stop();
                console.log(mappedExcelMatrix);

                //self.emitOutput(mappedExcelMatrix, self.mapper);
                self.checkForErrors();

                if (self.wizard.isProgressable()) {
                    callback(mappedExcelMatrix);
                }
            }
        }, 50);
    }

    private checkForErrors(): void {
        this.wizard.clearAlerts();
        if (this.readFiles.length === 0) {
            this.wizard.addCriteria("No files were selected.");
        }
    }
}

class SaveBankRowsStep {

    public readInBankRows: ReadInBankRow[];
    private wizard: Wizard;

    constructor(wizard: Wizard) {
        this.wizard = wizard;
    }
    
    start(mappedExcelMatrix: ReadInBankRow[][]): void {

        if (!mappedExcelMatrix || mappedExcelMatrix.length === 0) {
            console.error("Input matrix is empty!");
            return;
        }
    
        // TODO research and understand why this (loadingScreen part) isn't working!
        // Error: "Expression has changed after it was checked"
        // https://blog.angular-university.io/angular-debugging/
        //this.loadingScreen.start();
    
        new ReadInBankRowsMerger().searchForDuplicates(mappedExcelMatrix);
        this.readInBankRows = this.flattenReadInBankRows(mappedExcelMatrix);
    
        //this.loadingScreen.stop();
    
        this.checkForErrors();
        //this.nextStepChange.emit(this.readInBankRows);
    }

    private flattenReadInBankRows(matrix: ReadInBankRow[][]): ReadInBankRow[] {
        let ret: ReadInBankRow[] = [];
        let id: number = 0;
        for (let i = 0; i < matrix.length; i++) {
            let m = matrix[i];
            for (let j = 0; j < m.length; j++) {
                m[j].uiId = ++id;
                ret.push(m[j]);
            }
        }
        return ret;
    }

    checkForErrors(): void {
        this.wizard.clearAlerts();
        if (this.readInBankRows.length === 0) {
            this.wizard.addCriteria("Bankrow count is zero!");
        }
        if (!this.readInBankRows.some(x => !x.isExcluded)) {
            this.wizard.addCriteria("All bankrows are excluded!");
        }
    }
}

@Component({
    selector: 'app-read-in-component',
    templateUrl: './read-in.component.html',
    styleUrls: ['./read-in.component.scss']
})
export class ReadInComponent implements OnInit {

    public wizard: Wizard;
    public readInFilesStep: ReadInFilesStep;
    public saveBankRowsStep: SaveBankRowsStep;

    public mapper: ExcelBankRowMapper;

    constructor() { 
        this.initWizard();

        // Init global properties
        this.mapper = new ExcelBankRowMapper(this.getBankType());

        // Init 1. step
        this.readInFilesStep = new ReadInFilesStep(this.wizard, this.mapper);
        this.readInFilesStep.start();
    }

    ngOnInit() {
        
    }

    private initWizard(): void {
        let steps: WizardStep[] = [];
        steps.push(new WizardStep(
            "Step 1 - Read in exported files", 
            ["Select which files you want to read in."]));
        steps.push(new WizardStep(
            "Step 2 - Eliminate duplicates between read files", 
            [
                "Select the records you wish to save to the database. The program helps you by detecting duplicates across multiple read files."
            ]));
        steps.push(new WizardStep(
            "Step 3 - Compare with database and save", 
            [
                "Select the records you wish to save to the database.",
                "The program compared every single records selected from the previous step with the ones already existing in the database. Comparison is done by properties. Duplicates are shown as excluded (grayed out) rows. (You can decide to include them if you know what you're doing and think they're not duplicates)."
            ]));
        this.wizard = new Wizard(steps);
    }

    private getBankType(): BankType {
        // LATER ask user to provide us with this value or realize it based on the file
        return BankType.KH;
    }

    step2Next() {
        
    }

    step3Next() {
        // confirm and save
    }

    next() {
        this.wizard.next();
    }

    prev() {
        this.wizard.previous();
    }

    change_filesSelected(event): void {
        let self = this;
        this.readInFilesStep.onChangeFilesSelected(event, function(response: ReadInBankRow[][]) {
            if (self.wizard.next()) {
                self.saveBankRowsStep = new SaveBankRowsStep(self.wizard);
                self.saveBankRowsStep.start(response);
            }
        });
    }

    sortBy_bankRows(arr: Array<any>, property: string) {
        return arr.sort((a, b) => this.dateComparer(a.bankRow[property], b.bankRow[property]));
    }

    private dateComparer(a, b) {
        let d1 = (new Date(a)).getTime(), d2 = (new Date(b)).getTime();
        return d1 > d2 ? 1 : d1 === d2 ? 0 : -1;
    }
    
    click_toggleRowExclusion(row: ReadInBankRow): void {
        row.toggleExclusion();
        this.saveBankRowsStep.checkForErrors();
    }

    click_toggleDetails(row: ReadInBankRow): void {
        row.isDetailsOpen = !row.isDetailsOpen;
    }

    click_switchDetailsMenu(row: ReadInBankRow, i: number): void {
        row.detailsMenuPageAt = i;
    }

    getPrettyJson(obj: any): string {
        return JSON.stringify(obj, null, 4);  
    }
}
