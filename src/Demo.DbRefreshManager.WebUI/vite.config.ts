import { ProxyOptions, UserConfig, defineConfig } from 'vite';
import react from '@vitejs/plugin-react-swc';
import tsConfigPaths from 'vite-tsconfig-paths';
import eslint from 'vite-plugin-eslint';
import svgrPlugin from 'vite-plugin-svgr';
import fs from 'fs';
import { certFilePath, keyFilePath } from './aspnetcore-https';

const devHttpsProxy: ProxyOptions = {
    target: 'https://localhost:5001',
    secure: false
};

// https://vitejs.dev/config/
export default defineConfig(({ command }) => {
    const baseConfig: UserConfig = {
        plugins: [react(), tsConfigPaths(), eslint(), svgrPlugin()],
        build: { outDir: 'build' }
    };

    switch (command) {
        case 'serve': return {
            ...baseConfig,
            server: {
                strictPort: true,
                port: 3001,
                https: {
                    key: fs.readFileSync(keyFilePath),
                    cert: fs.readFileSync(certFilePath),
                },
                proxy: {
                    '/api': devHttpsProxy,
                    '/swagger': devHttpsProxy,
                    '/graphql': { ...devHttpsProxy, ws: true },
                    '/healthz': devHttpsProxy,
                    '/env': devHttpsProxy
                }
            }
        };

        default: return baseConfig;
    }
});