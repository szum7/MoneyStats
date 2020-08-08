import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { BankRow } from 'src/app/models/service-models/bank-row.model';
import { StepAlert } from "src/app/models/component-models/step-alert.model";
import { Rule } from 'src/app/models/service-models/rule.model';
import { RuleService } from 'src/app/services/rule.service';
import { GeneratedTransactionService } from 'src/app/services/generated-transaction.service';
import { GeneratedTransaction } from 'src/app/models/service-models/generated-transaction.model';
import { Common } from 'src/app/utilities/common.static';
import { Tag } from 'src/app/models/service-models/tag.model';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { TagService } from 'src/app/services/tag.service';
import { TagColorer } from './tag-colorer';
import { UsedRule } from './used-rule';
import { UsedGeneratedTransaction } from './used-generated-transaction';

@Component({
  selector: 'app-eval-transactions-component',
  templateUrl: './eval-transactions.component.html',
  styleUrls: ['./eval-transactions.component.scss'],
  styles: [
      `
      :host ::ng-deep .content .dropdown-component .dropdown input {
          border: none;
          background: #e5e5e5;
          height: 20px;
          vertical-align: top;
          font-size: 12px;
          width: 80px;
          padding: 0 10px;
      }
      `
  ]
})
export class EvalTransactionsComponent implements OnInit {

  @Input() params: BankRow[];
  @Input() mapper: ExcelBankRowMapper;
  @Output() nextStepChange = new EventEmitter<UsedGeneratedTransaction[]>();
  @Output() nextStepAlertsChange = new EventEmitter<string[]>();

  public get bankRows(): BankRow[] { return this.params; }

  public rules: UsedRule[];
  public transactions: UsedGeneratedTransaction[];

  private tagColorer: TagColorer;
  tags: Tag[];

  constructor(
    private loadingScreen: LoadingScreenService,
    private ruleService: RuleService,
    private generatedTransactionService: GeneratedTransactionService,
    private tagService: TagService) {

    this.rules = [];
    this.transactions = [];
    this.tagColorer = new TagColorer();

    // 1. get rules
    // 2. run the program on the shown BankRows
    // 3. get the GeneratedTransactions
    // 4. edit the transactions if needed
    // 5. save transactions
  }

  sout() {
    console.log(this.transactions);
  }

  ngOnInit() {
    this.getDataProgram();
  }

  private getDataProgram(): void {
    let self = this;

    self.getRules(function (rules: Rule[]) {
      self.rules = self.getUsedRulesFromRules(rules);
    });

    self.getTags(res => {
      self.tags = res;
    });
  }

  private getTags(callback: (response: Tag[]) => void): void {
    this.tagService.get().subscribe(r => {
      Common.ConsoleResponse("getTags", r);
      callback(r);
    }, e => {
      console.error(e);
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

  rightClick_removeTag(row: UsedGeneratedTransaction, tag: Tag): boolean {
    Common.removeFromArray(tag, row.value.tags);
    return false; // Avoid default browser action from the event
  }
}
