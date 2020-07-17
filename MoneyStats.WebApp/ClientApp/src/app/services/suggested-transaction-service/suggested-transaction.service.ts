import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { SuggestedTransaction } from "src/app/models/service-models/suggested-transaction.model";
import { GenericResponse } from "src/app/models/service-models/generic-response.model";

class SuggestedTransactionServiceMap { 
}

class SuggestedTransactionServiceLogic extends SuggestedTransactionServiceMap {
}

@Injectable()
export class SuggestedTransactionService extends SuggestedTransactionServiceLogic {

    constructor(
        @Inject('BASE_URL') private baseUrl: string,
        private base: BaseHttpService,
        private http: HttpClient) {

        super();
        this.base.set('suggestedTransaction', this.baseUrl, 'api/suggestedtransaction/');
    }

    save(data: SuggestedTransaction[]): Observable<GenericResponse> {
        if (this.base.isMocked()) {
            return this.getMock();
        }
        return this.http
            .post<GenericResponse>(this.base.url + 'save', data, this.base.getOptions());
    }

    private getMock(): Observable<GenericResponse> {
        return new Observable((observer) => {
            let res = new GenericResponse();

            observer.next(res);
            observer.complete();
        });
    }
}