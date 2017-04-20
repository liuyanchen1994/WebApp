import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class LoginService {

    oauthUrl = 'https://login.microsoftonline.com/common/oauth2/v2.0/authorize';
    clientId = '6c76318e-9729-45b6-abf2-89440f6fc536';
    redirectUri = 'http://localhost:4200/login';
    constructor(private http: Http) { }

    login() {
        const url = this.oauthUrl
            + '?client_id=' + this.clientId
            + '&response_type=id_token+token&redirect_uri'
            + this.redirectUri
            + '&scope=openid%20https%3A%2F%2Fgraph.microsoft.com%2Fmail.read'
            + '&response_mode=fragment'
            + '&state=MSDev';

        return this.http.get(url)
            .map((response: Response) => {
                let re = response.json();
                console.log(re);

            });
    }
}