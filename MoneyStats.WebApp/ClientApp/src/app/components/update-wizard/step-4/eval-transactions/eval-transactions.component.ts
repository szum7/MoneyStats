import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ReadBankRowForDbCompare } from 'src/app/models/component-models/read-bank-row-for-db-compare';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { BankRow } from 'src/app/models/service-models/bank-row.model';
import { StaticMessages } from 'src/app/utilities/input-messages.static';
import { StepAlert } from 'src/app/pages/update-page/update.page';
import { Transaction } from 'src/app/models/service-models/transaction.model';
import { RuleEvaluatorService } from 'src/app/services/rule-evaluator-service/rule-evaluator.service';
import { Rule } from 'src/app/models/service-models/rule.model';
import { RuleService } from 'src/app/services/rule-service/rule.service';

@Component({
  selector: 'app-eval-transactions-component',
  templateUrl: './eval-transactions.component.html',
  styleUrls: ['./eval-transactions.component.scss']
})
export class EvalTransactionsComponent implements OnInit {

  @Input() params: BankRow[];
  @Output() nextStepAlertsChange = new EventEmitter();

  public get bankRows(): BankRow[] { return this.params; }

  public rules: Rule[];

  constructor(
    private loadingScreen: LoadingScreenService,
    private ruleService: RuleService,
    private ruleEvaluatorService: RuleEvaluatorService) {
  }

  ngOnInit() {
    this.getDataProgram();
  }

  private getDataProgram(): void {
    let self = this;
    
    self.getRules(function(rules: Rule[]){
      self.rules = rules;
    })
  }

  evaluateProgram(): void {
    let self = this;

    self.getEvaluatedTransactions(self.rules, self.bankRows, function (response) {
      
    });
  }

  private getRules(callback: (response: Rule[]) => void): void {
    this.ruleService.get().subscribe(response => {
      console.log("=> getRules:");
      console.log(response);
      console.log("<=");
      callback(response);
    }, error => {
      console.error("Error: getRules");
      console.log(error);
    });
  }

  private getEvaluatedTransactions(rules: Rule[], bankRows: BankRow[], callback: (response: any[]/* TODO */) => void): void {
    this.ruleEvaluatorService.getEvaluatedTransactions(rules, bankRows).subscribe(response => {
      console.log("=> getEvaluatedTransactions:");
      console.log(response);
      console.log("<=");
      callback(response);
    }, error => {
      console.error("Error: getEvaluatedTransactions");
      console.log(error);
    });
  }

  private checkNextStepPossible(): void {
    let alerts = [];
    
    if (this.bankRows.length === 0) {
      alerts.push(new StepAlert("Bankrow count is zero!").setToCriteria());
    }

    // TODO ...

    this.nextStepAlertsChange.emit(alerts);
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

  click_toggleRowExclusion(row: ReadBankRowForDbCompare): void {
    row.toggleExclusion();
    
    this.checkNextStepPossible();
  }

}
