import { ReadInBankRow } from "./read-in-bank-row";
import { PropertyMapRow } from "./property-map-row";
import { BankRow } from "../service-models/bank-row.model";
import { BankType } from "../service-models/bank-type.enum";

type EnumDictionary<T extends string | symbol | number, U> = {
    [K in T]?: U;
};

class BankFileReadConfig {
    
    public bankType: BankType;
    public postProcessFunction: (matrix: any) => any;
    public columns: Array<ColumnDefinition>;

    /**
     * To be able to get columns by name in O(1). This is a speed over memory solution.
     */
    private reverseColumns: { [id: string] : number };
    
    /**
     * @param bankType 
     * @param postProcessFunction Can be set to null
     * @param columns Order matters!
     */
    constructor(bankType: BankType, postProcessFunction: (matrix: any) => any, columns: Array<ColumnDefinition>) {
        this.bankType = bankType;
        this.postProcessFunction = postProcessFunction;
        this.columns = columns;

        this.initReverseColumns(columns);
    }

    getColumnByName(name: string): ColumnDefinition {
        return this.columns[this.reverseColumns[name]];
    }

    getCssClassByColumnName(name: string): string {
        return this.getColumnByName(name).cssClass;
    }

    getColumnNames(): string[] {
        let ret: string[] = [];
        this.columns.forEach(item => {
            ret.push(item.columnName);
        });
        return ret;
    }

    private initReverseColumns(columns: Array<ColumnDefinition>){
        this.reverseColumns = {};
        for (let i = 0; i < columns.length; i++) {
            this.reverseColumns[columns[i].columnName] = i;
        }
    }
}

class ColumnDefinition {

    public columnName: string;
    public literal: string; // Könyvelés dátuma | Tranzakció azonosító  | ...
    public cssClass: string;
    public valueConverterFunction: (any) => any;
    public isOpen: boolean;
    public width: string;

    /**
     * @param columnName 
     * @param cssClass 
     * @param valueConverterFunction Can be set to null
     */
    constructor(columnName: string, literal: string, cssClass: string, valueConverterFunction: (any) => any) {
        this.columnName = columnName;
        this.cssClass = cssClass;
        this.valueConverterFunction = valueConverterFunction;
        this.isOpen = true;
    }
}

export class ExcelBankRowMapper {

    private readonly BANK_FILE_MAPS: EnumDictionary<BankType, BankFileReadConfig> = {
        [BankType.KH]: new BankFileReadConfig(BankType.KH, this.trimKAndHFromUnwantedRows, [
            new ColumnDefinition("accountingDate", "", null, this.getJsDateFromExcel),
            new ColumnDefinition("bankTransactionId", "", null, null),
            new ColumnDefinition("type", "", null, null),
            new ColumnDefinition("account", "", null, null),
            new ColumnDefinition("accountName", "", null, null),
            new ColumnDefinition("partnerAccount", "", null, null),
            new ColumnDefinition("partnerName", "", null, null),
            new ColumnDefinition("sum", "", null, null),
            new ColumnDefinition("currency", "", null, null),
            new ColumnDefinition("message", "", null, null)
        ]),
        [BankType.Raiffeisen]: new BankFileReadConfig(BankType.Raiffeisen, this.trimRaifeissenFromUnwantedRows, [
            new ColumnDefinition("type", "", null, null),
            new ColumnDefinition("accountingDate", "", null, this.getJsDateFromExcel),
            new ColumnDefinition("ignore1", "", null, null),
            new ColumnDefinition("transactionId", "", null, null),
            new ColumnDefinition("sum", "", null, null),
            new ColumnDefinition("partnerAccount", "", null, null),
            new ColumnDefinition("partnerName", "", null, null),
            new ColumnDefinition("messagePart1", "", null, null),
            new ColumnDefinition("messagePart2", "", null, null),
            new ColumnDefinition("messagePart3", "", null, null),
            new ColumnDefinition("messagePart4", "", null, null),
            new ColumnDefinition("messagePart5", "", null, null)
        ])
    };

    private readonly bankPropertyMaps: EnumDictionary<BankType, Array<PropertyMapRow>> = {
        [BankType.KH]: [
            new PropertyMapRow("A1", null, "100px", "accountingDate", this.getJsDateFromExcel),
            new PropertyMapRow("B1", null, "150px", "bankTransactionId", null),
            new PropertyMapRow("C1", null, "210px", "type", null),
            new PropertyMapRow("D1", null, "210px", "account", null),
            new PropertyMapRow("E1", null, "120px", "accountName", null),
            new PropertyMapRow("F1", null, "210px", "partnerAccount", null),
            new PropertyMapRow("G1", null, "130px", "partnerName", null),
            new PropertyMapRow("H1", "text-right", "80px", "sum", null),
            new PropertyMapRow("I1", null, "100px", "currency", null),
            new PropertyMapRow("J1", null, "auto", "message", null)
        ],
        [BankType.Raiffeisen]: [],
        [BankType.BudapestBank]: [],
        [BankType.CIB]: [],
        [BankType.OTP]: []
    };

