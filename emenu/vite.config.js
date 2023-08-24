import path from 'path';
import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import { VitePWA } from 'vite-plugin-pwa';
import proxyOptions from './proxyOptions';


// https://vitejs.dev/config/
export default defineConfig({
	plugins: [
		vue(),
		VitePWA({      
			manifest: { 
				name:"emenu",
				short_name:"ePOS Menu QR",
			  icons: [
				{
				  src: "/emenu.png",
				  sizes: "16x16",
				  type: "image/png",
				  purpose: "any maskable"
				}
			  ]
			},
			 
			devOptions: {
				enabled: true
			}
		  })
	],
	server: {
		port: 8080,
		proxy: proxyOptions
	},
	resolve: {
		alias: {
			'@': path.resolve(__dirname, 'src')
		}
	},
	build: {
		outDir: '../epos_restaurant_2023/public/emenu',
		emptyOutDir: true,
		// target: 'es2015',
		target: 'esnext'
	},
});
