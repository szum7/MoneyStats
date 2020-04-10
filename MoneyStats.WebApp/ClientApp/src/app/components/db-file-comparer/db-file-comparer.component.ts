import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileFileResult } from 'src/app/models/component-models/file-file-result';
import { BankRowService } from 'src/app/services/bank-row-service/bank-row.service';
import { DbTransaction } from '../../models/component-models/db-transaction';
import { BankRow } from '../../models/service-models/transaction.model';
import { LoadingScreenService } from '../../services/loading-screen-service/loading-screen.service';
import { RuleTransaction } from 'src/app/models/component-models/rule-transaction';

@Component({
  selector: 'app-db-file-comparer-component',
  templateUrl: './db-file-comparer.component.html',
  styleUrls: ['./db-file-comparer.component.scss']
})
export class DbFileComparerComponent implements OnInit {

    // TODO
    // get transactions from db
    // compare with read transactions
    // red out the duplicates, write reason and id
    // next step
  
    @Input() params: FileFileResult;
    @Output() nextStepChange = new EventEmitter();
    public get fileList(): Array<DbTransaction> { return this.params.transactionList; }

    constructor(
        private loadingScreen: LoadingScreenService,
        private transactionService: BankRowService) {        
    }

    ngOnInit(): void {
        this.program();
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
                tr.AccountingDate = el.AccountingDate;
                tr.Account = el.Account;
                tr.TransactionId = el.TransactionId;
                tr.Type = el.Type;
                tr.AccountName = el.AccountName;
                tr.PartnerAccount = el.PartnerAccount;
                tr.PartnerName = el.PartnerName;
                tr.Sum = el.Sum;
                tr.Currency = el.Currency;
                tr.Message = el.Message;
                tr.OriginalContentId = el.OriginalContentId;
                copy.push(tr);
            }
        }

        this.nextStepChange.emit(copy);
    }

    program() {
        let _this = this;
        _this.loadingScreen.start();
        _this.getTransactionsFromDb(function (dbList) {
            _this.compareDbToFileRows(dbList);
            _this.loadingScreen.stop();
        });
    }

    private getTransactionsFromDb(callback: (response: Array<BankRow>) => void) {
        this.transactionService.get().subscribe(response => {
            console.log(response);
            callback(response);
        }, error => {
            console.error("Couldn't get transactions from database!");
            console.log(error);
        })
    }

    private compareDbToFileRows(dbList: Array<BankRow>) {
        for (let i = 0; i < dbList.length; i++) {
            let dbRow: BankRow = dbList[i];
            for (let j = 0; j < this.fileList.length; j++) {
                let fileRow: DbTransaction = this.fileList[j];

                if (dbRow.getContentId() === fileRow.getContentId()) {
                    fileRow.compareResults.isSameContent = true;
                }
                if (dbRow.OriginalContentId === fileRow.getContentId()) {
                    fileRow.compareResults.isSameOriginalContent = true;
                }
            }
        }
    }
}
