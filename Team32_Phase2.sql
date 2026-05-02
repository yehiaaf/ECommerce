


CREATE TABLE [User](
    User_ID             INT             PRIMARY KEY NOT NULL,
    First_Name          NVARCHAR(50)    NOT NULL,
    Middle_Name         NVARCHAR(50),
    Last_Name           NVARCHAR(50)    NOT NULL,
    Username            NVARCHAR(50)    NOT NULL UNIQUE,
    [Password]          NVARCHAR(50)    NOT NULL,
    Email               NVARCHAR(100)   NOT NULL,
    Phone_Number        NVARCHAR(11),
    [Role]              NVARCHAR(20)    NOT NULL DEFAULT 'Customer',
    Registration_Date   DATETIME        NOT NULL DEFAULT GETDATE(),

    CONSTRAINT CHK_User_Email Check (Email LIKE '%_@_%._%'),
    CONSTRAINT CHK_User_Phone Check (Phone_Number LIKE '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    CONSTRAINT CHK_User_Role  Check ([Role] IN ('Admin','Customer'))
);
GO


CREATE TABLE [Address](
    Address_ID      INT             PRIMARY KEY NOT NULL,
    User_ID         INT             NOT NULL,
    House_Number    NVARCHAR(20),
    Street          NVARCHAR(100),
    City            NVARCHAR(50)    NOT NULL,
    Country         NVARCHAR(50)    NOT NULL DEFAULT 'Egypt',
    Postal_Code     NVARCHAR(20),

    CONSTRAINT FK_Address_User FOREIGN KEY (User_ID)
        REFERENCES [User](User_ID) ON DELETE CASCADE   -- CASCADE allowed only here
);
GO


CREATE TABLE Category(
    Category_ID     INT             PRIMARY KEY NOT NULL,
    [Name]          NVARCHAR(50)    NOT NULL UNIQUE,
    [Description]   NVARCHAR(200)
);
GO

.
CREATE TABLE Product(
    Product_ID      INT             PRIMARY KEY NOT NULL,
    Category_ID     INT             NOT NULL,
    [Name]          NVARCHAR(100)   NOT NULL,
    [Description]   NVARCHAR(500),
    Price           DECIMAL(18,2)   NOT NULL,
    Stock_Quantity  INT             NOT NULL DEFAULT 0,

    CONSTRAINT FK_Product_Category FOREIGN KEY (Category_ID)
        REFERENCES Category(Category_ID) ON DELETE NO ACTION,
    CONSTRAINT CHK_Product_Price Check (Price > 0),
    CONSTRAINT CHK_Product_Stock Check (Stock_Quantity >= 0)
);
GO


CREATE TABLE [Order](
    Order_ID        INT             PRIMARY KEY NOT NULL,
    User_ID         INT             NOT NULL,
    [Status]        NVARCHAR(30)    NOT NULL DEFAULT 'Pending',
    Order_Date      DATETIME        NOT NULL DEFAULT GETDATE(),
    Total_Amount    DECIMAL(18,2)   NOT NULL DEFAULT 0,

    CONSTRAINT FK_Order_User FOREIGN KEY (User_ID)
        REFERENCES [User](User_ID) ON DELETE NO ACTION,
    CONSTRAINT CHK_Order_Status Check ([Status] IN
        ('Pending','Processing','Shipped','Delivered','Cancelled')),
    CONSTRAINT CHK_Order_Total Check (Total_Amount >= 0)
);
GO


CREATE TABLE Order_Product(
    Order_ID        INT     NOT NULL,
    Product_ID      INT     NOT NULL,
    Quantity        INT     NOT NULL DEFAULT 1,

    CONSTRAINT PK_Order_Product PRIMARY KEY (Order_ID, Product_ID),
    CONSTRAINT FK_OP_Order   FOREIGN KEY (Order_ID)
        REFERENCES [Order](Order_ID)  ON DELETE NO ACTION,
    CONSTRAINT FK_OP_Product FOREIGN KEY (Product_ID)
        REFERENCES Product(Product_ID) ON DELETE NO ACTION,
    CONSTRAINT CHK_OP_Quantity Check (Quantity > 0)
);
GO


CREATE TABLE Shipping(
    Tracking_Number INT             PRIMARY KEY NOT NULL,   
    Order_ID        INT             NOT NULL,
    Shipment_Date   DATETIME,
    Delivery_Date   DATETIME,
    Shipping_Status NVARCHAR(30)    NOT NULL DEFAULT 'Preparing',

    CONSTRAINT UQ_Shipping_Order UNIQUE (Order_ID),                 
    CONSTRAINT FK_Shipping_Order FOREIGN KEY (Order_ID)
        REFERENCES [Order](Order_ID) ON DELETE NO ACTION,          
    CONSTRAINT CHK_Ship_Status Check (Shipping_Status IN
        ('Preparing','Out for Delivery','Delivered','Cancelled')),
    CONSTRAINT CHK_Ship_Dates  Check (Delivery_Date IS NULL OR Delivery_Date >= Shipment_Date)
);
GO


CREATE TABLE Payment(
    Payment_ID      INT             PRIMARY KEY NOT NULL,
    Order_ID        INT             NOT NULL,
    Payment_Date    DATETIME        NOT NULL DEFAULT GETDATE(),
    Payment_Method  NVARCHAR(30)    NOT NULL DEFAULT 'Cash',
    Payment_Amount  DECIMAL(18,2)   NOT NULL,
    Payment_Status  NVARCHAR(20)    NOT NULL DEFAULT 'Pending',

    CONSTRAINT UQ_Payment_Order UNIQUE (Order_ID),                  -- enforces 1:1
    CONSTRAINT FK_Payment_Order FOREIGN KEY (Order_ID)
        REFERENCES [Order](Order_ID) ON DELETE NO ACTION,           -- audit-safe
    CONSTRAINT CHK_Pay_Method Check (Payment_Method IN
        ('Cash','Credit Card','Debit Card','PayPal','Bank Transfer')),
    CONSTRAINT CHK_Pay_Status Check (Payment_Status IN
        ('Pending','Completed','Failed','Refunded')),
    CONSTRAINT CHK_Pay_Amount Check (Payment_Amount > 0)
);
GO





INSERT INTO [User] (User_ID, First_Name, Middle_Name, Last_Name, Username, [Password], Email, Phone_Number, [Role], Registration_Date) VALUES
(144, 'Yehia',   'Ahmed',   'Fathy',      'yehia1',  'pass123', 'yehia@gmail.com',   '01234567892', 'Admin', '2025-02-01'),
(1, 'Hajar',   'Mohamed', 'Jaber',      'hajar1',  'pass123', 'hajar@gmail.com',   '01234567890', 'Admin',    '2025-01-10'),
(2, 'Ahmed',   'Mohamed', 'Emara',      'ahmed2',  'pass123', 'emara@gmail.com',   '01234567891', 'Admin',    '2025-01-12'),
(11, 'Omar',    'Ahmed',   'Hussien',    'omar',   'pass123', 'omar@gmail.com',    '01234567893', 'Admin', '2025-02-05'),
(15, 'Ahmed',   'Hassan',  'Ahmed',      'hassan', 'pass123', 'hassan@gmail.com',  '01234567894', 'Admin', '2025-02-10'),
(13, 'Mahmoud', 'Ibrahim', 'Mahmoud',    'mahmoud','pass123', 'mahmoud@gmail.com', '01234567895', 'Admin', '2025-02-15'),
(14, 'Mohamed', 'Ahmed',   'Abdelwahab', 'moh',    'pass123', 'moh@gmail.com',     '01234567896', 'Admin', '2025-02-20'),
(8, 'Sara',    'Ali',     'Ibrahim',    'sara8',   'pass123', 'sara@gmail.com',    '01234567897', 'Customer', '2025-03-01'),
(9, 'Nour',    'Tarek',   'Hassan',     'nour9',   'pass123', 'nour@gmail.com',    '01234567898', 'Customer', '2025-03-05'),
(10,'Laila',   'Sami',    'Mostafa',    'laila10', 'pass123', 'laila@gmail.com',   '01234567899', 'Customer', '2025-03-10');
GO



INSERT INTO [Address] (Address_ID, User_ID, House_Number, Street, City, Country, Postal_Code) VALUES
(1, 1, '12', 'Nile St',     'Cairo',     'Egypt', '11511'),
(2, 2, '45', 'Tahrir St',   'Cairo',     'Egypt', '11512'),
(3, 3, '78', 'Pyramids Rd', 'Giza',      'Egypt', '12511'),
(4, 4, '90', 'Corniche',    'Alexandria','Egypt', '21500'),
(5, 5, '15', 'Zamalek',     'Cairo',     'Egypt', '11211'),
(6, 6, '33', 'Nasr City',   'Cairo',     'Egypt', '11765'),
(7, 7, '21', 'Maadi',       'Cairo',     'Egypt', '11431'),
(8, 8, '10', 'Heliopolis',  'Cairo',     'Egypt', '11341'),
(9, 9, '55', 'Dokki',       'Giza',      'Egypt', '12611'),
(10,10,'88', 'Mohandessin', 'Giza',      'Egypt', '12655');
GO


INSERT INTO Category (Category_ID, [Name], [Description]) VALUES
(1, 'Electronics', 'Phones, laptops, accessories'),
(2, 'Clothing',    'Men and women apparel'),
(3, 'Books',       'Educational and fiction books'),
(4, 'Home',        'Furniture and home decor'),
(5, 'Sports',      'Sports equipment and gear');
GO


INSERT INTO Product (Product_ID, Category_ID, [Name], [Description], Price, Stock_Quantity) VALUES
(1, 1, 'iPhone 15',      'Apple smartphone 128GB',          45000.00, 20),
(2, 1, 'Samsung Galaxy', 'Android smartphone 256GB',        30000.00, 15),
(3, 1, 'Dell Laptop',    'Core i7 16GB RAM',                40000.00, 10),
(4, 2, 'Men T-Shirt',    'Cotton t-shirt blue',               350.00, 50),
(5, 2, 'Women Dress',    'Summer dress red',                  800.00, 30),
(6, 3, 'Database Book',  'Fundamentals of Database Systems', 1200.00, 25),
(7, 3, 'C# Programming', 'Learn C# from scratch',             600.00, 40),
(8, 4, 'Office Chair',   'Ergonomic office chair',           2500.00,  8),
(9, 4, 'Wooden Table',   'Dining table 6 seats',             5500.00,  5),
(10,5, 'Football',       'FIFA standard football',            450.00, 35);
GO


INSERT INTO [Order] (Order_ID, User_ID, [Status], Order_Date, Total_Amount) VALUES
(1,  3, 'Delivered',  '2025-03-15 10:00:00', 45000.00),
(2,  4, 'Shipped',    '2025-03-20 11:30:00',  1550.00),
(3,  5, 'Processing', '2025-03-25 14:00:00', 30600.00),
(4,  6, 'Pending',    '2025-04-01 09:00:00',  3300.00),
(5,  7, 'Delivered',  '2025-04-05 16:45:00',  5850.00),
(6,  8, 'Cancelled',  '2025-04-10 12:00:00',   800.00),
(7,  9, 'Shipped',    '2025-04-15 13:30:00', 41200.00),
(8, 10, 'Delivered',  '2025-04-18 15:00:00',   450.00),
(9,  3, 'Processing', '2025-04-20 17:00:00',  2000.00),
(10, 4, 'Pending',    '2025-04-22 18:30:00',  3300.00);
GO


INSERT INTO Order_Product (Order_ID, Product_ID, Quantity) VALUES
(1, 1, 1),
(2, 6, 1), (2, 4, 1),
(3, 2, 1), (3, 7, 1),
(4, 8, 1), (4, 5, 1),
(5, 9, 1), (5, 4, 1),
(6, 5, 1),
(7, 3, 1), (7, 7, 2),
(8, 10, 1),
(9, 6, 1), (9, 5, 1),
(10, 8, 1), (10, 5, 1);
GO


INSERT INTO Shipping (Tracking_Number, Order_ID, Shipment_Date, Delivery_Date, Shipping_Status) VALUES
(1001, 1, '2025-03-16', '2025-03-18', 'Delivered'),
(1002, 2, '2025-03-21',  NULL,        'Out for Delivery'),
(1003, 3, '2025-03-26',  NULL,        'Preparing'),
(1004, 5, '2025-04-06', '2025-04-08', 'Delivered'),
(1005, 6,  NULL,         NULL,        'Cancelled'),
(1006, 7, '2025-04-16',  NULL,        'Out for Delivery'),
(1007, 8, '2025-04-19', '2025-04-20', 'Delivered'),
(1008, 9, '2025-04-21',  NULL,        'Preparing');
GO


INSERT INTO Payment (Payment_ID, Order_ID, Payment_Date, Payment_Method, Payment_Amount, Payment_Status) VALUES
(1, 1,  '2025-03-15', 'Credit Card',   45000.00, 'Completed'),
(2, 2,  '2025-03-20', 'Cash',           1550.00, 'Completed'),
(3, 3,  '2025-03-25', 'PayPal',        30600.00, 'Pending'),
(4, 4,  '2025-04-01', 'Bank Transfer',  3300.00, 'Pending'),
(5, 5,  '2025-04-05', 'Credit Card',    5850.00, 'Completed'),
(6, 6,  '2025-04-10', 'Cash',            800.00, 'Refunded'),
(7, 7,  '2025-04-15', 'Credit Card',   41200.00, 'Completed'),
(8, 8,  '2025-04-18', 'Debit Card',      450.00, 'Completed'),
(9, 9,  '2025-04-20', 'PayPal',         2000.00, 'Pending'),
(10,10, '2025-04-22', 'Cash',           3300.00, 'Pending');
GO


-- Awl select l product details for specific year
SELECT
    u.First_Name + ' ' + u.Last_Name AS CustomerName,
    o.Order_ID,
    o.Order_Date,
    o.[Status],
    o.Total_Amount
FROM [User] u
INNER JOIN [Order] o ON u.User_ID = o.User_ID
WHERE u.User_ID = 3;
GO

-- tane wahda l products in a category bel stock info
SELECT
    p.Product_ID,
    p.[Name]            AS ProductName,
    c.[Name]            AS CategoryName,
    p.Price,
    p.Stock_Quantity
FROM Product p
INNER JOIN Category c ON p.Category_ID = c.Category_ID
WHERE c.[Name] = 'Electronics'
ORDER BY p.Price DESC;
GO

-- query to show order line items with product names and subtotal
SELECT
    o.Order_ID,
    p.[Name]                   AS ProductName,
    op.Quantity,
    p.Price,
    (op.Quantity * p.Price)    AS Subtotal
FROM [Order] o
INNER JOIN Order_Product op ON o.Order_ID    = op.Order_ID
INNER JOIN Product p        ON op.Product_ID = p.Product_ID
ORDER BY o.Order_ID;
GO

-- query for tracking shipping status with the customer name 
SELECT
    s.Tracking_Number,
    u.First_Name + ' ' + u.Last_Name AS CustomerName,
    o.Order_ID,
    s.Shipping_Status,
    s.Shipment_Date,
    s.Delivery_Date
FROM Shipping s
INNER JOIN [Order] o ON s.Order_ID = o.Order_ID
INNER JOIN [User]  u ON o.User_ID  = u.User_ID
WHERE s.Shipping_Status IN ('Preparing','Out for Delivery');
GO

-- query for total revenue per category 
SELECT
    c.[Name]                    AS CategoryName,
    SUM(op.Quantity * p.Price)  AS TotalRevenue,
    COUNT(DISTINCT o.Order_ID)  AS NumberOfOrders
FROM Category c
INNER JOIN Product p        ON c.Category_ID = p.Category_ID
INNER JOIN Order_Product op ON p.Product_ID  = op.Product_ID
INNER JOIN [Order] o        ON op.Order_ID   = o.Order_ID
WHERE o.[Status] <> 'Cancelled'
GROUP BY c.[Name]
ORDER BY TotalRevenue DESC;
GO

--  query to list our top 3 customers by total spending
SELECT TOP 3
    u.User_ID,
    u.First_Name + ' ' + u.Last_Name AS CustomerName,
    COUNT(o.Order_ID)                AS TotalOrders,
    SUM(o.Total_Amount)              AS TotalSpent
FROM [User] u
INNER JOIN [Order] o ON u.User_ID = o.User_ID
WHERE o.[Status] <> 'Cancelled'
GROUP BY u.User_ID, u.First_Name, u.Last_Name
ORDER BY TotalSpent DESC;
GO

-- query for products with Low stock  
SELECT
    p.Product_ID,
    p.[Name] AS ProductName,
    c.[Name] AS CategoryName,
    p.Stock_Quantity
FROM Product p
INNER JOIN Category c ON p.Category_ID = c.Category_ID
WHERE p.Stock_Quantity < 15
ORDER BY p.Stock_Quantity ASC;
GO

-- query fro pending payments report
SELECT
    p.Payment_ID,
    o.Order_ID,
    u.First_Name + ' ' + u.Last_Name AS CustomerName,
    p.Payment_Amount,
    p.Payment_Method,
    p.Payment_Status
FROM Payment p
INNER JOIN [Order] o ON p.Order_ID = o.Order_ID
INNER JOIN [User]  u ON o.User_ID  = u.User_ID
WHERE p.Payment_Status = 'Pending';
GO

-- query to customers with or even without a stored address 

SELECT
    u.User_ID,
    u.First_Name + ' ' + u.Last_Name AS CustomerName,
    ISNULL(a.House_Number + ' ' + a.Street + ', ' + a.City + ', ' + a.Country,
           'No Address On File')     AS FullAddress
FROM [User] u
LEFT JOIN [Address] a ON u.User_ID = a.User_ID
WHERE u.[Role] = 'Customer';
GO

-- query for monthly sales summary 
SELECT
    YEAR(Order_Date)  AS [Year],
    MONTH(Order_Date) AS [Month],
    COUNT(Order_ID)   AS TotalOrders,
    SUM(Total_Amount) AS MonthlyRevenue
FROM [Order]
WHERE [Status] <> 'Cancelled'
GROUP BY YEAR(Order_Date), MONTH(Order_Date)
ORDER BY [Year], [Month];
GO

/*query for the customers whose total spend is above the
 average customer spend */
SELECT
    u.User_ID,
    u.First_Name + ' ' + u.Last_Name AS CustomerName,
    SUM(o.Total_Amount)              AS TotalSpent
FROM [User] u
INNER JOIN [Order] o ON u.User_ID = o.User_ID
WHERE o.[Status] <> 'Cancelled'
GROUP BY u.User_ID, u.First_Name, u.Last_Name
HAVING SUM(o.Total_Amount) > (
    SELECT AVG(AvgTable.UserTotal)
    FROM (
        SELECT SUM(Total_Amount) AS UserTotal
        FROM [Order]
        WHERE [Status] <> 'Cancelled'
        GROUP BY User_ID
    ) AS AvgTable
)
ORDER BY TotalSpent DESC;
GO



/*first procedure to place order*/
CREATE PROCEDURE sp_PlaceOrder
    @Order_ID  INT,
    @User_ID   INT,
    @Status    NVARCHAR(30) = 'Pending'
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

            
            IF NOT EXISTS (SELECT 1 FROM [User] WHERE User_ID = @User_ID)
            BEGIN
                RAISERROR('User does not exist.', 16, 1);
            END

            INSERT INTO [Order] (Order_ID, User_ID, [Status], Order_Date, Total_Amount)
            VALUES (@Order_ID, @User_ID, @Status, GETDATE(), 0);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrMsg NVARCHAR(2000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH
END;
GO

/* second one to add the product , also with stock decrementation*/
CREATE PROCEDURE sp_AddProductToOrder
    @Order_ID    INT,
    @Product_ID  INT,
    @Quantity    INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

            
            IF @Quantity <= 0
            BEGIN
                RAISERROR('Quantity must be greater than zero.', 16, 1);
            END

            
            DECLARE @CurrentStock INT;
            SELECT @CurrentStock = Stock_Quantity
            FROM Product
            WHERE Product_ID = @Product_ID;

            IF @CurrentStock IS NULL
            BEGIN
                RAISERROR('Product does not exist.', 16, 1);
            END

            IF @CurrentStock < @Quantity
            BEGIN
                RAISERROR('Insufficient stock for the requested product.', 16, 1);
            END

            -
            IF EXISTS (SELECT 1 FROM Order_Product
                       WHERE Order_ID = @Order_ID AND Product_ID = @Product_ID)
            BEGIN
                UPDATE Order_Product
                SET Quantity = Quantity + @Quantity
                WHERE Order_ID = @Order_ID AND Product_ID = @Product_ID;
            END
            ELSE
            BEGIN
                INSERT INTO Order_Product (Order_ID, Product_ID, Quantity)
                VALUES (@Order_ID, @Product_ID, @Quantity);
            END

           
            UPDATE Product
            SET Stock_Quantity = Stock_Quantity - @Quantity
            WHERE Product_ID = @Product_ID;

            
            DECLARE @NewTotal DECIMAL(18,2);
            SELECT @NewTotal = SUM(op.Quantity * p.Price)
            FROM Order_Product op
            INNER JOIN Product p ON op.Product_ID = p.Product_ID
            WHERE op.Order_ID = @Order_ID;

            UPDATE [Order]
            SET Total_Amount = ISNULL(@NewTotal, 0)
            WHERE Order_ID = @Order_ID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrMsg NVARCHAR(2000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH
END;
GO


CREATE PROCEDURE sp_UpdateOrderTotal
    @Order_ID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

            DECLARE @Total DECIMAL(18,2);

            SELECT @Total = SUM(op.Quantity * p.Price)
            FROM Order_Product op
            INNER JOIN Product p ON op.Product_ID = p.Product_ID
            WHERE op.Order_ID = @Order_ID;

            UPDATE [Order]
            SET Total_Amount = ISNULL(@Total, 0)
            WHERE Order_ID = @Order_ID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrMsg NVARCHAR(2000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH
END;
GO


CREATE PROCEDURE sp_UpdateOrderStatus
    @Order_ID  INT,
    @NewStatus NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

            IF @NewStatus NOT IN ('Pending','Processing','Shipped','Delivered','Cancelled')
            BEGIN
                RAISERROR('Invalid status value.', 16, 1);
            END

            IF NOT EXISTS (SELECT 1 FROM [Order] WHERE Order_ID = @Order_ID)
            BEGIN
                RAISERROR('Order does not exist.', 16, 1);
            END

            UPDATE [Order]
            SET [Status] = @NewStatus
            WHERE Order_ID = @Order_ID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        DECLARE @ErrMsg NVARCHAR(2000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH
END;
GO


CREATE PROCEDURE sp_SalesReport
    @StartDate DATETIME = NULL,
    @EndDate   DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @StartDate = ISNULL(@StartDate, DATEADD(MONTH, -1, GETDATE()));
    SET @EndDate   = ISNULL(@EndDate,   GETDATE());

    SELECT
        c.[Name]                    AS CategoryName,
        COUNT(DISTINCT o.Order_ID)  AS TotalOrders,
        SUM(op.Quantity * p.Price)  AS TotalRevenue,
        AVG(op.Quantity * p.Price)  AS AvgOrderValue
    FROM Category c
    INNER JOIN Product p        ON c.Category_ID = p.Category_ID
    INNER JOIN Order_Product op ON p.Product_ID  = op.Product_ID
    INNER JOIN [Order] o        ON op.Order_ID   = o.Order_ID
    WHERE o.Order_Date BETWEEN @StartDate AND @EndDate
      AND o.[Status] <> 'Cancelled'
    GROUP BY c.[Name]
    ORDER BY TotalRevenue DESC;
END;
GO




CREATE FUNCTION fn_GetUserFullName (@User_ID INT)
RETURNS NVARCHAR(150)
AS
BEGIN
    DECLARE @FullName NVARCHAR(150);

    SELECT @FullName =
        First_Name + ' '
        + ISNULL(Middle_Name + ' ', '')
        + Last_Name
    FROM [User]
    WHERE User_ID = @User_ID;

    RETURN ISNULL(@FullName, 'User Not Found');
END;
GO


CREATE FUNCTION fn_GetUserTotalSpending (@User_ID INT)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Total DECIMAL(18,2);

    SELECT @Total = SUM(Total_Amount)
    FROM [Order]
    WHERE User_ID = @User_ID
      AND [Status] <> 'Cancelled';

    RETURN ISNULL(@Total, 0);
END;
GO

 

CREATE FUNCTION fn_GetProductsByCategory (@Category_ID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT
        p.Product_ID,
        p.[Name]         AS ProductName,
        p.Price,
        p.Stock_Quantity
    FROM Product p
    WHERE p.Category_ID = @Category_ID
);
GO
