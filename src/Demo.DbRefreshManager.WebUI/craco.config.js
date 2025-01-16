const CracoAlias = require('craco-alias');
const CracoEsbuild = require('craco-esbuild');
const CracoSwc = require('craco-swc');

module.exports = {
    plugins: [
        {
            plugin: CracoAlias,
            options: {
                source: 'tsconfig',
                // baseUrl SHOULD be specified
                // plugin does not take it from tsconfig
                baseUrl: './src',
                /* tsConfigPath should point to the file where "baseUrl" and "paths" 
                are specified*/
                tsConfigPath: './tsconfig.paths.json'
            }
        },
        {
            plugin: CracoEsbuild,
            skipEsbuildJest: false
        },
        {
            plugin: CracoSwc
        }
    ],
};