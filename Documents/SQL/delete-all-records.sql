DELETE FROM [TransactionTagConn];
DELETE FROM [Transaction];
DELETE FROM [Tag];

DBCC CHECKIDENT ([TransactionTagConn], RESEED, 0);
DBCC CHECKIDENT ([Transaction], RESEED, 0);
DBCC CHECKIDENT ([Tag], RESEED, 0);