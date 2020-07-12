import { BankRow } from "./bank-row.model";
import { Tag } from "./tag.model";

export class Transaction {

    Title: string;
    Description: string;
    Date?: Date;
    Sum?: number;
    IsCustom: boolean;
    BankRowId?: number;
    Tags: Tag[];
    AggregatedReferences: BankRow[];
    
    get IsGroup(): boolean { return this.BankRowId == null; }

    constructor() {
        this.Tags = [];
        this.AggregatedReferences = [];
    }
}