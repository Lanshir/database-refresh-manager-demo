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
            'no-relative-import-paths': noRelativeImports,
            '@stylistic': stylistic
        },
        extends: [
            js.configs.recommended,
            tseslint.configs.recommended,
            reactHooks.configs['recommended-latest'],
            reactRefresh.configs.vite,
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
            '@stylistic/quotes': ['error', 'single'],
            '@stylistic/indent': ['warn', 4],

            // @typescript
            '@typescript-eslint/no-unused-vars': 'off',
            '@typescript-eslint/no-unused-expressions': 'off',
            '@typescript-eslint/no-empty-object-type': 'off',

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
