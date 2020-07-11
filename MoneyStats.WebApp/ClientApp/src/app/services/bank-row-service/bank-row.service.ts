import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { BankRow } from '../../models/service-models/bank-row.model';

class BankRowServiceMap {

    protected dummyMap(response: any[]): any {
        let r: BankRow[] = [];
        for (let i = 0; i < response.length; i++) {
            const e = response[i];
            
            let t = new BankRow();

            t.Id = e.id;

            t.set(
                e.accountingDate, 
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
            return this.getMock().pipe(map(this.dummyMap));
        }
        return this.http
            .get<any>(this.base.url + 'get')
            .pipe(map(this.dummyMap));
    }

    private getMock(): Observable<BankRow[]> {
        return new Observable((observer) => {
            let res: BankRow[] = [];

            res.push(new BankRow());
            res.push(new BankRow());
            res.push(new BankRow());
            res.push(new BankRow());
            res.push(new BankRow());
            
            res[0].set('Mon Feb 02 2015 01:00:00 GMT+0100 (Central European Standard Time)', 'ó kamat', 'Kamat', '10401945223571949481012', 'TÉTE BERTALAN', undefined, undefined, '456', 'HUF', '35HUF');
            res[1].set('2000-01-02 11:00:00', '', '', '', '', '', '', '', '', '');
            res[2].set('2000-01-03 12:00:00', '', '', '', '', '', '', '', '', '');
            res[3].set('2000-01-04 13:00:00', '', '', '', '', '', '', '', '', '');
            res[4].set('2000-01-05 14:00:00', '', '', '', '', '', '', '', '', '');

            observer.next(res);
            observer.complete();
        });
    }
}