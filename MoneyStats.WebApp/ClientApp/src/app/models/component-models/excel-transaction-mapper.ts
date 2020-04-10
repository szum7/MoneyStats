import { NewTransaction } from "./new-transaction";
import { PropertyMapRow } from "./property-map-row";

export class ExcelTransactionMapper {

    // This is K&H Bank specific. Create a different one for each banks. #FEATURE
    public propertyMaps: Array<PropertyMapRow> = [
        new PropertyMapRow("A1", null, "100px", "AccountingDate", this.getJsDateFromExcel),
        new PropertyMapRow("B1", null, "150px", "TransactionId", null),
        new PropertyMapRow("C1", null, "210px", "Type", null),
        new PropertyMapRow("D1", null, "210px", "Account", null),
        new PropertyMapRow("E1", null, "120px", "AccountName", null),
        new PropertyMapRow("F1", null, "210px", "PartnerAccount", null),
        new PropertyMapRow("G1", null, "130px", "PartnerName", null),
        new PropertyMapRow("H1", "text-right", "80px", "Sum", null),
        new PropertyMapRow("I1", null, "100px", "Currency", null),
        new PropertyMapRow("J1", null, "auto", "Message", null)
    ];

    constructor() {
    }

    getPropertyValue(obj: NewTransaction, key: string): any {
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

    mapTransactions(list: Array<any>): Array<NewTransaction> {
        let ms: Array<NewTransaction> = [];
        for (let i = 0; i < list.length; i++) {
            ms.push(this.mapTransaction(list[i]));
        }
        return ms;
    }

    private formatDate(date: string): string {
        return (new Date(date)).toISOString().substring(0, 10);
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