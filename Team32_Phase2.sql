use [master]
drop database ECommerce
create database ECommerce
use ECommerce

create table [user] (
user_id int identity(1,1) primary key ,
first_name varchar(20) not null check (first_name not like '%[^A-Za-z ]%') ,
middle_name varchar(20) check (middle_name not like '%[^A-Za-z ]%') ,
last_name varchar(20) not null check (last_name not like '%[^A-Za-z ]%') ,
username varchar(50) not null unique check (username not like '% %') ,
[password] varchar(255) not null ,
phone_number varchar(20) not null unique check (phone_number like '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
email varchar(100) not null unique check (email like '_%@_%._%') ,
registration_date date not null default getdate(),
[role] varchar(50) not null default 'customer' ,
constraint checkrole check ([role] in ('admin','customer')) 
);



create table category (
category_id int identity(1,1) primary key ,
[name] varchar(50) not null unique check (name not like '%[^A-Za-z ]%') ,
description varchar(255) not null
);



create table product (
product_id int identity(1,1) primary key ,
category_id int not null ,
name varchar(50) not null ,
price money not null check (price >= 0) ,
description varchar(500) not null ,
stock_quantity int not null check (stock_quantity >= 0) ,
foreign key (category_id) references category(category_id)
);


create table [address] (
address_id int identity(1,1) primary key ,
user_id int not null ,
country varchar(50) not null check (country not like '%[^A-Za-z ]%') ,
city varchar(50) not null check (city not like '%[^A-Za-z ]%') ,
street varchar(50) not null ,
House_number varchar(20) not null check (House_number not like '%[^0-9]%') ,
postal_code varchar(20) not null check (postal_code not like '%[^0-9]%') ,
foreign key (user_id) references [user](user_id)
);



create table [order] (
order_id int identity(1,1) primary key ,
user_id int not null ,
order_date date not null default getdate() ,
[status] varchar(20) not null default 'pending',
constraint checkstatus check (status in (
'pending','confirmed','paid','processing','shipped',
'out_for_delivery','delivered','cancelled',
'returned','refunded','failed'
)) ,
foreign key (user_id) references [user](user_id)
);


create table payment (
    payment_id int identity(1,1) primary key ,
    order_id int not null ,
    payment_date date not null default getdate(),
    payment_status varchar(20) not null ,
    constraint chk_payment_status check (payment_status in (
    'pending','authorized','paid','failed',
    'cancelled','refunded','partially_refunded'
    )) ,
    payment_amount money not null check (payment_amount >= 0) ,
    payment_method varchar(20) not null ,
    constraint chkpayment_method check (payment_method in (
    'cash','credit_card','debit_card','wallet',
    'bank_transfer','mobile_wallet','bnpl'
    )) ,
    foreign key (order_id) references [order](order_id)
);



create table shipping (
tracking_number int identity(1,1) primary key ,
order_id int not null ,
shipping_date date  ,
delivery_Date date ,
shipping_status varchar(30) not null ,
constraint chkshipping_status check (shipping_status in (
'pending','ready_for_shipment','picked_up','in_transit',
'out_for_delivery','delivered','failed_delivery',
'returned','lost','damaged'
)) ,
foreign key (order_id) references [order](order_id)
,
constraint datesChek check (delivery_Date >= shipping_date),
);


create table order_product (
order_id int not null ,
product_id int not null ,
quantity int not null check (quantity > 0) ,
primary key (order_id , product_id) ,
foreign key (order_id) references [order](order_id) ,
foreign key (product_id) references product(product_id)
);

-- USERS
insert into [user] 
(first_name, middle_name, last_name, username, [password], email, phone_number, [role], registration_date)
values
('Yehia','Ahmed','Fathy','yehia1','pass123','yehia@gmail.com','01234567892','Admin','2025-02-01'),
('Hajar','Mohamed','Jaber','hajar1','pass123','hajar@gmail.com','01234567890','Admin','2025-01-10'),
('Ahmed','Mohamed','Emara','ahmed2','pass123','emara@gmail.com','01234567891','Admin','2025-01-12'),
('Omar','Ahmed','Hussien','omar','pass123','omar@gmail.com','01234567893','Admin','2025-02-05'),
('Ahmed','Hassan','Ahmed','hassan','pass123','hassan@gmail.com','01234567894','Admin','2025-02-10'),
('Mahmoud','Ibrahim','Mahmoud','mahmoud','pass123','mahmoud@gmail.com','01234567895','Admin','2025-02-15'),
('Mohamed','Ahmed','Abdelwahab','moh','pass123','moh@gmail.com','01234567896','Admin','2025-02-20'),
('Sara','Ali','Ibrahim','sara8','pass123','sara@gmail.com','01234567897','Customer','2025-03-01'),
('Nour','Tarek','Hassan','nour9','pass123','nour@gmail.com','01234567898','Customer','2025-03-05'),
('Laila','Sami','Mostafa','laila10','pass123','laila@gmail.com','01234567899','Customer','2025-03-10');


-- CATEGORY
insert into category (name, description) values
('Electronics','Phones, laptops, accessories'),
('Clothing','Men and women apparel'),
('Books','Educational and fiction books'),
('Home','Furniture and home decor'),
('Sports','Sports equipment and gear');


-- PRODUCT
insert into product (category_id, name, description, price, stock_quantity) values
(1,'iPhone 15','Apple smartphone 128GB',45000,20),
(1,'Samsung Galaxy','Android smartphone 256GB',30000,15),
(1,'Dell Laptop','Core i7 16GB RAM',40000,10),
(2,'Men T-Shirt','Cotton t-shirt blue',350,50),
(2,'Women Dress','Summer dress red',800,30),
(3,'Database Book','Fundamentals of Database Systems',1200,25),
(3,'C# Programming','Learn C# from scratch',600,40),
(4,'Office Chair','Ergonomic office chair',2500,8),
(4,'Wooden Table','Dining table 6 seats',5500,5),
(5,'Football','FIFA standard football',450,35);


-- ADDRESS (depends on user_id starting from 1)
insert into [address] (user_id, house_number, street, city, country, postal_code) values
(1,'12','Nile St','Cairo','Egypt','11511'),
(2,'45','Tahrir St','Cairo','Egypt','11512'),
(3,'78','Pyramids Rd','Giza','Egypt','12511'),
(4,'90','Corniche','Alexandria','Egypt','21500'),
(5,'15','Zamalek','Cairo','Egypt','11211'),
(6,'33','Nasr City','Cairo','Egypt','11765'),
(7,'21','Maadi','Cairo','Egypt','11431'),
(8,'10','Heliopolis','Cairo','Egypt','11341'),
(9,'55','Dokki','Giza','Egypt','12611'),
(10,'88','Mohandessin','Giza','Egypt','12655');


-- ORDERS (no total_amount)
insert into [order] (user_id, status, order_date) values
(3,'delivered','2025-03-15'),
(4,'shipped','2025-03-20'),
(5,'processing','2025-03-25'),
(6,'pending','2025-04-01'),
(7,'delivered','2025-04-05'),
(8,'failed','2025-04-10'),
(9,'pending','2025-04-15'),
(10,'delivered','2025-04-18'),
(3,'processing','2025-04-20'),
(4,'pending','2025-04-22');


-- ORDER PRODUCT
insert into order_product (order_id, product_id, quantity) values
(1,1,1),
(2,6,1),(2,4,1),
(3,2,1),(3,7,1),
(4,8,1),(4,5,1),
(5,9,1),(5,4,1),
(6,5,1),
(7,3,1),(7,7,2),
(8,10,1),
(9,6,1),(9,5,1),
(10,8,1),(10,5,1);


-- SHIPPING
insert into shipping (order_id, shipping_date, delivery_date, shipping_status) values
(1,'2025-03-16','2025-03-18','delivered'),
(2,'2025-03-21',NULL,'ready_for_shipment'),
(3,'2025-03-26',NULL,'ready_for_shipment'),
(5,'2025-04-06','2025-04-08','delivered'),
(6,NULL,NULL,'lost'),
(7,'2025-04-16',NULL,'ready_for_shipment'),
(8,'2025-04-19','2025-04-20','delivered'),
(9,'2025-04-3',NULL,'ready_for_shipment');


-- PAYMENT
insert into payment (order_id, payment_date, payment_method, payment_amount, payment_status) values
(1,'2025-03-15','cash',45000,'paid'),
(2,'2025-03-20','cash',1550,'paid'),
(3,'2025-03-25','cash',30600,'paid'),
(4,'2025-04-01','cash',3300,'paid'),
(5,'2025-04-05','cash',5850,'paid'),
(6,'2025-04-10','cash',800,'refunded'),
(7,'2025-04-15','cash',41200,'paid'),
(8,'2025-04-18','cash',450,'paid'),
(9,'2025-04-20','cash',2000,'pending'),
(10,'2025-04-22','cash',3300,'pending');

GO
CREATE FUNCTION GetUserFullName (@User_ID INT)
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
create function getTotalAmount 
(@order_ID int)
 returns money
 begin 
 declare @total int
 
 select  @total = sum ([order_product].quantity* product.price) from [order_product]
 inner join product on [order_product].product_id=product.product_id
 where @order_ID = [order_product].order_ID

 
 
 
 return isnull(@total , 0)
 end;
 GO
CREATE PROCEDURE placeOrder
    @User_ID   INT,
    @Status    VARCHAR(20) = 'pending'
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewOrderID INT;

    BEGIN TRANSACTION;

    IF NOT EXISTS (SELECT 1 FROM [user] WHERE user_id = @User_ID)
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50000, 'user does not exist.', 1;
        RETURN;
    END

    INSERT INTO [order] (user_id, status, order_date)
    VALUES (@User_ID, @Status, GETDATE());

    SET @NewOrderID = SCOPE_IDENTITY();

    COMMIT TRANSACTION;

    SELECT @NewOrderID AS order_id;
END;
GO
create procedure addProductToOrder
    @order_id int,
    @product_id int,
    @quantity int
as
begin
    if @quantity <= 0
    begin
        print 'quantity must be greater than zero'
        return
    end

    if not exists (select * from product where product_id = @product_id)
    begin
        print 'product does not exist'
        return
    end

    if not exists (select * from [order] where order_id = @order_id)
    begin
        print 'order does not exist'
        return
    end

    declare @currentstock int
    select @currentstock = stock_quantity
    from product
    where product_id = @product_id

    if @currentstock < @quantity
    begin
        print 'not enough stock'
        return
    end

    if exists (select * from order_product 
               where order_id = @order_id and product_id = @product_id)
    begin
        update order_product
        set quantity = quantity + @quantity
        where order_id = @order_id and product_id = @product_id
    end
    else
    begin
        insert into order_product (order_id, product_id, quantity)
        values (@order_id, @product_id, @quantity)
    end

    update product
    set stock_quantity = stock_quantity - @quantity
    where product_id = @product_id

    
    print 'product added successfully'

end
go
create procedure salesreport
    @startdate date,
    @enddate date
as
begin

    select 
        c.name as category_name,
        count(distinct o.order_id) as total_orders,
        sum(op.quantity * p.price) as total_revenue
    from category c
    join product p on c.category_id = p.category_id
    join order_product op on p.product_id = op.product_id
    join [order] o on op.order_id = o.order_id
    where o.order_date between @startdate and @enddate
      and o.status <> 'cancelled'
    group by c.name
    order by total_revenue desc

end
