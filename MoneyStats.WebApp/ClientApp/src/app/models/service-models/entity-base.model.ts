export class EntityBase {
    id?: number;
    modifiedDate?: Date;
    createDate?: Date;

    protected setBase(obj: any): void {        
        this.id = obj.id;
        this.modifiedDate = obj.modifiedDate;
        this.createDate = obj.createDate;
    }
}