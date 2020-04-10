import { BankRow } from "src/app/models/service-models/transaction.model";

export class NewTransaction extends BankRow {
    
    isExcluded: boolean;
    inputMessage: string;

    constructor() {
        super();
        this.isExcluded = false;
    }
}