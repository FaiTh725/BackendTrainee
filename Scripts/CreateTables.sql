use Trainee;

create table Clients (
	Id int primary key identity(1,1),
	Name nvarchar(20),
	Email nvarchar(20),
	Phone nvarchar(20),
	CreatedAt datetime,
	UpdatedAt datetime,
)

create table Tasks (
	Id int primary key identity(1,1),
	Title nvarchar(20),
	Description nvarchar(100),
	Status int,
	ClientId int not null,
	CreatedAt datetime,
	UpdatedAt datetime,
	foreign key (ClientId) references Clients(Id) on delete cascade
)