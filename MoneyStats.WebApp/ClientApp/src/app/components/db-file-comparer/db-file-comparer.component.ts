import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileFileResult } from 'src/app/models/component-models/file-file-result';
import { BankRowService } from 'src/app/services/bank-row-service/bank-row.service';
import { ReadBankRowForInsertion } from '../../models/component-models/read-bank-row-for-insertion';
import { BankRow } from '../../models/service-models/bank-row.model';
import { LoadingScreenService } from '../../services/loading-screen-service/loading-screen.service';
import { RuleTransaction } from 'src/app/models/component-models/rule-transaction';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { BankType } from 'src/app/models/service-models/bank-type.enum';
import { PropertyMapRow } from 'src/app/models/component-models/property-map-row';

@Component({
  selector: 'app-db-file-comparer-component',
  templateUrl: './db-file-comparer.component.html',
  styleUrls: ['./db-file-comparer.component.scss']
})
export class DbFileComparerComponent implements OnInit {

    // TODO!!!
    // Régi transactionös program, lehet már más fog kelleni:
    // get bank rows from db
    // compare with read bank rows
    // red out the duplicates, write reason and id
    // next step
  
    @Input() params: FileFileResult;
    @Output() nextStepChange = new EventEmitter();
    public get mapper(): ExcelBankRowMapper { return this.params.mapper; }
    public get fileList(): Array<ReadBankRowForInsertion> { return this.params.bankRowList; }

    constructor(
        private loadingScreen: LoadingScreenService,
        private bankRowService: BankRowService) {     
    }

    ngOnInit(): void {
        this.program();
    }

    click_toggleColumnVisibility(propertyMap: PropertyMapRow): void {
        propertyMap.isOpen = !propertyMap.isOpen;
    }

    click_toggleRowExclusion(row: ReadBankRowForInsertion): void {
        row.isExcluded = !row.isExcluded;
    }

    click_sout() {
        console.log(this.params);
    }

    click_done(): void {
        let copy: Array<RuleTransaction> = [];
        for (let i = 0; i < this.fileList.length; i++) {
            const el = this.fileList[i];
            if (!el.isExcluded) {
                let tr: RuleTransaction = new RuleTransaction();
                tr.bankRow = el.bankRow;
                copy.push(tr);
            }
        }

        this.nextStepChange.emit(copy);
    }

    private program() {
        let _this = this;
        _this.loadingScreen.start();
        _this.getBankRowsFromDb(function (dbList) {
            _this.compareDbToFileRows(dbList);
            _this.loadingScreen.stop();
        });
    }

    private getBankRowsFromDb(callback: (response: Array<BankRow>) => void) {
        this.bankRowService.get().subscribe(response => {
            console.log(response);
            callback(response);
        }, error => {
            console.error("Couldn't get bank rows from database!");
            console.log(error);
        })
    }

    private compareDbToFileRows(dbList: Array<BankRow>) {
        for (let i = 0; i < dbList.length; i++) {
            let dbRow: BankRow = dbList[i];
            for (let j = 0; j < this.fileList.length; j++) {
                let fileRow: ReadBankRowForInsertion = this.fileList[j];

                if (dbRow.getContentId() === fileRow.bankRow.getContentId()) {
                    fileRow.compareResults.isSameContent = true;
                    fileRow.isExcluded = true;
                }
                if (false) { // TODO some validation could be here
                    fileRow.compareResults.isInvalid = true;
                    fileRow.isExcluded = true;
                }
            }
        }
    }
}
