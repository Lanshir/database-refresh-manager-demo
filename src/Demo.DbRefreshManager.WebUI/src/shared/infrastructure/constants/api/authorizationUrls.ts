/**
 * Url методов авторизации.. 
 */
const AuthorizationUrls = {
    /** GET */
    checkAuth: 'api/v1/authorization',
    /** POST */
    authorize: 'api/v1/authorization',
    /** DELETE */
    deauthorize: 'api/v1/authorization'
};

export default AuthorizationUrls as Readonly<typeof AuthorizationUrls>;