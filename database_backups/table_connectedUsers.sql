create table ConnectedUsers (
	Id varchar(128) not null,
	Nome varchar(max),
	ConnectionId varchar(128),
	CONSTRAINT PK_ConnectedUsers PRIMARY KEY (Id)
);