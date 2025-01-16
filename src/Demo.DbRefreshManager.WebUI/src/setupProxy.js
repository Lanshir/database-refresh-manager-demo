const { createProxyMiddleware } = require('http-proxy-middleware');

const context = ['/api', '/env', '/swagger', '/graphql', '/healthz'];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'https://localhost:5001',
        ws: true,
        secure: false,
        followRedirects: true
    });

    app.use(appProxy);
};
