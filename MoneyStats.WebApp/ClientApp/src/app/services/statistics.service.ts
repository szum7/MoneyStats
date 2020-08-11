import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { BaseHttpService } from './base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { BasicMonthlyBarchart } from "../models/service-models/basic-monthly-barchart.model";

class StatisticsServiceMap extends BaseHttpService {

    protected dummyMap(response: any): any {
        return response;
    }
}

class StatisticsServiceLogic extends StatisticsServiceMap {

}

@Injectable()
export class StatisticsService extends StatisticsServiceLogic {

    constructor(
        @Inject('BASE_URL') baseUrl: string,
        private http: HttpClient) {

        super();
        this.set('statistics', baseUrl, 'api/statistics/');
    }

    getBasicMonthlyBarchart(from: Date, to: Date): Observable<BasicMonthlyBarchart> {
        if (this.isMocked()) {
            return this.getBasicMonthlyBarchartMock();
        }

        let data = this.setGetParams([
            { name: "from", value: from.toUTCString() },
            { name: "to", value: to.toUTCString() }
        ]);

        return this.http
            .get<BasicMonthlyBarchart>(this.url + 'getbasicmonthlybarchart', data)
            .pipe(map(this.dummyMap));
    }

    private getBasicMonthlyBarchartMock(): Observable<any> {
        return new Observable((observer) => {
            let response: any = null;
            // TODO mock
            observer.next(response);
            observer.complete();
        });
    }
}