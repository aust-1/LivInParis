CREATE DATABASE PSI;

USE PSI--;

CREATE TABLE Account(
   account_id INT,
   password VARCHAR(50),
   PRIMARY KEY(account_id)
);

CREATE TABLE Ingredient(
   ingredient_name VARCHAR(50),
   is_vegetarian BOOLEAN ,
   is_vegan BOOLEAN ,
   is_gluten_free BOOLEAN ,
   is_halal BOOLEAN ,
   is_kosher BOOLEAN ,
   PRIMARY KEY(ingredient_name)
);

CREATE TABLE Adress(
   number INT,
   street VARCHAR(50),
   nearest_metro VARCHAR(50),
   PRIMARY KEY(number, street)
);

CREATE TABLE Dish(
   dish_name VARCHAR(50),
   dish_type ENUM('starter', 'main_course', 'dessert'),
   preparation_date DATE,
   expiration_date DATE,
   cuisine_nationality VARCHAR(50),
   quantity INT,
   price VARCHAR(50),
   photo TEXT,
   PRIMARY KEY(dish_name)
);

CREATE TABLE Customer(
   account_id INT,
   customer_rating DECIMAL(2,1),
   loyalty_rank ENUM('Classic', 'Bronze', 'Silver', 'Gold'),
   customer_is_banned BOOLEAN ,
   PRIMARY KEY(account_id),
   FOREIGN KEY(account_id) REFERENCES Account(account_id)
);

CREATE TABLE Chef(
   account_id INT,
   chef_rating DECIMAL(2,1),
   eats_on_site BOOLEAN ,
   chef_is_banned BOOLEAN ,
   number INT NOT NULL,
   street VARCHAR(50) NOT NULL,
   PRIMARY KEY(account_id),
   FOREIGN KEY(account_id) REFERENCES Account(account_id),
   FOREIGN KEY(number, street) REFERENCES Adress(number, street)
);

CREATE TABLE Transaction(
   transaction_id INT,
   transaction_datetime DATETIME,
   account_id INT NOT NULL,
   PRIMARY KEY(transaction_id),
   FOREIGN KEY(account_id) REFERENCES Customer(account_id)
);

CREATE TABLE Company(
   account_id INT,
   company_name VARCHAR(50),
   contact_first_name VARCHAR(50),
   contact_last_name VARCHAR(50),
   PRIMARY KEY(account_id),
   FOREIGN KEY(account_id) REFERENCES Customer(account_id)
);

CREATE TABLE Individual(
   account_id INT,
   last_name VARCHAR(50),
   first_name VARCHAR(50),
   email VARCHAR(100),
   phone_number INT,
   number INT NOT NULL,
   street VARCHAR(50) NOT NULL,
   PRIMARY KEY(account_id),
   FOREIGN KEY(account_id) REFERENCES Customer(account_id),
   FOREIGN KEY(number, street) REFERENCES Adress(number, street)
);

CREATE TABLE OrderLine(
   order_line_id INT,
   order_line_datetime DATETIME,
   duration INT,
   status ENUM('pending', 'prepared', 'in_delivery', 'delivered'),
   is_eat_in BOOLEAN ,
   number INT NOT NULL,
   street VARCHAR(50) NOT NULL,
   transaction_id INT NOT NULL,
   account_id INT NOT NULL,
   PRIMARY KEY(order_line_id),
   FOREIGN KEY(number, street) REFERENCES Adress(number, street),
   FOREIGN KEY(transaction_id) REFERENCES Transaction(transaction_id),
   FOREIGN KEY(account_id) REFERENCES Chef(account_id)
);

CREATE TABLE Review(
   review_id INT,
   review_type ENUM('client', 'cuisinier'),
   review_rating DECIMAL(2,1),
   comment VARCHAR(500),
   review_date DATE,
   order_line_id INT NOT NULL,
   PRIMARY KEY(review_id),
   FOREIGN KEY(order_line_id) REFERENCES OrderLine(order_line_id)
);

CREATE TABLE MenuProposal(
   account_id INT,
   proposal_date DATE,
   dish_name VARCHAR(50) NOT NULL,
   PRIMARY KEY(account_id, proposal_date),
   FOREIGN KEY(account_id) REFERENCES Chef(account_id),
   FOREIGN KEY(dish_name) REFERENCES Dish(dish_name)
);

CREATE TABLE Contains(
   ingredient_name VARCHAR(50),
   dish_name VARCHAR(50),
   PRIMARY KEY(ingredient_name, dish_name),
   FOREIGN KEY(ingredient_name) REFERENCES Ingredient(ingredient_name),
   FOREIGN KEY(dish_name) REFERENCES Dish(dish_name)
);

CREATE USER 'eliottfrancois'@'localhost' IDENTIFIED BY 'PSI';
GRANT ALL privileges ON psi.* TO 'eliottfrancois'@'localhost';
FLUSH PRIVILEGES; 

