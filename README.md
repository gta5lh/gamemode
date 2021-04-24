docker run --name postgresql -p 5432:5432 -e POSTGRES_PASSWORD=postgres -d postgres


EF Core:

Adding a Migration:

dotnet ef migrations add MyFirstMigration

Creating or Updating the Database:

dotnet ef database update

Removing a Migration:

dotnet ef migrations remove

Reverting a Migration:

dotnet ef database update MyFirstMigration

pgcli "postgres://postgres:postgres@localhost/gta?sslmode=disable"