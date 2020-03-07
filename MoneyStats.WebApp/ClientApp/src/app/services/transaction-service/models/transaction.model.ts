export class Transaction {

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
    OriginalContentId: string;
    Tags: Array<any>;

    public getContentId(): string {
        return this.AccountingDate + this.TransactionId + this.Type + this.Account + this.AccountName + this.PartnerAccount + this.PartnerName + this.Sum + this.Currency + this.Message;
    }
}