import { BankRow } from "src/app/models/service-models/bank-row.model";
import { InputMessages } from "src/app/utilities/input-messages.static";

export class ReadInBankRowAttribute {
    value: boolean; // is it excluded?
    message: string; // why it's excluded?
}

export class IsExcludedAttribute extends ReadInBankRowAttribute {
    constructor() {
        super();
        this.message = InputMessages.ROW_WILL_BE_EXCLUDED;
    }
}

export class ReadInBankRow {

    uiId: number;

    bankRow: BankRow;

    messages: string[];
    isExcludedAttr: IsExcludedAttribute;
    isDetailsOpen: boolean;
    detailsMenuPageAt: number;

    get hasAnActiveAlert(): boolean {
        return this.isExcludedAttr.value;
    }

    constructor() {
        this.messages = [];
        this.isExcludedAttr = new IsExcludedAttribute();
        this.isDetailsOpen = false;
        this.detailsMenuPageAt = 0;
    }

    public get(bankRow): ReadInBankRow {
        this.bankRow = bankRow;
        return this;
    }
}