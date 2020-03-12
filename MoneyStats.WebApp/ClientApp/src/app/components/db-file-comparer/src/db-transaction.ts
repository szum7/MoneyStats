import { Transaction } from "src/app/services/transaction-service/models/transaction.model";

export class DbTransaction extends Transaction {

    public isExcluded: boolean;

    constructor() {
        super();
        this.isExcluded = false;
    }
}