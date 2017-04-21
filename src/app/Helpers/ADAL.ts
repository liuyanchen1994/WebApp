/**
 * ADAL
 */
export class ADAL {
    instance = 'https://login.microsoftonline.com/';
    tenant = 'common/oauth2/v2.0/authorize?';
    clientId: string;
    redirectUri: string;

    constructor(clientId, redirectUri) {
        this.clientId = clientId;
        this.redirectUri = redirectUri;
    }
    getOAuthUrl() {
        return this.instance + this.tenant
            + 'client_id=' + this.clientId
            + '&redirect_uri=' + this.redirectUri
            + '&response_type=id_token'
            + '&scope=openid'
            + '&response_mode=fragment'
            + '&state=MSDev'
            + '&nonce=131323';
    }
}