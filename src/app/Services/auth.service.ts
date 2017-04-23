import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/switchMap';
import 'jsrsasign';
import 'crypto';
import { ADAL } from "app/Helpers/ADAL";


@Injectable()
export class AuthService {
  ADOpenidConfUrl = 'https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration';
  githubOpnidConfUrl = '';
  oauthUrl = 'https://login.microsoftonline.com/common/oauth2/v2.0/authorize';
  clientId = '6c76318e-9729-45b6-abf2-89440f6fc536';
  redirectUri = 'http://localhost:4200/login';
  constructor(private http: Http) { }

  /**
   * 获取openid配置
   */
  getOpenIdConf() {
    return this.http.get('assets/jwtconf.json');
  };
  /**
   * 获取用于AD Azure验证的keys
   */
  getADKeys() {
    return this.http.get('assets/commonkeys.json');
  }
  /**
   * 验证jwt body
   * @param jwtbody 
   */
  validateJwtBody(jwtbody: any): boolean {
    let issuer = jwtbody.issuer;
    let exp = jwtbody.exp;
    let nonce = jwtbody.nonce;

    if (nonce != ADAL.prototype.nonce || issuer != ADAL.prototype.issuer
      || exp <= new Date().getMilliseconds()) {
      return false;
    }
    return true;
  }
  /**
   * TODO:验证签名
   */
  validateSignature(signature, publicKey): boolean {
    return true;
  }
  /**
   * 获取jwt openid配置
   * @param type microsoft | github
   */
  getJwts(type) {
    let url: string;
    if (type == 'microsoft') {
      url = this.ADOpenidConfUrl;
    } else {
      url = this.githubOpnidConfUrl;
    }
    return this.http.get(url)
      .map(res => {
        console.log(res);
      });
  }

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
  };
}