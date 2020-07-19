import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Rule } from "src/app/models/service-models/rule.model";

class RuleServiceMap extends BaseHttpService {

    protected dummyMap(response: any[]): any {
        return response;
    }    
}

class RuleServiceLogic extends RuleServiceMap {

}

@Injectable()
export class RuleService extends RuleServiceLogic {

    constructor(
        @Inject('BASE_URL') baseUrl: string,
        private http: HttpClient) {

        super();
        this.set('rule', baseUrl, 'api/rule/');
    }

    get(): Observable<Rule[]> {
        if (this.isMocked()) {
            return this.getMock();
        }
        return this.http
            .get<Rule[]>(this.url + 'get')
            .pipe(map(this.dummyMap));
    }

    private getMock(): Observable<Rule[]> {
        return new Observable((observer) => {
            let res: Rule[] = [];
            
            // TODO mock

            res.push(new Rule());

            observer.next(res);
            observer.complete();
        });
    }
}