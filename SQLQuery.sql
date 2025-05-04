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