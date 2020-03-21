import { Transaction } from "src/app/models/service-models/transaction.model";

export class NewTransaction extends Transaction {
    
    isExcluded: boolean;
    inputMessage: string;

    constructor() {
        super();
        this.isExcluded = false;
    }
}