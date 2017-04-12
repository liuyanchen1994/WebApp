import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { TitleBarComponent } from './Components/title-bar/title-bar.component';
import { NavMenuComponent } from './Components/nav-menu/nav-menu.component';
import { CatalogComponent } from './Components/catalog/catalog.component';
import { NewsListComponent } from './Components/news-list/news-list.component';
import { NewsComponent } from './Pages/news/news.component';
import { DownloadComponent } from './Pages/download/download.component';
import { NotFoundComponent } from './Pages/not-found/not-found.component';
import { BingNewsService } from './Services/bing-news.service';
import { BaseService } from "app/Services/base.service";

const appRoutes: Routes = [
  { path: 'news', component: NewsComponent },
  { path: 'download', component: DownloadComponent },
  // {
  //   path: 'heroes',
  //   component: HeroListComponent,
  //   data: { title: 'Heroes List' }
  // },
  // { path: '',
  //   redirectTo: '/heroes',
  //   pathMatch: 'full'
  // },
  { path: '**', component: NewsComponent }
];


@NgModule({
  declarations: [
    AppComponent,
    TitleBarComponent,
    NavMenuComponent,
    CatalogComponent,
    NewsListComponent,
    NewsComponent,
    DownloadComponent,
    NotFoundComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    RouterModule.forRoot(appRoutes)
  ],
  providers: [
    BaseService,
    BingNewsService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }