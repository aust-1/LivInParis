\r livinparisroussilleteynier

CREATE TABLE
   Account (
      account_id INT AUTO_INCREMENT,
      account_email VARCHAR(100),
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
   Account (account_id, password)
VALUES
   (1, 'pass1234'),
   (2, 'secure567'),
   (3, 'chef7890'),
   (4, 'random654'),
   (5, 'qwerty999'),
   (6, 'foodie321'),
   (7, 'delish876'),
   (8, 'gourmet111'),
   (9, 'cuisine555'),
   (10, 'taste890'),
   (11, 'flavor987'),
   (12, 'savory654'),
   (13, 'chefpass123'),
   (14, 'cook987'),
   (15, 'plate876'),
   (16, 'feast222'),
   (17, 'dish789'),
   (18, 'yumyum654'),
   (19, 'bite321'),
   (20, 'recipe777'),
   (21, 'spice987'),
   (22, 'meal432'),
   (23, 'fresh999'),
   (24, 'delight789'),
   (25, 'sweet654'),
   (26, 'tasty321'),
   (27, 'zesty876'),
   (28, 'flavor999'),
   (29, 'chefmagic123'),
   (30, 'pastry888'),
   (31, 'eatwell999'),
   (32, 'munch654'),
   (33, 'grill567'),
   (34, 'fryit999'),
   (35, 'broil321'),
   (36, 'roast222'),
   (37, 'steam654'),
   (38, 'sizzle123'),
   (39, 'braise987'),
   (40, 'poach876'),
   (41, 'saute999'),
   (42, 'bake555'),
   (43, 'boil999'),
   (44, 'flambe432'),
   (45, 'caramel999'),
   (46, 'chop654'),
   (47, 'dice321'),
   (48, 'mince777'),
   (49, 'knead987'),
   (50, 'whisk789');

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
   Adress (number, street, nearest_metro)
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
   (
      30,
      'Rue du Faubourg Poissonnière',
      'Poissonnière'
   ),
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
      'Rue de la Goutte d’Or',
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
      'Trinité-d’Estienne d’Orves'
   ),
   (49, 'Rue d’Alésia', 'Alésia'),
   (50, 'Rue Cambronne', 'Cambronne');

INSERT INTO
   Chef (
      account_id,
      chef_rating,
      eats_on_site,
      chef_is_banned,
      number,
      street
   )
VALUES
   (26, 4.7, 1, 0, 2, 'Avenue des Champs-Élysées'),
   (28, 4.5, 0, 0, 3, 'Boulevard Haussmann'),
   (29, 4.8, 1, 0, 4, 'Rue Saint-Honoré'),
   (30, 4.3, 1, 0, 5, 'Place de la République'),
   (31, 4.2, 0, 0, 6, 'Rue de la Paix'),
   (32, 4.6, 1, 0, 7, 'Avenue Montaigne'),
   (33, 4.0, 1, 0, 8, 'Rue de Vaugirard'),
   (34, 4.1, 0, 0, 9, 'Boulevard Saint-Germain'),
   (35, 4.9, 1, 0, 10, 'Rue de Rennes'),
   (36, 4.4, 1, 0, 11, 'Rue Mouffetard'),
   (
      37,
      4.0,
      0,
      1,
      12,
      'Rue du Faubourg Saint-Antoine'
   ),
   (38, 3.8, 1, 0, 13, 'Rue de la Roquette'),
   (39, 4.5, 1, 0, 14, 'Avenue de Clichy'),
   (40, 4.3, 0, 0, 15, 'Rue Oberkampf'),
   (41, 4.2, 1, 0, 16, 'Rue de Belleville'),
   (42, 4.7, 0, 0, 17, 'Rue Lepic'),
   (43, 4.6, 1, 0, 18, 'Rue de la Pompe'),
   (44, 4.1, 1, 0, 19, 'Rue de Bercy'),
   (45, 3.9, 0, 1, 20, 'Rue des Rosiers'),
   (46, 4.0, 1, 0, 21, 'Rue du Bac'),
   (47, 4.8, 1, 0, 22, 'Avenue Victor Hugo'),
   (48, 3.7, 0, 1, 23, 'Rue de Sèvres'),
   (49, 4.5, 1, 0, 24, 'Rue de Passy'),
   (50, 4.3, 0, 0, 25, 'Rue des Martyrs');

