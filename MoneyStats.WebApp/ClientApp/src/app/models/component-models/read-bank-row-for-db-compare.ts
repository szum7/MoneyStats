import { BankRow } from "src/app/models/service-models/bank-row.model";
import { TableRow } from "./read-in-bank-row";

export class ReadBankRowForDbCompare extends TableRow {

    public uiId: number;
    public bankRow: BankRow;

    get hasAnActiveAlert(): boolean {
        if (this.isExcludedAttr.value) {
            return true;
        }
        // ...
        return false;
    }

    constructor() {
        super();
    }
}
