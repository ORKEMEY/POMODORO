FROM node:14.15
EXPOSE 8080
WORKDIR /usr/src/app
COPY root-service/package*.json ./
RUN npm install
COPY root-service/ .
CMD [ "node", "server.js" ]