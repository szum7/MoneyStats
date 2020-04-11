import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { map } from 'rxjs/operators';
import { LoadingScreenService } from 'src/app/services/loading-screen-service/loading-screen.service';
import { RuleTransaction } from 'src/app/models/component-models/rule-transaction';
import { DbFileResult } from 'src/app/models/component-models/db-file-result';

@Component({
  selector: 'app-rule-evaluator-component',
  templateUrl: './rule-evaluator.component.html',
  styleUrls: ['./rule-evaluator.component.scss']
})
export class RuleEvaluatorComponent implements OnInit {
    
    @Input() params: DbFileResult;
    @Output() nextStepChange = new EventEmitter();
    public get fileList(): Array<RuleTransaction> { return this.params.bankRowList; }
    
    constructor(private loadingScreen: LoadingScreenService) {
    }

    ngOnInit(): void { }
}
