import { BankRow } from "src/app/models/service-models/bank-row.model";
import { CompareResults } from "./compare-results";
import { TableRow } from "./read-in-bank-row";

export class ReadBankRowForDbCompare extends TableRow {

    public bankRow: BankRow;

    constructor() {
        super();
    }
}
