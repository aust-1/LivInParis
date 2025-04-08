\r livinparisroussilleteynier

CREATE TABLE
   Account (
      account_id INT AUTO_INCREMENT,
      account_email VARCHAR(100) NOT NULL UNIQUE,
      account_password VARCHAR(50),
      PRIMARY KEY (account_id)
   );

CREATE TABLE
   Ingredient (
      ingredient_id INT AUTO_INCREMENT,
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
      address_id INT AUTO_INCREMENT,
      address_number INT NOT NULL,
      street VARCHAR(50) NOT NULL,
      postal_code INT,
      nearest_metro VARCHAR(50),
      PRIMARY KEY (address_id),
      UNIQUE (address_number, street)
   );

CREATE TABLE
   Dish (
      dish_id INT AUTO_INCREMENT,
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
      account_id INT AUTO_INCREMENT,
      customer_rating DECIMAL(2, 1) CHECK (customer_rating BETWEEN 1 AND 5),
      loyalty_rank ENUM ('classic', 'bronze', 'silver', 'gold'),
      customer_is_banned BOOLEAN,
      PRIMARY KEY (account_id),
      FOREIGN KEY (account_id) REFERENCES Account (account_id) ON DELETE CASCADE
   );

CREATE TABLE
   Chef (
      account_id INT AUTO_INCREMENT,
      chef_rating DECIMAL(2, 1) CHECK (chef_rating BETWEEN 1 AND 5),
      eats_on_site BOOLEAN,
      chef_is_banned BOOLEAN,
      address_id INT NOT NULL,
      PRIMARY KEY (account_id),
      FOREIGN KEY (account_id) REFERENCES Account (account_id) ON DELETE CASCADE,
      FOREIGN KEY (address_id) REFERENCES Address (address_id) ON DELETE RESTRICT
   );

CREATE TABLE
   OrderTransaction (
      transaction_id INT AUTO_INCREMENT,
      transaction_datetime DATETIME,
      account_id INT NOT NULL,
      PRIMARY KEY (transaction_id),
      FOREIGN KEY (account_id) REFERENCES Customer (account_id) ON DELETE CASCADE
   );

CREATE TABLE
   Company (
      account_id INT AUTO_INCREMENT,
      company_name VARCHAR(50) UNIQUE,
      contact_first_name VARCHAR(50),
      contact_last_name VARCHAR(50),
      PRIMARY KEY (account_id),
      FOREIGN KEY (account_id) REFERENCES Customer (account_id) ON DELETE CASCADE
   );

CREATE TABLE
   Individual (
      account_id INT AUTO_INCREMENT,
      last_name VARCHAR(50),
      first_name VARCHAR(50),
      personal_email VARCHAR(100),
      phone_number VARCHAR(50),
      address_id INT NOT NULL,
      PRIMARY KEY (account_id),
      FOREIGN KEY (account_id) REFERENCES Customer (account_id) ON DELETE CASCADE,
      FOREIGN KEY (address_id) REFERENCES Address (address_id) ON DELETE RESTRICT
   );

CREATE TABLE
   OrderLine (
      order_line_id INT AUTO_INCREMENT,
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
      address_id INT,
      transaction_id INT,
      account_id INT,
      PRIMARY KEY (order_line_id),
      FOREIGN KEY (address_id) REFERENCES Address (address_id) ON DELETE SET NULL,
      FOREIGN KEY (transaction_id) REFERENCES OrderTransaction (transaction_id) ON DELETE SET NULL,
      FOREIGN KEY (account_id) REFERENCES Chef (account_id) ON DELETE SET NULL
   );

CREATE TABLE
   Review (
      review_id INT AUTO_INCREMENT,
      review_type ENUM ('customer', 'chef'),
      review_rating DECIMAL(2, 1) CHECK (review_rating BETWEEN 1 AND 5),
      comment VARCHAR(500),
      review_date DATE,
      order_line_id INT NOT NULL,
      PRIMARY KEY (review_id),
      FOREIGN KEY (order_line_id) REFERENCES OrderLine (order_line_id) ON DELETE CASCADE
   );

CREATE TABLE
   MenuProposal (
      account_id INT,
      proposal_date DATE,
      dish_id INT NOT NULL,
      PRIMARY KEY (account_id, proposal_date),
      FOREIGN KEY (account_id) REFERENCES Chef (account_id),
      FOREIGN KEY (dish_id) REFERENCES Dish (dish_id) ON DELETE RESTRICT
   );

CREATE TABLE
   Contains (
      ingredient_id INT,
      dish_id INT,
      PRIMARY KEY (ingredient_id, dish_id),
      FOREIGN KEY (ingredient_id) REFERENCES Ingredient (ingredient_id) ON DELETE CASCADE,
      FOREIGN KEY (dish_id) REFERENCES Dish (dish_id) ON DELETE CASCADE
   );

