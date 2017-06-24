import { Injectable } from '@angular/core';
import { Observable } from "rxjs/Observable";
import { BaseService } from "app/Services/base.service";
import { Catalog } from "app/Models/Entities/Catalog";

@Injectable()
export class CatalogService {

    constructor(private service: BaseService) { }


    /**
     * 获取目录列表
     */
    getCatalogList(): Observable<Catalog[]> {
        const url = '/api/manage/Catalog/GetList';
        return this.service.get<Catalog[]>(url);
    }
}

