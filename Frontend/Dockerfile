FROM node:18.14.2 AS build-stage

RUN mkdir /app
WORKDIR /app

COPY package.json ./

RUN npm install --force

COPY . .

RUN npm run build --force

FROM nginxinc/nginx-unprivileged
COPY --from=build-stage /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
CMD ["nginx", "-g", "daemon off;"]