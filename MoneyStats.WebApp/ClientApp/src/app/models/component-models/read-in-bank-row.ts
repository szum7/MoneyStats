import { BankRow } from "src/app/models/service-models/bank-row.model";
import { StaticMessages } from "src/app/utilities/input-messages.static";

/// <summary>
/// Toggleable attributes like inclusion/exclusion.
/// </summary>
export class TableRowAttribute {
    public value: boolean; // is it excluded?
    private _message: string; // why it's excluded?

    public get message(): string { return this._message; }

    constructor(message: string){
        this._message = message;
    }
}

export class IsExcludedAttribute extends TableRowAttribute {
    constructor() {
        super(StaticMessages.ROW_WILL_BE_EXCLUDED);
    }
}

export class TableRow {

    /// <summary>
    /// For program generated messages. Not toggleable.
    /// </summary>
    messages: string[];

    isDetailsOpen: boolean;

    detailsMenuPageAt: number;

    protected isExcludedAttr: IsExcludedAttribute;
    public get isExcluded(): boolean { return this.isExcludedAttr.value; }
    public get isExcludedMessage(): string { return this.isExcludedAttr.message; }

    public toggleExclusion(): void {
        this.isExcludedAttr.value = !this.isExcludedAttr.value;
    }

    public setToExclude(): void {
        this.isExcludedAttr.value = true;
    }

    public setToInclude(): void {
        this.isExcludedAttr.value = false;
    }

    constructor() {
        this.isExcludedAttr = new IsExcludedAttribute();
        this.isDetailsOpen = false;
        this.detailsMenuPageAt = 0;
        this.messages = [];
    }
}

export class ReadInBankRow extends TableRow {

    uiId: number;

    bankRow: BankRow;

    get hasAnActiveAlert(): boolean {
        if (this.isExcludedAttr.value) {
            return true;
        }
        // ...
        return false;
    }

    constructor() {
        super();
    }

    public get(bankRow): ReadInBankRow {
        this.bankRow = bankRow;
        return this;
    }
}