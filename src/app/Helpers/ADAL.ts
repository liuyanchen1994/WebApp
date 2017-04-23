/**
 * ADAL
 */
export class ADAL {
  public instance = 'https://login.microsoftonline.com/';
  public tenant = 'common/oauth2/v2.0/authorize?';
  public clientId: string;
  public redirectUri: string;
  public nonce = 'msdev.cc';
  public issuer = 'https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0';


  constructor(clientId, redirectUri) {
    this.clientId = clientId;
    this.redirectUri = redirectUri;
  }
  getOAuthUrl() {
    return this.instance + this.tenant
      + 'client_id=' + this.clientId
      + '&redirect_uri=' + this.redirectUri
      + '&response_type=id_token+id_token'
      + '&scope=openid+profile'
      + '&response_mode=fragment'
      + '&state=MSDev'
      + '&nonce=' + this.nonce;
  }
}