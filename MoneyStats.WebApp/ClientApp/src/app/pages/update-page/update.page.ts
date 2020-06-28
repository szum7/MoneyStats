import { Component, OnInit, ViewEncapsulation } from '@angular/core';
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

    constructor(title: string) {
        this.title = title;
    }

    public setToCriteria() {
        this.type = StepAlertType.Criteria;
        return this;
    }

    public setToMessage() {
        this.type = StepAlertType.Message;
        return this;
    }
}

export class CurrentStep {

    get isProgressable(): boolean {

        if (!this.stepAlerts)
            return true;

        for (let i = 0; i < this.stepAlerts.length; i++) {
            const element = this.stepAlerts[i];
            if (element.type == StepAlertType.Criteria)
                return false;
        }
        return true;
    }
    stepAlerts: StepAlert[];

    constructor() {
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
            new WizardStep("Compare with database", "#"),
            new WizardStep("Create transactions", "#")
        ];

        this.stepsAt = 0;

        this.currentStep = new CurrentStep();
    }

    public next(): boolean {
        if (this.stepsAt >= this.wizardSteps.length - 1)
            return false;

        if (!this.currentStep.isProgressable)
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
    styleUrls: ['./update.page.scss'],
    encapsulation: ViewEncapsulation.None
})
export class UpdatePage implements OnInit {

    wizard: UpdateWizard;
    results: UpdateResults;
    isTooltipsDisabled: boolean;
    get stepAlertType() { return StepAlertType; }
    get isStepReadyToProgress() {
        if (!this.wizard)
            return false;

        return this.wizard.currentStep.isProgressable;
    }

    constructor() {
        this.wizard = new UpdateWizard();
        this.results = new UpdateResults();
        this.isTooltipsDisabled = false;
    }

    ngOnInit(): void {
    }

    click_NextStep() {
        this.wizard.next();
    }

    click_PrevStep() {
        this.wizard.previous();
        // TODO could null out the last updateResult (first, second or third)
        // TODO alert user if sure about going back (handle misclicks)
    }

    click_toggleTitleTags(): void {
        this.isTooltipsDisabled = !this.isTooltipsDisabled;
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
}
