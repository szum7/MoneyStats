import { Component, Input } from '@angular/core';
import { RouterService } from 'src/app/services/router-service/router.service';
import { WizardStep } from 'src/app/models/component-models/wizard-step';


@Component({
  selector: 'app-wizard-navigation',
  templateUrl: './wizard-navigation.component.html',
  styleUrls: ['./wizard-navigation.component.scss']
})
export class WizardNavigationComponent {

    @Input() wizardSteps: WizardStep[];
    @Input() stepsAt: number;

    constructor(private router: RouterService) {
    }
}