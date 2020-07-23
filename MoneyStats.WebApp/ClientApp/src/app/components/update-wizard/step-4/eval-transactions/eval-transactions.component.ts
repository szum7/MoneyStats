import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ReadBankRowForDbCompare } from 'src/app/models/component-models/read-bank-row-for-db-compare';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { BankRow } from 'src/app/models/service-models/bank-row.model';
import { StepAlert } from "src/app/models/component-models/step-alert.model";
import { Rule } from 'src/app/models/service-models/rule.model';
import { RuleService } from 'src/app/services/rule-service/rule.service';
import { GeneratedTransactionService } from 'src/app/services/generated-transaction-service/generated-transaction.service';
import { GeneratedTransaction } from 'src/app/models/service-models/generated-transaction.model';
import { Common } from 'src/app/utilities/common.static';
import { TableRow, TableRowAttribute } from 'src/app/models/component-models/read-in-bank-row';
import { StaticMessages } from 'src/app/utilities/input-messages.static';

export class TagColorer {
  
  private colors: string[];
  private colorCounter: number;

  constructor() {
    this.colors = ["#7ea4e0", "#ace09b", "#e0c99b"];
    //this.colors = ["#e8e8e8", "#d6d6d6"];
    this.colorCounter = 0;
  }

  getColor(): string {
    if (this.colorCounter > this.colors.length - 1) {
      this.colorCounter = 0;
    }
    return this.colors[this.colorCounter++];
  }
}

export class UsedRule {
  isExcluded: boolean;
  value: Rule;

  constructor(value: Rule) {
    this.value = value;
    this.isExcluded = false;
  }

  public toggleExclusion(): void {
    this.isExcluded = !this.isExcluded;
  }
}

export class IsModifiedAttribute extends TableRowAttribute {
  constructor() {
    super(StaticMessages.ROW_IS_MODIFIED);
  }
}

export class UsedGeneratedTransaction extends TableRow {

  value: GeneratedTransaction;
  isModifiedAttr: IsModifiedAttribute; // TODO create (change) events to wire this alert in

  get bankRow(): BankRow {
    return this.value != null ? this.value.bankRowReference : null;
  }

  get bankRows(): BankRow[] {
    return this.value != null ? this.value.aggregatedBankRowReferences : null;
  }

  get hasAnActiveAlert(): boolean {
    if (this.isExcludedAttr.value || this.isModifiedAttr.value) {
      return true;
    }
    return false;
  }

  constructor(value: GeneratedTransaction) {
    super();
    this.value = value;
    this.isModifiedAttr = new IsModifiedAttribute();
  }
}

@Component({
  selector: 'app-eval-transactions-component',
  templateUrl: './eval-transactions.component.html',
  styleUrls: ['./eval-transactions.component.scss']
})
export class EvalTransactionsComponent implements OnInit {

  @Input() params: BankRow[];
  @Output() nextStepChange = new EventEmitter<UsedGeneratedTransaction[]>();
  @Output() nextStepAlertsChange = new EventEmitter<string[]>();

  public get bankRows(): BankRow[] { return this.params; }

  public rules: UsedRule[];
  public transactions: UsedGeneratedTransaction[];

  private tagColorer: TagColorer;

  constructor(
    private loadingScreen: LoadingScreenService,
    private ruleService: RuleService,
    private generatedTransactionService: GeneratedTransactionService) {

    this.rules = [];
    this.transactions = [];
    this.tagColorer = new TagColorer();

    // 1. get rules
    // 2. run the program on the shown BankRows
    // 3. get the GeneratedTransactions
    // 4. edit the transactions if needed
    // 5. save transactions
  }

  ngOnInit() {
    this.getDataProgram();
  }

  private getDataProgram(): void {
    let self = this;

    self.getRules(function (rules: Rule[]) {
      self.rules = self.getUsedRulesFromRules(rules);
    })
  }

  private getUsedRulesFromRules(rules: Rule[]): UsedRule[] {
    let ur: UsedRule[] = [];
    for (let i = 0; i < rules.length; i++) {
      ur.push(new UsedRule(rules[i]));
    }
    return ur;
  }

  private getRulesFromUsedRules(ur: UsedRule[]): Rule[] {
    let rules: Rule[] = [];
    for (let i = 0; i < ur.length; i++) {
      if (!ur[i].isExcluded) {
        rules.push(ur[i].value);
      }
    }
    return rules;
  }

  click_generatedTransactionsProgram(): void {
    let self = this;
    self.getGeneratedTransactions(self.getRulesFromUsedRules(self.rules), self.bankRows, function (response: GeneratedTransaction[]) {
      self.transactions = self.getUsedFromTransactions(response);
      self.emitOutput();
    });
  }

  private getUsedFromTransactions(from: GeneratedTransaction[]): UsedGeneratedTransaction[] {
    let ret: UsedGeneratedTransaction[] = [];
    for (let i = 0; i < from.length; i++) {
      const current: GeneratedTransaction = from[i];
      let item: UsedGeneratedTransaction = new UsedGeneratedTransaction(current);

      if (current.appliedRules.length > 0) {
        for (let j = 0; j < current.appliedRules.length; j++) {
          item.messages.push(current.appliedRules[j].fancyName);
        }
      }

      ret.push(item);
    }
    return ret;
  }

  private getRules(callback: (response: Rule[]) => void): void {
    this.ruleService.get().subscribe(response => {
      Common.ConsoleResponse("getRules", response);
      callback(response);
    }, error => {
      console.log(error);
    });
  }

  private getGeneratedTransactions(rules: Rule[], bankRows: BankRow[], callback: (response: GeneratedTransaction[]) => void): void {
    this.generatedTransactionService.getGenerated({
      rules: rules,
      bankRows: bankRows
    }).subscribe(response => {
      Common.ConsoleResponse("getGeneratedTransactions", response);
      callback(response);
    }, error => {
      console.log(error);
    });
  }

  private emitNextStepAlerts(): void {
    let alerts = [];

    if (this.bankRows.length === 0) {
      alerts.push(new StepAlert("Bankrow count is zero!").setToCriteria());
    }

    // TODO ...

    this.nextStepAlertsChange.emit(alerts);
  }

  private emitOutput(): void {
    this.nextStepChange.emit(this.transactions);
  }

  click_toggleDetails(row: UsedGeneratedTransaction): void {
    row.isDetailsOpen = !row.isDetailsOpen;
  }

  click_toggleRowExclusion(row: UsedRule): void {
    row.toggleExclusion();
  }

  click_switchDetailsMenu(row: UsedGeneratedTransaction, i: number): void {
    row.detailsMenuPageAt = i;
  }

  getBgColor(): string {
    return this.tagColorer.getColor();
  }

  /*
  sortBy_bankRows(arr: any[], property: string) {
    return arr.sort((a, b) => this.dateComparer(a.bankRow[property], b.bankRow[property]));
  }

  private dateComparer(a, b) {
    let d1 = (new Date(a)).getTime(), d2 = (new Date(b)).getTime();
    return d1 > d2 ? 1 : d1 === d2 ? 0 : -1;
  }
  */

}
