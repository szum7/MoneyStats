import { Component, OnInit } from '@angular/core';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { ExcelReader } from 'src/app/models/component-models/excel-reader';
import { ReadBankRowForDbCompare } from 'src/app/models/component-models/read-bank-row-for-db-compare';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { ReadInBankRowsMerger } from 'src/app/models/component-models/read-in-bank-rows-merger';
import { BankRow } from 'src/app/models/service-models/bank-row.model';
import { BankType } from 'src/app/models/service-models/bank-type.enum';
import { BankRowService } from 'src/app/services/bank-row.service';
import { StaticMessages } from 'src/app/utilities/input-messages.static';
import { Wizard, WizardNavStep } from '../wizard-navigator/wizard-navigator.component';

class ReadInFilesStep {

    private _readFiles: any[];
    private _reader: ExcelReader;
    private _wizard: Wizard;

    public get readFiles(): any[] {
        return this._readFiles;
    }

    constructor(wizard: Wizard, mapper: ExcelBankRowMapper) {
        this._readFiles = [];
        this._reader = new ExcelReader(mapper);
        this._wizard = wizard;
    }

    start(): void {
        this._readFiles = [];
        this.setInitAlerts();
    }

    private setInitAlerts(): void {
        this._wizard.clearAlerts();
        this._wizard.addCriteria("No files are selected.");
    }

    readSelectedFiles(event: any, callback: (response: ReadInBankRow[][]) => void): void {
        // Get the selected files
        let files = event.target.files;
    
        // Save filenames
        this._readFiles = files;
        
        if (files == null || files.length === 0) {
            console.error("No files were selected.");
            return;
        }

        // Read files
        let mappedExcelMatrix: ReadInBankRow[][] = this._reader.getBankRowMatrix(files);
    
        // Wait for reader to read files
        var self = this;
        //this.loadingScreen.start(); // TODO
        var finishedReadingInterval = setInterval(function () {    
            if (self._reader.isReadingFinished()) {
                clearInterval(finishedReadingInterval);

                // Evaluate read files
                if (mappedExcelMatrix == null || mappedExcelMatrix.length == 0) {
                    console.log("No read files/rows to work with.");
                }

                //self.loadingScreen.stop();
                console.log(mappedExcelMatrix);

                //self.emitOutput(mappedExcelMatrix, self.mapper);
                self.checkForErrors();

                if (self._wizard.isProgressable()) {
                    callback(mappedExcelMatrix);
                }
            }
        }, 50);
    }

    private checkForErrors(): void {
        this._wizard.clearAlerts();
        if (this._readFiles.length === 0) {
            this._wizard.addCriteria("No files were selected.");
        }
    }
}

class SaveBankRowsStep {

    private _readInBankRows: ReadInBankRow[];
    private _wizard: Wizard;

    public get readInBankRows(): ReadInBankRow[] {
        return this._readInBankRows;
    }

    constructor(wizard: Wizard) {
        this._wizard = wizard;
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
        this._readInBankRows = this.flattenReadInBankRows(mappedExcelMatrix);
    
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
        this._wizard.clearAlerts();
        if (this._readInBankRows.length === 0) {
            this._wizard.addCriteria("Bankrow count is zero!");
        }
        if (!this._readInBankRows.some(x => !x.isExcluded)) {
            this._wizard.addCriteria("All bankrows are excluded!");
        }
    }
}

class CompareWithDbStep {

    private _wizard: Wizard;
    private _bankRowService: BankRowService;
    private _bankRows: ReadBankRowForDbCompare[];

    constructor(wizard: Wizard, bankRowService: BankRowService) {
        this._wizard = wizard;
        this._bankRowService = bankRowService;
    }

    start(callback: (response: ReadBankRowForDbCompare[]) => void): void {
        let self = this;
        //self.loadingScreen.start();
        self.getBankRowsFromDb(function (dbList) {

            self.compareDbToFileRows(dbList);
            //self.loadingScreen.stop();
            console.log("Compare finished");

            callback(self._bankRows);
            //self.nextStepChange.emit(self.bankRows);
            self.checkForErrors();
        });
    }

    private getBankRowsFromDb(callback: (response: Array<BankRow>) => void): void {
        this._bankRowService.get().subscribe(response => {
            console.log("=> getBankRowsFromDb:");
            console.log(response);
            console.log("<=");
            callback(response);
        }, error => {
            console.error("Couldn't get bank rows from database!");
            console.log(error);
        });
    }

    // TODO do this server side!
    private compareDbToFileRows(dbList: Array<BankRow>): void {
        for (let i = 0; i < dbList.length; i++) {
            let dbRow: BankRow = dbList[i];
            for (let j = 0; j < this._bankRows.length; j++) {
                let fileRow: ReadBankRowForDbCompare = this._bankRows[j];

                if (dbRow.getContentId() === fileRow.bankRow.getContentId()) {
                    fileRow.messages.push(StaticMessages.MATCHING_READ_BANKROW_WITH_DB);
                    fileRow.setToExclude();
                }

            }
        }
    }

    private checkForErrors(): void {
        this._wizard.clearAlerts();

        // Error messages
        if (this._bankRows.length === 0) {
            this._wizard.addCriteria("Bankrow count is zero!");
        }
        if (!this._bankRows.some(x => !x.isExcluded)) {
            this._wizard.addCriteria("All bankrows are excluded!");
        }

        // Success messages
        if (this._wizard.isProgressable()) {
            this._wizard.addMessage("By clicking next step, the new BankRows will be saved to database!");
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
    public compareWithDbStep: CompareWithDbStep;

    public mapper: ExcelBankRowMapper;

    constructor(private bankRowService: BankRowService) { 
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
        let steps: WizardNavStep[] = [];

        steps.push(new WizardNavStep(
            "Step 1 - Read in exported files", 
            ["Select which files you want to read in."]));
        steps.push(new WizardNavStep(
            "Step 2 - Eliminate duplicates between read files", 
            ["Select the records you wish to save to the database. The program helps you by detecting duplicates across multiple read files."]));
        steps.push(new WizardNavStep(
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
        if (!this.wizard.isProgressable())
            return;

        this.compareWithDbStep = new CompareWithDbStep(this.wizard, this.bankRowService);
        this.compareWithDbStep.start(function(response: ReadBankRowForDbCompare[]) {
            
        });
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
        this.readInFilesStep.readSelectedFiles(event, function(response: ReadInBankRow[][]) {
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
