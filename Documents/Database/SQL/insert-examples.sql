﻿-- Tags
SET IDENTITY_INSERT [Tag] ON;

INSERT INTO [Tag] ([Id], [Title], [State]) VALUES (1, 'Food', 1);
INSERT INTO [Tag] ([Id], [Title], [State]) VALUES (2, 'Work', 1);
INSERT INTO [Tag] ([Id], [Title], [State]) VALUES (3, 'Software developer', 1);
INSERT INTO [Tag] ([Id], [Title], [State]) VALUES (4, 'Graphic design', 1);

SET IDENTITY_INSERT [Tag] OFF;


-- Transactions
SET IDENTITY_INSERT [Transaction] ON;

INSERT INTO [Transaction] ([Id], [AccountingDate], [TransactionId], [Type], [Account], [AccountName], [PartnerAccount], [PartnerName], [Sum], [Currency], [Message], [State]) VALUES 
(1, '2001-01-01 10:00:00', 'trid1', 'type1', '100000000', 'John Doe', '222222222', 'Food Ltd.', -1850, 'HUF', '', 1);
INSERT INTO [Transaction] ([Id], [AccountingDate], [TransactionId], [Type], [Account], [AccountName], [PartnerAccount], [PartnerName], [Sum], [Currency], [Message], [State]) VALUES 
(2, '2000-02-01 10:00:00', 'trid2', 'type2', '100000000', 'John Doe', '555555555', 'Microsoft', 250000, 'HUF', '2000-02', 1);

SET IDENTITY_INSERT [Transaction] OFF;


-- TransactionTagConns
INSERT INTO [TransactionTagConn] ([TransactionId], [TagId], [State]) VALUES (1, 1, 1);
INSERT INTO [TransactionTagConn] ([TransactionId], [TagId], [State]) VALUES (2, 2, 1);
INSERT INTO [TransactionTagConn] ([TransactionId], [TagId], [State]) VALUES (2, 3, 1);