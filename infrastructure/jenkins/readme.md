# Byndyusoft.Dotnet.Core.Infrastructure build with jenkins on docker swarm. 

This project is a preparation for creating a development stack. It includes loging system and monitoring components. Not recommended for use in production environment without additional settings and improvements.

## Prerequisites
* 4Gb 
* 20GB free disk space
* OS Linux compatible with docker (https://success.docker.com/article/compatibility-matrix)
* docker 18.6, docker-compose 1.22 (installed)
* git
* Internet connection for downloading docker images and repository.

## Deploy docker swarm
1. Docker swarm init  (docs: https://docs.docker.com/engine/reference/commandline/swarm_init/)

##  Deploy Jenkins CI in docker swarm
1. Git clone repository

2. ```cd <your dir>/Byndyusoft.Dotnet.Core.Infrastructure/infrastructure/jenkins & ./jenkins_init.sh  ```
	2.1 Result may be checked with bash command: ```$ docker service ls ```:
```sh
ID                  NAME                MODE                REPLICAS            IMAGE                       PORTS
5xsllh3y9l6f        bsapp_jenkins-ui    replicated          1/1                 jenkins/jenkins-bs:latest   *:8080->8080/tcp
```

3. Get password: 
```sh 
docker exec -it $(docker ps | grep bsapp_jenkins-ui | awk '{print $1}') tail /var/jenkins_home/secrets/initialAdminPassword
```
	
4. Go to login page (http://<server_ip/server_name>:8080/)  - paste password from 4, install sugested plugins, create first account and restart Jenkins

Installing plugins may take time...
![Swagger](https://raw.githubusercontent.com/SStolbov/Byndyusoft.Dotnet.Core.Infrastructure/master/infrastructure/jenkins/imgs/jenkins_plugins_install.png)


## Deploy infrastructure & Demo application
1.  Check integration job and run it to deploy monitoring stack
![Jenkins jobs](https://raw.githubusercontent.com/SStolbov/Byndyusoft.Dotnet.Core.Infrastructure/master/infrastructure/jenkins/imgs/jenkins_jobs.png)
2. Connect to grafana on port :3000 and set password for user admin = ```admin``` (It configured in jenkinsfile for application)
3. Deploy Demo-app 
Web application from https://github.com/Byndyusoft/Byndyusoft.Dotnet.Core.Infrastructure
```It will be deployed and available on http://</server_name>:5000/swagger/index.html```
![Swagger](https://raw.githubusercontent.com/SStolbov/Byndyusoft.Dotnet.Core.Infrastructure/master/infrastructure/jenkins/imgs/demo-app-swagger.png)

## Monitoring docker services
It uses prometheus with Grafana. It's maily a fork from https://github.com/stefanprodan/swarmprom 
Alertmanager used to send notifications.  Grafana from 5.2 also can send notifications
So for monitoring simple alerts just need to set notification method.

## Logging
ELK + logspout
After deploy logspout read console outputs from containers (without docker environment variable "LOGSPOUT=ignore" & and contains "_app" in name)  and send it to logstash. Lostash parse (docker + json) logs to elasticsearch service. Kibana & Grafana visualize data from elasticsearch. Grafna also can be used for alerts in logging system (e. g. logs with error level filltered for service "app" is more then 100 in last 1 minute)

## Management 
Portainer for Docker services deploed in build infrastructure step. It runs on port :9000. Password must be set on first login.
![Portainer](https://raw.githubusercontent.com/SStolbov/Byndyusoft.Dotnet.Core.Infrastructure/master/infrastructure/jenkins/imgs/portainer.png)


### Todos

 - Use K8s for deploy infrustructure
 - Service discovery (consul)
 - RabbitMQ management\backup\deploy from config