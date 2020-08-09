import { EntityBase } from "./entity-base.model";
import { AndConditionGroup } from "./and-condition-group.model";
import { RuleAction } from "./rule-action.model";
import { TransactionCreatedWithRule } from "./transaction-created-with-rule.model";

export class Rule extends EntityBase {
    title: string;
    fancyName?: string;
    andConditionGroups: AndConditionGroup[];
    ruleActions: RuleAction[];
    transactionCreatedWithRules: TransactionCreatedWithRule[];

    constructor() {
        super();
        this.andConditionGroups = [];
        this.ruleActions = [];
        this.transactionCreatedWithRules = [];
    }

    public toString = (): string => {
        if (this.andConditionGroups.length == 0 || this.ruleActions.length == 0)
            return "";

        return `(${this.andConditionGroups.join(") || (")}) => ${this.ruleActions.join(", ")}`;
    }

    public set(obj: any): Rule {
        this.title = obj.title;
        this.fancyName = obj.fancyName;
        this.setBase(obj);
        return this;
    }
}