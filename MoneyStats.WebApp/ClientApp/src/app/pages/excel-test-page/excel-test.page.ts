import { Component } from '@angular/core';
import * as XLSX from 'xlsx';
import { Transaction } from 'src/app/services/transaction-service/models/transaction.model';

class ExcelTransactionMapper {

    constructor() {
    }

    private getJsDateFromExcel(excelDate) {
        // JavaScript dates can be constructed by passing milliseconds
        // since the Unix epoch (January 1, 1970) example: new Date(12312512312);
        
        // 1. Subtract number of days between Jan 1, 1900 and Jan 1, 1970, plus 1  (Google "excel leap year bug")
        // Neet to add +1 for some reason - Sz. Aron
        // 2. Convert to milliseconds.
        return new Date((excelDate - (25567 + 1 + 1)) * 86400 * 1000); // TODO hour is at 01:00:00. Should be at 00:00:00
    }

    private mapTransaction(tr: any): Transaction {
        let m: Transaction = new Transaction();

        m.AccountingDate = (this.getJsDateFromExcel(tr["Könyvelés dátuma"])).toString();
        m.TransactionId = tr['Tranzakció azonosító']; // TODO weird column name, reading is faulty(?)
        m.Type = tr["Típus"];
        m.Account = tr["Könyvelési számla"];
        m.AccountName = tr["Könyvelési számla elnevezése"];
        m.PartnerAccount = tr["Partner számla"];
        m.PartnerName = tr["Partner elnevezése"];
        m.Sum = tr["Összeg"];
        m.Currency = tr["Összeg devizaneme"];
        m.Message = tr["Közlemény"];

        m.OriginalContentId = m.getContentId();

        return m;
    }

    public mapTransactions(list: Array<any>): Array<Transaction> {
        let ms: Array<Transaction> = [];
        for (let i = 0; i < list.length; i++) {
            ms.push(this.mapTransaction(list[i]));
        }
        return ms;
    }
}

@Component({
    selector: 'app-excel-test-page',
    templateUrl: './excel-test.page.html',
    styleUrls: ['./excel-test.page.scss']
})
export class ExcelTestPage {    
    
    readFiles: Array<any>;
    mappedExcelList: Array<any>;
    arrayBuffer: any;

    constructor() {
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
