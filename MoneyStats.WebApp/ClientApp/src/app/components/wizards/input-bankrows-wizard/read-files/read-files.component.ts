import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { ReadInBankRowsMerger } from 'src/app/models/component-models/read-in-bank-rows-merger';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { WizardNavigator } from 'src/app/components/wizards/wizard-navigator/wizard-navigator.component';
import { WizardStep } from '../../wizard-step.model';

export class ManageReadFilesInput {

    wizard: WizardNavigator;
    readInBankRow: ReadInBankRow[][];
    mapper: ExcelBankRowMapper;

    constructor(wizard: WizardNavigator, readInBankRow: ReadInBankRow[][], mapper: ExcelBankRowMapper) {
        this.wizard = wizard;
        this.readInBankRow = readInBankRow;
        this.mapper = mapper;
    }
}

@Component({
    selector: 'app-read-files-component',
    templateUrl: './read-files.component.html',
    styleUrls: ['./read-files.component.scss']
})
export class ReadFilesComponent extends WizardStep implements OnInit {

    @Input() $input: ManageReadFilesInput;
    public get mapper(): ExcelBankRowMapper { return this.$input.mapper; }
    public get mappedExcelMatrix(): ReadInBankRow[][] { return this.$input.readInBankRow; }
    public get wizard(): WizardNavigator { return this.$input.wizard; }

    @Output() nextStepChange = new EventEmitter<ReadInBankRow[]>();

    readInBankRows: ReadInBankRow[];

    constructor(private loadingScreen: LoadingScreenService) {
        super();
        this.readInBankRows = [];
    }

    ngOnInit() {
        this.program(this.mappedExcelMatrix);
    }

    /// <summary>
    /// Searches through the files matrix and 
    /// alerts duplicate rows to the user, set 
    /// them for omition. Plus flattens the 
    /// matrix into an array.
    /// </summary>
    program(mappedExcelMatrix: ReadInBankRow[][]) {

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

        this.checkForAlerts();
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

    sortBy_bankRows(arr: Array<any>, property: string) {
        return arr.sort((a, b) => this.dateComparer(a.bankRow[property], b.bankRow[property]));
    }

    private dateComparer(a, b) {
        let d1 = (new Date(a)).getTime(), d2 = (new Date(b)).getTime();
        return d1 > d2 ? 1 : d1 === d2 ? 0 : -1;
    }

    click_toggleRowExclusion(row: ReadInBankRow): void {
        row.toggleExclusion();
        this.checkForAlerts();
    }

    click_toggleDetails(row: ReadInBankRow): void {
        row.isDetailsOpen = !row.isDetailsOpen;
    }

    click_switchDetailsMenu(row: ReadInBankRow, i: number): void {
        row.detailsMenuPageAt = i;
    }

    protected checkForAlerts(): void {
        this.wizard.clearAlerts();
        
        if (this.readInBankRows.length === 0) {
            this.wizard.addCriteria("Bankrow count is zero!");
        }
        if (!this.readInBankRows.some(x => !x.isExcluded)) {
            this.wizard.addCriteria("All bankrows are excluded!");
        }
    }
    
    next(): void {
        if (!this.wizard.isProgressable()) {
            return;
        }
        this.nextStepChange.emit(this.readInBankRows);
        this.wizard.next();
    }

    previous(): void {
        this.wizard.previous();
    }

    // TODO https://stackblitz.com/edit/flex-table-column-resize
}
