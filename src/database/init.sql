\r livinparisroussilleteynier

CREATE TABLE
   Account (
      account_id INT,
      account_email VARCHAR(100),
      account_password VARCHAR(50),
      PRIMARY KEY (account_id)
   );

CREATE TABLE
   Ingredient (
      ingredient_id INT,
      ingredient_name VARCHAR(50) NOT NULL,
      is_vegetarian BOOLEAN,
      is_vegan BOOLEAN,
      is_gluten_free BOOLEAN,
      is_lactose_free BOOLEAN,
      is_halal BOOLEAN,
      is_kosher BOOLEAN,
      product_origin ENUM ('france', 'europe', 'other'),
      PRIMARY KEY (ingredient_id)
   );

CREATE TABLE
   Address (
      address_id VARCHAR(50),
      address_number INT NOT NULL,
      street VARCHAR(50) NOT NULL,
      postal_code INT,
      nearest_metro VARCHAR(50),
      PRIMARY KEY (address_id),
      UNIQUE (address_number, street)
   );

CREATE TABLE
   Dish (
      dish_id INT,
      dish_name VARCHAR(50) NOT NULL,
      dish_type ENUM ('starter', 'main_course', 'dessert'),
      expiry_time INT,
      cuisine_nationality VARCHAR(50),
      quantity INT CHECK (quantity >= 0),
      price DECIMAL(15, 2) CHECK (price >= 0),
      photo_path VARCHAR(50),
      PRIMARY KEY (dish_id)
   );

CREATE TABLE
   Customer (
      account_id INT,
      customer_rating DECIMAL(2, 1) CHECK (customer_rating BETWEEN 1 AND 5),
      loyalty_rank ENUM ('classic', 'bronze', 'silver', 'gold'),
      customer_is_banned BOOLEAN,
      PRIMARY KEY (account_id),
      FOREIGN KEY (account_id) REFERENCES Account (account_id)
   );

CREATE TABLE
   Chef (
      account_id INT,
      chef_rating DECIMAL(2, 1) CHECK (chef_rating BETWEEN 1 AND 5),
      eats_on_site BOOLEAN,
      chef_is_banned BOOLEAN,
      address_id VARCHAR(50) NOT NULL,
      PRIMARY KEY (account_id),
      FOREIGN KEY (account_id) REFERENCES Account (account_id),
      FOREIGN KEY (address_id) REFERENCES Address (address_id)
   );

CREATE TABLE
   Transaction (
      transaction_id INT,
      transaction_datetime DATETIME,
      account_id INT NOT NULL,
      PRIMARY KEY (transaction_id),
      FOREIGN KEY (account_id) REFERENCES Customer (account_id)
   );

CREATE TABLE
   Company (
      account_id INT,
      company_name VARCHAR(50) UNIQUE,
      contact_first_name VARCHAR(50),
      contact_last_name VARCHAR(50),
      PRIMARY KEY (account_id),
      FOREIGN KEY (account_id) REFERENCES Customer (account_id)
   );

CREATE TABLE
   Individual (
      account_id INT,
      last_name VARCHAR(50),
      first_name VARCHAR(50),
      personal_email VARCHAR(100),
      phone_number VARCHAR(50),
      address_id VARCHAR(50) NOT NULL,
      PRIMARY KEY (account_id),
      FOREIGN KEY (account_id) REFERENCES Customer (account_id),
      FOREIGN KEY (address_id) REFERENCES Address (address_id)
   );

CREATE TABLE
   OrderLine (
      order_line_id INT,
      order_line_datetime DATETIME,
      duration INT,
      order_line_status ENUM (
         'pending',
         'prepared',
         'delivering',
         'delivered',
         'canceled'
      ),
      is_eat_in BOOLEAN,
      address_id VARCHAR(50) NOT NULL,
      transaction_id INT NOT NULL,
      account_id INT NOT NULL,
      PRIMARY KEY (order_line_id),
      FOREIGN KEY (address_id) REFERENCES Address (address_id),
      FOREIGN KEY (transaction_id) REFERENCES Transaction (transaction_id),
      FOREIGN KEY (account_id) REFERENCES Chef (account_id)
   );

CREATE TABLE
   Review (
      review_id INT,
      review_type ENUM ('customer', 'chef'),
      review_rating DECIMAL(2, 1) CHECK (review_rating BETWEEN 1 AND 5),
      comment VARCHAR(500),
      review_date DATE,
      order_line_id INT NOT NULL,
      PRIMARY KEY (review_id),
      FOREIGN KEY (order_line_id) REFERENCES OrderLine (order_line_id)
   );

CREATE TABLE
   MenuProposal (
      account_id INT,
      proposal_date DATE,
      dish_id INT NOT NULL,
      PRIMARY KEY (account_id, proposal_date),
      FOREIGN KEY (account_id) REFERENCES Chef (account_id),
      FOREIGN KEY (dish_id) REFERENCES Dish (dish_id)
   );

CREATE TABLE
   Contains (
      ingredient_id INT,
      dish_id INT,
      PRIMARY KEY (ingredient_id, dish_id),
      FOREIGN KEY (ingredient_id) REFERENCES Ingredient (ingredient_id),
      FOREIGN KEY (dish_id) REFERENCES Dish (dish_id)
   );