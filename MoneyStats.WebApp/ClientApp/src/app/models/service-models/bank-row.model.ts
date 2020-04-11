import { BankType } from "./bank-type.enum";

export class BankRow {

    BankType: BankType;
    TransactionGroupId: number;

    //#region K&H Bank columns
    AccountingDate: string;
    TransactionId: string;
    Type: string;
    Account: string;
    AccountName: string;
    PartnerAccount: string;
    PartnerName: string;
    Sum: string;
    Currency: string;
    Message: string;
    //#endregion

    set(ad: string, ti: string, ty: string, ac: string, acn: string, pa: string, pn: string, sum: string, curr: string, msg: string): void {
        this.AccountingDate = ad;
        this.TransactionId = ti;
        this.Type = ty;
        this.Account = ac;
        this.AccountName = acn;
        this.PartnerAccount = pa;
        this.PartnerName = pn;
        this.Sum = sum;
        this.Currency = curr;
        this.Message = msg;
    }

    public getContentId(): string {
        return this.AccountingDate 
        + this.TransactionId 
        + this.Type 
        + this.Account 
        + this.AccountName 
        + this.PartnerAccount 
        + this.PartnerName 
        + this.Sum 
        + this.Currency 
        + this.Message;
    }
}