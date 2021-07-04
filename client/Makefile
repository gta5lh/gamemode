copy:
	@rm -rf ../../game-server/client_packages/cs_packages/gamemode/*
	@rsync -r --include=" *" --exclude=".vs" --exclude="/bin/" --exclude="/obj/" --exclude=".gitignore" --exclude="GamemodeClient.csproj" --exclude="GamemodeClient.sln" --exclude="stylecop.json" --exclude=".editorconfig" --exclude="/Properties/" --exclude="Makefile" --exclude="/dependencies/" --prune-empty-dirs . ../../game-server/client_packages/cs_packages/gamemode/
