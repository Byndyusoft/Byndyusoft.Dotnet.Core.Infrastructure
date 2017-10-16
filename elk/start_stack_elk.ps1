# Prepare script to run on boot2docker on windows
docker-machine.exe env | Invoke-Expression #set bott2docker environment
docker-machine ssh default  sudo sysctl -w vm.max_map_count=262144 #To use in boot2docker
$docker_ip=$(docker-machine ip)+":2377"  #Need to be in swarm
docker-machine ssh default sudo docker swarm init --advertise-addr $docker_ip  #Need to be in swarm
set COMPOSE_CONVERT_WINDOWS_PATHS=1;
docker-machine ssh default sudo mkdir -p /mnt/sda1/var/portainer/data
docker-machine ssh default sudo mkdir -p /mnt/sda1/logstash/
docker-machine ssh default  sudo mkdir -p /mnt/sda1/data/elasticsearch/nodes
$env:Path += ";C:\Program Files\Git\usr\bin"  #set for use scp.exe from git;
docker-machine scp logstash.conf default:/mnt/sda1/tmp;
docker-machine ssh default sudo cp /mnt/sda1/tmp/logstash.conf /mnt/sda1/logstash
docker stack deploy --compose-file docker-compose.yml bsapp


#remove stack: docker stack rm bsapp



# optional BS Infra deploy
cd ..
docker build -t bs-infra .
docker-machine ssh default sudo rm -r /mnt/sda1/tmp/out
docker-machine ssh default sudo docker run  --rm -w /app/src/Web.Application -v /mnt/sda1/tmp/out:/app/src/Web.Application/out  bs-infra dotnet publish -c Release -o /app/src/Web.Application/out
docker-machine ssh default "cd /mnt/sda1/tmp/out && sudo docker build -f /mnt/sda1/tmp/out/dockerfile.prd  -t bs-infra-img ."
# docker rm -f bs-infra-srv
# docker run -d -p 5001:5000  --name bs-infra-srv bs-infra-img
docker-machine ssh default "if [ $(docker service ls -f name=bs-infra-srv | grep bs-infra-srv|awk '{print $2}') ] ; then docker service rm bs-infra-srv; fi"
docker-machine ssh default docker service create -td --replicas 1 --network=bsapp_bsnet --constraint 'node.role==manager' --publish 5001:5000  --name bs-infra-srv bs-infra-img
docker service ls