import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ReadBankRowForDbCompare } from 'src/app/models/component-models/read-bank-row-for-db-compare';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { BankRow } from 'src/app/models/service-models/bank-row.model';
import { StaticMessages } from 'src/app/utilities/input-messages.static';
import { StepAlert } from 'src/app/pages/update-page/update.page';
import { Transaction } from 'src/app/models/service-models/transaction.model';
import { Rule } from 'src/app/models/service-models/rule.model';
import { RuleService } from 'src/app/services/rule-service/rule.service';
import { GeneratedTransactionService } from 'src/app/services/generated-transaction-service/generated-transaction.service';
import { GeneratedTransaction } from 'src/app/models/service-models/generated-transaction.model';
import { GenericResponse } from 'src/app/models/service-models/generic-response.model';

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
  public generatedTransactions: GeneratedTransaction[];

  constructor(
    private loadingScreen: LoadingScreenService,
    private ruleService: RuleService,
    private generatedTransactionService: GeneratedTransactionService) {
  }

  ngOnInit() {
    this.getDataProgram();
  }

  private getDataProgram(): void {
    let self = this;

    self.getRules(function (rules: Rule[]) {
      self.rules = rules;
    })
  }

  click_generatedTransactionsProgram(): void {
    let self = this;
    self.getGeneratedTransactions(self.rules, self.bankRows, function (response: GeneratedTransaction[]) {
      self.generatedTransactions = response;
    });
  }

  click_saveTransactionsProgram(): void {
    let self = this;
    self.saveTransactions(self.generatedTransactions, function (response: GenericResponse) {
      if (!response.isError) {
        console.log(response.message);
      } else {
        console.error(response.message);
      }
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

  private getGeneratedTransactions(rules: Rule[], bankRows: BankRow[], callback: (response: GeneratedTransaction[]) => void): void {
    this.generatedTransactionService.getGenerated({
      rules: rules,
      bankRows: bankRows
    }).subscribe(response => {
      console.log("=> getGeneratedTransactions:");
      console.log(response);
      console.log("<=");
      callback(response);
    }, error => {
      console.error("Error: getGeneratedTransactions");
      console.log(error);
    });
  }

  private saveTransactions(generatedTransactions: GeneratedTransaction[], callback: (response: GenericResponse) => void): void {
    this.generatedTransactionService.save(generatedTransactions).subscribe(response => {
      console.log("=> getRules:");
      console.log(response);
      console.log("<=");
      callback(response);
    }, error => {
      console.error("Error: getRules");
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
