import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { BankRow } from '../../models/service-models/bank-row.model';
import { Rule } from "src/app/models/service-models/rule.model";

class RuleEvaluatorServiceMap {

    protected dummyMap(response: any[]): any { // TODO
        return response;
    }    
}

class RuleEvaluatorServiceLogic extends RuleEvaluatorServiceMap {

}

@Injectable()
export class RuleEvaluatorService extends RuleEvaluatorServiceLogic {

    constructor(
        @Inject('BASE_URL') private baseUrl: string,
        private base: BaseHttpService,
        private http: HttpClient) {

        super();
        this.base.set('ruleEvaluator', this.baseUrl, 'api/TODO/');
    }

    getEvaluatedTransactions(rules: Rule[], bankRows: BankRow[]): Observable<any> { // TODO work out the method and returned value
        if (this.base.isMocked()) {
            return this.getEvaluatedTransactionsMock();
        }
        return this.http
            .get<any>(this.base.url + 'getEvaluatedTransactions')
            .pipe(map(this.dummyMap));
    }

    private getEvaluatedTransactionsMock(): Observable<any> { // TODO
        return new Observable((observer) => {
            let res: any = {};
            
            // TODO mock

            observer.next(res);
            observer.complete();
        });
    }
}