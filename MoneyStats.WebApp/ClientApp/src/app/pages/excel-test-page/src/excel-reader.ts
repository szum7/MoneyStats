import { NewTransaction } from "./new-transaction";
import { ExcelTransactionMapper } from "./excel-transaction-mapper";
import * as XLSX from 'xlsx';

export class ExcelReader {
    
    public inputFileNames: Array<string>;
    private finishedArray: Array<boolean>;

    constructor() {
        this.inputFileNames = [];
    }

    public isReadingFinished(): boolean {
        let i = 0;
        while (i < this.finishedArray.length) {
            if (this.finishedArray[i] === false) {
                return false;
            }
            i++;
        }
        return true;
    }

    public getTransactionMatrix(inputFiles: Array<any>): Array<Array<NewTransaction>> {
        
        let self = this;
        let mappedExcelMatrix: Array<Array<NewTransaction>> = [];
        let mapper: ExcelTransactionMapper = new ExcelTransactionMapper();

        this.initFinishedArray(inputFiles.length);

        for (let i = 0; i < inputFiles.length; i++) {

            let file = inputFiles[i];

            // Save filename
            this.inputFileNames.push(file.name);

            // Read file
            self.readFile(file, function(unmappedArray) {
                mappedExcelMatrix.push(mapper.mapTransactions(unmappedArray));
                self.finishedArray[i] = true;
            });
        }

        return mappedExcelMatrix;
    }

    private initFinishedArray(length: number): void {
        this.finishedArray = new Array<boolean>(length);
        for (let i = 0; i < length; i++) {
            this.finishedArray[i] = false;
        }
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
            var result = XLSX.utils.sheet_to_json(worksheet, { raw: true });
            callback(result);
        }
        fileReader.readAsArrayBuffer(file);
    }
}