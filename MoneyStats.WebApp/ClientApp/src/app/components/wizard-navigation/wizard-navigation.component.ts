import { Component } from '@angular/core';
import { RouterService } from 'src/app/services/router-service/router.service';

export class WizardStep {
    public title: string;
    public link: string;

    constructor(title: string, link: string){
        this.title = title;
        this.link = link;
    }
}

@Component({
  selector: 'app-wizard-navigation',
  templateUrl: './wizard-navigation.component.html',
  styleUrls: ['./wizard-navigation.component.scss']
})
export class WizardNavigationComponent {

    wizardSteps: WizardStep[];
    stepsAt: number;

    constructor(private router: RouterService) {
        this.wizardSteps = [
            new WizardStep("Import files", "#"),
            new WizardStep("Manage read files", "#"),
            new WizardStep("Create transactions", "#")
        ];

        this.stepsAt = 0;
    }
}