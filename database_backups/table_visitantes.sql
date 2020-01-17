create table dbo.Visitante
(
	Id INT Identity NOT NULL,
	Nome varchar(MAX),
	Email varchar(MAX),
	Celular varchar(MAX),
	Id_Consultor nvarchar(128),
	CONSTRAINT PK_Visitantes PRIMARY KEY (Id),
	CONSTRAINT FK_Consultores_Visitantes FOREIGN KEY (Id_Consultor)
	REFERENCES dbo.AspNetUsers (Id)
);