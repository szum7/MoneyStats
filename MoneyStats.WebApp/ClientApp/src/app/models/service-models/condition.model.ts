import { ConditionType } from "./condition-type.enum";
import { EntityBase } from "./entity-base.model";

export class Condition extends EntityBase {
    property: string;
    value: string;
    conditionType: ConditionType;

    public toString = (): string => {
        switch (this.conditionType) {
            case ConditionType.TrueRule: return "true";
            case ConditionType.IsEqualTo: return `${this.property} == "${this.value}"`;
            case ConditionType.IsGreaterThan: return `${this.property} > ${this.value}`;
            case ConditionType.IsLesserThan: return `${this.property} < ${this.value}`;
            case ConditionType.IsPropertyNull: return `${this.property} is Null`;
            case ConditionType.IsPropertyNotNull: return `${this.property} is not Null`;
            case ConditionType.ContainsValueOfProperty: return `${this.property}.Contains("${this.value}")`;
            //default: return `${this.property} - ${this.value} - ${this.conditionType}`;
            default: return "";
        }
    }

    public set(obj: any): Condition {
        this.property = obj.property;
        this.value = obj.value;
        this.conditionType = obj.conditionType;
        this.setBase(obj);
        return this;
    }
}