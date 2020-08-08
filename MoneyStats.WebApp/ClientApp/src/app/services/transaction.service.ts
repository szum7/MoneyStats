import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from './base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

class TransactionServiceMap extends BaseHttpService {

    protected dummyMap(response: any[]): any[] {        
        return response;
    } 
}

class TransactionServiceLogic extends TransactionServiceMap {
}

@Injectable()
export class TransactionService extends TransactionServiceLogic {

    constructor(
        @Inject('BASE_URL') baseUrl: string,
        private http: HttpClient) {
        super();
        this.set('transaction', baseUrl, 'api/transaction/');
    }

    getTransactionProperties(): Observable<string[]> {
        if (this.isMocked()) {
            return this.getTransactionPropertiesMock();
        }
        return this.http
            .get<string[]>(this.url + 'getTransactionProperties')
            .pipe(map(this.dummyMap));
    }

    private getTransactionPropertiesMock(): Observable<string[]> {
        return new Observable((observer) => {
            let res: string[] = [];

            // TODO mock

            observer.next(res);
            observer.complete();
        });
    }
}