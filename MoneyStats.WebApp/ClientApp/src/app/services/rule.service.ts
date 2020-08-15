import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from './base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Rule } from "src/app/models/service-models/rule.model";
import { AndConditionGroup } from "src/app/models/service-models/and-condition-group.model";
import { Condition } from "src/app/models/service-models/condition.model";
import { RuleAction } from "src/app/models/service-models/rule-action.model";
import { EmptyResponse } from "./base.response";

class RuleServiceMap extends BaseHttpService {

    protected mapRules(response: any[]): Rule[] {
        let rules: Rule[] = [];
        for (let i = 0; i < response.length; i++) {

            const ruleJson = response[i];

            let rule = new Rule().set(ruleJson);

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

    protected mapRule(response: any): Rule {
        let rule = new Rule().set(response);

        response.andConditionGroups.forEach(andConditionGroupJson => {
            let andConditionGroup = new AndConditionGroup().set(andConditionGroupJson);
            andConditionGroupJson.conditions.forEach(conditionJson => {
                andConditionGroup.conditions.push(new Condition().set(conditionJson));
            });
            rule.andConditionGroups.push(andConditionGroup);
        });

        response.ruleActions.forEach(ruleActionJson => {
            let ruleAction = new RuleAction().set(ruleActionJson);
            rule.ruleActions.push(ruleAction);
        });
        return rule;
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
            .get<Rule[]>(this.url + 'getwithentities')
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

    delete(id: number): Observable<boolean> {
        if (this.isMocked()) {
            return this.deleteMock(id);
        }
        return this.http
            .post<boolean>(this.url + 'delete', id, this.getOptions());
    }

    private deleteMock(id: number): Observable<boolean> {
        return new Observable((observer) => {
            let response: boolean = id > 0 ? true : false;
            observer.next(response);
            observer.complete();
        });
    }

    saveAll(rules: Rule[]): Observable<Rule[]> {
        if (this.isMocked()) {
            return this.saveAllMock(rules);
        }
        return this.http
            .post<Rule[]>(this.url + 'saveallrules', rules, this.getOptions())
            .pipe(map(this.mapRules));
    }

    private saveAllMock(rules: Rule[]): Observable<Rule[]> {
        return new Observable((observer) => {
            observer.next(rules);
            observer.complete();
        });
    }

    save(rule: Rule): Observable<Rule> {
        if (this.isMocked()) {
            return this.saveMock(rule);
        }
        return this.http
            .post<Rule>(this.url + 'save', rule, this.getOptions())
            .pipe(map(this.mapRule));
    }

    private saveMock(rule: Rule): Observable<Rule> {
        return new Observable((observer) => {
            observer.next(rule);
            observer.complete();
        });
    }
}