INSERT INTO
   Account (account_email, account_password)
VALUES
   ('user1@email.com', 'pass1234'),
   ('user2@email.com', 'secure567'),
   ('user3@email.com', 'chef7890'),
   ('user4@email.com', 'random654'),
   ('user5@email.com', 'qwerty999'),
   ('user6@email.com', 'foodie321'),
   ('user7@email.com', 'delish876'),
   ('user8@email.com', 'gourmet111'),
   ('user9@email.com', 'cuisine555'),
   ('user10@email.com', 'taste890'),
   ('user11@email.com', 'flavor987'),
   ('user12@email.com', 'savory654'),
   ('user13@email.com', 'chefpass123'),
   ('user14@email.com', 'cook987'),
   ('user15@email.com', 'plate876'),
   ('user16@email.com', 'feast222'),
   ('user17@email.com', 'dish789'),
   ('user18@email.com', 'yumyum654'),
   ('user19@email.com', 'bite321'),
   ('user20@email.com', 'recipe777'),
   ('user21@email.com', 'spice987'),
   ('user22@email.com', 'meal432'),
   ('user23@email.com', 'fresh999'),
   ('user24@email.com', 'delight789'),
   ('user25@email.com', 'sweet654'),
   ('user26@email.com', 'tasty321'),
   ('user27@email.com', 'zesty876'),
   ('user28@email.com', 'flavor999'),
   ('user29@email.com', 'chefmagic123'),
   ('user30@email.com', 'pastry888'),
   ('user31@email.com', 'eatwell999'),
   ('user32@email.com', 'munch654'),
   ('user33@email.com', 'grill567'),
   ('user34@email.com', 'fryit999'),
   ('user35@email.com', 'broil321'),
   ('user36@email.com', 'roast222'),
   ('user37@email.com', 'steam654'),
   ('user38@email.com', 'sizzle123'),
   ('user39@email.com', 'braise987'),
   ('user40@email.com', 'poach876'),
   ('user41@email.com', 'saute999'),
   ('user42@email.com', 'bake555'),
   ('user43@email.com', 'boil999'),
   ('user44@email.com', 'flambe432'),
   ('user45@email.com', 'caramel999'),
   ('user46@email.com', 'chop654'),
   ('user47@email.com', 'dice321'),
   ('user48@email.com', 'mince777'),
   ('user49@email.com', 'knead987'),
   ('user50@email.com', 'whisk789');

INSERT INTO
   Customer (
      account_id,
      customer_rating,
      loyalty_rank,
      customer_is_banned
   )
VALUES
   (1, 4.5, 'Gold', 0),
   (2, 3.8, 'Silver', 0),
   (3, 4.0, 'Bronze', 0),
   (4, 2.5, 'Classic', 1),
   (5, 5.0, 'Gold', 0),
   (6, 3.7, 'Silver', 0),
   (7, 4.2, 'Gold', 0),
   (8, 4.9, 'Gold', 0),
   (9, 3.5, 'Bronze', 0),
   (10, 4.1, 'Silver', 0),
   (11, 3.3, 'Classic', 0),
   (12, 4.7, 'Gold', 0),
   (13, 3.9, 'Silver', 0),
   (14, 4.3, 'Gold', 0),
   (15, 2.8, 'Classic', 1),
   (16, 4.6, 'Gold', 0),
   (17, 3.2, 'Bronze', 0),
   (18, 4.0, 'Silver', 0),
   (19, 3.6, 'Bronze', 0),
   (20, 4.8, 'Gold', 0),
   (21, 3.1, 'Silver', 0),
   (22, 4.4, 'Gold', 0),
   (23, 2.9, 'Classic', 1),
   (24, 5.0, 'Gold', 0),
   (25, 3.0, 'Bronze', 0);

INSERT INTO
   Address (address_number, street, nearest_metro)
