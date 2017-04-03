import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import { BingNewsListView } from 'app/Models/ViewModels/BingNewsListView';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { BaseService } from "app/Services/base.service";

@Injectable()
export class BingNewsService {


  constructor(private service: BaseService) { }
  // getNews(): Observable<BingNewsListView[]> {
  //   return this.http
  //     .get('http://api.msdev.cc/api/BingNews/PageList/1')
  //     .map((response: Response) => response.json());
  // }

  get(): Observable<BingNewsListView[]> {
    let url = "http://api.msdev.cc/api/BingNews/PageList/1";
    return this.service.get<BingNewsListView[]>(url);

  }
}