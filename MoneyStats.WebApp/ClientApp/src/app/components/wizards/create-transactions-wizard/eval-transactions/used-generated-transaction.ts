import { BankRow } from 'src/app/models/service-models/bank-row.model';
import { GeneratedTransaction } from 'src/app/models/service-models/generated-transaction.model';
import { TableRow, TableRowAttribute } from 'src/app/models/component-models/read-in-bank-row';
import { StaticMessages } from 'src/app/utilities/input-messages.static';

class IsModifiedAttribute extends TableRowAttribute {
    constructor() {
        super(StaticMessages.ROW_IS_MODIFIED);
    }
}

export class UsedGeneratedTransaction extends TableRow {

    value: GeneratedTransaction;
    originalValue: GeneratedTransaction;
    isModifiedAttr: IsModifiedAttribute; // TODO create (change) events to wire this alert in

    get bankRow(): BankRow {
        return this.value != null ? this.value.bankRowReference : null;
    }

    get bankRows(): BankRow[] {
        return this.value != null ? this.value.aggregatedBankRowReferences : null;
    }

    get hasAnActiveAlert(): boolean {
        if (this.isExcludedAttr.value || this.isModifiedAttr.value) {
            return true;
        }
        return false;
    }

    constructor(value: GeneratedTransaction) {
        super();
        this.value = value;
        this.isModifiedAttr = new IsModifiedAttribute();
        this.copyProperties(this.value, this.originalValue);
    }


    private copyProperties(from: GeneratedTransaction, to: GeneratedTransaction): void {
        to = new GeneratedTransaction();
        to.date = from.date;
        to.title = from.title;
        to.description = from.description;
        to.sum = from.sum;
        to.tags = [];
        for (let i = 0; i < from.tags.length; i++) {
            // No need to copy the tags's properties, it can stay as a reference.
            to.tags.push(from.tags[i]);
        }
    }

    resetToOriginal(): void {
        this.copyProperties(this.originalValue, this.value);
    }
}