VALUES
   (1, 'Rue de Rivoli', 'Châtelet'),
   (
      2,
      'Avenue des Champs-Élysées',
      'Franklin D. Roosevelt'
   ),
   (3, 'Boulevard Haussmann', 'Havre-Caumartin'),
   (4, 'Rue Saint-Honoré', 'Tuileries'),
   (5, 'Place de la République', 'République'),
   (6, 'Rue de la Paix', 'Opéra'),
   (7, 'Avenue Montaigne', 'Alma-Marceau'),
   (8, 'Rue de Vaugirard', 'Montparnasse-Bienvenüe'),
   (9, 'Boulevard Saint-Germain', 'Odéon'),
   (10, 'Rue de Rennes', 'Saint-Sulpice'),
   (11, 'Rue Mouffetard', 'Censier-Daubenton'),
   (12, 'Rue du Faubourg Saint-Antoine', 'Bastille'),
   (13, 'Rue de la Roquette', 'Voltaire'),
   (14, 'Avenue de Clichy', 'Place de Clichy'),
   (15, 'Rue Oberkampf', 'Parmentier'),
   (16, 'Rue de Belleville', 'Jourdain'),
   (17, 'Rue Lepic', 'Blanche'),
   (18, 'Rue de la Pompe', 'La Muette'),
   (19, 'Rue de Bercy', 'Bercy'),
   (20, 'Rue des Rosiers', 'Saint-Paul'),
   (21, 'Rue du Bac', 'Rue du Bac'),
   (22, 'Avenue Victor Hugo', 'Victor Hugo'),
   (23, 'Rue de Sèvres', 'Sèvres-Babylone'),
   (24, 'Rue de Passy', 'Passy'),
   (25, 'Rue des Martyrs', 'Saint-Georges'),
   (26, 'Rue de Charenton', 'Ledru-Rollin'),
   (27, 'Boulevard Voltaire', 'Oberkampf'),
   (28, 'Boulevard Raspail', 'Raspail'),
   (29, 'Rue Saint-Denis', 'Étienne Marcel'),
   (30, 'Rue du Faubourg', 'Poissonnière'),
   (31, 'Avenue de Wagram', 'Ternes'),
   (
      32,
      'Boulevard de Sébastopol',
      'Réaumur-Sébastopol'
   ),
   (33, 'Rue de Tolbiac', 'Tolbiac'),
   (34, 'Rue de la Glacière', 'Glacière'),
   (35, 'Rue de Courcelles', 'Courcelles'),
   (36, 'Rue de Charonne', 'Charonne'),
   (37, 'Boulevard Beaumarchais', 'Chemin Vert'),
   (38, 'Rue de Cléry', 'Sentier'),
   (39, 'Rue de Maubeuge', 'Gare du Nord'),
   (40, 'Rue de Lancry', 'Jacques Bonsergent'),
   (
      41,
      'Rue de la Goutte d_Or',
      'Barbès-Rochechouart'
   ),
   (42, 'Avenue Mozart', 'Ranelagh'),
   (43, 'Rue de Meaux', 'Laumière'),
   (44, 'Rue Saint-Maur', 'Rue Saint-Maur'),
   (45, 'Rue de la Convention', 'Convention'),
   (46, 'Rue Lafayette', 'Cadet'),
   (47, 'Rue de Dunkerque', 'Gare du Nord'),
   (
      48,
      'Rue de Provence',
      'Trinité-d_Estienne d_Orves'
   ),
   (49, 'Rue d_Alésia', 'Alésia'),
   (50, 'Rue Cambronne', 'Cambronne');

INSERT INTO
   Chef (
      account_id,
      chef_rating,
      eats_on_site,
      chef_is_banned,
      address_id
   )
VALUES
   (26, 4.7, 1, 0, 2),
   (28, 4.5, 0, 0, 3),
   (29, 4.8, 1, 0, 4),
   (30, 4.3, 1, 0, 5),
   (31, 4.2, 0, 0, 6),
   (32, 4.6, 1, 0, 7),
   (33, 4.0, 1, 0, 8),
   (34, 4.1, 0, 0, 9),
   (35, 4.9, 1, 0, 10),
   (36, 4.4, 1, 0, 11),
   (37, 4.0, 0, 1, 12),
   (38, 3.8, 1, 0, 13),
   (39, 4.5, 1, 0, 14),
   (40, 4.3, 0, 0, 15),
   (41, 4.2, 1, 0, 16),
   (42, 4.7, 0, 0, 17),
   (43, 4.6, 1, 0, 18),
   (44, 4.1, 1, 0, 19),
   (45, 3.9, 0, 1, 20),
   (46, 4.0, 1, 0, 21),
   (47, 4.8, 1, 0, 22),
   (48, 3.7, 0, 1, 23),
   (49, 4.5, 1, 0, 24),
   (50, 4.3, 0, 0, 25);

INSERT INTO
   OrderTransaction (transaction_datetime, account_id)
VALUES
   ('2023-10-01 12:00:00', 1),
   ('2023-10-02 13:00:00', 2),
   ('2023-10-03 14:00:00', 3),
   ('2023-10-04 15:00:00', 4),
   ('2023-10-05 16:00:00', 5),
   ('2023-10-01 17:00:00', 6),
   ('2023-10-02 18:00:00', 7),
   ('2023-10-03 19:00:00', 8),
   ('2023-10-04 20:00:00', 9),
   ('2023-10-05 21:00:00', 10),
   ('2023-10-06 22:00:00', 11),
   ('2023-10-01 23:00:00', 12),
   ('2023-10-02 00:00:00', 13),
   ('2023-10-03 01:00:00', 14),
   ('2023-10-04 02:00:00', 15),
   ('2023-10-05 03:00:00', 16),
   ('2023-10-06 04:00:00', 17),
   ('2023-10-01 05:00:00', 18),
   ('2023-10-02 06:00:00', 19),
   ('2023-10-03 07:00:00', 20);

