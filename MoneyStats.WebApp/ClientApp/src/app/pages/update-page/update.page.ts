import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef, AfterContentInit, AfterViewInit } from '@angular/core';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { ReadBankRowForDbCompare } from 'src/app/models/component-models/read-bank-row-for-db-compare';
import { BankRow } from 'src/app/models/service-models/bank-row.model';
import { BankType } from 'src/app/models/service-models/bank-type.enum';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { GeneratedTransaction } from 'src/app/models/service-models/generated-transaction.model';
import { GeneratedTransactionService } from 'src/app/services/generated-transaction-service/generated-transaction.service';
import { GenericResponse } from 'src/app/models/service-models/generic-response.model';
import { StepAlertType } from '../../models/component-models/step-alert-type.enum';
import { StepAlert } from '../../models/component-models/step-alert.model';
import { WizardStep } from '../../models/component-models/wizard-step.model';
import { UpdateResultsUtilities } from '../../models/component-models/update-results-utilities.model';
import { BankRowService } from 'src/app/services/bank-row-service/bank-row.service';
import { Common } from 'src/app/utilities/common.static';
import { RuleService } from 'src/app/services/rule-service/rule.service';
import { UsedGeneratedTransaction } from 'src/app/components/update-wizard/step-4/eval-transactions/eval-transactions.component';

export class ImportFilesStep extends WizardStep {

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

    $input: ReadInBankRow[][];

    constructor() {
        super("Step 2 - Manage read files");
    }

    setInput($input: ReadInBankRow[][]): void {
        this.$input = $input;
    }

