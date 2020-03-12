import { Component } from '@angular/core';
import { DbTransaction } from 'src/app/components/db-file-comparer/src/db-transaction';
import { StageType } from './src/stage-type.enum';
import { FileFileResult } from './src/file-file-result';
import { DbFileResult } from './src/db-file-result';

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

    change_fileFileOutput(output: Array<DbTransaction>): void {
        this.fileFileResult.transactionList = output;
        this.stage = StageType.dbFileCompare;
    }

    change_dbFileOutput(output: any): void {
        // Not yet implemented        
        this.stage = StageType.evaluateRules;
    }
}
