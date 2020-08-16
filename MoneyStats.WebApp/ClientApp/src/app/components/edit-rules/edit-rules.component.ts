import { Component, OnInit, Input } from '@angular/core';
import { Rule } from 'src/app/models/service-models/rule.model';
import { AndConditionGroup } from 'src/app/models/service-models/and-condition-group.model';
import { Condition } from 'src/app/models/service-models/condition.model';
import { ConditionType } from 'src/app/models/service-models/condition-type.enum';
import { RuleActionType } from 'src/app/models/service-models/rule-action-type.enum';
import { BankRowService } from 'src/app/services/bank-row.service';
import { Common, ConditionTypeObj, RuleActionTypeObj } from 'src/app/utilities/common.static';
import { RuleAction } from 'src/app/models/service-models/rule-action.model';
import { TransactionService } from 'src/app/services/transaction.service';
import { TagService } from 'src/app/services/tag.service';
import { Tag } from 'src/app/models/service-models/tag.model';
import { RuleService } from 'src/app/services/rule.service';

@Component({
    selector: 'app-edit-rules-component',
    templateUrl: './edit-rules.component.html',
    styleUrls: ['./edit-rules.component.scss'],
    styles: [
        `
        :host ::ng-deep .select-component .ac-select input {
            font-size: 12px;
            height: 25px !important;
            border: none  !important;
            background-color: #e5e5e5;
            padding: 0 6px;
            margin-right: 3px;
        }
        :host ::ng-deep .select-component .ac-select .results {
            top: 25px !important;
        }
        `
    ]
})
export class EditRulesComponent implements OnInit {

    @Input() isDeletionAllowed: boolean;
    @Input() isSavingAllowed: boolean;

    rules: Rule[];
    bankRowProperties: string[];
    transactionProperties: string[];

    conditionTypes: ConditionTypeObj[];
    actionTypes: RuleActionTypeObj[];
    tags: Tag[];

    constructor(
        private bankRowService: BankRowService,
        private transactionService: TransactionService,
        private tagService: TagService,
        private ruleService: RuleService) {
        this.rules = [];
        this.tags = [];

        this.conditionTypes = Common.getConditionTypes();
        this.actionTypes = Common.getActionTypes();
    }

    ngOnInit() {
        /* TODO
        - rule title-t bindolni valahova
        - double bind dropdowns (for loading existing rules)
        */

        let self = this;

        self.getBankRowProperties(res => {
            self.bankRowProperties = res;
            self.bankRowProperties.unshift("");
        });

        self.getTransactionProperties(res => {
            self.transactionProperties = res;
            self.transactionProperties.unshift("");
        });

        self.getTags((res) => {
            self.tags = res;
        });

        self.getRules(res => {
            self.rules = res;
        });
    }

    private getRules(callback: (response: Rule[]) => void): void {
        this.ruleService.get().subscribe(res => {
            Common.ConsoleResponse("getRules", res);
            callback(res);
        }, err => {
            console.log(err);
        })
    }

    private getTags(callback: (response: Tag[]) => void): void {
        this.tagService.get().subscribe(res => {
            Common.ConsoleResponse("getTags", res);
            callback(res);
        }, err => {
            console.log(err);
        });
    }

    private getTransactionProperties(callback: (response: string[]) => void): void {
        this.transactionService.getTransactionProperties().subscribe(res => {
            Common.ConsoleResponse("getTransactionProperties", res);
            callback(res);
        }, err => {
            console.log(err);
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

    private deleteRule(id: number, callback: (response: boolean) => void): void {
        this.ruleService.delete(id).subscribe(res => {
            callback(res);
        }, err => {
            console.log(err);
        });
    }

    private saveRules(rules: Rule[], callback: (response: Rule[]) => void): void {
        this.ruleService.save(rules).subscribe(res => {
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
        Common.removeFromArray(c, a.conditions);
    }

    click_removeRule(c: Rule): void {
        if (!this.isDeletionAllowed) {
            console.error("Deletion is not alowed!");
            return;
        }

        let self = this;

        if (c.id > 0) {
            self.deleteRule(c.id, res => {
                if (res === true) {
                    Common.removeFromArray(c, self.rules);
                } else {
                    console.log("Rule deletion error. Rule was possibly not found.");
                }
            });
        } else {
            Common.removeFromArray(c, self.rules);
        }
    }

    click_removeAndConditionGroup(c: AndConditionGroup, a: Rule): void {
        Common.removeFromArray(c, a.andConditionGroups);
    }

    click_removeRuleAction(c: RuleAction, a: Rule): void {
        Common.removeFromArray(c, a.ruleActions);
    }

    click_saveRule(rule: Rule): void {
        if (!this.isSavingAllowed) {
            console.error("Saving is not alowed!");
            (rule as any).saveTimestamp = new Date();
            return;
        }
        let self = this;
        self.saveRules([rule], res => {
            if (res.length === 1) {
                rule.id = res[0].id;
                (rule as any).saveTimestamp = new Date();
            } else {
                console.error("Save error");
            }
        });
    }

    change_conditionType(value: string, condition: Condition): void {
        condition.conditionType = parseInt(value);
        condition.property = null;
        condition.value = null;
    }

    change_actionType(value: string, action: RuleAction): void {
        action.ruleActionType = parseInt(value);
        action.property = null;
        action.tag = null;
        action.value = null;
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
