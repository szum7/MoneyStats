import { Transaction } from "./transaction.model";

export class BasicMonthlyBar {

    date: Date;
    income: number;
    expense: number;
    flow: number;
    transactions: Transaction[];
    isMissingMonth: boolean;

    constructor() {
        this.transactions = [];
    }
}
