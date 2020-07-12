import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { BankRow } from '../../models/service-models/bank-row.model';

class BankRowServiceMap {

    protected mapBankRow(response: any[]): any {
        let r: BankRow[] = [];
        for (let i = 0; i < response.length; i++) {
            const e = response[i];
            
            let t = new BankRow();

            t.Id = e.id;

            t.set(
                new Date(e.accountingDate), 
                e.bankTransactionId, 
                e.type, 
                e.account, 
                e.accountName, 
                e.partnerAccount, 
                e.partnerName, 
                e.sum, 
                e.currency, 
                e.message);

            r.push(t);
        }
        return r;
    }    
}

class BankRowServiceLogic extends BankRowServiceMap {

}

@Injectable()
export class BankRowService extends BankRowServiceLogic {

    constructor(
        @Inject('BASE_URL') private baseUrl: string,
        private base: BaseHttpService,
        private http: HttpClient) {

        super();
        this.base.set('bankRow', this.baseUrl, 'api/bankrow/');
    }

    get(): Observable<BankRow[]> {
        if (this.base.isMocked()) {
            return this.getMock();
        }
        return this.http
            .get<any>(this.base.url + 'get')
            .pipe(map(this.mapBankRow));
    }

    private getMock(): Observable<BankRow[]> {
        return new Observable((observer) => {
            let res: BankRow[] = [];

            res.push(new BankRow());
            res.push(new BankRow());
            res.push(new BankRow());
            res.push(new BankRow());
            res.push(new BankRow());
            
            res[0].set(new Date(2015, 0, 2), 'ó kamat', 'Kamat', '10401945223571949481012', 'TÉTE BERTALAN', undefined, undefined, 456, 'HUF', '35HUF');
            res[1].set(new Date(2015, 0, 3), '', '', '', '', '', '', 0, '', '');
            res[2].set(new Date(2015, 0, 4), '', '', '', '', '', '', 0, '', '');
            res[3].set(new Date(2015, 0, 5), '', '', '', '', '', '', 0, '', '');
            res[4].set(new Date(2015, 0, 6), '', '', '', '', '', '', 0, '', '');

            observer.next(res);
            observer.complete();
        });
    }
}