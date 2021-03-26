import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { ExcelReader } from 'src/app/models/component-models/excel-reader';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { BankType } from 'src/app/models/service-models/bank-type.enum';
import { WizardNavigator } from 'src/app/components/wizards/wizard-navigator/wizard-navigator.component';
import { WizardStep } from '../../wizard-step.model';

export class ChooseFileOutput {
    matrix: ReadInBankRow[][]; 
    mapper: ExcelBankRowMapper;
    constructor(matrix: ReadInBankRow[][], mapper: ExcelBankRowMapper) {
        this.mapper = mapper;
        this.matrix = matrix;
    }
}

/// <summary>
/// This component let's you browse for multiple files on your local machine, 
/// and will map the files' contents to the given bank format. Currently only 
/// K&H is supported, but later the user could specify the bank (or the program 
/// could take a guess).
/// </summary>
@Component({
    selector: 'app-choose-file',
    templateUrl: './choose-file.component.html',
    styleUrls: ['./choose-file.component.scss']
})
export class ChooseFileComponent extends WizardStep implements OnInit {

    @Input() wizard: WizardNavigator;
    @Output() nextStepChange = new EventEmitter<ChooseFileOutput>();

    public readFiles: any[];
    public isBusy: boolean;

    private reader: ExcelReader;
    private mapper: ExcelBankRowMapper;
    private mappedExcelMatrix: ReadInBankRow[][];

    constructor(private loadingScreen: LoadingScreenService) {
        super();
        this.readFiles = [];
        this.mapper = new ExcelBankRowMapper(this.getBankType());
        this.reader = new ExcelReader(this.mapper);
        this.isBusy = false;
    }

    ngOnInit() {
        this.checkForAlerts();
    }

    private getBankType(): BankType {
        // LATER ask user to provide us with this value or realize it based on the file
        return BankType.KH;
    }

    change_filesSelected(event): void {
        this.readSelectedFiles(event);
    }

    private readSelectedFiles(event): void {

        var self = this;

        self.isBusy = true;

        self.mappedExcelMatrix = null;

        // Get the selected files
        let files = event.target.files;

        // Save filenames
        self.readFiles = files;

        // Read files
        if (files == null || files.length === 0) {
            console.log("No files uploaded.");
            return;
        }

        // This is an async program
        self.mappedExcelMatrix = self.reader.getBankRowMatrix(files);

        // Wait for reader to read files
        //self.loadingScreen.start(); // TODO
        var finishedReadingInterval = setInterval(function () {

            if (self.reader.isReadingFinished()) {
                clearInterval(finishedReadingInterval);

                // Evaluate read files
                if (self.mappedExcelMatrix == null || self.mappedExcelMatrix.length == 0) {
                    console.log("No read files/rows to work with.");
                }

                //self.loadingScreen.stop();
                console.log(self.mappedExcelMatrix);

                self.checkForAlerts();
                self.isBusy = false;
            }
        }, 50);
    }

    protected checkForAlerts(): void {
        this.wizard.clearAlerts();

        if (this.readFiles.length === 0) {
            this.wizard.addCriteria("No files were selected.");
        }
    }

    private emitOutput(): void {
        this.nextStepChange.emit(new ChooseFileOutput(this.mappedExcelMatrix, this.mapper));
    }

    next(): void {
        if (!this.wizard.isProgressable()) {
            return;
        }
        if (this.isBusy) {
            return;
        }
        
        this.emitOutput();
        this.wizard.next();
    }
    
    previous(): void {
        throw new Error('Method not implemented.');
    }
}
