import { Component, OnInit } from '@angular/core';
import { LoginService } from "app/Services/login.service";
import { ADAL } from "app/Helpers/ADAL";
import { ActivatedRoute, Params } from "@angular/router";




@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    oauthUrl = '';
    constructor(private service: LoginService, private route: ActivatedRoute) { }

    ngOnInit() {
        let adal = new ADAL('6c76318e-9729-45b6-abf2-89440f6fc536', 'http://localhost:4200/login')
        this.oauthUrl = adal.getOAuthUrl();

        let params = this.route.snapshot.fragment.split('&');
        let id_token = params.find(m => m.includes('id_token'));
        console.log(id_token.split('=')[1]);

    }

    signIn() {
        window.open(this.oauthUrl);
    }

    userSignedIn(err, token) {
        console.log('userSignedIn called');
        if (!err) {
            console.log("token: " + token);
            this.showWelcomeMessage();
        }
        else {
            console.error("error: " + err);
        }
    }

    showWelcomeMessage() {

    }

}
