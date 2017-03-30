import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import { BingNewsListView } from 'app/Models/ViewModels/BingNewsListView';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class BingNewsService {
  constructor(private http: Http) { }


  getNews(): Observable<BingNewsListView[]> {
    return this.http
      .get('http://api.msdev.cc/api/BingNews/PageList')
      .map((response: Response) => {
        console.log(response.json());
        return response.json();
      });
  }

}