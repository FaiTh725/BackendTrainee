create trigger Clients_Inserted
on Clients
after insert
as
begin
update Clients
set UpdatedAt = GETDATE(), CreatedAt = GETDATE()
where Id in (select Id from inserted)
end;