import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ResultJson } from "app/Models/Common/ResultJson";
import { Toast } from "app/Helpers/Toast";


@Injectable()
export class BaseService {
    apiUrl = 'https://api.msdev.cc';
    constructor(private http: Http) {
    }

    get<T>(url: string, params?: any): Observable<T> {
        // TODO:判断url是否包含http，为完整地址
        url = this.apiUrl + url;
        if (params != null) {
            url += '?';
            for (let key in params) {
                if (params.hasOwnProperty(key)) {
                    url += key + '=' + encodeURI(params[key]) + '&';
                }
            }
        }
        return this.http.get(url)
            .map((res: Response) => {
                const re: ResultJson = res.json();
                console.log(re);
                if (re.errorCode === 0) {
                    return re.data;
                } else {
                    console.log('failed');
                    Toast.Error('加载失败');
                }
            })
            .catch(this.handleError);
    }

    delete<T>(url: string, params?: any): Observable<T> {
        // TODO:判断url是否包含http，为完整地址
        url = this.apiUrl + url;
        if (params != null) {
            url += '?';
            for (let key in params) {
                if (params.hasOwnProperty(key)) {
                    url += key + '=' + params[key] + '&';
                }
            }
        }
        return this.http.get(url)
            .map((res: Response) => {
                const re: ResultJson = res.json();
                console.log(re);
                if (re.errorCode === 0) {
                    return re.data;
                } else {
                    console.log('failed');

                    Toast.Error('删除失败');
                }
            })
            .catch(this.handleError);
    }

    post<T>(url: string, param?: any): Observable<T> {
        let params = new URLSearchParams();
        for (let key in param) {
            if (param.hasOwnProperty(key)) {
                params.append(key, param[key]);
            }
        }
        return this.http.post(url, params)
            .map((res: Response) => {
                let re: ResultJson = res.json();
                if (re.errorCode === 0) {
                    return re.data;
                } else {
                    Toast.Error('提交失败');
                }
            })
            .catch(this.handleError);
    }

    put<T>(url: string, param?: any): Observable<T> {
        let params = new URLSearchParams();
        for (let key in param) {
            if (param.hasOwnProperty(key)) {
                params.append(key, param[key]);
            }
        }
        return this.http.post(url, params)
            .map((res: Response) => {
                let re: ResultJson = res.json();
                if (re.errorCode === 0) {
                    return re.data;
                } else {
                    Toast.Error('提交失败');
                }
            })
            .catch(this.handleError);
    }
    /**
    * 处理http错误
    */
    private handleError(error: any) {
        console.log('http handle Error');
        let errMsg = (error.message) ? error.message :
            error.status ? `error.status - error.statusText` : 'Server error';
        console.error(errMsg); // log to console instead
        return Observable.throw(errMsg);
    }

}