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
  jwt = null;
  constructor(private service: AuthService, private route: ActivatedRoute) { }

  ngOnInit() {
    let adal = new ADAL('6c76318e-9729-45b6-abf2-89440f6fc536', 'http://localhost:4200/login')
    this.oauthUrl = adal.getOAuthUrl();
    this.getADFragment();
  }
  /**
   * 获取微软AD返回参数
   */
  getADFragment() {
    console.log('enter getADFragment');

    let fragment = this.route.snapshot.fragment;
    if (fragment != null && typeof (fragment) != 'undefined') {
      let params = fragment.split('&');
      let id_token = params.find(m => m.includes('id_token'));
      let jwt = id_token.split('=')[1];
      this.jwt = jwt;
      console.log(jwt);

      this.jwtHeader = this.parseJwt(jwt, 0);
      this.jwtBody = this.parseJwt(jwt, 1);
      this.signature = jwt.split('.')[2];

      console.log(this.jwtBody);
      console.log(this.jwtHeader);
      console.log(this.signature);

      this.getOIDC();
    }
  }

  /**
   * 获取openid config
   */
  getOIDC() {
    console.log('enter getOIDC');

    this.service.getOpenIdConf()
      .subscribe(arg => {
        this.jwtconf = arg.json();
        this.service.getADKeys()
          .subscribe(arg => {
            let keys = arg.json().keys;
            keys.forEach(element => {
              if (element['kid'] === this.jwtHeader.kid) {
                this.publickKey = element.x5c[0];
                this.issuer = element.issuer;
              }
            });

            //构造原始验证数据
            let data = this.jwt.split('.');
            data = data[0] + '.' + data[1];
            this.service.verifySignature(data, this.signature, this.publickKey);
          });
      });
  }


  parseJwt(token, number?) {
    var base64 = token.split('.')[number];
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
