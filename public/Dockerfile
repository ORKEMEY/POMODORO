#build
FROM node:14.15-alpine as build
WORKDIR /app
COPY public/ .

#run
FROM nginx:stable-alpine
COPY / /usr/share/nginx/html
# COPY nginx/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]