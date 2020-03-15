import { Transaction } from "src/app/services/transaction-service/models/transaction.model";
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
