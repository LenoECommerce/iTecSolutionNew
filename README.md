# Deployment

unter `.github/workflows` liegt die `deploy.yml` Datei. Die macht, dass beim push auf den main branch automatisch ein neues release gebaut wird

# Server-Einrichtung

Wie man nginx auf einem ubuntu server installiert:
[https://www.digitalocean.com/community/tutorials/how-to-install-nginx-on-ubuntu-20-04](https://www.digitalocean.com/community/tutorials/how-to-install-nginx-on-ubuntu-20-04)


Zuerst `sudo apt-get update && sudo apt-get upgrade`

Dann `sudo apt install nginx`.

Jetzt haben wir einen Server unter [http://212.227.140.237/](http://212.227.140.237/)

Unter nginx/ liegt die Konfiguration für nginx

hallo :)