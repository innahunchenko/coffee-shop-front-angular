FROM node:18 AS build
WORKDIR /app
COPY ./CoffeeShop.Client/CoffeeShop.Client/package*.json ./
RUN npm install -g @angular/cli
#RUN npm install --legacy-peer-deps
RUN npm install
COPY ./CoffeeShop.Client/CoffeeShop.Client ./
RUN ng build --configuration production
FROM nginx:alpine
COPY ./CoffeeShop.Client/CoffeeShop.Client/nginx.conf /etc/nginx/nginx.conf
#RUN rm /usr/share/nginx/html/index.html
COPY --from=build /app/dist/coffee-shop/browser /usr/share/nginx/html

COPY ./CoffeeShop.Client/CoffeeShop.Client/nginx.conf /etc/nginx/conf.d/default.conf
