docker-build:
	@sudo docker build . -t ghcr.io/gta5lh/gameserver:latest

docker-push:
	@docker push ghcr.io/gta5lh/gameserver:latest

rollout:
	@kubectl rollout restart deployment gameserver

deploy:	docker-build docker-push rollout
