import { Component } from '@angular/core';
import { RouterService } from 'src/app/services/router-service/router.service';

@Component({
  selector: 'app-wizard-test',
  templateUrl: './wizard-test.component.html',
  styleUrls: ['./wizard-test.component.scss']
})
export class WizardTestComponent {

    stepsAt: number;

    constructor(private router: RouterService) {

        this.stepsAt = 0;
    }
}