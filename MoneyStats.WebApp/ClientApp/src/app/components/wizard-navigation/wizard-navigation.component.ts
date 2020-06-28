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
  @Input() maxHeight: number;
  @Input() isTooltipsHidden: boolean;

  // HACK repeated value, scss also has a $circle-size
  // unfortunately I can't see how scss and angular could use
  // one value. ($circle-size depends on other scss variables)
  private readonly circleHeight: number = 18;

  get lineHeight(): string {
    let endPaddingOffset = (15 * 2) + 15; // HACK an estimate + correction
    let r = (this.maxHeight - endPaddingOffset) / (this.wizardSteps.length - 1);
    r -= this.circleHeight;
    return r + "px";
  }

  constructor(private router: RouterService) {
  }
}