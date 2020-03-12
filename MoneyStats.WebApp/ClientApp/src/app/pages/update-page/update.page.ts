import { Component } from '@angular/core';
import { DbTransaction } from 'src/app/components/db-file-comparer/src/db-transaction';

export enum StageType {
    fileFileCompare,
    dbFileCompare,
    evaluateRules
}

export class FileFileResult { // First step
    public transactionList: Array<DbTransaction>;
}

export class DbFileResult {

}

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
        this.stage = StageType.fileFileCompare;

        this.fileFileResult = new FileFileResult();
        this.dbFileResult = new DbFileResult();
    }

    click_changeStage(): void {
        this.stage = StageType.dbFileCompare;
    }

    change_fileFileOutput(output: Array<DbTransaction>): void {
        this.fileFileResult.transactionList = output;
    }

    click_testBinding() {
        this.fileFileResult.transactionList[0].isExcluded = true;
        this.fileFileResult.transactionList[1].isExcluded = true;
        this.fileFileResult.transactionList[2].isExcluded = true;
    }
}
