import { Component, OnInit } from '@angular/core';
import { StageType } from '../../models/component-models/stage-type.enum';
import { FileFileResult } from '../../models/component-models/file-file-result';
import { DbFileResult } from 'src/app/models/component-models/db-file-result';
import { RuleTransaction } from 'src/app/models/component-models/rule-transaction';
import { WizardStep } from 'src/app/models/component-models/wizard-step';

@Component({
    selector: 'app-update-page',
    templateUrl: './update.page.html',
    styleUrls: ['./update.page.scss']
})
export class UpdatePage implements OnInit {
    
    stageType = StageType;

    stage: StageType;    
    fileFileResult: FileFileResult;
    dbFileResult: DbFileResult;

    stepsAt: number;
    wizardSteps: WizardStep[];
    
    constructor() {
        this.fileFileResult = new FileFileResult();
        this.dbFileResult = new DbFileResult();

        this.stepsAt = 0;

        this.wizardSteps = [
            new WizardStep("Import files", "#"),
            new WizardStep("Manage read files", "#"),
            new WizardStep("Create transactions", "#")
        ];

        this.setFirstStage();
    }

    ngOnInit(): void {

    }

    click_NextStep() {
        this.stepsAt++;
    }

    click_PrevStep() {
        this.stepsAt--;
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
