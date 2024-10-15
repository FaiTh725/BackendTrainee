create trigger Task_Inserted
on Tasks
after insert
as
begin
update Tasks
set UpdatedAt = GETDATE(), CreatedAt = GETDATE()
where Id in (select Id from inserted)
end;