INSERT INTO
   Transaction (transaction_id, transaction_datetime, account_id)
VALUES
   (1, '2023-10-01 12:00:00', 1),
   (2, '2023-10-02 13:00:00', 2),
   (3, '2023-10-03 14:00:00', 3),
   (4, '2023-10-04 15:00:00', 4),
   (5, '2023-10-05 16:00:00', 5),
   (6, '2023-10-06 17:00:00', 6),
   (7, '2023-10-07 18:00:00', 7),
   (8, '2023-10-08 19:00:00', 8),
   (9, '2023-10-09 20:00:00', 9),
   (10, '2023-10-10 21:00:00', 10),
   (11, '2023-10-11 22:00:00', 11),
   (12, '2023-10-12 23:00:00', 12),
   (13, '2023-10-13 00:00:00', 13),
   (14, '2023-10-14 01:00:00', 14),
   (15, '2023-10-15 02:00:00', 15),
   (16, '2023-10-16 03:00:00', 16),
   (17, '2023-10-17 04:00:00', 17),
   (18, '2023-10-18 05:00:00', 18),
   (19, '2023-10-19 06:00:00', 19),
   (20, '2023-10-20 07:00:00', 20);

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
      order_line_id,
      order_line_datetime,
      duration,
      status,
      is_eat_in,
      number,
      street,
      transaction_id,
      account_id
   )
VALUES
   (
      1,
      '2023-10-01 12:15:00',
      30,
      'delivered',
      FALSE,
      26,
      'Rue de Charenton',
      1,
      26
   ),
   (
      3,
      '2023-10-03 14:30:00',
      25,
      'prepared',
      FALSE,
      28,
      'Boulevard Raspail',
      3,
      28
   ),
   (
      4,
      '2023-10-04 15:45:00',
      35,
      'pending',
      FALSE,
      29,
      'Rue Saint-Denis',
      4,
      29
   ),
   (
      5,
      '2023-10-05 16:50:00',
      40,
      'delivered',
      FALSE,
      30,
      'Rue du Faubourg Poissonnière',
      5,
      30
   ),
   (
      6,
      '2023-10-06 17:55:00',
      30,
      'prepared',
      FALSE,
      31,
      'Avenue de Wagram',
      6,
      31
   ),
   (
      7,
      '2023-10-07 18:00:00',
      20,
      'in_delivery',
      FALSE,
      32,
      'Boulevard de Sébastopol',
      7,
      32
   ),
   (
      8,
      '2023-10-08 19:10:00',
      25,
      'delivered',
      FALSE,
      33,
      'Rue de Tolbiac',
      8,
      33
   ),
   (
      9,
      '2023-10-09 20:20:00',
      35,
      'prepared',
      FALSE,
      34,
      'Rue de la Glacière',
      9,
      34
   ),
   (
      10,
      '2023-10-10 21:30:00',
      45,
      'in_delivery',
      FALSE,
      35,
      'Rue de Courcelles',
      10,
      35
   ),
   (
      11,
      '2023-10-11 22:40:00',
      30,
      'delivered',
      FALSE,
      36,
      'Rue de Charonne',
      11,
      36
   ),
   (
      12,
      '2023-10-12 23:50:00',
      25,
      'prepared',
      FALSE,
      37,
      'Boulevard Beaumarchais',
      12,
      37
   ),
   (
      13,
      '2023-10-13 01:00:00',
      35,
      'in_delivery',
      FALSE,
      38,
      'Rue de Cléry',
      13,
      38
   ),
   (
      14,
      '2023-10-14 02:10:00',
      40,
      'delivered',
      FALSE,
      39,
      'Rue de Maubeuge',
      14,
      39
   ),
   (
      15,
      '2023-10-15 03:20:00',
      30,
      'prepared',
      FALSE,
      40,
      'Rue de Lancry',
      15,
      40
   ),
   (
      16,
      '2023-10-16 04:30:00',
      25,
      'in_delivery',
      FALSE,
      41,
      'Rue de la Goutte d’Or',
      16,
      41
   ),
   (
      17,
      '2023-10-17 05:40:00',
      35,
      'delivered',
      FALSE,
      42,
      'Avenue Mozart',
      17,
      42
   ),
   (
      18,
      '2023-10-18 06:50:00',
      40,
      'prepared',
      FALSE,
      43,
      'Rue de Meaux',
      18,
      43
   ),
   (
      19,
      '2023-10-19 08:00:00',
      25,
      'in_delivery',
      FALSE,
      44,
      'Rue Saint-Maur',
      19,
      44
   ),
   (
      20,
      '2023-10-20 09:10:00',
      30,
      'delivered',
      FALSE,
      45,
      'Rue de la Convention',
      20,
      45
   );

