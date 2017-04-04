import { Component, OnInit } from '@angular/core';
import { BingNewsService } from "app/Services/bing-news.service";
import { BingNewsListView } from "app/Models/ViewModels/BingNewsListView";

@Component({
    selector: 'app-news-list',
    templateUrl: './news-list.component.html',
    styleUrls: ['./news-list.component.css']
})
export class NewsListComponent implements OnInit {
    news: BingNewsListView[];
    constructor(private service: BingNewsService) { }

    ngOnInit() {

        this.service.get()
            .subscribe(arg => {
                arg.forEach(element => {
                    element.url = "";
                });
                this.news = arg;
            });
    }

}