    private _config: BankFileReadConfig; // old name propertyMaps    

    get config(): BankFileReadConfig {
        return this._config;
    }

    get columns(): ColumnDefinition[] {
        if (this._config === null) {
            return null;
        }
        return this._config.columns;
    }

    get columnNames(): string[] {
        return this._config.getColumnNames();
    }

    constructor(bankType: BankType) {
        this._config = this.BANK_FILE_MAPS[bankType];
    }

    // TODO nem itt a helye, csak példa hogy kb így nézzen ki
    postProcessMatrix(selectedBank: BankType, data) {
        let func = this.BANK_FILE_MAPS[selectedBank].postProcessFunction;
        if (func === null) {
            return data;
        }
        return func(data);
    }

    trimKAndHFromUnwantedRows(data) {
        return data.splice(0, 1);
    }

    trimRaifeissenFromUnwantedRows(data) {
        let count = 0;
        while (count < data.length && data[count].type !== "Könyvelt tételek") count++;

        if (count < data.length) {
            return data.splice(0, count);
        } 
        return data;
    }

    getPropertyValue(obj: ReadInBankRow, key: string): any {
        // Custom rules
        if (key == "accountingDate") {
            return this.formatDate(obj[key]);
        }

        return obj[key];
    }

    getPropertyValueOrNull(obj: ReadInBankRow, key: string): any {
        let val;

        if (key == "accountingDate") {
            val = this.formatDate(obj[key]);
        } else {
            val = obj[key];
        }

        if (val == null || val == "") {
            return "null";
        }
        return val;
    }

    isPropertyMapUnset(): boolean {
        if (this._config == null || this._config.columns.length == 0)
            return true;
        return false;
    }

    // setPropertyMapLiterals(worksheet: any): void {
    //     for (let i = 0; i < this.propertyMaps.columns.length; i++) {
    //         let propertyMap = this.propertyMaps.columns[i];
    //         propertyMap.literal = worksheet[propertyMap.cell].v;
    //     }
    // }

    mapBankRows(list: Array<any>): Array<ReadInBankRow> {
        let array: Array<ReadInBankRow> = [];
        for (let i = 0; i < list.length; i++) {
            let item = new ReadInBankRow();
            item.bankRow = this.mapBankRow(list[i]);
            array.push(item);
        }
        return array;
    }

    private formatDate(date: Date | string): string {
        if (!date)
            return null;

        if (typeof date === "string")
            date = new Date(date);

        let r = date.getFullYear();
        let m = (date.getMonth() + 1).toString();
        m = m.length > 1 ? m : "0" + m;
        let d = date.getDate().toString();
        d = d.length > 1 ? d : "0" + d;

        return r + "-" + m + "-" + d;
    }

    private mapBankRow(obj: any): BankRow {
        let cast: BankRow = new BankRow();

        for (let i = 0; i < this._config.columns.length; i++) {

            const propertyMap = this._config.columns[i];

            if (["ignore1"].some(x => x === propertyMap.columnName)){
                continue;
            }

            let value = obj[propertyMap.columnName]; // Get the value

            if (value === null) {
                continue;
            }

            if (propertyMap.valueConverterFunction != null) { // Parse value if parse function is specified
                value = propertyMap.valueConverterFunction(value);
            }

            if (["messagePart1", "messagePart2", "messagePart3", "messagePart4", "messagePart5"].some(x => x === propertyMap.columnName)) {
                if (cast["message"] === null) {
                    cast["message"] = "";
                }
                cast["message"] += value;
            } else {
                cast[propertyMap.columnName] = value; // Set the value
            }
        }

        return cast;
    }

    private getJsDateFromExcel(excelDate): Date {
        // Dates in excel are stored in a weird number format

        // JavaScript dates can be constructed by passing milliseconds
        // since the Unix epoch (January 1, 1970) example: new Date(12312512312);

        // 1. Subtract number of days between Jan 1, 1900 and Jan 1, 1970, plus 1  (Google "excel leap year bug")
        // Need to add +1 for some reason - szum7
        // 2. Convert to milliseconds.
        return new Date((excelDate - (25567 + 1 + 1)) * 86400 * 1000); // HACK hour is at 01:00:00. Should be at 00:00:00
    }
}
