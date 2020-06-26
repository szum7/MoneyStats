import { ReadInBankRow } from "./read-in-bank-row";
import { ExcelBankRowMapper } from "./excel-bank-row-mapper";
import * as XLSX from 'xlsx';

export class ExcelReader {
    
    //public inputFileNames: Array<string>;
    private isFinishedArray: Array<boolean>;
    private mapper: ExcelBankRowMapper;

    constructor(mapper: ExcelBankRowMapper) {
        //this.inputFileNames = [];
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

    public getBankRowMatrix(inputFiles: Array<any>): Array<Array<ReadInBankRow>> {
        
        let self = this;
        let mappedExcelMatrix: Array<Array<ReadInBankRow>> = []; // One row is one document.
        //this.inputFileNames = [];

        this.initFinishedArray(inputFiles.length);

        for (let i = 0; i < inputFiles.length; i++) {

            let file = inputFiles[i];

            // Save filename
            //this.inputFileNames.push(file.name);

            // Read file
            self.readFile(file, function(unmappedArray) { // this is an async function
                mappedExcelMatrix.push(self.mapper.mapBankRows(unmappedArray));
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