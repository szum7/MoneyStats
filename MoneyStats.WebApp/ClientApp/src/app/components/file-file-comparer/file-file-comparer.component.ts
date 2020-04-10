import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { NewTransactionMerger } from '../../models/component-models/new-transaction-merger';
import { NewTransaction } from '../../models/component-models/new-transaction';
import { map } from 'rxjs/operators';
import { ExcelReader } from '../../models/component-models/excel-reader';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { ExcelTransactionMapper } from '../../models/component-models/excel-transaction-mapper';
import { PropertyMapRow } from '../../models/component-models/property-map-row';
import { BankRow } from 'src/app/models/service-models/transaction.model';
import { DbTransaction } from '../../models/component-models/db-transaction';

@Component({
  selector: 'app-file-file-comparer-component',
  templateUrl: './file-file-comparer.component.html',
  styleUrls: ['./file-file-comparer.component.scss']
})
export class FileFileComparerComponent implements OnInit {

    // New transactions page
    // 1. file merge stage (exclude duplicates between xml rows from multiple files)
    // 2. db merge stage (exclude duplicates between db rows and xml rows)
    // 3. eval rule and edit stage (evaluate rules and allow edition to rows)

    // Edit transactions page
    // 1. list transactions from db for edition    

    reader: ExcelReader;
    mapper: ExcelTransactionMapper;
    transactionList: Array<NewTransaction>;
    @Output() nextStepChange = new EventEmitter();
    
    constructor(private loadingScreen: LoadingScreenService) {
        this.mapper = new ExcelTransactionMapper();
        this.reader = new ExcelReader(this.mapper);
        this.transactionList = [];
    }

    ngOnInit(): void { }

    toggleColumnVisibility(propertyMap: PropertyMapRow): void {
        propertyMap.isOpen = !propertyMap.isOpen;
    }

    dateComparer(a, b){
        let d1 = (new Date(a)).getTime();
        let d2 = (new Date(b)).getTime();
        return d1 > d2 ? 1 : d1 === d2 ? 0 : -1
    }

    sortBy(arr: Array<any>, property: string) {
        return arr.sort((a, b) => this.dateComparer(a[property], b[property]));
    }

    click_toggleRowExclusion(row: NewTransaction): void {
        row.isExcluded = !row.isExcluded;
    }

    change_filesSelected(event) {

        let files = event.target.files;

        // Read files
        if (files == null || files.length === 0) {
            console.log("No files uploaded.");
            return;
        }
        let mappedExcelMatrix: Array<Array<NewTransaction>> = this.reader.getTransactionMatrix(files);
        console.log(mappedExcelMatrix);

        this.loadingScreen.start();
        
        // Wait for reader to read files
        var _this = this;
        var finishedReadingInterval = setInterval(function() {
            
            if (_this.reader.isReadingFinished()) {
                clearInterval(finishedReadingInterval);
                
                // Evaluate read files
                if (mappedExcelMatrix == null || mappedExcelMatrix.length == 0) {
                    console.log("No read files/rows to work with.");
                    return;
                }
                
                let merger: NewTransactionMerger = new NewTransactionMerger();
                merger.setExclusion(mappedExcelMatrix);
                _this.transactionList = [].concat.apply([], mappedExcelMatrix);

                _this.loadingScreen.stop();
            }
        }, 10);        
    }

    click_next(): void {
        // Output transaction list
        let copy: Array<DbTransaction> = [];
        for (let i = 0; i < this.transactionList.length; i++) {
            const el = this.transactionList[i];
            if (!el.isExcluded) {
                let tr: DbTransaction = new DbTransaction();
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
}