INSERT INTO
   Review (
      review_id,
      review_type,
      review_rating,
      comment,
      review_date,
      order_line_id
   )
VALUES
   (
      1,
      'client',
      4.5,
      'The food was delicious and delivered on time!',
      '2023-10-01',
      1
   ),
   (
      3,
      'client',
      4.0,
      'Loved the variety in the menu and the quick delivery.',
      '2023-10-03',
      3
   ),
   (
      4,
      'client',
      2.5,
      'The food quality was not up to the mark.',
      '2023-10-04',
      4
   ),
   (
      5,
      'client',
      5.0,
      'Excellent food and service, will order again!',
      '2023-10-05',
      5
   ),
   (
      6,
      'cuisinier',
      3.7,
      'The client was polite and provided clear instructions.',
      '2023-10-06',
      6
   ),
   (
      7,
      'client',
      4.2,
      'Great taste and timely delivery.',
      '2023-10-07',
      7
   ),
   (
      8,
      'client',
      4.9,
      'Amazing food quality and very friendly delivery person.',
      '2023-10-08',
      8
   ),
   (
      9,
      'cuisinier',
      3.5,
      'The client was easy to work with and appreciated the meal.',
      '2023-10-09',
      9
   ),
   (
      10,
      'client',
      4.1,
      'A bit pricey but the food was worth it.',
      '2023-10-10',
      10
   ),
   (
      11,
      'client',
      3.3,
      'The food was okay, but the delivery was late.',
      '2023-10-11',
      11
   ),
   (
      12,
      'client',
      4.7,
      'Everything was perfect, from order to delivery!',
      '2023-10-12',
      12
   ),
   (
      13,
      'cuisinier',
      3.9,
      'The client was satisfied with the customization request.',
      '2023-10-13',
      13
   ),
   (
      14,
      'client',
      4.3,
      'I enjoyed the variety and the freshness of the food.',
      '2023-10-14',
      14
   ),
   (
      15,
      'client',
      2.8,
      'Disappointed with the cold food upon delivery.',
      '2023-10-15',
      15
   ),
   (
      16,
      'client',
      4.6,
      'Highly recommend this service for quick and tasty meals.',
      '2023-10-16',
      16
   ),
   (
      17,
      'cuisinier',
      3.2,
      'The client requested a last-minute change, which was challenging.',
      '2023-10-17',
      17
   ),
   (
      18,
      'client',
      4.0,
      'Good food and the delivery was on time.',
      '2023-10-18',
      18
   ),
   (
      19,
      'client',
      3.6,
      'The food was good, but the delivery was a bit slow.',
      '2023-10-19',
      19
   ),
   (
      20,
      'client',
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
      is_halal,
      is_kosher
   )
