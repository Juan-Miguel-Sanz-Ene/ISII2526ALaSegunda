INSERT INTO [dbo].[AspNetUsers] ([Id], [Name], [Surname], [Address], [AccountCreationDate], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'1', N'Alejandro', N'Gomez', N'Av España', N'2025-10-13 00:00:00', N'alex', N'alex', N'alex@mail.es', N'alex@mail.es', 1, N'alex', NULL, NULL, N'666666666', 1, 0, NULL, 0, 0), (N'2', N'Alejandro2', N'Gomez2', N'Av España', N'2025-10-13 00:00:00', N'alex2', N'alex2', N'alex2@mail.es', N'alex2@mail.es', 1, N'alex2', NULL, NULL, N'666666667', 1, 0, NULL, 0, 0)

SET IDENTITY_INSERT [dbo].[PaymentMethods] ON
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (1, N'1', N'Paypal', N'666666666', NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF

SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [Street], [PostalCode], [NameSurname], [Description], [Date], [Rating], [TotalPrice], [State], [CustomerId], [PaymentMethodId]) VALUES (4, N'Ab', N'Av españa', N'02002', N'Alejandro Gomez', NULL, N'2025-10-13 00:00:00', 5, CAST(5.00 AS Decimal(10, 2)), 0, N'1', 1)
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF
SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [Street], [PostalCode], [NameSurname], [Description], [Date], [Rating], [TotalPrice], [State], [CustomerId], [PaymentMethodId]) VALUES (5, N'Ab', N'Av españa', N'02005', N'Alejandro Gomez', NULL, N'2025-10-13 00:00:00', 5, CAST(5.00 AS Decimal(10, 2)), 0, N'1', 1)
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF
SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON
INSERT INTO [dbo].[PurchaseOrders] ([Id], [City], [Street], [PostalCode], [NameSurname], [Description], [Date], [Rating], [TotalPrice], [State], [CustomerId], [PaymentMethodId]) VALUES (1, N'Ab', N'Av españa', N'02003', N'Alejandro Gomez', NULL, N'2025-10-13 00:00:00', 5, CAST(10.00 AS Decimal(10, 2)), 0, N'1', 1)
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF


SET IDENTITY_INSERT [dbo].[DeliveryDrivers] ON
INSERT INTO [dbo].[DeliveryDrivers] ([id], [Available], [Name]) VALUES (1, 1, N'Juan')
SET IDENTITY_INSERT [dbo].[DeliveryDrivers] OFF

SET IDENTITY_INSERT [dbo].[DeliveryAssignments] ON
INSERT INTO [dbo].[DeliveryAssignments] ([Id], [DeliveryAssignmentDone], [ExtraReward], [PersonalMessage], [DeliveryManid]) VALUES (2, N'2025-10-16 00:00:00', CAST(10.00 AS Decimal(5, 2)), N'Message', 1)
SET IDENTITY_INSERT [dbo].[DeliveryAssignments] OFF

INSERT INTO [dbo].[PurchaseDeliveries] ([DeliveryAssignmentId], [PurchaseOrderId], [Date], [Priority]) VALUES (2, 4, N'2025-10-14 00:00:00', 0)
