import { Component, OnInit } from '@angular/core';
import { Wizard, WizardStep } from '../wizard-navigator/wizard-navigator.component';

@Component({
    selector: 'app-read-in-component',
    templateUrl: './read-in.component.html',
    styleUrls: ['./read-in.component.scss']
})
export class ReadInComponent implements OnInit {

    public wizard: Wizard;

    constructor() { 
        let steps: WizardStep[] = [];
        steps.push(new WizardStep(
            "Step 1 - Introduction to 'Save bank data from exported files'", 
            [
                "This wizard will take you through the steps of saving bank exported excel data to the MoneyStats database.", 
                "Read about - bank rows.", 
                "Read about - transactions",
                "Read about - Supported banks and export files"
            ]));
        steps.push(new WizardStep(
            "Step 2 - Read in exported files", 
            ["Select which files you want to read in."]));
            steps.push(new WizardStep(
            "Step 3 - Eliminate duplicates and save", 
            [
                "Select the records you wish to save to the database.", 
                "The program helps you by detecting duplicates across multiple read files. It compares every single read records with the ones already existing in the database by their properties. Duplicates are shown as excluded (grayed out) rows. (You can decide to include them if you think they're not duplicates)."
            ]));
        this.wizard = new Wizard(steps);
    }

    ngOnInit() {
        
    }

    next() {
        this.wizard.next();
    }

    prev() {
        this.wizard.previous();
    }

}
