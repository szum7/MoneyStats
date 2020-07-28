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
import { Tag } from 'src/app/models/service-models/tag.model';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { BankType } from 'src/app/models/service-models/bank-type.enum';
import { TagService } from 'src/app/services/tag-service/tag.service';

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
  originalValue: GeneratedTransaction;
  isModifiedAttr: IsModifiedAttribute; // TODO create (change) events to wire this alert in

  tagStr: string; // for autocomplete dropdown input
  tagResults: Tag[]; // for autocomplete dropdown tag results

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
    this.copyProperties(this.value, this.originalValue);
    this.tagResults = [];
  }

  private copyProperties(from: GeneratedTransaction, to: GeneratedTransaction): void {
    to = new GeneratedTransaction();
    to.date = from.date;
    to.title = from.title;
    to.description = from.description;
    to.sum = from.sum;
    to.tags = [];
    for (let i = 0; i < from.tags.length; i++) {
      // No need to copy the tags's properties, it can stay as a reference.
      to.tags.push(from.tags[i]);
    }
  }

  resetToOriginal(): void {
    this.copyProperties(this.originalValue, this.value);
  }
}

export class TagDropdown {
  tags: Tag[];

  constructor(private tagService: TagService) {
    this.tags = [];

    this.init();
  }

  getResults(str: string, excludes: Tag[]): Tag[] {
    if (this.tags.length === 0) {
      return [];
    }

    let ret: Tag[] = [];
    str = str.toLowerCase();

    let check: (str: string, tag: Tag) => boolean = null;
    if (!isNaN(Number(str))) { // id
      check = (str: string, tag: Tag) => Number(tag.id) === Number(str);
    } else { // title
      check = (str: string, tag: Tag) => tag.title.toLowerCase().includes(str);
    }

    this.tags.forEach(tag => {
      if (check(str, tag)) {
        if (!Common.containsObjectOnId(tag, excludes)) {
          ret.push(tag);
        }
      }
    });

    return ret;
  }

  private init(): void {
    let self = this;
    this.getTags(function (r) {
      self.tags = r;
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
}

@Component({
  selector: 'app-eval-transactions-component',
  templateUrl: './eval-transactions.component.html',
  styleUrls: ['./eval-transactions.component.scss']
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
  tagDropdown: TagDropdown;

  constructor(
    private loadingScreen: LoadingScreenService,
    private ruleService: RuleService,
    private generatedTransactionService: GeneratedTransactionService,
    private tagService: TagService) {

    this.rules = [];
    this.transactions = [];
    this.tagColorer = new TagColorer();
    this.tagDropdown = new TagDropdown(tagService);

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

  change_getTags(row: UsedGeneratedTransaction): void {
    // IMPROVE add a delay, don't search for every change
    let str = row.tagStr;
    if (isNaN(Number(str)) && str.length <= 1) {
      row.tagResults = [];
      return;
    }
    row.tagResults = this.tagDropdown.getResults(str, row.value.tags);
  }

  click_selectTag(row: UsedGeneratedTransaction, tag: Tag): void {
    if (Common.containsObjectOnId(tag, row.value.tags))
      return;

    row.value.tags.push(tag);

    const index = row.tagResults.indexOf(tag);
    if (index > -1) {
      row.tagResults.splice(index, 1);
    }
  }

  click_tagsResultReset(row: UsedGeneratedTransaction): void {
    row.tagResults = [];
  }

  rightClick_removeTag(row: UsedGeneratedTransaction, tag: Tag): boolean {
    const index = row.value.tags.indexOf(tag);
    if (index > -1) {
      row.value.tags.splice(index, 1);
    }
    return false; // Avoid default browser action from the event
  }
}
