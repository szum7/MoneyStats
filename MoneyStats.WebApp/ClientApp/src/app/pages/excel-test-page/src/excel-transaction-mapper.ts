import { NewTransaction } from "./new-transaction";

export class ExcelTransactionMapper {

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

    private mapTransaction(tr: any): NewTransaction {
        let m: NewTransaction = new NewTransaction();

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

    public mapTransactions(list: Array<any>): Array<NewTransaction> {
        let ms: Array<NewTransaction> = [];
        for (let i = 0; i < list.length; i++) {
            ms.push(this.mapTransaction(list[i]));
        }
        return ms;
    }
}