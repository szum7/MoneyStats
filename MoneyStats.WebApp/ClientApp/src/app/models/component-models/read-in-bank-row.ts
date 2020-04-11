import { BankRow } from "src/app/models/service-models/bank-row.model";

export class ReadInBankRow {
    
    bankRow: BankRow;

    isExcluded: boolean;
    inputMessage: string;

    constructor() {
        this.isExcluded = false;
    }
}