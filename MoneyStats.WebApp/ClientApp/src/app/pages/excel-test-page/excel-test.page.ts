import { Component } from '@angular/core';
import { NewTransactionMerger } from './src/new-transaction-merger';
import { NewTransaction } from './src/new-transaction';
import { map } from 'rxjs/operators';
import { ExcelReader } from './src/excel-reader';

@Component({
    selector: 'app-excel-test-page',
    templateUrl: './excel-test.page.html',
    styleUrls: ['./excel-test.page.scss']
})
export class ExcelTestPage {
    
    // New transactions page
    // 1. file merge stage (exclude duplicates between xml rows from multiple files)
    // 2. db merge stage (exclude duplicates between db rows and xml rows)
    // 3. eval rule and edit stage (evaluate rules and allow edition to rows)

    // Edit transactions page
    // 1. list transactions from db for edition
    
    // TODO rendetrakni ebben a controllerben. Új osztályokat létrehozni, stb.

    reader: ExcelReader;
    mappedExcelList: Array<Array<NewTransaction>>;
    flattenedExcelList: Array<NewTransaction>;
    
    constructor() {
        this.reader = new ExcelReader();
    }

    dateComparer(a, b){
        let d1 = (new Date(a)).getTime();
        let d2 = (new Date(b)).getTime();
        return d1 > d2 ? 1 : d1 === d2 ? 0 : -1
    }

    sortBy(arr: Array<any>, property: string) {
        return arr.sort((a, b) => this.dateComparer(a[property], b[property]));
    }

    formatDate(date: string): string {
        return (new Date(date)).toISOString().substring(0, 10);
    }

    click_toggleRowExclusion(row: NewTransaction): void {
        row.isExcluded = !row.isExcluded;
    }

    click_evaluateReadFiles() {
        if (this.mappedExcelList == null || this.mappedExcelList.length == 0) {
            console.log("No read files/rows to work with.");
            return;
        }

        let merger: NewTransactionMerger = new NewTransactionMerger();
        merger.setExclusion(this.mappedExcelList);
        this.flattenedExcelList = [].concat.apply([], this.mappedExcelList);
    }

    change_filesSelected(event) {
        this.reader.inputFiles = event.target.files;
        for (let i = 0; i < event.target.files.length; i++) { // TODO find a typescript linq (map doesn't work (?))
            const file = event.target.files[i];
            this.reader.inputFileNames.push(file.name);
        }
    }

    click_upload() {

        if (this.reader.inputFiles == null) {
            console.log("No files uploaded.");
            return;
        }

        this.mappedExcelList = this.reader.getTransactionMatrix();
        console.log(this.mappedExcelList);
    }
}
