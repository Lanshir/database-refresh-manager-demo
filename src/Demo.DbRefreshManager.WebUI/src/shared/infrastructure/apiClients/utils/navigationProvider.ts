/** Провайдер функций навигации. */
const NavigationProvider: NavigationProviderType = {
    /** Редирект на страницу авторизации. */
    toLoginPage: () => {}
};

type NavigationProviderType = {
    toLoginPage: () => void
};

export default NavigationProvider;