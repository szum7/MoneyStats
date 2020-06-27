import { Component, OnInit, Input } from '@angular/core';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { PropertyMapRow } from 'src/app/models/component-models/property-map-row';
import { ReadBankRowForInsertion } from 'src/app/models/component-models/read-bank-row-for-insertion';

@Component({
  selector: 'app-eval-transactions-component',
  templateUrl: './eval-transactions.component.html',
  styleUrls: ['./eval-transactions.component.scss']
})
export class EvalTransactionsComponent implements OnInit {

  @Input() params: ReadBankRowForInsertion[];
  @Input() mapper: ExcelBankRowMapper;
  public get bankRows(): ReadBankRowForInsertion[] { return this.params; }

  constructor() { }

  ngOnInit() {
    this.program();
  }

  private program(): void {
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

  click_toggleRowExclusion(row: ReadBankRowForInsertion): void {
      row.isExcluded = !row.isExcluded;
  }

}
