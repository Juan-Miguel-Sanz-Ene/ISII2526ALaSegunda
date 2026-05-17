INSERT INTO [dbo].[AspNetUsers] (
    [Id],[Name],[Surname],[Address],[AccountCreationDate],
    [EmailConfirmed],[PhoneNumberConfirmed],[TwoFactorEnabled],
    [LockoutEnabled],[AccessFailedCount]
) VALUES
(N'1', N'Pepe', N'Perez', N'Calle Inventada', '2020-12-06T00:00:00',
 1, 1, 1, 0, 0);

SET IDENTITY_INSERT [dbo].[PaymentMethods] ON;
INSERT INTO [dbo].[PaymentMethods]
    ([Id],[UserId],[Discriminator],[TelephoneNumber],[CreditCardNumber],[ExpirationDate],[Email])
VALUES
    (1, N'1', N'Bizum', N'+34611343434', NULL, NULL, NULL);
SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF;

SET IDENTITY_INSERT [dbo].[Brands] ON;
INSERT INTO [dbo].[Brands] ([Id],[Location],[Name]) VALUES (1,N'Madrid',N'Zara');
INSERT INTO [dbo].[Brands] ([Id],[Location],[Name]) VALUES (2,N'Barcelona',N'Uniqlo');
SET IDENTITY_INSERT [dbo].[Brands] OFF;

SET IDENTITY_INSERT [dbo].[Products] ON;
INSERT INTO [dbo].[Products] ([ProductId],[Name],[Description],[Colour],[Price],[Stock],[IsReturnable],[BrandId])
VALUES (1,N'Jacket',NULL,N'Red', 20.00,100,1,1),
       (2,N'Shirt', NULL,N'Blue',10.00,100,0,2);
SET IDENTITY_INSERT [dbo].[Products] OFF;

UPDATE [dbo].[Products] SET [Stock] = 100 WHERE [ProductId] = 1;
UPDATE [dbo].[Products] SET [Stock] = 100 WHERE [ProductId] = 2;

-- 5) Pedido (hijo de AspNetUsers y PaymentMethods)
SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON;
INSERT INTO [dbo].[PurchaseOrders]
([Id],[City],[Street],[PostalCode],[NameSurname],[Description],[Date],[Rating],[TotalPrice],[State],[CustomerId],[PaymentMethodId])
VALUES
(10, N'Albacete', N'Calle Inventada', N'02001', N'Pepito', NULL, '2025-12-12T00:00:00',
 NULL, 12.00, 1, N'1', 1);
SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF;


INSERT INTO dbo.PurchaseProducts (PurchaseOrderId, ProductId, Price, Quantity)
VALUES
  (10, 1, 20.00, 1),  
  (10, 2, 10.00, 1);  