import { Component, OnInit } from '@angular/core';
import { BingNewsService } from 'app/Services/bing-news.service';
import { BingNewsListView } from 'app/Models/ViewModels/BingNewsListView';

@Component({
    selector: 'app-news',
    templateUrl: './news.component.html',
    styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {
    constructor() { }
    ngOnInit() {

    }

}
