import { BankRow } from "src/app/models/service-models/bank-row.model";
import { CompareResults } from "./compare-results";

export class ReadBankRowForInsertion {

    public bankRow: BankRow;

    public isExcluded: boolean;
    public compareResults: CompareResults;

    constructor() {
        this.isExcluded = false;
        this.compareResults = new CompareResults();
    }
}
