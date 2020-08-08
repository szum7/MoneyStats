import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Rule } from "src/app/models/service-models/rule.model";
import { AndConditionGroup } from "src/app/models/service-models/and-condition-group.model";
import { Condition } from "src/app/models/service-models/condition.model";
import { RuleAction } from "src/app/models/service-models/rule-action.model";

class RuleServiceMap extends BaseHttpService {

    protected mapRules(response: any[]): Rule[] {
        let rules: Rule[] = [];
        for (let i = 0; i < response.length; i++) {
            
            const ruleJson = response[i];

            let rule = new Rule();

            ruleJson.andConditionGroups.forEach(andConditionGroupJson => {
                let andConditionGroup = new AndConditionGroup().set(andConditionGroupJson);
                andConditionGroupJson.conditions.forEach(conditionJson => {
                    andConditionGroup.conditions.push(new Condition().set(conditionJson));
                });
                rule.andConditionGroups.push(andConditionGroup);
            });

            ruleJson.ruleActions.forEach(ruleActionJson => {
                let ruleAction = new RuleAction().set(ruleActionJson);
                rule.ruleActions.push(ruleAction);
            });

            rules.push(rule);
        }
        return rules;
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
            .pipe(map(this.mapRules));
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