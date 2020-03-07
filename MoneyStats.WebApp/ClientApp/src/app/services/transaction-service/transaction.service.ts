import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

class TransactionServiceMap {
    protected dummyPipe(response: any): any {
        return response;
    }
}

class TransactionServiceLogic extends TransactionServiceMap {

}

@Injectable()
export class TransactionService extends TransactionServiceLogic {  

    serviceUrl: string = 'api/TestTable/';

    constructor(
        private base: BaseHttpService, 
        private http: HttpClient, 
        @Inject('BASE_URL') private baseUrl: string) {
        super();
    }

    testCall(): Observable<any> {
        return this.http
            .get<any>(this.baseUrl + this.serviceUrl + 'testcall')
            .pipe(map(this.dummyPipe));
    }

    get(): Observable<any> {
        return this.http
            .get<any>(this.baseUrl + this.serviceUrl + 'get')
            .pipe(map(this.dummyPipe));
    }
}