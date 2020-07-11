import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef, AfterContentInit, AfterViewInit } from '@angular/core';
import { WizardStep } from 'src/app/models/component-models/wizard-step';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { ReadBankRowForInsertion } from 'src/app/models/component-models/read-bank-row-for-insertion';
import { BankRow } from 'src/app/models/service-models/bank-row.model';
import { BankType } from 'src/app/models/service-models/bank-type.enum';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';

export enum StepAlertType {
    Criteria,
    Message,
    GreenText
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

    public setToGreenText() {
        this.type = StepAlertType.GreenText;
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
    stepAlerts: StepAlert[]; // IMPROVE make this a singleton service and modify the messages through that (?)

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
            new WizardStep("Step 1 - Import files", "#"),
            new WizardStep("Step 2 - Manage read files", "#"),
            new WizardStep("Step 3 - Compare with database", "#"),
            new WizardStep("Step 4 - Create transactions", "#")
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
    encapsulation: ViewEncapsulation.None // TODO write comment why this is needed
})
export class UpdatePage implements OnInit, AfterViewInit {

    wizard: UpdateWizard;
    results: UpdateResults;
    isTooltipsDisabled: boolean;
    get stepAlertType() { return StepAlertType; }
    get isStepReadyToProgress() {
        if (!this.wizard)
            return false;

        return this.wizard.currentStep.isProgressable;
    }

    @ViewChild('wizardNavElement', null) wizardNavView: ElementRef;
    @ViewChild('btnsElement', null) btnsView: ElementRef;
    wizardNavMaxHeight: number;

    constructor(private loadingScreen: LoadingScreenService) {
        this.wizard = new UpdateWizard();
        this.results = new UpdateResults();
        this.isTooltipsDisabled = true;

        this.test();
    }

    test() {
        this.results.firstResult = [
            [
                new ReadInBankRow().get(new BankRow().get('Mon Feb 02 2015 01:00:00 GMT+0100 (Central European Standard Time)', 'ó kamat', 'Kamat', '10401945223571949481012', 'TÉTE BERTALAN', undefined, undefined, '456', 'HUF', '35HUF')),
                new ReadInBankRow().get(new BankRow().get('2000-01-02 11:00:00', '', '', '', '', '', '', '', '', '')),
                new ReadInBankRow().get(new BankRow().get('2000-01-03 12:00:00', '', '', '', '', '', '', '', '', '')),
                new ReadInBankRow().get(new BankRow().get('2000-01-04 13:00:00', '', '', '', '', '', '', '', '', '')),
                new ReadInBankRow().get(new BankRow().get('2000-01-05 14:00:00', '', '', '', '', '', '', '', '', ''))
            ]
        ];
        this.results.utils.bankMapper = new ExcelBankRowMapper(BankType.KH);
        this.wizard.stepsAt = 1;
    }

    ngAfterViewInit(): void {
        this.initWizardNavPositionAndHeight();
    }

    private initWizardNavPositionAndHeight(): void {
        let btnsHeight = this.btnsView.nativeElement.offsetHeight;
        this.wizardNavView.nativeElement.style.top = btnsHeight + "px";
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

    output_secondStepAlertChange($output: StepAlert[]): void {
        this.wizard.currentStep.stepAlerts = $output;
    }
}
