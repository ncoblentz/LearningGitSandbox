$imageids = docker ps -q
foreach ($imageid in $imageids) {
	Write-Host "Removing and stopping $imageid"
	docker stop $imageid
	docker rm $imageid
}

docker rm web1
docker rm web2
docker rm loadbalancer
#docker rm database

dotnet restore
dotnet build
dotnet publish

cd Docker/Database
#docker build . -t postgresv2
docker run --name database -p5432:5432 -d postgresv2

cd ../..
docker build . -t aspnetcorekestrel
docker run --name web1 -d -p 5000:5000 aspnetcorekestrel
docker run --name web2 -d -p 5001:5000 aspnetcorekestrel

cd Docker/loadbalancer
docker build -t haproxy .
docker run --name loadbalancer -d --link web1 --link web2 -p 80:80 haproxy haproxy -f /usr/local/etc/haproxy/haproxy.cfg