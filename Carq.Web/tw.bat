@echo off
SET NODE_ENV=production
npx tailwindcss -i wwwroot/css/_tw.css -o wwwroot/css/_tw.min.css --minify && minify wwwroot/js/site.js > wwwroot/js/site.min.js
