import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileFileResult } from 'src/app/pages/update-page/src/file-file-result';
import { TransactionService } from 'src/app/services/transaction-service/transaction.service';

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

    constructor(
        private transactionService: TransactionService) {        
    }

    ngOnInit(): void {
        this.transactionService.get().subscribe(response => {
            console.log(response);
        }, error => {
            console.log(error);
        })
    }

    click_sout() {
        console.log(this.params);
    }

    click_done() {
        // TODO
        this.nextStepChange.emit({});
    }
}