INSERT INTO
   Company (
      account_id,
      company_name,
      contact_first_name,
      contact_last_name
   )
VALUES
   (21, 'Tech Innovators', 'Alice', 'Johnson'),
   (22, 'Green Solutions', 'Bob', 'Smith'),
   (23, 'Urban Developers', 'Charlie', 'Brown'),
   (24, 'Healthcare Partners', 'Diana', 'Davis'),
   (25, 'Finance Experts', 'Eve', 'Wilson');

INSERT INTO
   OrderLine (
      order_line_datetime,
      duration,
      order_line_status,
      is_eat_in,
      address_id,
      transaction_id,
      account_id
   )
VALUES
   (
      '2023-10-01 12:15:00',
      30,
      'delivered',
      FALSE,
      26,
      1,
      26
   ),
   (
      '2023-10-03 14:30:00',
      25,
      'prepared',
      FALSE,
      28,
      3,
      28
   ),
   (
      '2023-10-04 15:45:00',
      35,
      'pending',
      FALSE,
      29,
      4,
      29
   ),
   (
      '2023-10-05 16:50:00',
      40,
      'delivered',
      FALSE,
      30,
      5,
      30
   ),
   (
      '2023-10-01 17:55:00',
      30,
      'prepared',
      FALSE,
      31,
      6,
      31
   ),
   (
      '2023-10-02 18:00:00',
      20,
      'in_delivery',
      FALSE,
      32,
      7,
      32
   ),
   (
      '2023-10-03 19:10:00',
      25,
      'delivered',
      FALSE,
      33,
      8,
      33
   ),
   (
      '2023-10-04 20:20:00',
      35,
      'prepared',
      FALSE,
      34,
      9,
      34
   ),
   (
      '2023-10-05 21:30:00',
      45,
      'in_delivery',
      FALSE,
      35,
      10,
      35
   ),
   (
      '2023-10-06 22:40:00',
      30,
      'delivered',
      FALSE,
      36,
      11,
      36
   ),
   (
      '2023-10-01 23:50:00',
      25,
      'prepared',
      FALSE,
      37,
      12,
      37
   ),
   (
      '2023-10-02 01:00:00',
      35,
      'in_delivery',
      FALSE,
      38,
      13,
      38
   ),
   (
      '2023-10-03 02:10:00',
      40,
      'delivered',
      FALSE,
      39,
      14,
      39
   ),
   (
      '2023-10-04 03:20:00',
      30,
      'prepared',
      FALSE,
      40,
      15,
      40
   ),
   (
      '2023-10-05 04:30:00',
      25,
      'in_delivery',
      FALSE,
      41,
      16,
      41
   ),
   (
      '2023-10-06 05:40:00',
      35,
      'delivered',
      FALSE,
      42,
      17,
      42
   ),
   (
      '2023-10-01 06:50:00',
      40,
      'prepared',
      FALSE,
      43,
      18,
      43
   ),
   (
      '2023-10-02 08:00:00',
      25,
      'in_delivery',
      FALSE,
      44,
      19,
      44
   ),
   (
      '2023-10-03 09:10:00',
      30,
      'delivered',
      FALSE,
      45,
      20,
      45
   );

INSERT INTO
   Review (
      review_type,
      review_rating,
      comment,
      review_date,
      order_line_id
   )
