import { EntityBase } from "./entity-base.model";
import { RuleActionType } from "./rule-action-type.enum";
import { Tag } from "./tag.model";

export class RuleAction extends EntityBase {

    title?: string;
    property?: string;
    value?: string;
    ruleActionType: RuleActionType;
    tag?: Tag;
    tagId?: number;

    public toString = (): string => {
        switch (this.ruleActionType) {
            case RuleActionType.Omit:
                return "Omit";
            case RuleActionType.AddTag:
                return (this.tag == null) ? `AddTag(${this.tagId})"` : `AddTag(${this.tag.id}, "${this.tag.title}")`;
            case RuleActionType.SetValueOfProperty:
                return `${this.property} = "${this.value}"`;
            case RuleActionType.AggregateToMonthlyTransaction:
                return "GroupToMonth";
            default:
                return "";
        }
    }

    public set(obj: any): RuleAction {
        this.title = obj.title;
        this.property = obj.property;
        this.value = obj.value;
        this.ruleActionType = obj.ruleActionType;
        if (obj.tag)
            this.tag = new Tag().set(obj.tag);
        this.tagId = obj.tagId;
        this.setBase(obj);
        return this;
    }
}