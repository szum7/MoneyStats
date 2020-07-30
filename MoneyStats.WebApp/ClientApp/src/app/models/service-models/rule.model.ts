import { EntityBase } from "./entity-base.model";
import { AndConditionGroup } from "./and-condition-group.model";
import { RuleAction } from "./rule-action.model";

export class Rule extends EntityBase {
    title: string;
    fancyName: string;
    andConditionGroups: AndConditionGroup[];
    ruleActions: RuleAction[];

    constructor() {
        super();
        this.andConditionGroups = [];
        this.ruleActions = [];
    }

    public toString = (): string => {
        if (this.andConditionGroups.length == 0 || this.ruleActions.length == 0)
            return "";

        return `(${this.andConditionGroups.join(") || (")}) => ${this.ruleActions.join(", ")}`;
    }
}