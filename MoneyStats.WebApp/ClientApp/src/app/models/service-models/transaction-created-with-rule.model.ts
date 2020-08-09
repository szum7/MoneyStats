import { Rule } from "./rule.model";
import { Transaction } from "./transaction.model";

export class TransactionCreatedWithRule {
    ruleId: number;
    transactionId: number;
    rule: Rule;
    transaction: Transaction;
}