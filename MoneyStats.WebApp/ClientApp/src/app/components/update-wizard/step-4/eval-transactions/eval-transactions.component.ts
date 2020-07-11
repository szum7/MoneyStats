import { Component, OnInit, Input } from '@angular/core';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { PropertyMapRow } from 'src/app/models/component-models/property-map-row';
import { ReadBankRowForDbCompare } from 'src/app/models/component-models/read-bank-row-for-db-compare';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { BankRow } from 'src/app/models/service-models/bank-row.model';
import { BankRowService } from 'src/app/services/bank-row-service/bank-row.service';
import { StaticMessages } from 'src/app/utilities/input-messages.static';

@Component({
  selector: 'app-eval-transactions-component',
  templateUrl: './eval-transactions.component.html',
  styleUrls: ['./eval-transactions.component.scss']
})
export class EvalTransactionsComponent implements OnInit {

  @Input() params: ReadBankRowForDbCompare[];
  @Input() mapper: ExcelBankRowMapper;
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
    self.loadingScreen.start();
    self.getBankRowsFromDb(function (dbList) {
      self.compareDbToFileRows(dbList);
      self.loadingScreen.stop();
      console.log("Compare finished");
    });
  }

  private getBankRowsFromDb(callback: (response: Array<BankRow>) => void): void {
    this.bankRowService.get().subscribe(response => {
      console.log(response);
      callback(response);
    }, error => {
      console.error("Couldn't get bank rows from database!");
      console.log(error);
    })
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

  sortBy_bankRows(arr: any[], property: string) {
    return arr.sort((a, b) => this.dateComparer(a.bankRow[property], b.bankRow[property]));
  }

  private dateComparer(a, b) {
    let d1 = (new Date(a)).getTime(), d2 = (new Date(b)).getTime();
    return d1 > d2 ? 1 : d1 === d2 ? 0 : -1;
  }

  click_toggleColumnVisibility(propertyMap: PropertyMapRow): void {
    propertyMap.isOpen = !propertyMap.isOpen;
  }

  click_toggleRowExclusion(row: ReadBankRowForDbCompare): void {
    row.toggleExclusion();
  }

}
