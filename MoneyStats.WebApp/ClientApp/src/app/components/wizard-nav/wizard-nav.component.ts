import { Component, OnInit, Input } from '@angular/core';
import { WizardStep } from 'src/app/models/component-models/wizard-step';
import { RouterService } from 'src/app/services/router-service/router.service';

@Component({
  selector: 'app-wizard-nav-component',
  templateUrl: './wizard-nav.component.html',
  styleUrls: ['./wizard-nav.component.scss']
})
export class WizardNavComponent implements OnInit {

  @Input() wizardSteps: WizardStep[];
  @Input() stepsAt: number;
  @Input() maxHeight: number;
  @Input() isTooltipsHidden: boolean;

  get stepWidthPercent(): number {
    return 100 / (this.wizardSteps.length - 1);
  }

  constructor(private router: RouterService) {
  }

  ngOnInit() {
  }

}
