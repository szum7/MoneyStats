import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Rule } from "src/app/models/service-models/rule.model";
import { BankRow } from "src/app/models/service-models/bank-row.model";
import { GeneratedTransaction } from "src/app/models/service-models/generated-transaction.model";
import { GenericResponse } from "src/app/models/service-models/generic-response.model";

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

    getGenerated(data: { rules: Rule[], bankRows: BankRow[] }): Observable<GeneratedTransaction[]> {
        if (this.base.isMocked()) {
            return this.getGeneratedMock();
        }
        return this.http
            .post<GeneratedTransaction[]>(this.base.url + 'getgenerated', data, this.base.getOptions());
    }

    private getGeneratedMock(): Observable<GeneratedTransaction[]> {
        return new Observable((observer) => {
            let res = [];
            
            // TODO mock

            observer.next(res);
            observer.complete();
        });
    }

    save(data: GeneratedTransaction[]): Observable<GenericResponse> {
        if (this.base.isMocked()) {
            return this.saveMock();
        }
        return this.http
            .post<GenericResponse>(this.base.url + 'save', data, this.base.getOptions());
    }

    private saveMock(): Observable<GenericResponse> {
        return new Observable((observer) => {
            let res = new GenericResponse();

            // TODO mock

            observer.next(res);
            observer.complete();
        });
    }
}