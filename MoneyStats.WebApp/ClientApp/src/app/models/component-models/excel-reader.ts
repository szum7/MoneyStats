import { NewTransaction } from "./new-transaction";
import { ExcelTransactionMapper } from "./excel-transaction-mapper";
import * as XLSX from 'xlsx';

export class ExcelReader {
    
    public inputFileNames: Array<string>;
    private isFinishedArray: Array<boolean>;
    private mapper: ExcelTransactionMapper;

    constructor(mapper: ExcelTransactionMapper) {
        this.inputFileNames = [];
        this.mapper = mapper;
    }

    public isReadingFinished(): boolean {
        let i = 0;
        while (i < this.isFinishedArray.length) {
            if (this.isFinishedArray[i] === false) {
                return false;
            }
            i++;
        }
        return true;
    }

    public getTransactionMatrix(inputFiles: Array<any>): Array<Array<NewTransaction>> {
        
        let self = this;
        let mappedExcelMatrix: Array<Array<NewTransaction>> = [];
        this.inputFileNames = [];

        this.initFinishedArray(inputFiles.length);

        for (let i = 0; i < inputFiles.length; i++) {

            let file = inputFiles[i];

            // Save filename
            this.inputFileNames.push(file.name);

            // Read file
            self.readFile(file, function(unmappedArray) {
                mappedExcelMatrix.push(self.mapper.mapTransactions(unmappedArray));
                self.isFinishedArray[i] = true;
            });
        }

        return mappedExcelMatrix;
    }

    private initFinishedArray(length: number): void {
        this.isFinishedArray = new Array<boolean>(length);
        for (let i = 0; i < length; i++) {
            this.isFinishedArray[i] = false;
        }
    }

    private readFile(file: File, callback: (response: any) => void): void {

        let fileReader = new FileReader();
        let arrayBuffer: any;
        let _this = this;

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

            if (_this.mapper.isPropertyMapUnset()) {
                _this.mapper.setPropertyMapLiterals(worksheet);
            }

            var result = XLSX.utils.sheet_to_json(worksheet, { raw: true });
            callback(result);
        }
        fileReader.readAsArrayBuffer(file);
    }
}