import { ReadInBankRow } from "./read-in-bank-row";
import { PropertyMapRow } from "./property-map-row";
import { BankRow } from "../service-models/bank-row.model";
import { BankType } from "../service-models/bank-type.enum";

type EnumDictionary<T extends string | symbol | number, U> = {
    [K in T]?: U;
};

export class ExcelBankRowMapper {

    private readonly bankPropertyMaps: EnumDictionary<BankType, Array<PropertyMapRow>> = {
        [BankType.KH]: [
            new PropertyMapRow("A1", null, "100px", "AccountingDate", this.getJsDateFromExcel),
            new PropertyMapRow("B1", null, "150px", "BankTransactionId", null),
            new PropertyMapRow("C1", null, "210px", "Type", null),
            new PropertyMapRow("D1", null, "210px", "Account", null),
            new PropertyMapRow("E1", null, "120px", "AccountName", null),
            new PropertyMapRow("F1", null, "210px", "PartnerAccount", null),
            new PropertyMapRow("G1", null, "130px", "PartnerName", null),
            new PropertyMapRow("H1", "text-right", "80px", "Sum", null),
            new PropertyMapRow("I1", null, "100px", "Currency", null),
            new PropertyMapRow("J1", null, "auto", "Message", null)
        ],
        [BankType.BudapestBank]: [],
        [BankType.CIB]: [],
        [BankType.OTP]: []
    };

    public propertyMaps: Array<PropertyMapRow>;

    constructor(bankType: BankType) {
        this.propertyMaps = this.bankPropertyMaps[bankType];
    }

    getPropertyValue(obj: ReadInBankRow, key: string): any {
        // Custom rules
        if (key == "AccountingDate") {
            return this.formatDate(obj[key]);
        }

        return obj[key];
    }

    isPropertyMapUnset(): boolean {
        if (this.propertyMaps == null || this.propertyMaps.length == 0)
            return true;
        return this.propertyMaps[0].literal == null || this.propertyMaps[0].literal == "";
    }

    setPropertyMapLiterals(worksheet: any): void {
        for (let i = 0; i < this.propertyMaps.length; i++) {
            let propertyMap = this.propertyMaps[i];
            propertyMap.literal = worksheet[propertyMap.cell].v;
        }
    }

    mapBankRows(list: Array<any>): Array<ReadInBankRow> {
        let array: Array<ReadInBankRow> = [];
        for (let i = 0; i < list.length; i++) {
            let item = new ReadInBankRow();
            item.bankRow = this.mapBankRow(list[i]);
            array.push(item);
        }
        return array;
    }

    private formatDate(date: Date): string {
        if (!date)
            return null;
        return date.toISOString().substring(0, 10);
    }

    private mapBankRow(obj: any): BankRow {
        let cast: BankRow = new BankRow();

        for (let i = 0; i < this.propertyMaps.length; i++) {
            const propertyMap = this.propertyMaps[i];
            let value = obj[propertyMap.literal]; // Get the value
            if (propertyMap.parser != null) {
                value = propertyMap.parser(value); // Parse value if parse function is specified
            }
            cast[propertyMap.property] = value; // Set the value
        }

        cast = this.setAdditionalProperties(cast);

        return cast;
    }

    private setAdditionalProperties(m: BankRow): BankRow {
        //m.OriginalContentId = m.getContentId();
        return m;
    }

    private getJsDateFromExcel(excelDate): Date {
        // Dates in excel are stored in a weird number format

        // JavaScript dates can be constructed by passing milliseconds
        // since the Unix epoch (January 1, 1970) example: new Date(12312512312);
        
        // 1. Subtract number of days between Jan 1, 1900 and Jan 1, 1970, plus 1  (Google "excel leap year bug")
        // Neet to add +1 for some reason - szum7
        // 2. Convert to milliseconds.
        return new Date((excelDate - (25567 + 1 + 1)) * 86400 * 1000); // HACK hour is at 01:00:00. Should be at 00:00:00
    }
}
