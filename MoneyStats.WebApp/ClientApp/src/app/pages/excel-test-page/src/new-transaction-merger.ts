import { NewTransaction } from "./new-transaction";
import { InputMessages } from "src/app/utilities/input-messages.static";

export class NewTransactionMerger {

    public defineRows(matrix: Array<Array<NewTransaction>>) {
        for (let i = 0; i < matrix.length; i++) {
            let sheet = matrix[i];
            for (let j = 0; j < sheet.length; j++) {
                let tr = sheet[j];

                // Search for duplication
                let k = 0;
                while (k < matrix.length && !tr.isExcluded) {
                    if (i !== k) { // Only search for duplicates in other sheets
                        let foreignSheet = matrix[k];
                        let l = 0;
                        while (l < foreignSheet.length && !tr.isExcluded) {
                            let foreignRow = foreignSheet[l];
                            if (!foreignRow.isExcluded &&  // Only exclude one row per duplication(s)
                                tr.OriginalContentId === foreignRow.OriginalContentId) {
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

    private setRowForExclusion(row: NewTransaction): void {
        row.isExcluded = true;
        row.inputMessage = InputMessages.READ_FILES_DUPLICATION;
    }
}