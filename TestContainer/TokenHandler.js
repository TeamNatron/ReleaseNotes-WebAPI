class TokenHandler {
  constructor() {
    if (!TokenHandler.instance) {
      TokenHandler.instance = this;
    }
    return TokenHandler.instance;
  }

  accessToken;

  setAccessToken = token => {
    this.accessToken = token;
  };

  getAccessToken = () => {
    return this.accessToken;
  };
}

const instance = new TokenHandler();

module.exports = instance;
