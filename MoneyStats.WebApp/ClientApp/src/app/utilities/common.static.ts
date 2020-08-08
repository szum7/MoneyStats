import { ConditionType } from "../models/service-models/condition-type.enum";
import { RuleActionType } from "../models/service-models/rule-action-type.enum";

export class ConditionTypeObj {
    id: number;
    title: string;

    constructor(no: number) {
        this.id = no;
        this.title = ConditionType[no];
    }
}

export class RuleActionTypeObj {
    id: number;
    title: string;

    constructor(no: number) {
        this.id = no;
        this.title = RuleActionType[no];
    }
}

export class Common {
    public static ConsoleResponse(name: string, response: any): void {
        console.log("=> " + name + ":");
        console.log(response);
        console.log("<=");
    }

    public static containsObjectOnId(obj: any, list: any[]): boolean {
        for (let i = 0; i < list.length; i++) {
            if (list[i].id === obj.id) {
                return true;
            }
        }
        return false;
    }

    public static removeFromArray(obj: any, list: any[]): void {
        const index = list.indexOf(obj);
        if (index > -1) {
            list.splice(index, 1);
        }
    }

    public static getConditionTypes(): ConditionTypeObj[] {
        return [
            new ConditionTypeObj(ConditionType.Unset),
            new ConditionTypeObj(ConditionType.ContainsValueOfProperty),
            new ConditionTypeObj(ConditionType.IsEqualTo),
            new ConditionTypeObj(ConditionType.IsGreaterThan),
            new ConditionTypeObj(ConditionType.IsLesserThan),
            new ConditionTypeObj(ConditionType.IsPropertyNotNull),
            new ConditionTypeObj(ConditionType.IsPropertyNull),
            new ConditionTypeObj(ConditionType.TrueRule)
        ];
    }

    public static getActionTypes(): RuleActionTypeObj[] {
        return [
            new RuleActionTypeObj(RuleActionType.Unset),
            new RuleActionTypeObj(RuleActionType.AddTag),
            new RuleActionTypeObj(RuleActionType.AggregateToMonthlyTransaction),
            new RuleActionTypeObj(RuleActionType.Omit),
            new RuleActionTypeObj(RuleActionType.SetValueOfProperty)
        ];
    }
}