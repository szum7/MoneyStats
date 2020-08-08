import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from './base-http.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Tag } from "src/app/models/service-models/tag.model";

class TagServiceMap extends BaseHttpService {

    protected mapTags(response: any[]): Tag[] {
        let r: Tag[] = [];
        for (let i = 0; i < response.length; i++) {
            const e = response[i];
            r.push(new Tag().set(e));
        }
        return r;
    }    
}

class TagServiceLogic extends TagServiceMap {

}

@Injectable()
export class TagService extends TagServiceLogic {

    constructor(
        @Inject('BASE_URL') baseUrl: string,
        private http: HttpClient) {

        super();
        this.set('tag', baseUrl, 'api/tag/');
    }

    get(): Observable<Tag[]> {
        if (this.isMocked()) {
            return this.getMock();
        }
        return this.http
            .get<Tag[]>(this.url + 'get')
            .pipe(map(this.mapTags));
    }

    private getMock(): Observable<Tag[]> {
        return new Observable((observer) => {
            let res: Tag[] = [];
            
            // TODO mock

            res.push(new Tag());

            observer.next(res);
            observer.complete();
        });
    }
}