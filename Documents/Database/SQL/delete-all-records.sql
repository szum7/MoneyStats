-- [Deprecated] Use ConsoleApp instead!
QUIT;

DELETE FROM [TransactionTagConn];
DELETE FROM [Transaction];
DELETE FROM [Tag];
DELETE FROM [BankRow];

DBCC CHECKIDENT ([TransactionTagConn], RESEED, 0);
DBCC CHECKIDENT ([Transaction], RESEED, 0);
DBCC CHECKIDENT ([Tag], RESEED, 0);
DBCC CHECKIDENT ([BankRow], RESEED, 0);