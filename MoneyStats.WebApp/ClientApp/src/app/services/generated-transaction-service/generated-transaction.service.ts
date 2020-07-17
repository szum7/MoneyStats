import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Rule } from "src/app/models/service-models/rule.model";
import { BankRow } from "src/app/models/service-models/bank-row.model";
import { SuggestedTransaction } from "src/app/models/service-models/suggested-transaction.model";

class GeneratedTransactionServiceMap {
}

class GeneratedTransactionServiceLogic extends GeneratedTransactionServiceMap {
}

@Injectable()
export class GeneratedTransactionService extends GeneratedTransactionServiceLogic {

    constructor(
        @Inject('BASE_URL') private baseUrl: string,
        private base: BaseHttpService,
        private http: HttpClient) {

        super();
        this.base.set('generatedTransaction', this.baseUrl, 'api/generatedtransaction/');
    }

    getSuggesteds(data: { rules: Rule[], bankRows: BankRow[] }): Observable<SuggestedTransaction[]> {
        if (this.base.isMocked()) {
            return this.getMock();
        }
        return this.http
            .post<SuggestedTransaction[]>(this.base.url + 'getsuggesteds', data, this.base.getOptions());
    }

    private getMock(): Observable<SuggestedTransaction[]> {
        return new Observable((observer) => {
            let res = [];

            observer.next(res);
            observer.complete();
        });
    }
}