VALUES
   ('Tomate', TRUE, TRUE, TRUE, TRUE, TRUE),
   (
      'Poitrine de poulet',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      FALSE
   ),
   ('Saumon', FALSE, FALSE, TRUE, TRUE, TRUE),
   ('Tofu', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Aubergine', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Farine de blé', TRUE, TRUE, FALSE, TRUE, TRUE),
   ('Lait', TRUE, FALSE, TRUE, TRUE, TRUE),
   ('Fromage', TRUE, FALSE, TRUE, TRUE, TRUE),
   ('Œufs', TRUE, FALSE, TRUE, TRUE, TRUE),
   ('Beurre', TRUE, FALSE, TRUE, TRUE, TRUE),
   ('Huile d’olive', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Ail', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Oignon', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Champignons', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Riz', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Lentilles', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Pois chiches', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Concombre', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Carotte', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Bœuf', FALSE, FALSE, TRUE, FALSE, FALSE),
   ('Porc', FALSE, FALSE, TRUE, FALSE, FALSE),
   ('Crevettes', FALSE, FALSE, TRUE, TRUE, FALSE),
   ('Homard', FALSE, FALSE, TRUE, TRUE, FALSE),
   ('Miel', TRUE, FALSE, TRUE, TRUE, TRUE),
   ('Avocat', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Cacahuètes', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Amandes', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Noix de cajou', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Sauce soja', TRUE, TRUE, FALSE, TRUE, TRUE),
   ('Lait de coco', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Maïs', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Épinards', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Chou frisé', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Courgette', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Poivron', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Pomme de terre', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Patate douce', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Potiron', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Avoine', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Yaourt', TRUE, FALSE, TRUE, TRUE, TRUE),
   ('Basilic', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Coriandre', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Thym', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Menthe', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Céleri', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Citron', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Citron vert', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Gingembre', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Curcuma', TRUE, TRUE, TRUE, TRUE, TRUE);

INSERT INTO
   Ingredient (
      ingredient_name,
      is_vegetarian,
      is_vegan,
      is_gluten_free,
      is_halal,
      is_kosher
   )
VALUES
   ('Steak haché', FALSE, FALSE, TRUE, FALSE, FALSE),
   (
      'Fromage cheddar',
      TRUE,
      FALSE,
      TRUE,
      FALSE,
      FALSE
   ),
   (
      'Pain à hamburger',
      TRUE,
      FALSE,
      FALSE,
      TRUE,
      FALSE
   ),
   (
      'Porc mariné (en fines tranches)',
      FALSE,
      FALSE,
      TRUE,
      FALSE,
      FALSE
   ),
   ('Tortillas de maïs', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Ananas', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Frites', TRUE, TRUE, TRUE, TRUE, TRUE), -- Vérifier l'huile de cuisson pour le végane
   (
      'Fromage en grains',
      TRUE,
      FALSE,
      TRUE,
      FALSE,
      FALSE
   ),
   ('Sauce brune', FALSE, FALSE, FALSE, FALSE, FALSE), -- Contient souvent du bouillon de viande et de la farine de blé
   ('Haricots noirs', TRUE, TRUE, TRUE, TRUE, TRUE),
   (
      'Diverses coupes de porc',
      FALSE,
      FALSE,
      TRUE,
      FALSE,
      FALSE
   ),
   (
      'Diverses coupes de bœuf',
      FALSE,
      FALSE,
      TRUE,
      FALSE,
      FALSE
   ),
   (
      'Poisson blanc cru',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      TRUE
   ),
   ('Piments', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Mozzarella', TRUE, FALSE, TRUE, FALSE, FALSE),
   ('Basilic frais', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Safran', TRUE, TRUE, TRUE, TRUE, TRUE),
   (
      'Mélange de fruits de mer',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      TRUE
   ),
   ('Poulet grillé', FALSE, FALSE, TRUE, TRUE, TRUE),
   ('Porc grillé', FALSE, FALSE, TRUE, TRUE, TRUE),
   ('Agneau grillé', FALSE, FALSE, TRUE, TRUE, TRUE),
   ('Pain pita', TRUE, TRUE, FALSE, TRUE, FALSE),
   ('Sauce tzatziki', TRUE, FALSE, TRUE, TRUE, TRUE), -- Contient du yaourt
   (
      'Viande hachée (porc/bœuf)',
      FALSE,
      FALSE,
      TRUE,
      FALSE,
      FALSE
   ),
   ('Chou', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Huile de sésame', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Vinaigre', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Nouilles de riz', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Germes de soja', TRUE, TRUE, TRUE, TRUE, TRUE),
   (
      'Poulet cuit en sauce',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      TRUE
   ),
   ('Épices diverses', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Pain naan', TRUE, TRUE, FALSE, TRUE, FALSE),
   ('Légumes variés', TRUE, TRUE, TRUE, TRUE, TRUE),
   (
      'Gochujang (pâte de piment)',
      TRUE,
      TRUE,
      FALSE,
      TRUE,
      TRUE
   ), -- Contient souvent du blé
   ('Agneau mijoté', FALSE, FALSE, TRUE, TRUE, TRUE),
   ('Fruits secs', TRUE, TRUE, TRUE, TRUE, TRUE),
   ('Poulet mijoté', FALSE, FALSE, TRUE, TRUE, TRUE),
   ('Farine de teff', TRUE, TRUE, TRUE, TRUE, TRUE),
   (
      'Ragoût (avec viande/légumes)',
      FALSE,
      FALSE,
      FALSE,
      TRUE,
      TRUE
   ), -- Dépend des ingrédients
   (
      'Viande hachée (bœuf/agneau)',
      FALSE,
      FALSE,
      TRUE,
      FALSE,
      FALSE
   ),
   (
      'Sauce de viande',
      FALSE,
      FALSE,
      FALSE,
      FALSE,
      FALSE
   ),
   (
      'Pommes de terre cuites sous terre',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE
   ),
   (
      'Kumara (patate douce)',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE
   ),
   ('Fèves', TRUE, TRUE, TRUE, TRUE, TRUE),
   (
      'Tahini (pâte de sésame)',
      TRUE,
      TRUE,
      TRUE,
      TRUE,
      TRUE
   ),
   (
      'Agneau grillé sur brochette',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      TRUE
   ),
   (
      'Poulet grillé sur brochette',
      FALSE,
      FALSE,
      TRUE,
      TRUE,
      TRUE
   ),
   (
      'Bœuf grillé sur brochette',
      FALSE,
      FALSE,
      TRUE,
      FALSE,
      FALSE
   );

INSERT INTO
   Dish (
      dish_name,
      dish_type,
      preparation_date,
      expiration_date,
      cuisine_nationality,
      quantity,
      price,
      photo
   )
VALUES
   (
      'Ratatouille',
      'main_course',
      '2025-04-01',
      '2025-04-03',
      'Française',
      10,
      15.99,
      NULL
   ),
   (
      'Bœuf Bourguignon',
      'main_course',
      '2025-04-01',
      '2025-04-02',
      'Française',
      8,
      22.50,
      NULL
   ),
   (
      'Coq au Vin',
      'main_course',
      '2025-04-01',
      '2025-04-02',
      'Française',
      6,
      19.99,
      NULL
   ),
   (
      'Soupe à l’Oignon',
      'starter',
      '2025-04-01',
      '2025-04-02',
      'Française',
      12,
      9.50,
      NULL
   ),
   (
      'Poulet Basquaise',
      'main_course',
      '2025-04-01',
      '2025-04-02',
      'Française',
      8,
      17.99,
      NULL
   ),
   (
      'Salade Niçoise',
      'starter',
      '2025-04-01',
      '2025-04-02',
      'Française',
      15,
      12.99,
      NULL
   ),
   (
      'Quiche Lorraine',
      'main_course',
      '2025-04-01',
      '2025-04-03',
      'Française',
      10,
      14.99,
      NULL
   ),
   (
      'Tartiflette',
      'main_course',
      '2025-04-01',
      '2025-04-02',
      'Française',
      7,
      18.99,
      NULL
   ),
   (
      'Gratin Dauphinois',
      'main_course',
      '2025-04-01',
      '2025-04-03',
      'Française',
      9,
      11.99,
      NULL
   ),
   (
      'Bouillabaisse',
      'main_course',
      '2025-04-01',
      '2025-04-02',
      'Française',
      6,
      24.99,
      NULL
   ),
   (
      'Cassoulet',
      'main_course',
      '2025-04-01',
      '2025-04-03',
      'Française',
      7,
      20.99,
      NULL
   ),
   (
      'Tarte Tatin',
      'dessert',
      '2025-04-01',
      '2025-04-04',
      'Française',
      12,
      8.99,
      NULL
   ),
   (
      'Mousse au Chocolat',
      'dessert',
      '2025-04-01',
      '2025-04-05',
      'Française',
      15,
      6.99,
      NULL
   ),
   (
      'Crêpes',
      'dessert',
      '2025-04-01',
      '2025-04-03',
      'Française',
      20,
      7.99,
      NULL
   ),
   (
      'Soufflé au Fromage',
      'main_course',
      '2025-04-01',
      '2025-04-02',
      'Française',
      6,
      13.99,
      NULL
   );

INSERT INTO
   Dish (
      dish_name,
      dish_type,
      preparation_date,
      expiration_date,
      cuisine_nationality,
      quantity,
      price,
      photo
   )
VALUES
   (
      'Cheeseburger and Fries',
      'main_course',
      '2025-04-04',
      '2025-04-05',
      'American',
      15,
      9.99,
      NULL
   ),
   (
      'Tacos al Pastor',
      'main_course',
      '2025-04-04',
      '2025-04-05',
      'Mexican',
      20,
      12.50,
      NULL
   ),
   (
      'Poutine',
      'main_course',
      '2025-04-04',
      '2025-04-05',
      'Canadian',
      12,
      8.75,
      NULL
   ),
   (
      'Feijoada',
      'main_course',
      '2025-04-04',
      '2025-04-06',
      'Brazilian',
      10,
      16.25,
      NULL
   ),
   (
      'Ceviche',
      'starter',
      '2025-04-04',
      '2025-04-04',
      'Peruvian',
      18,
      11.00,
      NULL
   ),
   (
      'Pizza Margherita',
      'main_course',
      '2025-04-04',
      '2025-04-05',
      'Italian',
      25,
      14.00,
      NULL
   ),
   (
      'Paella',
      'main_course',
      '2025-04-04',
      '2025-04-05',
      'Spanish',
      8,
      25.50,
      NULL
   ),
   (
      'Souvlaki',
      'main_course',
      '2025-04-04',
      '2025-04-05',
      'Greek',
      16,
      10.50,
      NULL
   ),
   (
      'Dumplings (Jiaozi)',
      'starter',
      '2025-04-04',
      '2025-04-05',
      'Chinese',
      30,
      7.99,
      NULL
   ),
   (
      'Sushi (Nigiri)',
      'main_course',
      '2025-04-04',
      '2025-04-04',
      'Japanese',
      14,
      18.00,
      NULL
   ),
   (
      'Butter Chicken',
      'main_course',
      '2025-04-04',
      '2025-04-06',
      'Indian',
      11,
      15.75,
      NULL
   ),
   (
      'Pad Thai',
      'main_course',
      '2025-04-04',
      '2025-04-05',
      'Thai',
      22,
      13.25,
      NULL
   ),
   (
      'Bibimbap',
      'main_course',
      '2025-04-04',
      '2025-04-05',
      'Korean',
      17,
      14.50,
      NULL
   ),
   (
      'Tagine',
      'main_course',
      '2025-04-04',
      '2025-04-06',
      'Moroccan',
      9,
      17.00,
      NULL
   ),
   (
      'Jollof Rice',
      'main_course',
      '2025-04-04',
      '2025-04-06',
      'Nigerian',
      13,
      12.00,
      NULL
   ),
   (
      'Injera with Wat',
      'main_course',
      '2025-04-04',
      '2025-04-05',
      'Ethiopian',
      7,
      16.50,
      NULL
   ),
   (
      'Meat Pie',
      'main_course',
      '2025-04-04',
      '2025-04-05',
      'Australian',
      19,
      6.99,
      NULL
   ),
   (
      'Hangi',
      'main_course',
      '2025-04-04',
      '2025-04-06',
      'New Zealander',
      5,
      28.00,
      NULL
   ),
   (
      'Falafel',
      'starter',
      '2025-04-04',
      '2025-04-05',
      'Lebanese',
      28,
      9.25,
      NULL
   ),
   (
      'Kebab',
      'main_course',
      '2025-04-04',
      '2025-04-05',
      'Turkish',
      21,
      11.75,
      NULL
   );

INSERT INTO
   MenuProposal (account_id, proposal_date, dish_name)
VALUES
   (26, '2025-04-01', 'Ratatouille'),
   (28, '2025-04-02', 'Bœuf Bourguignon'),
   (29, '2025-04-03', 'Coq au Vin'),
   (30, '2025-04-04', 'Soupe à l’Oignon'),
   (31, '2025-04-05', 'Poulet Basquaise'),
   (32, '2025-04-06', 'Salade Niçoise'),
   (33, '2025-04-07', 'Quiche Lorraine'),
   (34, '2025-04-08', 'Tartiflette'),
   (35, '2025-04-09', 'Gratin Dauphinois'),
   (36, '2025-04-10', 'Bouillabaisse'),
   (37, '2025-04-11', 'Cassoulet'),
   (38, '2025-04-12', 'Tarte Tatin'),
   (39, '2025-04-13', 'Mousse au Chocolat'),
   (40, '2025-04-14', 'Crêpes'),
   (41, '2025-04-15', 'Soufflé au Fromage');

INSERT INTO
   MenuProposal (account_id, proposal_date, dish_name)
VALUES
   (26, '2025-04-16', 'Cheeseburger and Fries'),
   (28, '2025-04-18', 'Poutine'),
   (29, '2025-04-19', 'Feijoada'),
   (30, '2025-04-20', 'Ceviche'),
   (31, '2025-04-21', 'Pizza Margherita'),
   (32, '2025-04-22', 'Paella'),
   (33, '2025-04-23', 'Souvlaki'),
   (34, '2025-04-24', 'Dumplings (Jiaozi)'),
   (35, '2025-04-25', 'Sushi (Nigiri)'),
   (36, '2025-04-26', 'Butter Chicken'),
   (37, '2025-04-27', 'Pad Thai'),
   (38, '2025-04-28', 'Bibimbap'),
   (39, '2025-04-29', 'Tagine'),
   (40, '2025-04-30', 'Jollof Rice'),
   (41, '2025-05-01', 'Injera with Wat');

INSERT INTO
   Contains (dish_name, ingredient_name)
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
   ('Soupe à l’Oignon', 'Oignon'),
   ('Soupe à l’Oignon', 'Beurre'),
   ('Soupe à l’Oignon', 'Fromage'),
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
   ('Soufflé au Fromage', 'Œufs');

-- Cheeseburger and Fries
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Steak haché', 'Cheeseburger and Fries'),
   ('Fromage cheddar', 'Cheeseburger and Fries'),
   ('Pain à hamburger', 'Cheeseburger and Fries'),
   ('Tomate', 'Cheeseburger and Fries'),
   ('Oignon', 'Cheeseburger and Fries'),
   ('Frites', 'Cheeseburger and Fries');

-- Poutine
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Frites', 'Poutine'),
   ('Fromage en grains', 'Poutine'),
   ('Sauce brune', 'Poutine');

-- Feijoada
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Haricots noirs', 'Feijoada'),
   ('Diverses coupes de porc', 'Feijoada'),
   ('Oignon', 'Feijoada'),
   ('Ail', 'Feijoada');

-- Ceviche
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Poisson blanc cru', 'Ceviche'),
   ('Citron vert', 'Ceviche'),
   ('Oignon', 'Ceviche'),
   ('Coriandre', 'Ceviche'),
   ('Piments', 'Ceviche');

-- Pizza Margherita
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Farine de blé', 'Pizza Margherita'),
   ('Tomate', 'Pizza Margherita'),
   ('Mozzarella', 'Pizza Margherita'),
   ('Basilic frais', 'Pizza Margherita'),
   ('Huile d’olive', 'Pizza Margherita');

-- Paella
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Riz', 'Paella'),
   ('Mélange de fruits de mer', 'Paella'),
   ('Safran', 'Paella'),
   ('Poivron', 'Paella'),
   ('Tomate', 'Paella'),
   ('Oignon', 'Paella');

-- Souvlaki
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Poulet grillé sur brochette', 'Souvlaki'),
   ('Pain pita', 'Souvlaki'),
   ('Sauce tzatziki', 'Souvlaki'),
   ('Tomate', 'Souvlaki'),
   ('Oignon', 'Souvlaki');

-- Dumplings (Jiaozi)
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Farine de blé', 'Dumplings (Jiaozi)'),
   ('Chou', 'Dumplings (Jiaozi)'),
   ('Viande hachée (porc/bœuf)', 'Dumplings (Jiaozi)'),
   ('Gingembre', 'Dumplings (Jiaozi)'),
   ('Coriandre', 'Dumplings (Jiaozi)'),
   ('Ail', 'Dumplings (Jiaozi)'),
   ('Sauce soja', 'Dumplings (Jiaozi)'),
   ('Huile de sésame', 'Dumplings (Jiaozi)');

-- Sushi (Nigiri)
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Riz', 'Sushi (Nigiri)'),
   ('Saumon', 'Sushi (Nigiri)'),
   ('Vinaigre', 'Sushi (Nigiri)');

-- Butter Chicken
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Poulet cuit en sauce', 'Butter Chicken'),
   ('Tomate', 'Butter Chicken'),
   ('Crème', 'Butter Chicken'),
   ('Épices diverses', 'Butter Chicken'),
   ('Beurre', 'Butter Chicken');

-- Pad Thai
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Nouilles de riz', 'Pad Thai'),
   ('Crevettes', 'Pad Thai'),
   ('Germes de soja', 'Pad Thai'),
   ('Cacahuètes', 'Pad Thai'),
   ('Œufs', 'Pad Thai'),
   ('Sauce soja', 'Pad Thai'),
   ('Coriandre', 'Pad Thai');

-- Bibimbap
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Riz', 'Bibimbap'),
   ('Légumes variés', 'Bibimbap'),
   ('Œufs', 'Bibimbap'),
   ('Gochujang (pâte de piment)', 'Bibimbap'),
   ('Viande hachée (bœuf/agneau)', 'Bibimbap');

-- Tagine
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Agneau mijoté', 'Tagine'),
   ('Fruits secs', 'Tagine'),
   ('Épices diverses', 'Tagine'),
   ('Amandes', 'Tagine'),
   ('Carotte', 'Tagine');

-- Jollof Rice
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Riz', 'Jollof Rice'),
   ('Tomate', 'Jollof Rice'),
   ('Poivron', 'Jollof Rice'),
   ('Oignon', 'Jollof Rice'),
   ('Épices diverses', 'Jollof Rice');

-- Injera with Wat
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Farine de teff', 'Injera with Wat'),
   ('Ragoût (avec viande/légumes)', 'Injera with Wat'),
   ('Oignon', 'Injera with Wat'),
   ('Ail', 'Injera with Wat'),
   ('Épices diverses', 'Injera with Wat');

-- Meat Pie
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Farine de blé', 'Meat Pie'),
   ('Beurre', 'Meat Pie'),
   ('Viande hachée (bœuf/agneau)', 'Meat Pie'),
   ('Oignon', 'Meat Pie');

-- Hangi
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Pommes de terre cuites sous terre', 'Hangi'),
   ('Viande hachée (bœuf/agneau)', 'Hangi'),
   ('Légumes variés', 'Hangi'),
   ('Kumara (patate douce)', 'Hangi');

-- Falafel
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Pois chiches', 'Falafel'),
   ('Ail', 'Falafel'),
   ('Coriandre', 'Falafel'),
   ('Persil', 'Falafel'),
   ('Tahini (pâte de sésame)', 'Falafel');

-- Kebab
INSERT INTO
   Contains (ingredient_name, dish_name)
VALUES
   ('Agneau grillé sur brochette', 'Kebab'),
   ('Pain pita', 'Kebab'),
   ('Oignon', 'Kebab'),
   ('Tomate', 'Kebab'),
   ('Sauce de viande', 'Kebab');