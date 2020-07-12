import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { ExcelReader } from 'src/app/models/component-models/excel-reader';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { BankType } from 'src/app/models/service-models/bank-type.enum';
import { StepAlert } from 'src/app/pages/update-page/update.page';

/// <summary>
/// This component let's you browse for multiple files on your local machine, 
/// and will map the files' contents to the given bank format. Currently only 
/// K&H is supported, but later the user could specify the bank (or the program 
/// could take a guess).
/// </summary>
@Component({
  selector: 'app-choose-file',
  templateUrl: './choose-file.component.html',
  styleUrls: ['./choose-file.component.scss']
})
export class ChooseFileComponent implements OnInit {

  @Output() nextStepChange = new EventEmitter<{ matrix: ReadInBankRow[][], mapper: ExcelBankRowMapper }>();
  @Output() nextStepAlertsChange = new EventEmitter();

  public readFiles: any[];

  private reader: ExcelReader;
  private mapper: ExcelBankRowMapper;

  constructor(private loadingScreen: LoadingScreenService) {
    this.readFiles = [];
    this.mapper = new ExcelBankRowMapper(this.getBankType());
    this.reader = new ExcelReader(this.mapper);
  }

  ngOnInit() {
    this.checkNextStepPossible();
  }

  private getBankType(): BankType {
    // LATER ask user to provide us with this value or realize it based on the file
    return BankType.KH;
  }

  change_filesSelected(event): void {

    // Get the selected files
    let files = event.target.files;

    // Save filenames
    this.readFiles = files;

    // Read files
    if (files == null || files.length === 0) {
      console.log("No files uploaded.");
      return;
    }
    let mappedExcelMatrix: ReadInBankRow[][] = this.reader.getBankRowMatrix(files);

    // Wait for reader to read files
    var self = this;
    this.loadingScreen.start();
    var finishedReadingInterval = setInterval(function () {

      if (self.reader.isReadingFinished()) {
        clearInterval(finishedReadingInterval);

        // Evaluate read files
        if (mappedExcelMatrix == null || mappedExcelMatrix.length == 0) {
          console.log("No read files/rows to work with.");
        }

        self.loadingScreen.stop();
        console.log(mappedExcelMatrix);

        self.emitOutput(mappedExcelMatrix, self.mapper);
        self.checkNextStepPossible();
      }
    }, 10);
  }

  private checkNextStepPossible(): void {
    let alerts = [];
    
    if (this.readFiles.length === 0) {
      alerts.push(new StepAlert("No files were selected.").setToCriteria());
    }

    this.nextStepAlertsChange.emit(alerts);
  }

  private emitOutput(mappedExcelMatrix: ReadInBankRow[][], mapper: ExcelBankRowMapper): void {
    this.nextStepChange.emit({ matrix: mappedExcelMatrix, mapper: mapper });
  }
}
