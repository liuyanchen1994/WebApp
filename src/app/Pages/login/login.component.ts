import { Component, OnInit } from '@angular/core';
import { ADAL } from 'app/Helpers/ADAL';
import { ActivatedRoute, Params } from '@angular/router';
import { AuthService } from 'app/Services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  oauthUrl = '';
  publickKey: string;//公钥
  signature: string; //签名
  issuer: string; //颂布方
  jwtconf: any;
  keys: any;
  jwtHeader = null;
  jwtBody = null;
  constructor(private service: AuthService, private route: ActivatedRoute) { }

  ngOnInit() {
    let adal = new ADAL('6c76318e-9729-45b6-abf2-89440f6fc536', 'http://localhost:4200/login')
    this.oauthUrl = adal.getOAuthUrl();

  }
  /**
   * 获取微软AD返回参数
   */
  getADFragment() {
    let fragment = this.route.snapshot.fragment;
    if (fragment != null && typeof (fragment) != 'undefined') {
      let params = fragment.split('&');
      let id_token = params.find(m => m.includes('id_token'));
      let jwt = id_token.split('=')[1];

      this.jwtHeader = this.parseJwt(jwt, 0);
      this.jwtBody = this.parseJwt(jwt, 1);
      this.signature = jwt.split('.')[2];

      this.getOIDC();
    }
  }

  /**
   * 获取openid config
   */
  getOIDC() {
    this.service.getOpenIdConf()
      .subscribe(arg => {
        this.jwtconf = arg.json();
        this.service.getADKeys()
          .subscribe(arg => {
            let keys = arg.json().keys;
            keys.forEach(element => {
              if (element['kid'] === this.jwtHeader.kid) {
                this.publickKey = element.n;
                this.issuer = element.issuer;
                console.log(element);
              }
            });
          });
      });
  }


  parseJwt(token, number) {
    var base64Url = token.split('.')[number];
    var base64 = base64Url.replace('+', '-').replace('/', '_').replace('=', '');
    return JSON.parse(window.atob(base64));
  };
  signIn() {
    window.location.href = this.oauthUrl;
  }

  userSignedIn(err, token) {
    console.log('userSignedIn called');
    if (!err) {
      console.log('token: ' + token);
      this.showWelcomeMessage();
    }
    else {
      console.error('error: ' + err);
    }
  }

  showWelcomeMessage() {

  }

}
