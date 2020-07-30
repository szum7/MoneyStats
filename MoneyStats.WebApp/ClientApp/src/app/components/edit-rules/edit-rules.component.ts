import { Component, OnInit, Input } from '@angular/core';
import { Rule } from 'src/app/models/service-models/rule.model';
import { AndConditionGroup } from 'src/app/models/service-models/and-condition-group.model';
import { Condition } from 'src/app/models/service-models/condition.model';
import { ConditionType } from 'src/app/models/service-models/condition-type.enum';
import { RuleActionType } from 'src/app/models/service-models/rule-action-type.enum';
import { BankRowService } from 'src/app/services/bank-row-service/bank-row.service';
import { Common } from 'src/app/utilities/common.static';
import { RuleAction } from 'src/app/models/service-models/rule-action.model';


@Component({
    selector: 'app-edit-rules-component',
    templateUrl: './edit-rules.component.html',
    styleUrls: ['./edit-rules.component.scss']
})
export class EditRulesComponent implements OnInit {

    rules: Rule[];
    //ruleActionTypeVariable = RuleActionType;
    //conditionTypeVariable = ConditionType;
    propertyList: string[];

    conditionTypes: string[];
    actionTypes: string[];

    constructor(private bankRowService: BankRowService) {
        this.rules = [];

        this.conditionTypes = [
            ConditionType[ConditionType.ContainsValueOfProperty],
            ConditionType[ConditionType.IsEqualTo],
            ConditionType[ConditionType.IsGreaterThan],
            ConditionType[ConditionType.IsLesserThan],
            ConditionType[ConditionType.IsPropertyNotNull],
            ConditionType[ConditionType.IsPropertyNull],
            ConditionType[ConditionType.TrueRule]
        ];

        this.actionTypes = [
            RuleActionType[RuleActionType.AddTag],
            RuleActionType[RuleActionType.AggregateToMonthlyTransaction],
            RuleActionType[RuleActionType.Omit],
            RuleActionType[RuleActionType.SetValueOfProperty]
        ];
    }

    ngOnInit() {
        let self = this;
        this.getBankRowProperties(res => {
            self.propertyList = res;
        });
    }

    private getBankRowProperties(callback: (response: string[]) => void): void {
        this.bankRowService.getBankRowProperties().subscribe(res => {
            Common.ConsoleResponse("getBankRowProperties", res);
            callback(res);
        }, err => {
            console.log(err);
        });
    }

    sout_rules(): void {
        console.log(this.rules);
    }

    click_addRule(): void {
        this.rules.push(new Rule());
    }

    click_addRuleAction(rule: Rule): void {
        rule.ruleActions.push(new RuleAction());
    }

    click_addAndConditionGroup(rule: Rule): void {
        rule.andConditionGroups.push(new AndConditionGroup());
    }

    click_addCondition(andConditionGroup: AndConditionGroup): void {
        andConditionGroup.conditions.push(new Condition());
    }

    click_removeCondition(c: Condition, a: AndConditionGroup): void {
        const index = a.conditions.indexOf(c);
        if (index > -1) {
            a.conditions.splice(index, 1);
        }
    }

    click_removeRuleAction(c: RuleAction, a: Rule): void {
        const index = a.ruleActions.indexOf(c);
        if (index > -1) {
            a.ruleActions.splice(index, 1);
        }
    }

    change_conditionType(value: string, condition: Condition): void {
        condition.conditionType = ConditionType[value];
    }

    change_actionType(value: string, action: RuleAction): void {
        action.ruleActionType = RuleActionType[value];
    }

    change_conditionProperty(value: string, condition: Condition): void {
        condition.property = value;
    }

    change_actionProperty(value: string, action: RuleAction): void {
        action.property = value;
    }

    isPropertyVisible(condition: Condition): boolean {
        return condition.conditionType != null && condition.conditionType != ConditionType.TrueRule;
    }

    isTextValueVisible(condition: Condition): boolean {
        return condition.conditionType == ConditionType.IsEqualTo ||
            condition.conditionType == ConditionType.ContainsValueOfProperty;
    }

    isNumberValueVisible(condition: Condition): boolean {
        return condition.conditionType == ConditionType.IsGreaterThan ||
            condition.conditionType == ConditionType.IsLesserThan;
    }

    isActionTagVisible(action: RuleAction): boolean {
        return action.ruleActionType == RuleActionType.AddTag;
    }

    isActionValueVisible(action: RuleAction): boolean {
        return action.ruleActionType == RuleActionType.SetValueOfProperty;
    }

    isActionPropertyVisible(action: RuleAction): boolean {
        return action.ruleActionType == RuleActionType.SetValueOfProperty;
    }

    isTypeSet(condition: Condition): boolean {
        return condition.conditionType != ConditionType.Unset;
    }

    isActionTypeSet(action: RuleAction): boolean {
        return action.ruleActionType != RuleActionType.Unset;
    }
}
