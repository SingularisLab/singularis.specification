create table Users (
	Id int identity not null,
	Email nvarchar(255) not null,
	Lastname nvarchar(255) not null,
	Firstname nvarchar(255) not null,

	constraint PK_User primary key(Id))

create table Characters (
	Id int identity not null,
	Name nvarchar(255) not null,
	CreatedAt datetime not null,
	UserId int null,

	constraint PK_Character primary key(Id),
	constraint FK_Character_User foreign key(UserId) references Users(Id))

create table Items (
	Id int identity not null,
	Name nvarchar(255) not null,
	CharacterId int null,

	constraint PK_Item primary key(Id),
	constraint FK_Item_Character foreign key(CharacterId) references Characters(Id))

create table Runes (
	Id int identity not null,
	TargetAttribute nvarchar(255) not null,
	Modifier int not null,
	
	constraint PK_Rune primary key(Id))	


create table ItemsToRunes (
	ItemId int not null,
	RuneId int not null,
	
	constraint PK_ItemToRune primary key clustered(ItemId, RuneId),
	constraint FK_ItemToRune_Item foreign key (ItemId) references Items(Id),
	constraint FK_ItemToRune_Rune foreign key (RuneId) references Runes(Id))

go

create view ReadCharacters as(
	select 
		Id,
		Name,
		UserId
	from Characters
)

go


set identity_insert Users on
insert into Users (Id, Email, Lastname, Firstname)
values
	(1, 'pede.ultrices.a@inmagna.ca','Herrera','Mufutau'),
	(2, 'leo@SuspendissesagittisNullam.edu','Shelton','Camden'),
	(3, 'vel.arcu.eu@odio.edu','Snider','Amela'),
	(4, 'ipsum@dictummi.ca','Watts','Jescie'),
	(5, 'ligula.Nullam@interdum.co.uk','Kerr','Miriam')
set identity_insert Users off


set identity_insert Characters on
insert into Characters (Id, Name, CreatedAt, UserId)
values
	(1, 'ac', '2019-01-01', 1),
	(2, 'risus', '2020-01-02', 1),
	(3, 'nulla', '2018-01-01', 2),
	(4, 'adipiscing', '2019-03-03', 3),
	(5, 'erat', '2020-01-04', 4),
	(6, 'porta', '2017-04-01', 5),
	(7, 'convallis', '2020-01-08', 3),
	(8, 'sodales', '2017-07-02', 3),
	(9, 'magna', '2019-09-01', 3),
	(10, 'facilisis', '2019-09-30', 2);
set identity_insert Characters off

set identity_insert Runes on
insert into Runes (Id, TargetAttribute, Modifier)
values
	(1, 'erat.',-73),
	(2, 'Duis',7),
	(3, 'arcu',-24)
set identity_insert Runes off

set identity_insert Items on
insert into Items (Id, Name, CharacterId)
values
	(1, 'egestas,', 1),
	(2, 'Sed', 1),
	(3, 'libero.', 1),
	(4, 'scelerisque', 2),
	(5, 'in', 3),
	(6, 'tincidunt', 5),
	(7, 'interdum.', 5),
	(8, 'ipsum.', 5),
	(9, 'ullamcorper,', 9),
	(10, 'nostra,', 9);
set identity_insert Items off

insert into ItemsToRunes (RuneId, ItemId)
values
	(1, 1),
	(1, 2),
	(2, 3),
	(3, 5)




