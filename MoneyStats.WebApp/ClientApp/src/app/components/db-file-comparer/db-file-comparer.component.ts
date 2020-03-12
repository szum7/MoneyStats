import { Component, OnInit, Input } from '@angular/core';
import { FileFileResult } from 'src/app/pages/update-page/update.page';

@Component({
  selector: 'app-db-file-comparer-component',
  templateUrl: './db-file-comparer.component.html',
  styleUrls: ['./db-file-comparer.component.scss']
})
export class DbFileComparerComponent implements OnInit {
  
    @Input() params: FileFileResult;

    constructor() {
    }

    ngOnInit(): void {
    }

    click_test() {
        this.params.transactionList[4].isExcluded = true;
        this.params.transactionList[5].isExcluded = true;
        this.params.transactionList[6].isExcluded = true;
    }

    click_sout() {
        console.log(this.params);
    }
}