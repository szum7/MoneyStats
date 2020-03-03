import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

class TestTableServiceMap {
    protected dummyPipe(response: any): any {
        return response;
    }
}

class TestTableServiceLogic extends TestTableServiceMap {

}

@Injectable()
export class TestTableService extends TestTableServiceLogic {

    constructor(
        private base: BaseHttpService, 
        private http: HttpClient, 
        @Inject('BASE_URL') private baseUrl: string) {
        super();
    }

    testCall(): Observable<any> {
        return this.http
            .get<any>(this.baseUrl + 'api/TestTable/testcall')
            .pipe(map(this.dummyPipe));
    }

    get(): Observable<any> {
        return this.http
            .get<any>(this.baseUrl + 'api/TestTable/get')
            .pipe(map(this.dummyPipe));
    }

}
