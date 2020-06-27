import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { ReadInBankRowsMerger } from 'src/app/models/component-models/read-in-bank-rows-merger';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { PropertyMapRow } from 'src/app/models/component-models/property-map-row';
import { ReadBankRowForInsertion } from 'src/app/models/component-models/read-bank-row-for-insertion';

@Component({
  selector: 'app-read-files-component',
  templateUrl: './read-files.component.html',
  styleUrls: ['./read-files.component.scss']
})
export class ReadFilesComponent implements OnInit {

  @Input() params: ReadInBankRow[][];
  @Input() mapper: ExcelBankRowMapper;
  @Output() nextStepChange = new EventEmitter();

  readInBankRows: Array<ReadInBankRow>;

  constructor(private loadingScreen: LoadingScreenService) {
    this.readInBankRows = [];
  }

  ngOnInit() {
    this.program(this.params);
  }

  /// <summary>
  /// Searches through the files matrix and 
  /// alerts duplicate rows to the user, set 
  /// them for omition. Plus flattens the 
  /// matrix into an array.
  /// </summary>
  program(mappedExcelMatrix: Array<Array<ReadInBankRow>>) {

    if (mappedExcelMatrix.length === 0) {
      console.error("Input matrix is empty!");
      return;
    }

    this.loadingScreen.start();

    new ReadInBankRowsMerger().searchForDuplicates(mappedExcelMatrix);
    this.readInBankRows = this.flattenMatrix(mappedExcelMatrix);

    this.loadingScreen.stop();

    this.emitOutput(this.readInBankRows);
  }

  private flattenMatrix(matrix: any[][]): any[] {
    return [].concat.apply([], matrix);
  }

  click_toggleColumnVisibility(propertyMap: PropertyMapRow): void {
    propertyMap.isOpen = !propertyMap.isOpen;
  }
  
  sortBy_bankRows(arr: Array<any>, property: string) {
    return arr.sort((a, b) => this.dateComparer(a.bankRow[property], b.bankRow[property]));
  }
  
  private dateComparer(a, b) {
    let d1 = (new Date(a)).getTime(), d2 = (new Date(b)).getTime();
    return d1 > d2 ? 1 : d1 === d2 ? 0 : -1;
  }
  
  click_toggleRowExclusion(row: ReadInBankRow): void {
    row.isExcluded = !row.isExcluded;
    this.emitOutput(this.readInBankRows); // TODO optimaze this, don't run the for iteration on the whole array every time
  }

  private emitOutput(bankRows: ReadInBankRow[]): void {
    let output: ReadBankRowForInsertion[] = [];

    for (let i = 0; i < this.readInBankRows.length; i++) {
        const el = this.readInBankRows[i];
        if (!el.isExcluded) {
            let tr: ReadBankRowForInsertion = new ReadBankRowForInsertion();
            tr.bankRow = el.bankRow;
            output.push(tr);
        }
    }
    this.nextStepChange.emit(output);
  }
}
