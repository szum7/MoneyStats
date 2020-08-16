import { BankRow } from "./bank-row.model";
import { Tag } from "./tag.model";

export class Transaction {

    title: string;
    description: string;
    date?: Date;
    sum?: number;
    isCustom: boolean;
    bankRowId?: number;
    tags: Tag[];
    aggregatedReferences: BankRow[];
    appliedRules: string;
    
    get isGroup(): boolean { return this.bankRowId == null; }

    constructor() {
        this.tags = [];
        this.aggregatedReferences = [];
    }
}