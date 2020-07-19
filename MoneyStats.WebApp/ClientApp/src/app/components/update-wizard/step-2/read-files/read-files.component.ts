import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { ReadInBankRowsMerger } from 'src/app/models/component-models/read-in-bank-rows-merger';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { StepAlert } from 'src/app/models/component-models/step-alert.model';

@Component({
  selector: 'app-read-files-component',
  templateUrl: './read-files.component.html',
  styleUrls: ['./read-files.component.scss']
})
export class ReadFilesComponent implements OnInit {

  @Input() params: ReadInBankRow[][];
  @Input() mapper: ExcelBankRowMapper;
  @Output() nextStepChange = new EventEmitter<ReadInBankRow[]>();
  @Output() nextStepAlertsChange = new EventEmitter<string[]>();

  readInBankRows: ReadInBankRow[];

  constructor(private loadingScreen: LoadingScreenService) {
    this.readInBankRows = [];
  }

  ngOnInit() {
    this.program(this.params);
  }

  isComponentInitable(): boolean {
    return this.params != null && this.mapper != null;
  }

  /// <summary>
  /// Searches through the files matrix and 
  /// alerts duplicate rows to the user, set 
  /// them for omition. Plus flattens the 
  /// matrix into an array.
  /// </summary>
  program(mappedExcelMatrix: ReadInBankRow[][]) {

    if (!mappedExcelMatrix || mappedExcelMatrix.length === 0) {
      console.error("Input matrix is empty!");
      return;
    }

    // TODO research and understand why this (loadingScreen part) isn't working!
    // Error: "Expression has changed after it was checked"
    // https://blog.angular-university.io/angular-debugging/
    //this.loadingScreen.start();

    new ReadInBankRowsMerger().searchForDuplicates(mappedExcelMatrix);
    this.readInBankRows = this.flattenReadInBankRows(mappedExcelMatrix);

    //this.loadingScreen.stop();

    this.emitNextStepAlerts();
    this.nextStepChange.emit(this.readInBankRows);
  }

  private flattenReadInBankRows(matrix: ReadInBankRow[][]): ReadInBankRow[] {
    let ret: ReadInBankRow[] = [];
    let id: number = 0;
    for (let i = 0; i < matrix.length; i++) {
      let m = matrix[i];
      for (let j = 0; j < m.length; j++) {
        m[j].uiId = ++id;
        ret.push(m[j]);
      }
    }
    return ret;
  }

  private flattenMatrix(matrix: any[][]): any[] {
    return [].concat.apply([], matrix);
  }

  sortBy_bankRows(arr: Array<any>, property: string) {
    return arr.sort((a, b) => this.dateComparer(a.bankRow[property], b.bankRow[property]));
  }

  private dateComparer(a, b) {
    let d1 = (new Date(a)).getTime(), d2 = (new Date(b)).getTime();
    return d1 > d2 ? 1 : d1 === d2 ? 0 : -1;
  }

  click_toggleRowExclusion(row: ReadInBankRow): void {
    row.toggleExclusion();
    this.emitNextStepAlerts();
    this.nextStepChange.emit(this.readInBankRows);
  }

  click_toggleDetails(row: ReadInBankRow): void {
    row.isDetailsOpen = !row.isDetailsOpen;
  }

  click_switchDetailsMenu(row: ReadInBankRow, i: number): void {
    row.detailsMenuPageAt = i;
  }

  private emitNextStepAlerts(): void {
    let alerts = [];

    if (this.readInBankRows.length === 0) {
      alerts.push(new StepAlert("Bankrow count is zero!").setToCriteria());
    }
    if (!this.readInBankRows.some(x => !x.isExcluded)) {
      alerts.push(new StepAlert("All bankrows are excluded!").setToCriteria());
    }

    this.nextStepAlertsChange.emit(alerts);
  }

  // TODO https://stackblitz.com/edit/flex-table-column-resize
}
