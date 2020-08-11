import { HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { EmptyResponse } from './base.response';
import { environment } from 'src/environments/environment';

@Injectable()
export class BaseHttpService {

    private serviceName: string;
    private baseUrl: string;
    private serviceUrl: string;
    get url(): string { return this.baseUrl + this.serviceUrl; }

    constructor() {

        // IMPROVE BaseHttpService structure
        // Could have a generic map and logic property but typescript can't (really) instanciate generic types.
        // Could do it with constructor parameters, but then the injections would also have to be passed in.
    }

    protected set(serviceName: string, baseUrl: string, serviceUrl: string): void {
        this.serviceName = serviceName;
        this.baseUrl = baseUrl;
        this.serviceUrl = serviceUrl;
    }

    protected getOptions(): { headers: HttpHeaders } {
        const header: HttpHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + null
        });
        return { headers: header };
    }

    /**
     * Map a server side no-data response type to client side no-data response type
     * @param response Has errorMessages and isValid properties. No data.
     */
    protected mapEmptyResponse(response: any): EmptyResponse {
        const ret = new EmptyResponse();
        ret.data = null;
        ret.errorMessages = response.errorMessages;
        ret.isValid = response.isValid;
        return ret;
    }

    protected setGetParams(paramList: { name: string, value: string }[]): { headers: HttpHeaders, params: HttpParams } {
        let headers = new HttpHeaders();
        headers.append('Content-Type', 'application/json');
        let httpParams = new HttpParams();
        paramList.forEach(paramItem => {
            httpParams = httpParams.set(paramItem.name, paramItem.value);
        });
        return { headers: headers, params: httpParams };
    }

    isMocked(): boolean {
        return environment.mock.all || environment.mock[this.serviceName];
    }
}
