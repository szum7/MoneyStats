import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ReadInBankRowsMerger } from '../../models/component-models/read-in-bank-rows-merger';
import { ReadInBankRow } from '../../models/component-models/read-in-bank-row';
import { map } from 'rxjs/operators';
import { ExcelReader } from '../../models/component-models/excel-reader';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { ExcelBankRowMapper } from '../../models/component-models/excel-bank-row-mapper';
import { PropertyMapRow } from '../../models/component-models/property-map-row';
import { BankRow } from 'src/app/models/service-models/bank-row.model';
import { ReadBankRowForInsertion } from '../../models/component-models/read-bank-row-for-insertion';
import { BankType } from 'src/app/models/service-models/bank-type.enum';
import { FileFileResult } from 'src/app/models/component-models/file-file-result';

@Component({
  selector: 'app-file-file-comparer-component',
  templateUrl: './file-file-comparer.component.html',
  styleUrls: ['./file-file-comparer.component.scss']
})
export class FileFileComparerComponent implements OnInit {

    // New bank rows page
    // 1. file merge stage (exclude duplicates between xml rows from multiple files)
    // 2. db merge stage (exclude duplicates between db rows and xml rows)
    // 3. eval rule and edit stage (evaluate rules and allow edition to rows)

    // Edit bank rows page
    // 1. list bank rows from db for edition    

    reader: ExcelReader;
    mapper: ExcelBankRowMapper;
    readInBankRows: Array<ReadInBankRow>;
    @Output() nextStepChange = new EventEmitter();
    
    constructor(private loadingScreen: LoadingScreenService) {
        this.readInBankRows = [];
        this.mapper = new ExcelBankRowMapper(this.getBankType());
        this.reader = new ExcelReader(this.mapper);
    }

    ngOnInit(): void { }

    private getBankType(): BankType {
        // LATER ask user to provide us with this value or realize it based on the file
        return BankType.KH;
    }

    click_toggleColumnVisibility(propertyMap: PropertyMapRow): void {
        propertyMap.isOpen = !propertyMap.isOpen;
    }

    sortBy_bankRows(arr: Array<any>, property: string) {
        return arr.sort((a, b) => this.dateComparer(a.bankRow[property], b.bankRow[property]));
    }

    private dateComparer(a, b){
        let d1 = (new Date(a)).getTime();
        let d2 = (new Date(b)).getTime();
        return d1 > d2 ? 1 : d1 === d2 ? 0 : -1;
    }

    click_toggleRowExclusion(row: ReadInBankRow): void {
        row.isExcluded = !row.isExcluded;
    }

    change_filesSelected(event) {

        // Get the selected files
        let files = event.target.files;

        // Read files
        if (files == null || files.length === 0) {
            console.log("No files uploaded.");
            return;
        }
        let mappedExcelMatrix: Array<Array<ReadInBankRow>> = this.reader.getBankRowMatrix(files);
        console.log(mappedExcelMatrix);

        this.loadingScreen.start();
        
        // Wait for reader to read files
        var self = this;
        var finishedReadingInterval = setInterval(function() {
            
            if (self.reader.isReadingFinished()) {
                clearInterval(finishedReadingInterval);
                
                // Evaluate read files
                if (mappedExcelMatrix == null || mappedExcelMatrix.length == 0) {
                    console.log("No read files/rows to work with.");
                    return;
                }
                
                let merger: ReadInBankRowsMerger = new ReadInBankRowsMerger();
                merger.searchForDuplicates(mappedExcelMatrix);
                self.readInBankRows = [].concat.apply([], mappedExcelMatrix); // join the matrix's rows into one array

                self.loadingScreen.stop();
            }
        }, 10);        
    }

    click_next(): void {

        let output: FileFileResult = new FileFileResult();

        output.mapper = this.mapper;

        for (let i = 0; i < this.readInBankRows.length; i++) {
            const el = this.readInBankRows[i];
            if (!el.isExcluded) {
                let tr: ReadBankRowForInsertion = new ReadBankRowForInsertion();
                tr.bankRow = el.bankRow;
                output.bankRowList.push(tr);
            }
        }
        
        this.nextStepChange.emit(output);
    }
}
