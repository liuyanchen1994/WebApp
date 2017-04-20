import { Component, OnInit } from '@angular/core';
import { LoginService } from "app/Services/login.service";

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

    constructor(private service: LoginService) { }

    ngOnInit() {
        this.service.login()
            .subscribe(arg => {
                console.log(arg);

            }
            );


    }



}
