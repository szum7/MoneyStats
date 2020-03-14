import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Transaction } from './models/transaction.model';

class TransactionServiceMap {

    protected dummyMap(response: any): any {
        return response;
    }    
}

class TransactionServiceLogic extends TransactionServiceMap {

}

@Injectable()
export class TransactionService extends TransactionServiceLogic {

    constructor(
        @Inject('BASE_URL') private baseUrl: string,
        private base: BaseHttpService,
        private http: HttpClient) {

        super();
        this.base.set('transaction', this.baseUrl, 'api/transaction/');
    }

    get(): Observable<Transaction[]> {
        if (this.base.isMocked()) {
            return this.getMock().pipe(map(this.dummyMap));
        }
        return this.http
            .get<any>(this.base.url + 'get')
            .pipe(map(this.dummyMap));
    }

    private getMock(): Observable<Transaction[]> {
        return new Observable((observer) => {
            let res: Transaction[] = [];

            res.push(new Transaction());
            res.push(new Transaction());
            res.push(new Transaction());
            res.push(new Transaction());
            res.push(new Transaction());
            
            res[0].set('2000-01-01 10:00:00', '', '', '', '', '', '', '', '', '');
            res[1].set('2000-01-02 11:00:00', '', '', '', '', '', '', '', '', '');
            res[2].set('2000-01-03 12:00:00', '', '', '', '', '', '', '', '', '');
            res[3].set('2000-01-04 13:00:00', '', '', '', '', '', '', '', '', '');
            res[4].set('2000-01-05 14:00:00', '', '', '', '', '', '', '', '', '');

            observer.next(res);
            observer.complete();
        });
    }
}