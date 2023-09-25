FROM node:18-alpine as build

ENV PATH /app/node_modules/.bin:$PATH
RUN npm i -g npm

WORKDIR /usr/src/app
COPY package.json ./
COPY package-lock.json ./

RUN npm ci 
COPY . .
RUN npm run build

# production environment
FROM nginx:stable-alpine

COPY --from=build /usr/src/app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

