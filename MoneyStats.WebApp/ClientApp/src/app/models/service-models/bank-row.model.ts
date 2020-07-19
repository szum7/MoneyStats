import { BankType } from "./bank-type.enum";

export class BankRow {

    Id: number;
    BankType: BankType;
    TransactionGroupId: number;

    //#region K&H Bank columns
    AccountingDate: Date;
    BankTransactionId: string;
    Type: string;
    Account: string;
    AccountName: string;
    PartnerAccount: string;
    PartnerName: string;
    Sum: number;
    Currency: string;
    Message: string;
    //#endregion

    set(ad: Date, bti: string, ty: string, ac: string, acn: string, pa: string, pn: string, sum: number, curr: string, msg: string): void {
        this.AccountingDate = ad;
        this.BankTransactionId = bti;
        this.Type = ty;
        this.Account = ac;
        this.AccountName = acn;
        this.PartnerAccount = pa;
        this.PartnerName = pn;
        this.Sum = sum;
        this.Currency = curr;
        this.Message = msg;
    }

    get(ad: Date, bti: string, ty: string, ac: string, acn: string, pa: string, pn: string, sum: number, curr: string, msg: string): BankRow {
        this.AccountingDate = ad;
        this.BankTransactionId = bti;
        this.Type = ty;
        this.Account = ac;
        this.AccountName = acn;
        this.PartnerAccount = pa;
        this.PartnerName = pn;
        this.Sum = sum;
        this.Currency = curr;
        this.Message = msg;
        return this;
    }

    public getContentId(): string {
        return this.AccountingDate.toDateString() 
        + this.nullCheck(this.BankTransactionId)
        + this.nullCheck(this.Type)
        + this.nullCheck(this.Account)
        + this.nullCheck(this.AccountName) 
        + this.nullCheck(this.PartnerAccount) 
        + this.nullCheck(this.PartnerName) 
        + this.Sum.toString() 
        + this.nullCheck(this.Currency) 
        + this.nullCheck(this.Message);
    }

    private nullCheck(prop: any): any {
        return prop ? prop : "";
    }
}
