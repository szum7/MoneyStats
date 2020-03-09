import { NewTransaction } from "./new-transaction";

export class PropertyMapRow {
    
    cell: string; // A1 | B2 | ...
    literal: string; // Könyvelés dátuma | Tranzakció azonosító  | ...
    property: string; // AccountingDate | TransactionId | ...
    parser: Function;

    constructor(cell: string, property: string, parser: Function) {
        this.cell = cell;
        this.property = property;
        this.parser = parser;
    }
}

export class ExcelTransactionMapper {

    public propertyMaps: Array<PropertyMapRow> = [
        new PropertyMapRow("A1", "AccountingDate", this.getJsDateFromExcel),
        new PropertyMapRow("B1", "TransactionId", null),
        new PropertyMapRow("C1", "Type", null),
        new PropertyMapRow("D1", "Account", null),
        new PropertyMapRow("E1", "AccountName", null),
        new PropertyMapRow("F1", "PartnerAccount", null),
        new PropertyMapRow("G1", "PartnerName", null),
        new PropertyMapRow("H1", "Sum", null),
        new PropertyMapRow("I1", "Currency", null),
        new PropertyMapRow("J1", "Message", null)
    ];

    constructor() {
    }

    public isPropertyMapUnset(): boolean {
        if (this.propertyMaps == null || this.propertyMaps.length == 0)
            return true;
        return this.propertyMaps[0].literal == null || this.propertyMaps[0].literal == "";
    }

    public mapTransactions(list: Array<any>): Array<NewTransaction> {
        let ms: Array<NewTransaction> = [];
        for (let i = 0; i < list.length; i++) {
            ms.push(this.mapTransaction(list[i]));
        }
        return ms;
    }

    public setPropertyMapLiterals(worksheet: any): void {
        for (let i = 0; i < this.propertyMaps.length; i++) {
            let propertyMap = this.propertyMaps[i];
            propertyMap.literal = worksheet[propertyMap.cell].v;
        }
    }

    private mapTransaction(tr: any): NewTransaction {
        let m: NewTransaction = new NewTransaction();

        for (let i = 0; i < this.propertyMaps.length; i++) {
            const propertyMap = this.propertyMaps[i];
            let value = tr[propertyMap.literal];
            if (propertyMap.parser != null) {
                value = propertyMap.parser(value);
            }
            m[propertyMap.property] = value;
        }

        m = this.setAdditionalProperties(m);

        return m;
    }

    private setAdditionalProperties(m: NewTransaction): NewTransaction {
        m.OriginalContentId = m.getContentId();
        return m;
    }

    private getJsDateFromExcel(excelDate): string {
        // JavaScript dates can be constructed by passing milliseconds
        // since the Unix epoch (January 1, 1970) example: new Date(12312512312);
        
        // 1. Subtract number of days between Jan 1, 1900 and Jan 1, 1970, plus 1  (Google "excel leap year bug")
        // Neet to add +1 for some reason - Sz. Aron
        // 2. Convert to milliseconds.
        return (new Date((excelDate - (25567 + 1 + 1)) * 86400 * 1000)).toString(); // TODO hour is at 01:00:00. Should be at 00:00:00
    }
}