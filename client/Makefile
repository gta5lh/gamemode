copy:
	@rm -rf ../../game-server/client_packages/cs_packages/gamemode/*
	@rsync -r --include=" *" --exclude=".vs" --exclude="/bin/" --exclude="/obj/" --exclude=".gitignore" --exclude="GamemodeClient.csproj" --exclude="GamemodeClient.sln" --exclude="stylecop.json" --exclude=".editorconfig" --exclude="/Properties/" --exclude="Makefile" --exclude="/dependencies/" --prune-empty-dirs . ../../game-server/client_packages/cs_packages/gamemode/

build-ui:
	@npm --prefix ../ui run build
	@rsync -r --include=" *" ../ui/build ../../game-server/client_packages/cs_packages/gamemode/Frontend/v2/
