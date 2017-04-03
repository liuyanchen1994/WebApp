import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ResultJson } from "app/Models/Common/ResultJson";
import { Toast } from "app/Helpers/Toast";


@Injectable()
export class BaseService {
    constructor(private http: Http) {
    }
    get<T>(url: string): Observable<T> {
        return this.http.get(url)
            .map((res: Response) => {
                var re: ResultJson = res.json();
                console.log(re);
                if (re.errorCode == 0) {
                    return re.data;
                } else {
                    console.log("failed");

                    Toast.Error("加载失败");
                }
            })
            .catch(this.handleError);
    }
    post<T>(url: string, param: any): Observable<T> {
        let params = new URLSearchParams();
        for (var key in param) {
            if (param.hasOwnProperty(key)) {
                params.append(key, param[key]);
            }
        }
        return this.http.post(url, params)
            .map((res: Response) => {
                var re: ResultJson = res.json();
                if (re.errorCode == 0) {
                    return re.data;
                } else {
                    Toast.Error("加载失败");
                }
            })
            .catch(this.handleError);
    }

    /**
    * Handle HTTP error
    */
    private handleError(error: any) {
        let errMsg = (error.message) ? error.message :
            error.status ? `error.status - error.statusText` : 'Server error';
        console.error(errMsg); // log to console instead
        return Observable.throw(errMsg);
    }
}