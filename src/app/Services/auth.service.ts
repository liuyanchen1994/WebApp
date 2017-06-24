import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/switchMap';
import { createVerify, getHashes } from "crypto-browserify";
import { ADAL } from "app/Helpers/ADAL";
import { Buffer } from "buffer";


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
  verifySignature(source, signature, publicKey): boolean {
    let verifier = createVerify('RSA-SHA256');
    // source = JSON.stringify(source);
    verifier.update(source);
    publicKey = this.insert_str(publicKey, '\n\r', 64);
    publicKey = '-----BEGIN CERTIFICATE-----\n\r' + publicKey + '-----END CERTIFICATE-----';
    let re = verifier.verify(publicKey, signature, 'base64');
    console.log(re);
    return re;
  }

  insert_str(str, insert_str, sn) {
    var newstr = "";
    for (var i = 0; i < str.length; i += sn) {
      var tmp = str.substring(i, i + sn);
      newstr += tmp + insert_str;
    }
    return newstr;
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

  getToken() {

    return;
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

  logout() {

  }
}