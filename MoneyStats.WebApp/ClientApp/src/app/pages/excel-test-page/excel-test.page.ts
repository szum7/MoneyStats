import { Component } from '@angular/core';
import * as XLSX from 'xlsx';
import { ExcelTransactionMapper } from './src/excel-transaction-mapper';
import { NewTransactionMerger } from './src/new-transaction-merger';
import { NewTransaction } from './src/new-transaction';

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
    
    readFiles: Array<any>;
    mappedExcelList: Array<Array<NewTransaction>>;
    flattenedExcelList: Array<NewTransaction>;
    arrayBuffer: any;

    constructor() {
    }

    toggleRowExclusion(row: NewTransaction): void {
        row.isExcluded = !row.isExcluded;
    }

    evaluateReadFiles() {
        if (this.mappedExcelList == null || this.mappedExcelList.length == 0) {
            console.log("No read files/rows to work with.");
            return;
        }

        let merger: NewTransactionMerger = new NewTransactionMerger();
        merger.defineRows(this.mappedExcelList);
        this.flattenedExcelList = [].concat.apply([], this.mappedExcelList);
    }

    filesSelected(event) {
        this.readFiles = event.target.files;
    }

    upload() {
        let self = this;

        if (self.readFiles == null) {
            console.log("No files uploaded.");
            return;
        }

        self.mappedExcelList = [];
        let mapper: ExcelTransactionMapper = new ExcelTransactionMapper();

        for (let i = 0; i < self.readFiles.length; i++) {
            self.readFile(self.readFiles[i], function(unmappedArray) {
                self.mappedExcelList.push(mapper.mapTransactions(unmappedArray));
            });
        }

        console.log(self.mappedExcelList);
    }

    readFile(file: File, callback: Function) {

        let fileReader = new FileReader();
        let arrayBuffer: any;

        fileReader.onload = (e) => {
            arrayBuffer = fileReader.result;
            var data = new Uint8Array(arrayBuffer);
            var arr = new Array();
            for (var i = 0; i != data.length; ++i) {
                arr[i] = String.fromCharCode(data[i]);
            }
            var bstr = arr.join("");
            var workbook = XLSX.read(bstr, { type: "binary" });
            var first_sheet_name = workbook.SheetNames[0];
            var worksheet = workbook.Sheets[first_sheet_name];
            callback(XLSX.utils.sheet_to_json(worksheet, { raw: true }));
        }
        fileReader.readAsArrayBuffer(file);
    }
}
