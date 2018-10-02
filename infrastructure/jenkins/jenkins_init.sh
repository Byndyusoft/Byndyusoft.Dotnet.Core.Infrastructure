mkdir -p /var/jenkins_home/jobs && \
mkdir -p /var/jenkins_home/plugins && \
cp -r jobs/ /var/jenkins_home/ && \
cp -r plugins/ /var/jenkins_home/ && \
export dockerpath=$(which docker) && \
docker-compose build && \
docker stack deploy --with-registry-auth --compose-file docker-compose.yml bsapp