    getOutput(): ReadBankRowForDbCompare[] {

        let ret: ReadBankRowForDbCompare[] = [];
        let cast: ReadBankRowForDbCompare[] = this.$output;

        for (let i = 0; i < cast.length; i++) {
            const el = cast[i];
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

    constructor() {
        super("Step 3 - Compare with database");
    }

    setInput($input: ReadBankRowForDbCompare[]): void {
        this.$input = $input;
    }

    getOutput(): BankRow[] {

        let ret: BankRow[] = [];
        let cast: ReadBankRowForDbCompare[] = this.$output;

        for (let i = 0; i < cast.length; i++) {
            if (!cast[i].isExcluded) {
                ret.push(cast[i].bankRow);
            }
        }
        return ret;
    }
}

export class EvalTransactionsStep extends WizardStep {

    $input: BankRow[];

    constructor() {
        super("Step 4 - Create transactions");
    }

    setInput($input: any): void {
        this.$input = $input;
    }

    getOutput(): GeneratedTransaction[] {

        let ret: GeneratedTransaction[] = [];
        let cast: UsedGeneratedTransaction[] = this.$output;

        for (let i = 0; i < cast.length; i++) {
            if (!cast[i].isExcluded){
                ret.push(cast[i].value);
            }
        }
        return ret;
    }
}

export class EmptyStep extends WizardStep {

    constructor() {
        super("");
    }

    setInput($input: any): void {
    }

    getOutput(): GeneratedTransaction[] {
        return null;
    }
}

export class UpdateWizard {

    public stepsAt: number;
    public wizardSteps: WizardStep[];
    public utils: UpdateResultsUtilities;

    private generatedTransactionService: GeneratedTransactionService;
    private bankRowService: BankRowService;

    public get currentStep(): WizardStep {
        if (this.stepsAt < 0) {
            return new EmptyStep();
        }
        return this.wizardSteps[this.stepsAt];
    }

    public get nextStep(): WizardStep {
        if (this.stepsAt + 1 <= this.wizardSteps.length - 1) {
            return this.wizardSteps[this.stepsAt + 1];
        }
        return null;
    }

    constructor(bankRowService: BankRowService, generatedTransactionService: GeneratedTransactionService) {

        this.generatedTransactionService = generatedTransactionService;
        this.bankRowService = bankRowService;

        this.wizardSteps = [
            new ImportFilesStep(),
            new ManageReadFilesStep(),
            new CompareWithDatabaseStep(),
            new EvalTransactionsStep()
        ];

        this.stepsAt = -1;

        this.utils = new UpdateResultsUtilities();
    }

    public setFirstStep(): void {
        this.stepsAt = 0;
    }

    public next(): void {
        if (this.stepsAt >= this.wizardSteps.length - 1)
            return;

        if (!this.currentStep.isProgressable)
            return;

        let self = this;
        this.beforeNextStepActions(() => {
            self.stepsAt++;
        });
    }

    public previous(): boolean {
        if (this.stepsAt <= 0)
            return false;

        this.stepsAt--;

        return true;
    }

    private beforeNextStepActions(callback: () => void): void { // TODO make the outputs strongly types somehow (?)
        let self = this;
        switch (this.stepsAt) {
            case 0:
                let o: { matrix: ReadInBankRow[][], mapper: ExcelBankRowMapper } = this.currentStep.getOutput();
                this.utils.bankMapper = o.mapper;
                this.nextStep.setInput(o.matrix);
                callback();
                break;
            case 1:
                this.nextStep.setInput(this.currentStep.getOutput());
                callback();
                break;
            case 2:
                let cast: BankRow[] = this.currentStep.getOutput();
                self.saveBankRows(cast, function (response) {
                    cast = response;
                    self.nextStep.setInput(response);
                    callback();
                });
                break;
            case 3:
                self.saveTransactions(self.currentStep.getOutput(), function (response: GenericResponse) {
                    if (response.isError) {
                        console.error(response.message);
                    } else {
                        console.log(response.message);
                        // NEXT this is the end, transactions are saved. Some endscreen (?)
                        // NEXT test and remove unused, old code
                    }
                });
                break;
            default:
                break;
        }
    }

    private saveTransactions(generatedTransactions: GeneratedTransaction[], callback: (response: GenericResponse) => void): void {
        this.generatedTransactionService.save(generatedTransactions).subscribe(response => {
            Common.ConsoleResponse("saveTransactions", response);
            callback(response);
        }, error => {
            console.log(error);
        });
    }

    private saveBankRows(bankRows: BankRow[], callback: (bankRows: BankRow[]) => void): void {
        this.bankRowService.save(bankRows).subscribe(response => {
            Common.ConsoleResponse("saveBankRows", response);
            callback(response);
        }, error => {
            console.error(error);
        })
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

    constructor(
        private loadingScreen: LoadingScreenService,
        private bankRowService: BankRowService,
        private generatedTransactionService: GeneratedTransactionService,
        private ruleService: RuleService) {

        this.wizard = new UpdateWizard(bankRowService, generatedTransactionService);
        this.isTooltipsDisabled = true;
    }

    ngAfterViewInit(): void {
        this.initWizardNavPositionAndHeight();
    }

    ngOnInit(): void {
        this.wizard.setFirstStep();

        //this.testLastStep();
    }

    test() { // TEST
        (this.wizard.wizardSteps[1] as ManageReadFilesStep).$input = [
            [
                // Already in db
                new ReadInBankRow().get(new BankRow().get(new Date(1999, 0, 1), null, null, null, null, null, null, 2000, null, '')),
                new ReadInBankRow().get(new BankRow().get(new Date(2010, 9, 10), 'bankTransactionId', 'type', 'account', 'accountName', 'partnerAccount', 'partnerName', 1, 'currency', 'message')),
                // Not yet in db
                new ReadInBankRow().get(new BankRow().get(new Date(2013, 0, 1), '', '', '', '', '', '', 0, '', 'test1')),
                new ReadInBankRow().get(new BankRow().get(new Date(2014, 0, 1), '', '', '', '', '', '', 0, '', 'test2'))
            ]
        ];
        this.wizard.utils.bankMapper = new ExcelBankRowMapper(BankType.KH);
        this.wizard.stepsAt = 1;
    }

    private testLastStep(): void {
        this.wizard.utils.bankMapper = new ExcelBankRowMapper(BankType.KH);

        let self = this;
        this.bankRowService.get().subscribe(r => {
            Common.ConsoleResponse("testLastStep BankRows:", r);
            self.wizard.wizardSteps[3].setInput(r);
            self.wizard.stepsAt = 3;
        }, e => {
            console.log(e);
        });
    }

    private initWizardNavPositionAndHeight(): void {
        let btnsHeight = this.btnsView.nativeElement.offsetHeight;
        this.wizardNavView.nativeElement.style.top = btnsHeight + "px";
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

    output_change($output: any): void {
        this.wizard.currentStep.$output = $output;
    }

    output_stepAlertChange($output: StepAlert[]): void {
        this.wizard.currentStep.stepAlerts = $output;
    }
}
