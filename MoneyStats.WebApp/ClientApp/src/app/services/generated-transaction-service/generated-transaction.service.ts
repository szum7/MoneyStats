import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpResponseBase } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Rule } from "src/app/models/service-models/rule.model";
import { BankRow } from "src/app/models/service-models/bank-row.model";
import { GeneratedTransaction } from "src/app/models/service-models/generated-transaction.model";
import { GenericResponse } from "src/app/models/service-models/generic-response.model";

class GeneratedTransactionServiceMap extends BaseHttpService {
}

class GeneratedTransactionServiceLogic extends GeneratedTransactionServiceMap {
}

@Injectable()
export class GeneratedTransactionService extends GeneratedTransactionServiceLogic {

    constructor(
        @Inject('BASE_URL') baseUrl: string,
        private http: HttpClient) {

        super();
        this.set('generatedTransaction', baseUrl, 'api/generatedtransaction/');
    }

    getGenerated(data: { rules: Rule[], bankRows: BankRow[] }): Observable<GeneratedTransaction[]> {
        if (this.isMocked()) {
            return this.getGeneratedMock();
        }
        return this.http
            .post<GeneratedTransaction[]>(this.url + 'getgenerated', data, this.getOptions());
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
        if (this.isMocked()) {
            return this.saveMock();
        }
        return this.http
            .post<GenericResponse>(this.url + 'save', data, this.getOptions());
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