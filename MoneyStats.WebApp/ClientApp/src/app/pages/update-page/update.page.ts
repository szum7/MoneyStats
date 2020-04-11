import { Component } from '@angular/core';
import { ReadBankRowForInsertion } from 'src/app/models/component-models/read-bank-row-for-insertion';
import { StageType } from '../../models/component-models/stage-type.enum';
import { FileFileResult } from '../../models/component-models/file-file-result';
import { DbFileResult } from 'src/app/models/component-models/db-file-result';
import { RuleTransaction } from 'src/app/models/component-models/rule-transaction';

@Component({
    selector: 'app-update-page',
    templateUrl: './update.page.html',
    styleUrls: ['./update.page.scss']
})
export class UpdatePage {
    
    public stageType = StageType;

    public stage: StageType;    
    public fileFileResult: FileFileResult;
    public dbFileResult: DbFileResult;
    
    constructor() {
        this.fileFileResult = new FileFileResult();
        this.dbFileResult = new DbFileResult();

        this.setFirstStage();
    }

    private setFirstStage(): void {
        this.stage = StageType.fileFileCompare;
    }

    change_fileFileOutput(output: FileFileResult): void {
        this.fileFileResult = output;
        this.stage = StageType.dbFileCompare;
    }

    change_dbFileOutput(output: Array<RuleTransaction>): void {
        this.dbFileResult.bankRowList = output;
        this.stage = StageType.evaluateRules;
    }

    change_ruleEvaluatorOutput(output: any): void {
        // Not yet implemented
    }
}
