alter table Atendimentos
	drop constraint FK_Atendimentos_Consultor;

alter table Atendimentos
	add constraint FK_Atendimentos_Consultor
		foreign key (Id_Consultor) references AspNetUsers(ID) on delete cascade;

alter table Mensagens
	drop constraint FK_Mensagens_Atendimento

alter table Mensagens
	add constraint FK_Mensagens_Atendimento
		foreign key (Id_Atendimento) references Atendimentos(Id) on delete cascade;

alter table Atendimentos
	alter column Data DateTime;