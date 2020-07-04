import { BankRow } from "src/app/models/service-models/bank-row.model";

export class ReadInBankRow {
    
    uiId: number;

    bankRow: BankRow;

    isExcluded: boolean;
    inputMessage: string;
    isDetailsOpen: boolean;

    constructor() {
        this.isExcluded = false;
        this.isDetailsOpen = false;
    }

    public get(bankRow): ReadInBankRow{
        this.bankRow = bankRow;
        return this;
    }
}