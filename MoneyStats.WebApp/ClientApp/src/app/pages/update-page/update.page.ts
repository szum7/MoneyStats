import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef, AfterContentInit, AfterViewInit } from '@angular/core';
import { WizardStep } from 'src/app/models/component-models/wizard-step';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { ReadBankRowForDbCompare } from 'src/app/models/component-models/read-bank-row-for-db-compare';
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
            new WizardStep("Step 1 - Import files", null),
            new WizardStep("Step 2 - Manage read files", null),
            new WizardStep("Step 3 - Compare with database", null),
            new WizardStep("Step 4 - Create transactions", null)
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
    // ...
}

export class UpdateResults {
    firstResult: ReadInBankRow[][];
    secondResult: ReadBankRowForDbCompare[];
    thirdResult: BankRow[];
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

        //this.test();
    }

    test() { // TEST
        this.results.firstResult = [
            [
                // Already in db
                new ReadInBankRow().get(new BankRow().get(new Date(1999, 0, 1), null, null, null, null, null, null, 2000, null, '')),
                new ReadInBankRow().get(new BankRow().get(new Date(2010, 9, 10), 'bankTransactionId', 'type', 'account', 'accountName', 'partnerAccount', 'partnerName', 1, 'currency', 'message')),
                // Not yet in db
                new ReadInBankRow().get(new BankRow().get(new Date(2013, 0, 1), '', '', '', '', '', '', 0, '', 'test1')),
                new ReadInBankRow().get(new BankRow().get(new Date(2014, 0, 1), '', '', '', '', '', '', 0, '', 'test2'))
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
        this.results.firstResult = $output.matrix;
        this.results.utils.bankMapper = $output.mapper;
    }

    output_secondStep($output: ReadBankRowForDbCompare[]): void {
        this.results.secondResult = $output;
    }

    output_thirdStep($output: BankRow[]): void {
        this.results.thirdResult = $output;
    }

    output_stepAlertChange($output: StepAlert[]): void {
        this.wizard.currentStep.stepAlerts = $output;
    }
}
