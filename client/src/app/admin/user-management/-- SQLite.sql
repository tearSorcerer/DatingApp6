-- SQLite
SELECT UserName,PasswordHash
FROM AspNetUsers;


update AspNetUsers
set PasswordHash = 'AQAAAAIAAYagAAAAEJTQARH4V1ljzBReUMpI9LnSfgDAhVraIX9XH1kGKzoBFTNKs+KGisygVy7VQU6AwA=='
where UserName = 'bob'