VALUES
   (
      'customer',
      4.5,
      'The food was delicious and delivered on time!',
      '2023-10-01',
      1
   ),
   (
      'customer',
      4.0,
      'Loved the variety in the menu and the quick delivery.',
      '2023-10-03',
      3
   ),
   (
      'customer',
      2.5,
      'The food quality was not up to the mark.',
      '2023-10-04',
      4
   ),
   (
      'customer',
      5.0,
      'Excellent food and service, will order again!',
      '2023-10-05',
      5
   ),
   (
      'chef',
      3.7,
      'The customer was polite and provided clear instructions.',
      '2023-10-06',
      6
   ),
   (
      'customer',
      4.2,
      'Great taste and timely delivery.',
      '2023-10-07',
      7
   ),
   (
      'customer',
      4.9,
      'Amazing food quality and very friendly delivery person.',
      '2023-10-08',
      8
   ),
   (
      'chef',
      3.5,
      'The customer was easy to work with and appreciated the meal.',
      '2023-10-09',
      9
   ),
   (
      'customer',
      4.1,
      'A bit pricey but the food was worth it.',
      '2023-10-10',
      10
   ),
   (
      'customer',
      3.3,
      'The food was okay, but the delivery was late.',
      '2023-10-11',
      11
   ),
   (
      'customer',
      4.7,
      'Everything was perfect, from order to delivery!',
      '2023-10-12',
      12
   ),
   (
      'chef',
      3.9,
      'The customer was satisfied with the customization request.',
      '2023-10-13',
      13
   ),
   (
      'customer',
      4.3,
      'I enjoyed the variety and the freshness of the food.',
      '2023-10-14',
      14
   ),
   (
      'customer',
      2.8,
      'Disappointed with the cold food upon delivery.',
      '2023-10-15',
      15
   ),
   (
      'customer',
      4.6,
      'Highly recommend this service for quick and tasty meals.',
      '2023-10-16',
      16
   ),
   (
      'chef',
      3.2,
      'The customer requested a last-minute change, which was challenging.',
      '2023-10-17',
      17
   ),
   (
      'customer',
      4.0,
      'Good food and the delivery was on time.',
      '2023-10-18',
      18
   ),
   (
      'customer',
      3.6,
      'The food was good, but the delivery was a bit slow.',
      '2023-10-19',
      19
   ),
   (
      'customer',
      4.8,
      'One of the best food delivery experiences I’ve had.',
      '2023-10-20',
      20
   );

INSERT INTO
   Ingredient (
      ingredient_name,
      is_vegetarian,
      is_vegan,
      is_gluten_free,
      is_lactose_free,
      is_halal,
      is_kosher,
      product_origin
   )
