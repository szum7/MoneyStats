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

@Component({
  selector: 'app-eval-transactions-component',
  templateUrl: './eval-transactions.component.html',
  styleUrls: ['./eval-transactions.component.scss']
})
export class EvalTransactionsComponent implements OnInit {

  @Input() params: BankRow[];
  @Output() nextStepChange = new EventEmitter<GeneratedTransaction[]>();
  @Output() nextStepAlertsChange = new EventEmitter<string[]>();

  public get bankRows(): BankRow[] { return this.params; }

  public rules: UsedRule[];
  public generatedTransactions: GeneratedTransaction[];

  constructor(
    private loadingScreen: LoadingScreenService,
    private ruleService: RuleService,
    private generatedTransactionService: GeneratedTransactionService) {

    this.rules = [];

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
      self.generatedTransactions = response;
      self.emitOutput();
    });
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
    this.nextStepChange.emit(this.generatedTransactions);
  }

  sortBy_bankRows(arr: any[], property: string) {
    return arr.sort((a, b) => this.dateComparer(a.bankRow[property], b.bankRow[property]));
  }

  private dateComparer(a, b) {
    let d1 = (new Date(a)).getTime(), d2 = (new Date(b)).getTime();
    return d1 > d2 ? 1 : d1 === d2 ? 0 : -1;
  }

  click_toggleDetails(row: ReadBankRowForDbCompare): void {
    row.isDetailsOpen = !row.isDetailsOpen;
  }

  click_toggleRowExclusion(row: UsedRule): void {
    row.toggleExclusion();
  }

}
