import { ReadInBankRow } from "./read-in-bank-row";
import { InputMessages } from "src/app/utilities/input-messages.static";

export class ReadInBankRowsMerger {

    public searchForDuplicates(matrix: Array<Array<ReadInBankRow>>) {
        for (let i = 0; i < matrix.length; i++) {
            let sheet = matrix[i];
            for (let j = 0; j < sheet.length; j++) {
                let tr = sheet[j];

                // Search for duplication
                let k = 0;
                while (k < matrix.length && !tr.isExcludedAttr.value) {
                    if (i !== k) { // Only search for duplicates in other sheets
                        let foreignSheet = matrix[k];
                        let l = 0;
                        while (l < foreignSheet.length && !tr.isExcludedAttr.value) {
                            let foreignRow = foreignSheet[l];
                            if (!foreignRow.isExcludedAttr.value &&  // Only exclude one row per duplication(s)
                                tr.bankRow.getContentId() === foreignRow.bankRow.getContentId()) {
                                this.setRowForExclusion(tr);
                            }
                            l++;
                        }
                    }
                    k++;
                }                
            }
        }
    }

    private setRowForExclusion(row: ReadInBankRow): void {
        row.isExcludedAttr.value = true;
        row.messages.push(InputMessages.READ_FILES_DUPLICATION);
    }
}