\r livinparisroussilleteynier

CREATE TABLE
   Address (
      address_id INT AUTO_INCREMENT,
      address_number INT NOT NULL,
      street VARCHAR(100) NOT NULL,
      PRIMARY KEY (address_id),
      UNIQUE (address_number, street)
   );

CREATE TABLE
   Account (
      account_id INT AUTO_INCREMENT,
      account_user_name VARCHAR(100) NOT NULL,
      account_password VARCHAR(50) NOT NULL,
      PRIMARY KEY (account_id),
      UNIQUE (account_user_name)
   );

CREATE TABLE
   Chef (
      account_id INT,
      chef_is_banned BOOLEAN NOT NULL,
      address_id INT NOT NULL,
      PRIMARY KEY (account_id),
      FOREIGN KEY (account_id) REFERENCES Account (account_id) ON DELETE CASCADE,
      FOREIGN KEY (address_id) REFERENCES Address (address_id) ON DELETE RESTRICT
   );

CREATE TABLE
   Customer (
      account_id INT,
      customer_is_banned BOOLEAN NOT NULL,
      PRIMARY KEY (account_id),
      FOREIGN KEY (account_id) REFERENCES Account (account_id) ON DELETE CASCADE
   );

CREATE TABLE
   Company (
      account_id INT,
      company_name VARCHAR(50) NOT NULL,
      contact_first_name VARCHAR(50),
      contact_last_name VARCHAR(50),
      PRIMARY KEY (account_id),
      UNIQUE (company_name),
      FOREIGN KEY (account_id) REFERENCES Customer (account_id) ON DELETE CASCADE
   );

CREATE TABLE
   Individual (
      account_id INT,
      last_name VARCHAR(50) NOT NULL,
      first_name VARCHAR(50) NOT NULL,
      personal_email VARCHAR(100) NOT NULL,
      phone_number VARCHAR(50) NOT NULL,
      address_id INT NOT NULL,
      PRIMARY KEY (account_id),
      UNIQUE (phone_number),
      FOREIGN KEY (account_id) REFERENCES Customer (account_id) ON DELETE CASCADE,
      FOREIGN KEY (address_id) REFERENCES Address (address_id) ON DELETE RESTRICT
   );

CREATE TABLE
   Dish (
      dish_id INT AUTO_INCREMENT,
      dish_name VARCHAR(50) NOT NULL,
      dish_type ENUM ('starter', 'main_course', 'dessert') NOT NULL,
      expiry_time INT NOT NULL,
      cuisine_nationality VARCHAR(50) NOT NULL,
      quantity INT NOT NULL CHECK (quantity >= 0),
      price DECIMAL(10, 2) NOT NULL CHECK (price >= 0),
      products_origin ENUM ('france', 'europe', 'other') NOT NULL,
      photo_path VARCHAR(255),
      PRIMARY KEY (dish_id)
   );

CREATE TABLE
   Ingredient (
      ingredient_id INT AUTO_INCREMENT,
      ingredient_name VARCHAR(50) NOT NULL,
      is_vegetarian BOOLEAN NOT NULL,
      is_vegan BOOLEAN NOT NULL,
      is_gluten_free BOOLEAN NOT NULL,
      is_lactose_free BOOLEAN NOT NULL,
      is_halal BOOLEAN NOT NULL,
      is_kosher BOOLEAN NOT NULL,
      PRIMARY KEY (ingredient_id),
      UNIQUE (ingredient_name)
   );

CREATE TABLE
   Contains (
      ingredient_id INT,
      dish_id INT,
      PRIMARY KEY (ingredient_id, dish_id),
      FOREIGN KEY (ingredient_id) REFERENCES Ingredient (ingredient_id) ON DELETE CASCADE,
      FOREIGN KEY (dish_id) REFERENCES Dish (dish_id) ON DELETE CASCADE
   );

CREATE TABLE
   MenuProposal (
      account_id INT,
      proposal_date DATE NOT NULL,
      dish_id INT NOT NULL,
      PRIMARY KEY (account_id, proposal_date),
      FOREIGN KEY (account_id) REFERENCES Chef (account_id) ON DELETE CASCADE,
      FOREIGN KEY (dish_id) REFERENCES Dish (dish_id) ON DELETE RESTRICT
   );

CREATE TABLE
   OrderTransaction (
      transaction_id INT AUTO_INCREMENT,
      transaction_datetime DATETIME NOT NULL,
      account_id INT NOT NULL,
      PRIMARY KEY (transaction_id),
      FOREIGN KEY (account_id) REFERENCES Customer (account_id) ON DELETE CASCADE
   );

CREATE TABLE
   OrderLine (
      order_line_id INT AUTO_INCREMENT,
      order_line_datetime DATETIME NOT NULL,
      order_line_status ENUM (
         'in_cart',
         'pending',
         'prepared',
         'delivering',
         'delivered',
         'canceled'
      ) NOT NULL,
      address_id INT NOT NULL,
      transaction_id INT NOT NULL,
      account_id INT NOT NULL,
      PRIMARY KEY (order_line_id),
      FOREIGN KEY (address_id) REFERENCES Address (address_id) ON DELETE RESTRICT,
      FOREIGN KEY (transaction_id) REFERENCES OrderTransaction (transaction_id) ON DELETE CASCADE,
      FOREIGN KEY (account_id) REFERENCES Chef (account_id) ON DELETE CASCADE
   );

CREATE TABLE
   Review (
      review_id INT AUTO_INCREMENT,
      reviewer_type ENUM ('customer', 'chef') NOT NULL,
      review_rating DECIMAL(2, 1) CHECK (review_rating BETWEEN 1 AND 5),
      comment VARCHAR(500),
      review_date DATE NOT NULL,
      order_line_id INT NOT NULL,
      PRIMARY KEY (review_id),
      FOREIGN KEY (order_line_id) REFERENCES OrderLine (order_line_id) ON DELETE CASCADE
   );
