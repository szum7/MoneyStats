import { Component } from '@angular/core';

export enum StageType {
    fileFileCompare,
    dbFileCompare,
    evaluateRules
}

@Component({
    selector: 'app-update-page',
    templateUrl: './update.page.html',
    styleUrls: ['./update.page.scss']
})
export class UpdatePage {
    
    public stageType = StageType;
    public stage: StageType;

    constructor() {
        this.stage = StageType.fileFileCompare;
    }

}
