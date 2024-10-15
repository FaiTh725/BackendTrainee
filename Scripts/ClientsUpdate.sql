create trigger Clients_Update
on Clients
after update
as
begin
update Clients
set UpdatedAt = GETDATE()
where Id in (select Id from inserted)
end;