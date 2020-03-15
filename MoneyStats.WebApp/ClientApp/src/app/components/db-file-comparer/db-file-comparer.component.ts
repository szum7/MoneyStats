import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileFileResult } from 'src/app/pages/update-page/src/file-file-result';
import { TransactionService } from 'src/app/services/transaction-service/transaction.service';
import { DbTransaction } from './src/db-transaction';
import { Transaction } from '../../services/transaction-service/models/transaction.model';
import { LoadingScreenService } from '../../services/loading-screen-service/loading-screen.service';

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
        private transactionService: TransactionService) {        
    }

    ngOnInit(): void {
        this.program();
    }

    click_sout() {
        console.log(this.params);
    }

    click_done() {

        // TODO copy to rule-file model

        this.nextStepChange.emit({});
    }

    program() {
        let _this = this;
        _this.loadingScreen.start();
        _this.getTransactionsFromDb(function (dbList) {
            _this.compareDbToFileRows(dbList);
            _this.loadingScreen.stop();
        });
    }

    private getTransactionsFromDb(callback: (response: Array<Transaction>) => void) {
        this.transactionService.get().subscribe(response => {
            console.log(response);
            callback(response);
        }, error => {
            console.error("Couldn't get transactions from database!");
            console.log(error);
        })
    }

    private compareDbToFileRows(dbList: Array<Transaction>) {
        for (let i = 0; i < dbList.length; i++) {
            let dbRow: Transaction = dbList[i];
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
