CREATE TABLE users
(
    user_id int primary key identity,
    username varchar(50) not null,
    user_password varchar(10) not null,
    user_name varchar(50) not null,
    user_phone varchar(20)
)

INSERT INTO users VALUES('admin',123,'User 1','253-253666')

SELECT * FROM users;

CREATE TABLE category
(
 category_id INT PRIMARY KEY IDENTITY,
 category_name VARCHAR(50)
 )

 CREATE TABLE tables
 (
    table_id INT PRIMARY KEY IDENTITY,
    table_name VARCHAR(15)
 )

 CREATE TABLE staff
(
	staff_id INT PRIMARY KEY IDENTITY,
	staff_name VARCHAR(50),
	staff_phone VARCHAR(50 ),
	staff_role VARCHAR(50)
);

CREATE TABLE products
(
	product_id INT PRIMARY KEY IDENTITY,
	product_name VARCHAR(50),
	product_price FLOAT,
	category_id INT,
	product_image IMAGE
);

ALTER TABLE products
ADD CONSTRAINT fk_category
FOREIGN KEY (category_id) 
REFERENCES category(category_id);

SELECT product_id, product_name, product_price, c.category_id, c.category_name FROM products p inner join category c on p.category_id = c.category_id;
