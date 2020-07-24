import { BankRow } from "./bank-row.model";
import { Tag } from "./tag.model";
import { Rule } from "./rule.model";

export class GeneratedTransaction {

    title: string;
    description: string;
    date: Date;
    sum: number;

    isCustom: boolean;

    bankRowReference: BankRow;
    tags: Tag[];
    appliedRules: Rule[];
    aggregatedBankRowReferences: BankRow[];

    get bankRowArray(): BankRow[] {
        if (this.bankRowReference != null) {
            return [this.bankRowReference];
        }
        return this.aggregatedBankRowReferences;
    }
}