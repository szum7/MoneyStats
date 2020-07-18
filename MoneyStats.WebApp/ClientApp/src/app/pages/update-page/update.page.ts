import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef, AfterContentInit, AfterViewInit } from '@angular/core';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { ReadBankRowForDbCompare } from 'src/app/models/component-models/read-bank-row-for-db-compare';
import { BankRow } from 'src/app/models/service-models/bank-row.model';
import { BankType } from 'src/app/models/service-models/bank-type.enum';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { GeneratedTransaction } from 'src/app/models/service-models/generated-transaction.model';
import { GeneratedTransactionService } from 'src/app/services/generated-transaction-service/generated-transaction.service';

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

export abstract class WizardStep {

    public title: string;
    public stepAlerts: StepAlert[];

    public get isProgressable(): boolean {
        if (!this.stepAlerts) {
            return true;
        }
        if (this.stepAlerts.some(x => x.type == StepAlertType.Criteria)) {
            return false;
        }
        return true;
    }

    constructor(title: string) {
        this.title = title;
        this.stepAlerts = [];
    }

    abstract getOutput(): any;
    abstract setInput($input: any): void;
}

export class ImportFilesStep extends WizardStep {

    $output: ReadInBankRow[][];

    constructor() {
        super("Step 1 - Import files");
    }

    setInput($input: any): void {
        throw new Error("The first step has no input!");
    }

    getOutput(): ReadInBankRow[][] {
        return this.$output;
    }
}

export class ManageReadFilesStep extends WizardStep {

    $input: ReadInBankRow[];
    $output: ReadBankRowForDbCompare[];

    constructor() {
        super("Step 2 - Manage read files");
    }

    setInput($input: ReadInBankRow[]): void {
        this.$input = $input;
    }

    getOutput(): ReadBankRowForDbCompare[] {
        let ret: ReadBankRowForDbCompare[] = [];

        for (let i = 0; i < this.$input.length; i++) {
            const el = this.$input[i];
            if (!el.isExcluded) {

                let tr: ReadBankRowForDbCompare = new ReadBankRowForDbCompare();

                tr.uiId = el.uiId;
                tr.bankRow = el.bankRow;

                ret.push(tr);
            }
        }
        return ret;
    }
}

export class CompareWithDatabaseStep extends WizardStep {

    $input: ReadBankRowForDbCompare[];
    $output: BankRow[];

    constructor() {
        super("Step 3 - Compare with database");
    }

    setInput($input: ReadBankRowForDbCompare[]): void {
        this.$input = $input;
    }

    getOutput(): BankRow[] {
        let ret: BankRow[] = [];
        for (let i = 0; i < this.$input.length; i++) {
            if (!this.$input[i].isExcluded) {
                ret.push(this.$input[i].bankRow);
            }
        }
        return ret;
    }
}

export class EvalTransactionsStep extends WizardStep {

    $input: BankRow[];
    $output: GeneratedTransaction[];

    constructor() {
        super("Step 4 - Create transactions");
    }

    setInput($input: any): void {
        this.$input = $input;
    }

    getOutput(): GeneratedTransaction[] {
        return this.$output;
    }
}

export class Wizard {

    stepsAt: number;
    wizardSteps: WizardStep[];
    currentStep: WizardStep;

    constructor() {
    }

    public next(): void {
        if (this.stepsAt >= this.wizardSteps.length - 1)
            return;

        if (!this.currentStep.isProgressable)
            return;

        this.stepsAt++;
        this.currentStep = this.wizardSteps[this.stepsAt];
    }

    public previous(): boolean {
        if (this.stepsAt <= 0)
            return false;

        this.stepsAt--;
        this.currentStep = this.wizardSteps[this.stepsAt];

        return true;
    }
}

export class UpdateWizard extends Wizard {

    public utils: UpdateResultsUtilities;

    constructor() {
        super();

        this.wizardSteps = [
            new ImportFilesStep(),
            new ManageReadFilesStep(),
            new CompareWithDatabaseStep(),
            new EvalTransactionsStep()
        ];

        this.stepsAt = 0;
        this.currentStep = this.wizardSteps[this.stepsAt];

        this.utils = new UpdateResultsUtilities();
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
    encapsulation: ViewEncapsulation.None // TODO add comment why this is needed (?)
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

    output_firstStep($output: { isValid: boolean, matrix: ReadInBankRow[][], mapper: ExcelBankRowMapper, alerts: [] }): void {
        if (!$output.isValid){
            this.wizard.currentStep.stepAlerts = $output.alerts;
            return;
        }

        //...
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
