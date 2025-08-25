import js from '@eslint/js';
import globals from 'globals';
import tseslint from 'typescript-eslint';
import stylistic from '@stylistic/eslint-plugin'
import reactHooks from 'eslint-plugin-react-hooks';
import reactRefresh from 'eslint-plugin-react-refresh';
import noRelativeImports from 'eslint-plugin-no-relative-import-paths';
import { globalIgnores } from 'eslint/config'

export default tseslint.config([
    globalIgnores(['dist']),
    {
        // Note: there should be no other properties in this object
        ignores: [
            '*.config.{js,mjs,cjs,ts,mts,cts}',
            'aspnetcore*.{js,ts}'
        ],
    },
    {
        files: ['**/*.{ts,tsx}'],
        plugins: {
            'no-relative-import-paths': noRelativeImports
        },
        extends: [
            js.configs.recommended,
            tseslint.configs.recommended,
            reactHooks.configs['recommended-latest'],
            reactRefresh.configs.vite,
            stylistic.configs.recommended
        ],
        languageOptions: {
            ecmaVersion: 2020,
            globals: globals.browser,
        },
    },
    {
        rules: {
            'no-debugger': 'warn',
            'no-empty': 'warn',
            'no-extra-boolean-cast': 'off',

            // max characters per line
            'max-len': [
                'warn',
                { 'code': 120 }
            ],

            // no ../../folder imports
            'no-relative-import-paths/no-relative-import-paths': [
                'error',
                { 'allowSameFolder': true }
            ],

            // @stylistic
            '@stylistic/indent': ['warn', 4],
            '@stylistic/indent-binary-ops': ['warn', 4],
            '@stylistic/jsx-indent-props': ['warn', 4],
            '@stylistic/quotes': ['error', 'single'],
            '@stylistic/semi': ['error', 'always'],
            '@stylistic/no-extra-semi': ['error'],
            '@stylistic/quote-props': ["off"],
            '@stylistic/comma-dangle': ['off'],
            '@stylistic/multiline-ternary': ['off'],
            '@stylistic/jsx-first-prop-new-line': ['off'],
            '@stylistic/jsx-max-props-per-line': ['off'],

            // @typescript
            '@typescript-eslint/no-unused-vars': 'off',
            '@typescript-eslint/no-unused-expressions': 'off',

            // import restrictions
            '@typescript-eslint/no-restricted-imports': [
                'error',
                {
                    'patterns': [
                        {
                            // Restrict import of whole font awesome library for reducing bundle size.
                            // https://stackoverflow.com/questions/57935672/how-to-reduce-fontawesome-bundle-size
                            'group': ['@fortawesome/*-icons', '!@fortawesome/*-icons/'],
                            'message': 'Use import of concrete icons from @fortawesome/**/iconName instead'
                        }
                    ]
                }
            ]
        }
    }
]);
