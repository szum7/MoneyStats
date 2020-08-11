import { Transaction } from "./transaction.model";

export class BasicMonthlyBar {

    date: Date;
    income: number;
    expense: number;
    flow: number;
    transactions: Transaction[];

    constructor() {
        this.transactions = [];
    }
}
