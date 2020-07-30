import { Condition } from "./condition.model";
import { EntityBase } from "./entity-base.model";

export class AndConditionGroup extends EntityBase {
    conditions: Condition[];

    constructor() {
        super();
        this.conditions = [];
    }

    public toString = (): string => {
        if (this.conditions.length == 0)
            return "";

        return this.conditions.join(" && ");
    }
}