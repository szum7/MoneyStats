import { EntityBase } from "./entity-base.model";

export class Tag extends EntityBase{
    title: string;
    description: string;
    
    public set(obj: any): Tag {
        this.title = obj.title;
        this.description = obj.description;
        this.setBase(obj);
        return this;
    }
}