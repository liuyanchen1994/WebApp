import { Component, OnInit } from '@angular/core';
import { BingNewsService } from "app/Services/bing-news.service";
import { BingNewsListView } from "app/Models/ViewModels/BingNewsListView";
import { Toast } from "app/Helpers/Toast";

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
                    const start = element.url.indexOf("?r=");
                    element.url = element.url.substring(start + 3, element.url.length);
                    element.updatedTime = new Date(element.updatedTime);

                });
                this.news = arg;
            });
    }
}
