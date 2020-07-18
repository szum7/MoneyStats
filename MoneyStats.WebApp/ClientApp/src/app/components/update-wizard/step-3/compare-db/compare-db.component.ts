import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ReadBankRowForDbCompare } from 'src/app/models/component-models/read-bank-row-for-db-compare';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { BankRowService } from 'src/app/services/bank-row-service/bank-row.service';
import { BankRow } from 'src/app/models/service-models/bank-row.model';
import { StaticMessages } from 'src/app/utilities/input-messages.static';
import { StepAlert } from 'src/app/pages/update-page/update.page';

@Component({
  selector: 'app-compare-db-component',
  templateUrl: './compare-db.component.html',
  styleUrls: ['./compare-db.component.scss']
})
export class CompareDbComponent implements OnInit {

  @Input() params: ReadBankRowForDbCompare[];
  @Input() mapper: ExcelBankRowMapper;
  @Output() nextStepChange = new EventEmitter<ReadBankRowForDbCompare[]>();
  @Output() nextStepAlertsChange = new EventEmitter<string[]>();

  bb: BankRow[]; // TODO create this from params

  public get bankRows(): ReadBankRowForDbCompare[] { return this.params; }

  constructor(
    private loadingScreen: LoadingScreenService,
    private bankRowService: BankRowService) {
  }

  ngOnInit() {
    this.program();
  }

  private program(): void {
    let self = this;
    //self.loadingScreen.start(); // TODO ExpressionChangedAfterItHasBeenCheckedError error
    self.getBankRowsFromDb(function (dbList) {
      self.compareDbToFileRows(dbList);
      //self.loadingScreen.stop();
      console.log("Compare finished");

      self.nextStepChange.emit(self.bankRows);
      self.emitNextStepAlerts();
    });
  }

  private getBankRowsFromDb(callback: (response: Array<BankRow>) => void): void {
    this.bankRowService.get().subscribe(response => {
      console.log("=> getBankRowsFromDb:");
      console.log(response);
      console.log("<=");
      callback(response);
    }, error => {
      console.error("Couldn't get bank rows from database!");
      console.log(error);
    });
  }

  private compareDbToFileRows(dbList: Array<BankRow>): void {
    for (let i = 0; i < dbList.length; i++) {
      let dbRow: BankRow = dbList[i];
      for (let j = 0; j < this.bankRows.length; j++) {
        let fileRow: ReadBankRowForDbCompare = this.bankRows[j];

        if (dbRow.getContentId() === fileRow.bankRow.getContentId()) {
          fileRow.messages.push(StaticMessages.MATCHING_READ_BANKROW_WITH_DB);
          fileRow.setToExclude();
        }

      }
    }
  }

  private emitNextStepAlerts(): void {
    let alerts = [];

    if (this.bankRows.length === 0) {
      alerts.push(new StepAlert("Bankrow count is zero!").setToCriteria());
    }
    if (!this.bankRows.some(x => !x.isExcluded)) {
      alerts.push(new StepAlert("All bankrows are excluded!").setToCriteria());
    }

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

    this.nextStepChange.emit(this.bankRows);
    this.emitNextStepAlerts();
  }

  click_saveBankRows(): void {
    let self = this;
    self.saveBankRows(self.bb, function (response: BankRow[]) {
      self.bb = response;
    });
  }

  private saveBankRows(bankRows: BankRow[], callback: (bankRows: BankRow[]) => void): void {
    this.bankRowService.save(bankRows).subscribe(response => {
      console.log(response);
      callback(response);
    }, error => {
      console.error(error);
    })
  }
}
