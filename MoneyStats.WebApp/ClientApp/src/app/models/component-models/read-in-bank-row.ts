import { BankRow } from "src/app/models/service-models/bank-row.model";

export class ReadInBankRow {
    
    uiId: number;

    bankRow: BankRow;

    isExcluded: boolean;
    inputMessage: string;

    constructor() {
        this.isExcluded = false;
    }

    public get(bankRow): ReadInBankRow{
        this.bankRow = bankRow;
        return this;
    }
}