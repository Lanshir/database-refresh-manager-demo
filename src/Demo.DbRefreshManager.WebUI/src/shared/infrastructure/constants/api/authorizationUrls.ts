/**
 * Url методов авторизации..
 */
const AuthorizationUrls = {
    /** GET */
    checkAuth: 'api/v1/auth/login',
    /** POST */
    authorize: 'api/v1/auth/login',
    /** DELETE */
    deauthorize: 'api/v1/auth/login'
};

export default AuthorizationUrls as Readonly<typeof AuthorizationUrls>;
