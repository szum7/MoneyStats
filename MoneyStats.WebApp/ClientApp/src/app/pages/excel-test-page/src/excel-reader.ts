import { NewTransaction } from "./new-transaction";
import { ExcelTransactionMapper } from "./excel-transaction-mapper";
import * as XLSX from 'xlsx';

export class ExcelReader {

    public inputFiles: Array<any>;
    public inputFileNames: Array<string>;

    constructor() {
        this.inputFileNames = [];
        this.inputFiles = [];
    }

    public getTransactionMatrix(): Array<Array<NewTransaction>> {
        
        let self = this;
        let mappedExcelMatrix: Array<Array<NewTransaction>> = [];
        let mapper: ExcelTransactionMapper = new ExcelTransactionMapper();

        for (let i = 0; i < self.inputFiles.length; i++) {
            self.readFile(self.inputFiles[i], function(unmappedArray) {
                mappedExcelMatrix.push(mapper.mapTransactions(unmappedArray)); // TODO test if this never runs after method returns (is it in sync?)
            });
        }

        return mappedExcelMatrix;
    }

    private readFile(file: File, callback: Function): void {

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