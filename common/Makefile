copy:
	@rm -rf ../game-server/client_packages/cs_packages/gamemode-common/*
	@rsync -r --include=" *" --exclude=".vs" --exclude="/bin/" --exclude="/obj/" --exclude=".gitignore" --exclude="GamemodeCommon.csproj" --exclude="GamemodeCommon.sln" --exclude="stylecop.json" --exclude=".editorconfig" --exclude="/Properties/" --exclude="Makefile" --exclude="/dependencies/" --exclude="/.git/" --prune-empty-dirs . ../game-server/client_packages/cs_packages/gamemode-common/
