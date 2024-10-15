create trigger Task_Update
on Tasks
after update
as
begin
update Tasks
set UpdatedAt = GETDATE()
where Id in (select Id from inserted)
end;