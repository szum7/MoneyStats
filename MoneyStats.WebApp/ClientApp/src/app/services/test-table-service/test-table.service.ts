import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

class TestTableServiceMap extends BaseHttpService {
    protected dummyPipe(response: any): any {
        return response;
    }
}

class TestTableServiceLogic extends TestTableServiceMap {

}

@Injectable()
export class TestTableService extends TestTableServiceLogic {

    constructor(
        private http: HttpClient, 
        @Inject('BASE_URL') baseUrl: string) {
        super();
        this.set('test', baseUrl, 'api/TestTable/');
    }

    testCall(): Observable<any> {
        return this.http
            .get<any>(this.url + 'testcall')
            .pipe(map(this.dummyPipe));
    }

    get(): Observable<any> {
        return this.http
            .get<any>(this.url + 'get')
            .pipe(map(this.dummyPipe));
    }

    postTest(data: any): Observable<any> {
        const header: HttpHeaders = new HttpHeaders({
            'Content-Type':  'application/json',
            'Authorization': 'Bearer xyz'
        });
        return this.http
            .post<any>(this.url + 'mypost', data, this.getOptions())
            .pipe(map(this.dummyPipe));
    }

}
