import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

const isLocal = env.NODE_ENV === 'development' 
    && !env.CI 
    && !env.VERCEL 
    && !env.NETLIFY 
    && !env.GITHUB_ACTIONS
    && !env.GITLAB_CI
    && !env.BUILDKITE;

const certificateName = "vueapp1.client";
let baseFolder;
let certFilePath;
let keyFilePath;
let httpsOptions = false;

if (isLocal) {
    baseFolder =
        env.APPDATA !== undefined && env.APPDATA !== ''
            ? `${env.APPDATA}/ASP.NET/https`
            : `${env.HOME}/.aspnet/https`;

    certFilePath = path.join(baseFolder, `${certificateName}.pem`);
    keyFilePath = path.join(baseFolder, `${certificateName}.key`);

    if (!fs.existsSync(baseFolder)) {
        fs.mkdirSync(baseFolder, { recursive: true });
    }

    if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
        try {
            if (0 !== child_process.spawnSync('dotnet', [
                'dev-certs',
                'https',
                '--export-path',
                certFilePath,
                '--format',
                'Pem',
                '--no-password',
            ], { stdio: 'inherit' }).status) {
                throw new Error("Could not create certificate.");
            }
        } catch (err) {
            console.warn('Warning: Could not create HTTPS certificate. HTTPS will be disabled.', err.message);
            httpsOptions = false;
        }
    }

    httpsOptions = {
        key: fs.readFileSync(keyFilePath),
        cert: fs.readFileSync(certFilePath),
    };
}

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7114';

// https://vitejs.dev/config/
export default defineConfig({
    base: env.VERCEL ? '/' : '/machine-finder/',
    // plugins: [vue()],
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            '^/weatherforecast': {
                target,
                secure: false
            }
        },
        port: parseInt(env.DEV_SERVER_PORT || '60665'),
        https: httpsOptions
    }
})
