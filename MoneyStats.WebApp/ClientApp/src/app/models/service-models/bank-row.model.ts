import { BankType } from "./bank-type.enum";
import { EntityBase } from "./entity-base.model";

export class BankRow extends EntityBase {

    bankType: BankType;
    transactionGroupId: number;

    //#region K&H Bank columns
    accountingDate: Date;
    bankTransactionId: string;
    type: string;
    account: string;
    accountName: string;
    partnerAccount: string;
    partnerName: string;
    sum: number;
    currency: string;
    message: string;
    //#endregion

    set(ad: Date, bti: string, ty: string, ac: string, acn: string, pa: string, pn: string, sum: number, curr: string, msg: string): void {
        this.accountingDate = ad;
        this.bankTransactionId = bti;
        this.type = ty;
        this.account = ac;
        this.accountName = acn;
        this.partnerAccount = pa;
        this.partnerName = pn;
        this.sum = sum;
        this.currency = curr;
        this.message = msg;
    }

    get(ad: Date, bti: string, ty: string, ac: string, acn: string, pa: string, pn: string, sum: number, curr: string, msg: string): BankRow {
        this.accountingDate = ad;
        this.bankTransactionId = bti;
        this.type = ty;
        this.account = ac;
        this.accountName = acn;
        this.partnerAccount = pa;
        this.partnerName = pn;
        this.sum = sum;
        this.currency = curr;
        this.message = msg;
        return this;
    }

    public getContentId(): string {
        
        let date: string = "";

        try {
            date = this.accountingDate.toDateString();
        } catch (error) {
            console.log(error);
            console.log(this);
        }

        return date
            + this.nullCheck(this.bankTransactionId)
            + this.nullCheck(this.type)
            + this.nullCheck(this.account)
            + this.nullCheck(this.accountName)
            + this.nullCheck(this.partnerAccount)
            + this.nullCheck(this.partnerName)
            + this.sum.toString()
            + this.nullCheck(this.currency)
            + this.nullCheck(this.message);
    }

    private nullCheck(prop: any): any {
        return prop ? prop : "";
    }
}
