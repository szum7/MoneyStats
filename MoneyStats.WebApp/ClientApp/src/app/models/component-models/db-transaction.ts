import { Transaction } from "src/app/models/service-models/transaction.model";
import { CompareResults } from "./compare-results";

export class DbTransaction extends Transaction {

    public isExcluded: boolean;
    public compareResults: CompareResults;

    constructor() {
        super();
        this.isExcluded = false;
        this.compareResults = new CompareResults();
    }
}
