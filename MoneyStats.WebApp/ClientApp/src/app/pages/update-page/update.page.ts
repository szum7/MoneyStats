import { Component, OnInit } from '@angular/core';
import { WizardStep } from 'src/app/models/component-models/wizard-step';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { ReadBankRowForInsertion } from 'src/app/models/component-models/read-bank-row-for-insertion';

export enum StepAlertType {
    Criteria,
    Message
}

export class StepAlert {
    title: string;
    type: StepAlertType;
}

export class CurrentStep {
    isProgressPossible: boolean;
    stepAlerts: string[];

    constructor() {
        this.isProgressPossible = true; // TODO add criterias and validations (where needed)
        this.stepAlerts = [];
    }

    public clearAlerts(): void {
        this.stepAlerts = [];
    }
}

export class UpdateWizard {

    stepsAt: number;
    wizardSteps: WizardStep[];
    currentStep: CurrentStep;

    constructor() {
        this.wizardSteps = [
            new WizardStep("Import files", "#"),
            new WizardStep("Manage read files", "#"),
            new WizardStep("Create transactions", "#")
        ];

        this.stepsAt = 0;

        this.currentStep = new CurrentStep();
    }

    public next(): boolean {
        if (this.stepsAt >= this.wizardSteps.length - 1)
            return false;

        if (!this.currentStep.isProgressPossible)
            return false;

        this.stepsAt++;
        this.currentStep = new CurrentStep();

        return true;
    }

    public previous(): boolean {
        if (this.stepsAt <= 0)
            return false;

        this.stepsAt--;
        this.currentStep = new CurrentStep();

        return true;
    }
}

/// <summary>
/// A class for utilities used throughout multiple steps.
/// </summary>
export class UpdateResultsUtilities {
    bankMapper: ExcelBankRowMapper;
}

export class UpdateResults {
    firstResult: ReadInBankRow[][];
    secondResult: ReadBankRowForInsertion[];
    thirdResult: any; // TODO
    utils: UpdateResultsUtilities;

    constructor() {
        this.utils = new UpdateResultsUtilities();
    }
}

@Component({
    selector: 'app-update-page',
    templateUrl: './update.page.html',
    styleUrls: ['./update.page.scss']
})
export class UpdatePage implements OnInit {

    wizard: UpdateWizard;
    results: UpdateResults;

    constructor() {
        this.wizard = new UpdateWizard();
        this.results = new UpdateResults();
    }

    ngOnInit(): void {
    }

    click_NextStep() {
        this.wizard.next();
    }

    click_PrevStep() {
        this.wizard.previous();
        // TODO could null out the last updateResult (first, second or third)
    }

    output_firstStep($output: { matrix: ReadInBankRow[][], mapper: ExcelBankRowMapper }): void {
        // TODO Check if everything is okay and set step-alerts
        this.results.firstResult = $output.matrix;
        this.results.utils.bankMapper = $output.mapper;
    }

    output_secondStep($output: ReadBankRowForInsertion[]): void {
        // TODO Check if everything is okay and set step-alerts
        this.results.secondResult = $output;
    }

    // private setFirstStage(): void {
    //     this.stage = StageType.fileFileCompare;
    // }

    // change_fileFileOutput(output: FileFileResult): void {
    //     this.fileFileResult = output;
    //     this.stage = StageType.dbFileCompare;
    // }

    // change_dbFileOutput(output: Array<RuleTransaction>): void {
    //     this.dbFileResult.bankRowList = output;
    //     this.stage = StageType.evaluateRules;
    // }

    // change_ruleEvaluatorOutput(output: any): void {
    //     // Not yet implemented
    // }
}
