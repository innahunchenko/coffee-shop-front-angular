FROM node:18 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install -g @angular/cli
#RUN npm install --legacy-peer-deps
RUN npm install
COPY . .
RUN ng build --configuration production
FROM nginx:alpine
RUN apk update && apk add --no-cache ca-certificates && update-ca-certificates
COPY nginx.conf /etc/nginx/nginx.conf
#RUN rm /usr/share/nginx/html/index.html
COPY --from=build /app/dist/coffee-shop/browser /usr/share/nginx/html

# Copy SSL certificates
COPY localhost.crt /etc/ssl/certs/localhost.crt
COPY localhost-key.pem /etc/ssl/private/localhost-key.pem

# Add SSL certificate to trusted certificates
RUN cp /etc/ssl/certs/localhost.crt /usr/local/share/ca-certificates/localhost.crt && \
    update-ca-certificates

COPY nginx.conf /etc/nginx/conf.d/default.conf
