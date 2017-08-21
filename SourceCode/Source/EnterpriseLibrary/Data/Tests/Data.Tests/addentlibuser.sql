if not exists (select * from master.dbo.syslogins where loginname = N'entlib')
BEGIN
	declare @logindb nvarchar(132), @loginlang nvarchar(132) select @logindb = N'Northwind', @loginlang = N'us_english'
	if @logindb is null or not exists (select * from master.dbo.sysdatabases where name = @logindb)
		select @logindb = N'master'
	if @loginlang is null or (not exists (select * from master.dbo.syslanguages where name = @loginlang) and @loginlang <> N'us_english')
		select @loginlang = @@language
	exec sp_addlogin N'entlib', 'hdf7&834k(*KA', @logindb, @loginlang
END
GO

USE [Northwind]
GO

if not exists (select * from dbo.sysusers where name = N'entlib')
	EXEC sp_grantdbaccess N'entlib', N'entlib'
GO

exec sp_addrolemember N'db_owner', N'entlib'
GO

