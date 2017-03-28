import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { TitleBarComponent } from './Components/title-bar/title-bar.component';
import { NavMenuComponent } from './Components/nav-menu/nav-menu.component';
import { CatalogComponent } from './Components/catalog/catalog.component';


@NgModule({
  declarations: [
    AppComponent,
    TitleBarComponent,
    NavMenuComponent,
    CatalogComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
