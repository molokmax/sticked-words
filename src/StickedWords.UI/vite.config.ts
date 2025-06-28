import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/
export default defineConfig({
  build: {
    outDir: '../StickedWords.API/wwwroot',
    emptyOutDir: true,
    // watch: true,    // Включить watch-режим
    minify: false,  // Отключить минификацию
    sourcemap: true, // Включить sourcemaps
  },
  plugins: [
    vue(),
    vueDevTools(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    },
  },
})