VALUES
   (
      'Tomate',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'other'
   ),
   (
      'Poitrine de poulet',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      TRUE,
      FALSE,
      'france'
   ),
   (
      'Saumon',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ),
   (
      'Tofu',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Aubergine',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Farine de blé',
      TRUE,
      TRUE,
      FALSE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ),
   (
      'Lait',
      TRUE,
      FALSE,
      TRUE,
      FALSE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Fromage',
      TRUE,
      FALSE,
      TRUE,
      FALSE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Œufs',
      TRUE,
      FALSE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Beurre',
      TRUE,
      FALSE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Huile d_olive',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ),
   (
      'Ail',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Oignon',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Champignons',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Riz',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'other'
   ),
   (
      'Lentilles',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'other'
   ),
   (
      'Pois chiches',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ),
   (
      'Concombre',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ),
   (
      'Carotte',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Bœuf',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      FALSE,
      FALSE,
      'france'
   ),
   (
      'Agneau',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      FALSE,
      FALSE,
      'france'
   ),
   (
      'Dinde',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      TRUE,
      FALSE,
      'france'
   ),
   (
      'Poisson',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ),
   (
      'Crevettes',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      TRUE,
      FALSE,
      'france'
   ),
   (
      'Porc',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      FALSE,
      FALSE,
      'other'
   ),
   (
      'Homard',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      TRUE,
      FALSE,
      'other'
   ),
   (
      'Miel',
      TRUE,
      FALSE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Avocat',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'other'
   ),
   (
      'Cacahuètes',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'other'
   ),
   (
      'Amandes',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'other'
   ),
   (
      'Noix de cajou',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'other'
   ),
   (
      'Sauce soja',
      TRUE,
      TRUE,
      FALSE,
      TRUE,
      TRUE,
      TRUE,
      'other'
   ),
   (
      'Lait de coco',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'other'
   ),
   (
      'Maïs',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'other'
   ),
   (
      'Épinards',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Chou frisé',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ),
   (
      'Courgette',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ),
   (
      'Poivron',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ),
   (
      'Pomme de terre',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ),
   (
      'Patate douce',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Potiron',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Avoine',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Yaourt',
      TRUE,
      FALSE,
      TRUE,
      FALSE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Basilic',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Coriandre',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Thym',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Menthe',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Céleri',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ),
   (
      'Citron',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'france'
   ),
   (
      'Citron vert',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ),
   (
      'Gingembre',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ),
   (
      'Curcuma',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      'europe'
   ) (
      'Steak haché',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      FALSE,
      FALSE,
      'other'
   ),
   (
      'Fromage cheddar',
      TRUE,
      FALSE,
      TRUE,
      FALSE,
      TRUE,
      FALSE,
      'other'
   ),
   (
      'Pain à hamburger',
      TRUE,
      TRUE,
      FALSE,
      TRUE,
      TRUE,
      FALSE,
      'europe'
   ),
   (
      'Fromage en grains',
      TRUE,
      FALSE,
      TRUE,
      FALSE,
      FALSE
   );

INSERT INTO
   Dish (
      dish_name,
      dish_type,
      expiry_time,
      cuisine_nationality,
      quantity,
      price,
      photo_path
   )
VALUES
   (
      'Ratatouille',
      'main_course',
      2,
      'french',
      10,
      15.99,
      NULL
   ),
   (
      'Bœuf Bourguignon',
      'main_course',
      2,
      'french',
      8,
      22.50,
      NULL
   ),
   (
      'Coq au Vin',
      'main_course',
      3,
      'french',
      6,
      19.99,
      NULL
   ),
   (
      'Soupe à l_Oignon',
      'starter',
      4,
      'french',
      12,
      9.50,
      NULL
   ),
   (
      'Poulet Basquaise',
      'main_course',
      3,
      'french',
      8,
      17.99,
      NULL
   ),
   (
      'Salade Niçoise',
      'starter',
      1,
      'french',
      15,
      12.99,
      NULL
   ),
   (
      'Quiche Lorraine',
      'main_course',
      2,
      'french',
      10,
      14.99,
      NULL
   ),
   (
      'Tartiflette',
      'main_course',
      3,
      'french',
      7,
      18.99,
      NULL
   ),
   (
      'Gratin Dauphinois',
      'main_course',
      2,
      'french',
      9,
      11.99,
      NULL
   ),
   (
      'Bouillabaisse',
      'main_course',
      4,
      'french',
      6,
      24.99,
      NULL
   ),
   (
      'Cassoulet',
      'main_course',
      3,
      'arabic',
      7,
      20.99,
      NULL
   ),
   (
      'Mousse au Chocolat',
      'dessert',
      1,
      'french',
      15,
      6.99,
      NULL
   ),
   (
      'Crêpes',
      'dessert',
      1,
      'french',
      20,
      7.99,
      NULL
   ),
   (
      'Soufflé au Fromage',
      'main_course',
      2,
      'french',
      6,
      13.99,
      NULL
   ),
   (
      'Hamburger',
      'main_course',
      2,
      'german',
      15,
      9.99,
      NULL
   ),
   (
      'Tacos al Pastor',
      'main_course',
      2,
      'mexican',
      20,
      12.50,
      NULL
   ),
   (
      'Poutine',
      'main_course',
      2,
      'canadian',
      12,
      8.75,
      NULL
   ),
   (
      'Feijoada',
      'main_course',
      2,
      'brazilian',
      10,
      16.25,
      NULL
   ),
   (
      'Ceviche',
      'starter',
      2,
      'peruvian',
      6,
      11.00,
      NULL
   ),
   (
      'Pizza Margherita',
      'main_course',
      2,
      'italian',
      2,
      14.00,
      NULL
   ),
   (
      'Paella',
      'main_course',
      1,
      'Spanish',
      8,
      25.50,
      NULL
   ),
   (
      'Sushi (Nigiri)',
      'main_course',
      1,
      'japanese',
      14,
      18.00,
      NULL
   ),
   (
      'Bibimbap',
      'main_course',
      3,
      'korean',
      17,
      14.50,
      NULL
   ),
   (
      'Tajine',
      'main_course',
      4,
      'arabic',
      9,
      17.00,
      NULL
   ),
   (
      'Falafel',
      'starter',
      2,
      'arabic',
      3,
      9.25,
      NULL
   ),
   (
      'Kebab',
      'main_course',
      1,
      'arabic',
      1,
      11.75,
      NULL
   );

INSERT INTO
   MenuProposal (account_id, proposal_date, dish_id)
VALUES
   (26, '2023-10-01', 1),
   (28, '2023-10-01', 2),
   (29, '2023-10-01', 3),
   (30, '2023-10-01', 4),
   (31, '2023-10-01', 5),
   (32, '2023-10-01', 6),
   (33, '2023-10-01', 7),
   (34, '2023-10-01', 8),
   (35, '2023-10-01', 9),
   (36, '2023-10-01', 10),
   (37, '2023-10-01', 23),
   (38, '2023-10-01', 24),
   (39, '2023-10-01', 25),
   (40, '2023-10-01', 26),
   (41, '2023-10-01', 27),
   (26, '2023-10-02', 5),
   (28, '2023-10-02', 6),
   (29, '2023-10-02', 7),
   (30, '2023-10-02', 8),
   (31, '2023-10-02', 9),
   (32, '2023-10-02', 10),
   (33, '2023-10-02', 23),
   (34, '2023-10-02', 24),
   (35, '2023-10-02', 25),
   (36, '2023-10-02', 26),
   (37, '2023-10-02', 27),
   (38, '2023-10-02', 1),
   (39, '2023-10-02', 2),
   (40, '2023-10-02', 3),
   (41, '2023-10-02', 4),
   (26, '2023-10-03', 1),
   (28, '2023-10-03', 2),
   (29, '2023-10-03', 3),
   (30, '2023-10-03', 4),
   (31, '2023-10-03', 5),
   (32, '2023-10-03', 6),
   (33, '2023-10-03', 7),
   (34, '2023-10-03', 8),
   (35, '2023-10-03', 9),
   (36, '2023-10-03', 10),
   (37, '2023-10-03', 23),
   (38, '2023-10-03', 24),
   (39, '2023-10-03', 25),
   (40, '2023-10-03', 26),
   (41, '2023-10-03', 27),
   (26, '2023-10-04', 5),
   (28, '2023-10-04', 6),
   (29, '2023-10-04', 7),
   (30, '2023-10-04', 8),
   (31, '2023-10-04', 9),
   (32, '2023-10-04', 10),
   (33, '2023-10-04', 23),
   (34, '2023-10-04', 24),
   (35, '2023-10-04', 25),
   (36, '2023-10-04', 26),
   (37, '2023-10-04', 27),
   (38, '2023-10-04', 1),
   (39, '2023-10-04', 2),
   (40, '2023-10-04', 3),
   (41, '2023-10-04', 4),
   (26, '2023-10-05', 1),
   (28, '2023-10-05', 2),
   (29, '2023-10-05', 3),
   (30, '2023-10-05', 4),
   (31, '2023-10-05', 5),
   (32, '2023-10-05', 6),
   (33, '2023-10-05', 7),
   (34, '2023-10-05', 8),
   (35, '2023-10-05', 9),
   (36, '2023-10-05', 10),
   (37, '2023-10-05', 23),
   (38, '2023-10-05', 24),
   (39, '2023-10-05', 25),
   (40, '2023-10-05', 26),
   (41, '2023-10-05', 27),
   (26, '2023-10-06', 5),
   (28, '2023-10-06', 6),
   (29, '2023-10-06', 7),
   (30, '2023-10-06', 8),
   (31, '2023-10-06', 9),
   (32, '2023-10-06', 10),
   (33, '2023-10-06', 23),
   (34, '2023-10-06', 24),
   (35, '2023-10-06', 25),
   (36, '2023-10-06', 26),
   (37, '2023-10-06', 27),
   (38, '2023-10-06', 1),
   (39, '2023-10-06', 2),
   (40, '2023-10-06', 3),
   (41, '2023-10-06', 4);

//FIXME: Contains avec Id

INSERT INTO
   Contains (dish_id, ingredient_id)
VALUES
   ('Ratatouille', 'Tomate'),
   ('Ratatouille', 'Aubergine'),
   ('Ratatouille', 'Courgette'),
   ('Ratatouille', 'Poivron'),
   ('Ratatouille', 'Oignon'),
   ('Bœuf Bourguignon', 'Bœuf'),
   ('Bœuf Bourguignon', 'Vin rouge'),
   ('Bœuf Bourguignon', 'Oignon'),
   ('Bœuf Bourguignon', 'Carotte'),
   ('Coq au Vin', 'Poitrine de poulet'),
   ('Coq au Vin', 'Vin rouge'),
   ('Coq au Vin', 'Champignons'),
   ('Coq au Vin', 'Oignon'),
   ('Soupe à l_Oignon', 'Oignon'),
   ('Soupe à l_Oignon', 'Beurre'),
   ('Soupe à l_Oignon', 'Fromage'),
   ('Poulet Basquaise', 'Poitrine de poulet'),
   ('Poulet Basquaise', 'Poivron'),
   ('Poulet Basquaise', 'Tomate'),
   ('Poulet Basquaise', 'Oignon'),
   ('Salade Niçoise', 'Tomate'),
   ('Salade Niçoise', 'Œufs'),
   ('Salade Niçoise', 'Olives'),
   ('Salade Niçoise', 'Thon'),
   ('Salade Niçoise', 'Concombre'),
   ('Quiche Lorraine', 'Œufs'),
   ('Quiche Lorraine', 'Lait'),
   ('Quiche Lorraine', 'Fromage'),
   ('Quiche Lorraine', 'Farine de blé'),
   ('Tartiflette', 'Pommes de terre'),
   ('Tartiflette', 'Fromage'),
   ('Tartiflette', 'Crème fraîche'),
   ('Gratin Dauphinois', 'Pommes de terre'),
   ('Gratin Dauphinois', 'Lait'),
   ('Gratin Dauphinois', 'Beurre'),
   ('Bouillabaisse', 'Saumon'),
   ('Bouillabaisse', 'Crevettes'),
   ('Bouillabaisse', 'Homard'),
   ('Bouillabaisse', 'Tomate'),
   ('Cassoulet', 'Haricots blancs'),
   ('Cassoulet', 'Bœuf'),
   ('Cassoulet', 'Carotte'),
   ('Cassoulet', 'Oignon'),
   ('Tarte Tatin', 'Pommes'),
   ('Tarte Tatin', 'Beurre'),
   ('Tarte Tatin', 'Farine de blé'),
   ('Mousse au Chocolat', 'Chocolat'),
   ('Mousse au Chocolat', 'Œufs'),
   ('Crêpes', 'Farine de blé'),
   ('Crêpes', 'Lait'),
   ('Crêpes', 'Œufs'),
   ('Soufflé au Fromage', 'Fromage'),
   ('Soufflé au Fromage', 'Lait'),
   ('Soufflé au Fromage', 'Œufs'),
   ('Steak haché', 'Cheeseburger and Fries'),
   ('Fromage cheddar', 'Cheeseburger and Fries'),
   ('Pain à hamburger', 'Cheeseburger and Fries'),
   ('Tomate', 'Cheeseburger and Fries'),
   ('Oignon', 'Cheeseburger and Fries'),
   ('Frites', 'Cheeseburger and Fries'),
   ('Frites', 'Poutine'),
   ('Fromage en grains', 'Poutine'),
   ('Sauce brune', 'Poutine'),
   ('Haricots noirs', 'Feijoada'),
   ('Diverses coupes de porc', 'Feijoada'),
   ('Oignon', 'Feijoada'),
   ('Ail', 'Feijoada'),
   ('Poisson blanc cru', 'Ceviche'),
   ('Citron vert', 'Ceviche'),
   ('Oignon', 'Ceviche'),
   ('Coriandre', 'Ceviche'),
   ('Piments', 'Ceviche'),
   ('Farine de blé', 'Pizza Margherita'),
   ('Tomate', 'Pizza Margherita'),
   ('Mozzarella', 'Pizza Margherita'),
   ('Basilic frais', 'Pizza Margherita'),
   ('Huile d_olive', 'Pizza Margherita'),
   ('Riz', 'Paella'),
   ('Mélange de fruits de mer', 'Paella'),
   ('Safran', 'Paella'),
   ('Poivron', 'Paella'),
   ('Tomate', 'Paella'),
   ('Oignon', 'Paella'),
   ('Poulet grillé sur brochette', 'Souvlaki'),
   ('Pain pita', 'Souvlaki'),
   ('Sauce tzatziki', 'Souvlaki'),
   ('Tomate', 'Souvlaki'),
   ('Oignon', 'Souvlaki'),
   ('Farine de blé', 'Dumplings (Jiaozi)'),
   ('Chou', 'Dumplings (Jiaozi)'),
   ('Viande hachée (porc/bœuf)', 'Dumplings (Jiaozi)'),
   ('Gingembre', 'Dumplings (Jiaozi)'),
   ('Coriandre', 'Dumplings (Jiaozi)'),
   ('Ail', 'Dumplings (Jiaozi)'),
   ('Sauce soja', 'Dumplings (Jiaozi)'),
   ('Huile de sésame', 'Dumplings (Jiaozi)'),
   ('Riz', 'Sushi (Nigiri)'),
   ('Saumon', 'Sushi (Nigiri)'),
   ('Vinaigre', 'Sushi (Nigiri)'),
   ('Poulet cuit en sauce', 'Butter Chicken'),
   ('Tomate', 'Butter Chicken'),
   ('Crème', 'Butter Chicken'),
   ('Épices diverses', 'Butter Chicken'),
   ('Beurre', 'Butter Chicken'),
   ('Nouilles de riz', 'Pad Thai'),
   ('Crevettes', 'Pad Thai'),
   ('Germes de soja', 'Pad Thai'),
   ('Cacahuètes', 'Pad Thai'),
   ('Œufs', 'Pad Thai'),
   ('Sauce soja', 'Pad Thai'),
   ('Coriandre', 'Pad Thai'),
   ('Riz', 'Bibimbap'),
   ('Légumes variés', 'Bibimbap'),
   ('Œufs', 'Bibimbap'),
   ('Gochujang (pâte de piment)', 'Bibimbap'),
   ('Viande hachée (bœuf/agneau)', 'Bibimbap'),
   ('Agneau mijoté', 'Tagine'),
   ('Fruits secs', 'Tagine'),
   ('Épices diverses', 'Tagine'),
   ('Amandes', 'Tagine'),
   ('Carotte', 'Tagine'),
   ('Farine de teff', 'Injera with Wat'),
   ('Ragoût (avec viande/légumes)', 'Injera with Wat'),
   ('Oignon', 'Injera with Wat'),
   ('Ail', 'Injera with Wat'),
   ('Épices diverses', 'Injera with Wat'),
   ('Pois chiches', 'Falafel'),
   ('Ail', 'Falafel'),
   ('Coriandre', 'Falafel'),
   ('Persil', 'Falafel'),
   ('Tahini (pâte de sésame)', 'Falafel'),
   ('Agneau grillé sur brochette', 'Kebab'),
   ('Pain pita', 'Kebab'),
   ('Oignon', 'Kebab'),
   ('Tomate', 'Kebab'),
   ('Sauce de viande', 'Kebab');