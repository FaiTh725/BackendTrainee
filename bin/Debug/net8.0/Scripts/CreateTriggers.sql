create trigger Clients_Update
on Clients
after update
as
begin
update Clients
set UpdatedAt = GETDATE()
where Id in (select Id from inserted)
end;


create trigger Clients_Inserted
on Clients
after insert
as
begin
update Clients
set UpdatedAt = GETDATE(), CreatedAt = GETDATE()
where Id in (select Id from inserted)
end;


create trigger Task_Update
on Tasks
after update
as
begin
update Tasks
set UpdatedAt = GETDATE()
where Id in (select Id from inserted)
end;


create trigger Task_Inserted
on Tasks
after insert
as
begin
update Tasks
set UpdatedAt = GETDATE(), CreatedAt = GETDATE()
where Id in (select Id from inserted)
end;
