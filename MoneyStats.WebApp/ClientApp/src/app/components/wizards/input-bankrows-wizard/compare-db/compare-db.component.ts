import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ReadBankRowForDbCompare } from 'src/app/models/component-models/read-bank-row-for-db-compare';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { BankRowService } from 'src/app/services/bank-row.service';
import { BankRow } from 'src/app/models/service-models/bank-row.model';
import { StaticMessages } from 'src/app/utilities/input-messages.static';
import { WizardNavigator } from 'src/app/components/wizards/wizard-navigator/wizard-navigator.component';
import { Common } from 'src/app/utilities/common.static';
import { WizardStep } from '../../wizard-step.model';

export class CompareDbInput {

    wizard: WizardNavigator;
    readBankRowForDbCompare: ReadBankRowForDbCompare[];
    mapper: ExcelBankRowMapper;

    constructor(wizard: WizardNavigator, readBankRowForDbCompare: ReadBankRowForDbCompare[], mapper: ExcelBankRowMapper) {
        this.wizard = wizard;
        this.readBankRowForDbCompare = readBankRowForDbCompare;
        this.mapper = mapper;
    }
}

@Component({
    selector: 'app-compare-db-component',
    templateUrl: './compare-db.component.html',
    styleUrls: ['./compare-db.component.scss']
})
export class CompareDbComponent extends WizardStep implements OnInit {

    @Input() $input: CompareDbInput;
    public get bankRows(): ReadBankRowForDbCompare[] { return this.$input.readBankRowForDbCompare; }
    public get mapper(): ExcelBankRowMapper { return this.$input.mapper; }
    public get wizard(): WizardNavigator { return this.$input.wizard; }

    @Output() nextStepChange = new EventEmitter<string>();

    isError: boolean;

    constructor(
        private loadingScreen: LoadingScreenService,
        private bankRowService: BankRowService) {
        super();
        this.isError = false;
    }

    ngOnInit() {
        this.program();
    }

    private program(): void {
        let self = this;
        //self.loadingScreen.start(); // TODO ExpressionChangedAfterItHasBeenCheckedError error
        self.getBankRowsFromDb(function (dbList) {
            self.compareDbToFileRows(dbList, self.bankRows);
            //self.loadingScreen.stop();
            console.log("Compare finished.");

            self.checkForAlerts();
        });
    }

    private getBankRowsFromDb(callback: (response: Array<BankRow>) => void): void {
        let self = this;
        self.bankRowService.get().subscribe(response => {
            Common.ConsoleResponse("getBankRowsFromDb", response);
            callback(response);
        }, error => {
            console.error("Couldn't get bank rows from database!");
            console.log(error);

            self.isError = true;
            self.wizard.addError("The program was unable to get the bankrows from the server!");
        });
    }

    private compareDbToFileRows(dbList: Array<BankRow>, localList: ReadBankRowForDbCompare[]): void {
        for (let i = 0; i < dbList.length; i++) {
            let dbRow: BankRow = dbList[i];
            for (let j = 0; j < localList.length; j++) {
                let fileRow: ReadBankRowForDbCompare = localList[j];

                if (dbRow.getContentId() === fileRow.bankRow.getContentId()) {
                    fileRow.messages.push(StaticMessages.MATCHING_READ_BANKROW_WITH_DB);
                    fileRow.setToExclude();
                }
            }
        }
    }

    sortBy_bankRows(arr: any[], property: string) {
        return arr.sort((a, b) => this.dateComparer(a.bankRow[property], b.bankRow[property]));
    }

    private dateComparer(a, b) {
        let d1 = (new Date(a)).getTime(), d2 = (new Date(b)).getTime();
        return d1 > d2 ? 1 : d1 === d2 ? 0 : -1;
    }

    click_toggleDetails(row: ReadBankRowForDbCompare): void {
        row.isDetailsOpen = !row.isDetailsOpen;
    }

    click_toggleRowExclusion(row: ReadBankRowForDbCompare): void {
        row.toggleExclusion();
        this.checkForAlerts();
    }

    click_switchDetailsMenu(row: ReadBankRowForDbCompare, i: number): void {
        row.detailsMenuPageAt = i;
    }

    protected checkForAlerts(): void {
        this.wizard.clearAlerts();

        if (this.bankRows.length === 0) {
            this.wizard.addCriteria("Bankrow count is zero!");
        }
        if (!this.bankRows.some(x => !x.isExcluded)) {
            this.wizard.addCriteria("All bankrows are excluded!");
        }
    }
    
    next(): void {
        if (!this.wizard.isProgressable()) {
            return;
        }

        let self = this;
        self.saveBankRows(self.getIncludedBankRows(self.bankRows), function (response) {
            // TODO Add Ids or exclusion to the table data. Otherwise, the next step's previous() would show already saved records.
            self.nextStepChange.emit("Success");
            self.wizard.next();
        });
    }

    previous(): void {
        this.wizard.previous();
    }

    private getIncludedBankRows(toCast: ReadBankRowForDbCompare[]): BankRow[] {
        let ret: BankRow[] = [];
        for (let i = 0; i < toCast.length; i++) {
            if (!toCast[i].isExcluded) {
                ret.push(toCast[i].bankRow);
            }
        }
        return ret;
    }

    private saveBankRows(bankRows: BankRow[], callback: (bankRows: BankRow[]) => void): void {
        this.bankRowService.save(bankRows).subscribe(response => {
            Common.ConsoleResponse("saveBankRows", response);
            callback(response);
        }, error => {
            console.error(error);
        })
    }
}
