create table dbo.Visitante
(
	Id INT Identity NOT NULL,
	Nome varchar(MAX),
	Email varchar(MAX),
	Celular varchar(MAX),
	CONSTRAINT PK_Visitantes PRIMARY KEY (Id)
);

create table dbo.Atendimentos
(
	Id int identity not null, 
	Id_Consultor nvarchar(128) not null,
	Id_Visitante int,
	Encerrado bit not null,
	Data DateTime null,
	CONSTRAINT PK_Atendimentos PRIMARY KEY (Id),
	CONSTRAINT FK_Atendimentos_Consultor foreign key (Id_Consultor) references AspNetUsers(Id),
	constraint FK_Atendimentos_Visitante foreign key (Id_Visitante) references Visitante(Id) 
);

create table dbo.Mensagens
(
	Id int identity not null,
	Mensagem varchar(255),
	Arquivo varchar(max),
	enviadoPorConsultor nvarchar(128),
	enviadoPorVisitante int,
	Id_Atendimento int,
	CONSTRAINT PK_Mensagens PRIMARY KEY (Id),
	CONSTRAINT FK_Mensagens_Atendimento FOREIGN KEY (Id_Atendimento) references Atendimentos(Id)
);