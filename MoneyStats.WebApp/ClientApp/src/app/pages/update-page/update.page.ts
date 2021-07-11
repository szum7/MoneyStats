import { Component, OnInit } from '@angular/core';
import { ReadInBankRow } from 'src/app/models/component-models/read-in-bank-row';
import { ExcelBankRowMapper } from 'src/app/models/component-models/excel-bank-row-mapper';
import { ReadBankRowForDbCompare } from 'src/app/models/component-models/read-bank-row-for-db-compare';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { GeneratedTransaction } from 'src/app/models/service-models/generated-transaction.model';
import { GeneratedTransactionService } from 'src/app/services/generated-transaction.service';
import { GenericResponse } from 'src/app/models/service-models/generic-response.model';
import { Common } from 'src/app/utilities/common.static';
import { WizardNavigator, WizardNavigatorStep } from 'src/app/components/wizards/wizard-navigator/wizard-navigator.component';
import { ChooseFileOutput } from 'src/app/components/wizards/input-bankrows-wizard/choose-file/choose-file.component';
import { ManageReadFilesInput } from 'src/app/components/wizards/input-bankrows-wizard/read-files/read-files.component';
import { CompareDbInput } from 'src/app/components/wizards/input-bankrows-wizard/compare-db/compare-db.component';

@Component({
    selector: 'app-update-page',
    templateUrl: './update.page.html',
    styleUrls: ['./update.page.scss'],
    //encapsulation: ViewEncapsulation.None // TODO add comment why this is needed (?)
})
export class UpdatePage implements OnInit {

    wizard: WizardNavigator;
    manageReadFilesInput: ManageReadFilesInput;
    compareDbInput: CompareDbInput;

    private mapper: ExcelBankRowMapper;

    constructor(
        private loadingScreen: LoadingScreenService,
        private generatedTransactionService: GeneratedTransactionService) {
        
        this.initWizard();
    }

    ngOnInit(): void {
    }

    private initWizard(): void {
        let steps: WizardNavigatorStep[] = [];

        steps.push(new WizardNavigatorStep(
            "Step 1 - Read in exported files", 
            [
                "Import files from your local drive.",
                "Supported file types: .xls, .xlsx",
                "Supported bank type: K&H"
            ]));
        steps.push(new WizardNavigatorStep(
            "Step 2 - Eliminate duplicates between read files", 
            [
                "Select the records you wish to save to the database. The program helps you by detecting duplicates across multiple read files."
            ]));
        steps.push(new WizardNavigatorStep(
            "Step 3 - Compare with database and save", 
            [
                "Select the records you wish to save to the database.",
                "The program compared every single records selected from the previous step with the ones already existing in the database. Comparison is done by properties. Duplicates are shown as excluded (grayed out) rows. (You can decide to include them if you know what you're doing and think they're not duplicates)."
            ]));
        
        this.wizard = new WizardNavigator(steps);
    }

    outputChange_step1($output: ChooseFileOutput): void {
        this.mapper = $output.mapper;
        this.manageReadFilesInput = new ManageReadFilesInput(this.wizard, $output.matrix, $output.mapper);
    }

    outputChange_step2($output: ReadInBankRow[]): void {
        if (this.mapper == null) {
            console.error("Mapper is null!");
            return;
        }

        let cast: ReadBankRowForDbCompare[] = [];

        for (let i = 0; i < $output.length; i++) {
            if (!$output[i].isExcluded) {
                let c = $output[i];

                let m = new ReadBankRowForDbCompare();
                m.bankRow = c.bankRow;
                m.uiId = c.uiId;

                cast.push(m);
            }
        }

        if (cast.length === 0) {
            console.error("List is empty!");
            return;
        }

        this.compareDbInput = new CompareDbInput(this.wizard, cast, this.mapper);
    }

    outputChange_step3($output: any): void {
        // Some sort of success message or log. Save happens inside the component.
    }

    private saveTransactions(generatedTransactions: GeneratedTransaction[], callback: (response: GenericResponse) => void): void {
        this.generatedTransactionService.save(generatedTransactions).subscribe(response => {
            Common.ConsoleResponse("saveTransactions", response);
            callback(response);
        }, error => {
            console.log(error);
        });
    }
}
