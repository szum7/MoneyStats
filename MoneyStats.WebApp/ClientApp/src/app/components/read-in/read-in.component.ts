import { Component, OnInit } from '@angular/core';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { ExcelReader } from 'src/app/models/component-models/excel-reader';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
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

    filesSelected(event: any, callback: (response: ReadInBankRow[][]) => void): void {
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

@Component({
    selector: 'app-read-in-component',
    templateUrl: './read-in.component.html',
    styleUrls: ['./read-in.component.scss']
})
export class ReadInComponent implements OnInit {

    public wizard: Wizard;
    public readInFilesStep: ReadInFilesStep;

    private mapper: ExcelBankRowMapper;

    constructor() { 
        this.initWizard();

        // Init global properties
        this.mapper = new ExcelBankRowMapper(this.getBankType());

        // Init 1. step
        this.readInFilesStep = new ReadInFilesStep(this.wizard, this.mapper);
    }

    ngOnInit() {
        
    }

    private initWizard(): void {
        let steps: WizardStep[] = [];
        steps.push(new WizardStep(
            "Step 1 - Introduction to 'Save bank data from exported files'", 
            [
                "This wizard will take you through the steps of saving bank exported excel data to the MoneyStats database.", 
                "Read about - bank rows.", 
                "Read about - transactions",
                "Read about - Supported banks and export files"
            ]));
        steps.push(new WizardStep(
            "Step 2 - Read in exported files", 
            ["Select which files you want to read in."]));
            steps.push(new WizardStep(
            "Step 3 - Eliminate duplicates and save", 
            [
                "Select the records you wish to save to the database.", 
                "The program helps you by detecting duplicates across multiple read files. It compares every single read records with the ones already existing in the database by their properties. Duplicates are shown as excluded (grayed out) rows. (You can decide to include them if you think they're not duplicates)."
            ]));
        this.wizard = new Wizard(steps);
    }

    private getBankType(): BankType {
        // LATER ask user to provide us with this value or realize it based on the file
        return BankType.KH;
    }

    firstStepNext() {
        this.wizard.next();
        this.readInFilesStep.start();
    }

    thirdStepNext() {
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
        this.readInFilesStep.filesSelected(event, function(response: ReadInBankRow[][]) {
            console.log(response);
        });
    }
}
