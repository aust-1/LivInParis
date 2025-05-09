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
      dish_type ENUM ('Starter', 'MainCourse', 'Dessert') NOT NULL,
      expiry_time INT NOT NULL,
      cuisine_nationality VARCHAR(50) NOT NULL,
      quantity INT NOT NULL CHECK (quantity >= 0),
      price DECIMAL(10, 2) NOT NULL CHECK (price >= 0),
      products_origin ENUM ('France', 'Europe', 'Other') NOT NULL,
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
         'InCart',
         'Pending',
         'Preparing',
         'Delivering',
         'Delivered',
         'Canceled'
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
      reviewer_type ENUM ('Customer', 'Chef') NOT NULL,
      review_rating DECIMAL(2, 1) CHECK (review_rating BETWEEN 1 AND 5),
      comment VARCHAR(500),
      review_date DATE NOT NULL,
      order_line_id INT NOT NULL,
      PRIMARY KEY (review_id),
      FOREIGN KEY (order_line_id) REFERENCES OrderLine (order_line_id) ON DELETE CASCADE
   );

INSERT INTO
   Address (address_number, street)
VALUES
   (63, 'Boulevard Haussmann'),
   (97, 'Avenue Mozart'),
   (89, 'Boulevard Haussmann'),
   (75, 'Boulevard Haussmann'),
   (71, 'Rue de Rivoli'),
   (937, 'Boulevard Haussmann'),
   (62, 'Boulevard Haussmann'),
   (62, 'Rue Mouffetard'),
   (568, 'Avenue Mozart'),
   (50, 'Rue de Rivoli'),
   (80, 'Boulevard Haussmann'),
   (5, 'Avenue Mozart'),
   (99, 'Rue de Rivoli'),
   (31, 'Boulevard Haussmann'),
   (97, 'Rue de Rivoli'),
   (9, 'Rue de Rivoli'),
   (22, 'Rue Mouffetard'),
   (1, 'Rue de Rivoli'),
   (29, 'Rue Oberkampf'),
   (84, 'Rue Mouffetard'),
   (222, 'Avenue Mozart'),
   (36, 'Rue Oberkampf'),
   (2, 'Avenue Mozart'),
   (195, 'Rue Mouffetard'),
   (979, 'Rue Oberkampf'),
   (52, 'Rue de Rivoli'),
   (10, 'Boulevard Haussmann'),
   (300, 'Boulevard Haussmann'),
   (94, 'Rue de Rivoli'),
   (6, 'Boulevard Haussmann'),
   (96, 'Rue Mouffetard'),
   (4, 'Rue Oberkampf'),
   (20, 'Rue de Rivoli'),
   (61, 'Avenue Mozart'),
   (92, 'Rue Oberkampf'),
   (86, 'Avenue Mozart'),
   (895, 'Avenue Mozart'),
   (51, 'Avenue Mozart'),
   (6, 'Rue de Rivoli'),
   (47, 'Rue Oberkampf'),
   (46, 'Rue Mouffetard'),
   (8, 'Rue Mouffetard'),
   (60, 'Rue Oberkampf'),
   (6, 'Rue Oberkampf'),
   (32, 'Avenue Mozart'),
   (43, 'Rue Oberkampf'),
   (66, 'Rue Oberkampf'),
   (3, 'Rue Mouffetard'),
   (8, 'Rue de Rivoli'),
   (14, 'Rue Campagne Première');

INSERT INTO
   Account (account_user_name, account_password)
VALUES
   ('user0', 's)I7fTPUQk'),
   ('user1', '4fSVDBi!!0'),
   ('user2', '$2zwR0Ax%4'),
   ('user3', 'TRjUb@tw!8'),
   ('user4', '#oK&ozmqo0'),
   ('user5', '+Ae$7ROe(w'),
   ('user6', 'oZ1gYQtxb$'),
   ('user7', ')8jvgBhrod'),
   ('user8', ')&khBXtG7u'),
   ('user9', '^^9ZrdnzWg'),
   ('user10', 'un+5jYd0_S'),
   ('user11', 'npzO4HsxM$'),
   ('user12', '7A3v!vNk)Z'),
   ('user13', '#kIXiDzwo3'),
   ('user14', ')haVw0K5Y0'),
   ('user15', '*+M$vHws87'),
   ('user16', 'eS1iY45t_C'),
   ('user17', '5*80_CqKEs'),
   ('user18', 'C)7HlyPkLE'),
   ('user19', '!uYT1koj0q'),
   ('user20', '3REfJ1xf)l'),
   ('user21', '&)r4Cry^u8'),
   ('user22', '+ah@IJvrY0'),
   ('user23', 'GV35Zs^@k+'),
   ('user24', 'BZfV4#3mk)'),
   ('user25', 's9rY^GNj^*'),
   ('user26', '$YlPRMFmm6'),
   ('user27', '7R14LdWDs#'),
   ('user28', 'zHFumA_M_0'),
   ('user29', 'Y_65AYcmAw'),
   ('user30', '3*1Ip3r9$)'),
   ('user31', 'p7C9(ZhfC+'),
   ('user32', 'E0QXke33!p'),
   ('user33', '&5TZlVT5#+'),
   ('user34', '14tUIiXg%f'),
   ('user35', 'NN@3ZOe0oB'),
   ('user36', 't*)0RRkvM5'),
   ('user37', 'XofrG5Yel+'),
   ('user38', 'UIdq(T1yO_'),
   ('user39', 'm0JLTVdx*6'),
   ('user40', '2I1ZxCMh*3'),
   ('user41', '$*+8r0Gilc'),
   ('user42', 'ju2aBjZ3)A'),
   ('user43', 'Uxk4CEss_f'),
   ('user44', 'Ce$*8FKiOV'),
   ('user45', '(71^HTkiK%'),
   ('user46', '#8QtYePQvs'),
   ('user47', 'e)d1wMWaH5'),
   ('user48', '$CoANlwfV8'),
   ('user49', '&urSYkBR20'),
   ('austin', '451520');

INSERT INTO
   Chef (account_id, chef_is_banned, address_id)
VALUES
   (1, false, 16),
   (2, false, 13),
   (3, false, 22),
   (4, false, 12),
   (5, false, 14),
   (6, false, 14),
   (7, false, 9),
   (8, false, 21),
   (9, false, 2),
   (10, false, 7),
   (11, true, 23),
   (12, true, 22),
   (13, true, 14),
   (14, false, 1),
   (15, false, 25),
   (16, false, 1),
   (17, true, 5),
   (18, false, 7),
   (19, false, 7),
   (20, true, 7),
   (21, false, 15),
   (22, false, 19),
   (23, false, 1),
   (24, false, 1),
   (25, true, 22),
   (51, false, 2);

INSERT INTO
   customer (account_id, customer_is_banned)
VALUES
   (11, false),
   (12, false),
   (13, true),
   (14, true),
   (15, false),
   (16, false),
   (17, false),
   (18, false),
   (19, false),
   (20, false),
   (21, false),
   (22, false),
   (23, true),
   (24, true),
   (25, false),
   (26, true),
   (27, false),
   (28, false),
   (29, true),
   (30, false),
   (31, true),
   (32, true),
   (33, false),
   (34, false),
   (35, false),
   (36, true),
   (37, false),
   (38, false),
   (39, false),
   (40, false),
   (41, false),
   (42, false),
   (43, false),
   (44, false),
   (45, true),
   (46, false),
   (47, false),
   (48, false),
   (49, false),
   (50, true),
   (51, false);

INSERT INTO
   individual (
      account_id,
      last_name,
      first_name,
      personal_email,
      phone_number,
      address_id
   )
VALUES
   (
      29,
      'Diallo',
      'Alix',
      'adamhenriette@guyot.fr',
      '397369864',
      25
   ),
   (
      27,
      'Ribeiro',
      'Gabrielle',
      'agrondin@tele2.fr',
      '+33 (0)8 02 60 29 59',
      18
   ),
   (
      11,
      'Imbert',
      'Jeanne',
      'martine00@iFrance.com',
      '+33 (0)4 41 87 84 68',
      16
   ),
   (
      40,
      'Barbier',
      'Anne',
      'michellecharrier@charrier.fr',
      '+33 3 40 14 81 01',
      14
   ),
   (
      45,
      'Baron',
      'Maryse',
      'peltiervirginie@yahoo.fr',
      '675258537',
      10
   ),
   (
      15,
      'Camus',
      'AmÃ©lie',
      'dtessier@jacques.com',
      '+33 (0)4 62 49 28 46',
      12
   ),
   (
      36,
      'Potier',
      'Claude',
      'kleinmarthe@gmail.com',
      '03 46 74 73 63',
      8
   ),
   (
      41,
      'Evrard',
      'Claire',
      'benoit58@free.fr',
      '307850319',
      12
   ),
   (
      43,
      'Cohen',
      'Ã‰ric',
      'perrierpaul@wanadoo.fr',
      '472792509',
      2
   ),
   (
      23,
      'Lopes',
      'Thibaut',
      'william89@thibault.org',
      '+33 1 21 44 72 88',
      2
   ),
   (
      16,
      'Maurice',
      'Susanne',
      'luc14@hubert.fr',
      '06 14 14 53 78',
      9
   ),
   (
      24,
      'Blot',
      'Alain',
      'kpetit@roy.net',
      '+33 (0)2 54 00 23 68',
      23
   ),
   (
      44,
      'Olivier',
      'Laure',
      'isabellecamus@benoit.fr',
      '+33 6 15 70 29 96',
      1
   ),
   (
      21,
      'Daniel',
      'CÃ©lina',
      'astridbesson@carlier.net',
      '06 57 50 72 51',
      25
   ),
   (
      47,
      'Moreau',
      'Marguerite',
      'michelle48@jacquet.com',
      '+33 (0)4 26 80 41 05',
      5
   ),
   (
      38,
      'Rey',
      'Alexandria',
      'henrisimon@lefevre.fr',
      '+33 (0)2 35 79 38 50',
      15
   ),
   (
      12,
      'Maillet',
      'Charlotte',
      'isaac76@guilbert.fr',
      '587770947',
      23
   ),
   (
      20,
      'Blanchard',
      'ZoÃ©',
      'cbarre@tiscali.fr',
      '805925841',
      25
   ),
   (
      39,
      'Moreau',
      'Marie',
      'alfred92@fabre.com',
      '+33 2 65 66 34 83',
      24
   ),
   (
      33,
      'Navarro',
      'Laurent',
      'gabrieljacob@marchand.com',
      '04 34 58 78 45',
      1
   ),
   (
      51,
      'Roussille',
      'Eliott',
      'eliottroussille@gmail.com',
      '+33663115034',
      2
   );

INSERT INTO
   Company (
      account_id,
      Company.company_name,
      contact_first_name,
      contact_last_name
   )
VALUES
   (23, 'Paris S.A.R.L.', 'Guy', 'Leleu'),
   (
      29,
      'Renault LÃ©vy S.A.S.',
      'Ã‰douard',
      'Ferreira'
   ),
   (17, 'Antoine SARL', 'Nath', 'Durand'),
   (22, 'Imbert Barre et Fils', 'Ã‰tienne', 'Pierre'),
   (20, 'Berthelot Dupuy S.A.S.', 'MichÃ¨le', 'Joly'),
   (32, 'Martineau Millet S.A.S.', 'Olivier', 'Morin'),
   (44, 'Hoarau SA', 'AnaÃ¯s', 'Rey'),
   (
      49,
      'Dias Charpentier S.A.',
      'Marcelle',
      'Rolland'
   ),
   (41, 'Rodriguez', 'Zacharie', 'Guilbert'),
   (
      35,
      'Michaud Valentin et Fils',
      'Laetitia',
      'Guillot'
   );

INSERT INTO
   dish (
      dish_name,
      dish_type,
      expiry_time,
      cuisine_nationality,
      quantity,
      price,
      products_origin,
      photo_path
   )
VALUES
   (
      'Plat_0',
      'MainCourse',
      51,
      'indienne',
      19,
      3.69,
      'Other',
      'plat_0.jpg'
   ),
   (
      'Plat_1',
      'Starter',
      96,
      'mexicaine',
      11,
      10.65,
      'Other',
      'plat_1.jpg'
   ),
   (
      'Plat_2',
      'Dessert',
      52,
      'italienne',
      5,
      8.01,
      'Other',
      'plat_2.jpg'
   ),
   (
      'Plat_3',
      'Dessert',
      54,
      'mexicaine',
      9,
      13.32,
      'Other',
      'plat_3.jpg'
   ),
   (
      'Plat_4',
      'Starter',
      37,
      'japonaise',
      11,
      14.98,
      'France',
      'plat_4.jpg'
   ),
   (
      'Plat_5',
      'Dessert',
      37,
      'indienne',
      13,
      13.95,
      'France',
      'plat_5.jpg'
   ),
   (
      'Plat_6',
      'Starter',
      47,
      'mexicaine',
      8,
      3.21,
      'France',
      'plat_6.jpg'
   ),
   (
      'Plat_7',
      'MainCourse',
      49,
      'indienne',
      7,
      10.33,
      'France',
      'plat_7.jpg'
   ),
   (
      'Plat_8',
      'Dessert',
      64,
      'franÃ§aise',
      5,
      10.62,
      'Europe',
      'plat_8.jpg'
   ),
   (
      'Plat_9',
      'Starter',
      91,
      'mexicaine',
      16,
      6.04,
      'Europe',
      'plat_9.jpg'
   ),
   (
      'Plat_10',
      'Starter',
      81,
      'franÃ§aise',
      20,
      5.09,
      'Europe',
      'plat_10.jpg'
   ),
   (
      'Plat_11',
      'MainCourse',
      92,
      'italienne',
      17,
      13.49,
      'Europe',
      'plat_11.jpg'
   ),
   (
      'Plat_12',
      'MainCourse',
      54,
      'indienne',
      16,
      6.78,
      'Europe',
      'plat_12.jpg'
   ),
   (
      'Plat_13',
      'Starter',
      81,
      'japonaise',
      6,
      13.75,
      'Other',
      'plat_13.jpg'
   ),
   (
      'Plat_14',
      'MainCourse',
      36,
      'mexicaine',
      8,
      6.30,
      'Other',
      'plat_14.jpg'
   ),
   (
      'Plat_15',
      'MainCourse',
      28,
      'japonaise',
      19,
      13.25,
      'Other',
      'plat_15.jpg'
   ),
   (
      'Plat_16',
      'MainCourse',
      93,
      'mexicaine',
      20,
      3.61,
      'France',
      'plat_16.jpg'
   ),
   (
      'Plat_17',
      'Dessert',
      95,
      'franÃ§aise',
      6,
      5.15,
      'Europe',
      'plat_17.jpg'
   ),
   (
      'Plat_18',
      'Dessert',
      61,
      'indienne',
      13,
      9.85,
      'Other',
      'plat_18.jpg'
   ),
   (
      'Plat_19',
      'MainCourse',
      58,
      'mexicaine',
      5,
      3.71,
      'Other',
      'plat_19.jpg'
   ),
   (
      'Plat_20',
      'MainCourse',
      92,
      'italienne',
      11,
      14.57,
      'Other',
      'plat_20.jpg'
   ),
   (
      'Plat_21',
      'Dessert',
      66,
      'franÃ§aise',
      11,
      6.82,
      'France',
      'plat_21.jpg'
   ),
   (
      'Plat_22',
      'Dessert',
      65,
      'indienne',
      16,
      13.97,
      'France',
      'plat_22.jpg'
   ),
   (
      'Plat_23',
      'Starter',
      69,
      'japonaise',
      19,
      6.89,
      'France',
      'plat_23.jpg'
   ),
   (
      'Plat_24',
      'Dessert',
      90,
      'japonaise',
      7,
      4.76,
      'Other',
      'plat_24.jpg'
   );

INSERT INTO
   Ingredient (
      ingredient_name,
      is_vegetarian,
      is_vegan,
      is_gluten_free,
      is_lactose_free,
      is_halal,
      is_kosher
   )
VALUES
   ('Boeuf', false, false, true, true, true, false),
   ('Poulet', false, false, true, true, true, false),
   ('Carottes', true, true, true, true, true, true),
   ('Oignons', true, true, true, true, true, true),
   ('Lait', true, false, true, false, true, true),
   ('Tomates', true, true, true, true, true, true),
   ('Coriandre', true, true, true, true, true, true),
   ('Riz', true, true, true, true, true, true),
   ('Crè\\me', true, false, true, false, true, true),
   ('Curry', true, true, true, true, true, true),
   ('Saumon', false, false, true, true, true, true),
   ('P\\àtes', true, true, false, true, true, true),
   ('Champignons', true, true, true, true, true, true),
   ('Poivrons', true, true, true, true, true, true),
   (
      'Mozzarella',
      true,
      false,
      true,
      false,
      true,
      true
   );

INSERT INTO
   contains (ingredient_id, dish_id)
VALUES
   (7, 17),
   (3, 13),
   (10, 6),
   (11, 14),
   (13, 8),
   (15, 5),
   (6, 11),
   (1, 24),
   (5, 3),
   (12, 18),
   (8, 11),
   (3, 24),
   (9, 19),
   (13, 1),
   (7, 12),
   (15, 25),
   (5, 5),
   (5, 23),
   (13, 3),
   (15, 18),
   (3, 1),
   (5, 16),
   (1, 12),
   (3, 12),
   (11, 4),
   (14, 21),
   (3, 21),
   (15, 13),
   (2, 22),
   (6, 22),
   (4, 4),
   (12, 17),
   (14, 14),
   (13, 9),
   (11, 18),
   (1, 25),
   (12, 1),
   (12, 19),
   (5, 13),
   (13, 2),
   (10, 12),
   (8, 24),
   (10, 21),
   (1, 18),
   (6, 8),
   (3, 9),
   (5, 6),
   (10, 14),
   (2, 1),
   (1, 2),
   (6, 19),
   (12, 14),
   (14, 11),
   (4, 19),
   (10, 7),
   (1, 4),
   (11, 6),
   (10, 16),
   (11, 15),
   (10, 25),
   (11, 24),
   (6, 3),
   (6, 12),
   (14, 4),
   (6, 21),
   (8, 3),
   (9, 11),
   (5, 22),
   (2, 23),
   (6, 5),
   (7, 22),
   (10, 2),
   (5, 15),
   (9, 13),
   (2, 7),
   (2, 16),
   (6, 25),
   (5, 8),
   (5, 17),
   (8, 16),
   (10, 13);

INSERT INTO
   menuproposal (account_id, proposal_date, dish_id)
VALUES
   (1, '2025-04-22', 2),
   (1, '2025-04-28', 16),
   (2, '2025-04-27', 24),
   (2, '2025-04-29', 6),
   (2, '2025-04-23', 4),
   (3, '2025-04-23', 1),
   (3, '2025-04-23', 8),
   (4, '2025-04-20', 18),
   (4, '2025-04-20', 2),
   (4, '2025-04-23', 3),
   (4, '2025-04-24', 1),
   (4, '2025-04-29', 5),
   (5, '2025-04-28', 16),
   (5, '2025-04-22', 20),
   (5, '2025-04-23', 20),
   (5, '2025-04-27', 4),
   (5, '2025-04-29', 7),
   (6, '2025-04-27', 14),
   (6, '2025-04-23', 25),
   (6, '2025-04-20', 18),
   (6, '2025-04-21', 2),
   (6, '2025-04-22', 11),
   (7, '2025-04-24', 12),
   (7, '2025-04-26', 11),
   (7, '2025-04-23', 25),
   (7, '2025-04-25', 14),
   (8, '2025-04-21', 17),
   (8, '2025-04-26', 7),
   (9, '2025-04-26', 3),
   (9, '2025-04-26', 22),
   (9, '2025-04-28', 20),
   (9, '2025-04-20', 4),
   (9, '2025-04-28', 13),
   (10, '2025-04-21', 3),
   (10, '2025-04-25', 8),
   (10, '2025-04-22', 8),
   (10, '2025-04-30', 6),
   (11, '2025-04-23', 4),
   (11, '2025-04-21', 1),
   (11, '2025-04-23', 20),
   (11, '2025-04-25', 5),
   (11, '2025-04-23', 13),
   (12, '2025-04-20', 11),
   (12, '2025-04-21', 24),
   (12, '2025-04-26', 9),
   (12, '2025-04-26', 19),
   (13, '2025-04-30', 2),
   (13, '2025-04-21', 15),
   (13, '2025-04-28', 3),
   (13, '2025-04-26', 17),
   (13, '2025-04-29', 8),
   (14, '2025-04-24', 16),
   (14, '2025-04-27', 12),
   (14, '2025-04-28', 1),
   (15, '2025-04-27', 20),
   (15, '2025-04-29', 22),
   (15, '2025-04-25', 2),
   (15, '2025-04-21', 13),
   (16, '2025-04-24', 7),
   (16, '2025-04-20', 24),
   (16, '2025-04-21', 14),
   (16, '2025-04-27', 23),
   (16, '2025-04-23', 18),
   (17, '2025-04-23', 16),
   (17, '2025-04-28', 13),
   (17, '2025-04-25', 7),
   (17, '2025-04-29', 7),
   (17, '2025-04-25', 13),
   (18, '2025-04-26', 6),
   (18, '2025-04-22', 25),
   (18, '2025-04-22', 23),
   (18, '2025-04-22', 14),
   (19, '2025-04-27', 19),
   (19, '2025-04-25', 10),
   (19, '2025-04-26', 13),
   (19, '2025-04-22', 24),
   (19, '2025-04-20', 12),
   (20, '2025-04-22', 1),
   (20, '2025-04-30', 10),
   (21, '2025-04-26', 7),
   (21, '2025-04-28', 10),
   (21, '2025-04-21', 10),
   (21, '2025-04-21', 8),
   (22, '2025-04-28', 5),
   (22, '2025-04-27', 19),
   (22, '2025-04-20', 4),
   (22, '2025-04-28', 20),
   (23, '2025-04-25', 3),
   (23, '2025-04-29', 25),
   (24, '2025-04-29', 25),
   (24, '2025-04-22', 24),
   (24, '2025-04-23', 2),
   (24, '2025-04-21', 6),
   (24, '2025-04-25', 14),
   (25, '2025-04-27', 16),
   (25, '2025-04-24', 12);

INSERT INTO
   OrderTransaction (transaction_datetime, account_id)
VALUES
   ('2025-04-21 03:55:00', 15),
   ('2025-04-22 20:01:00', 18),
   ('2025-04-25 11:25:00', 27),
   ('2025-04-27 13:14:00', 20),
   ('2025-04-23 17:27:00', 24),
   ('2025-04-21 16:34:00', 40),
   ('2025-04-23 03:47:00', 43),
   ('2025-04-29 11:49:00', 11),
   ('2025-04-24 20:54:00', 42),
   ('2025-04-29 01:37:00', 27),
   ('2025-04-25 21:44:00', 16),
   ('2025-04-27 13:49:00', 13),
   ('2025-04-29 17:14:00', 48),
   ('2025-04-28 11:32:00', 35),
   ('2025-04-23 20:44:00', 19),
   ('2025-04-22 07:18:00', 13),
   ('2025-04-27 10:08:00', 27),
   ('2025-04-20 17:24:00', 13),
   ('2025-04-20 14:44:00', 23),
   ('2025-04-24 19:29:00', 25),
   ('2025-04-23 17:50:00', 29),
   ('2025-04-28 22:08:00', 17),
   ('2025-04-24 05:03:00', 43),
   ('2025-04-21 12:12:00', 49),
   ('2025-04-24 20:37:00', 12),
   ('2025-04-21 12:18:00', 29),
   ('2025-04-24 16:28:00', 44),
   ('2025-04-25 08:43:00', 19),
   ('2025-04-20 14:01:00', 44),
   ('2025-04-27 03:32:00', 44);

INSERT INTO
   orderline (
      order_line_datetime,
      order_line_status,
      address_id,
      transaction_id,
      account_id
   )
VALUES
   ('2025-04-21 04:52:00', 'Prepared', 4, 1, 17),
   ('2025-04-22 20:24:00', 'Delivering', 22, 2, 25),
   ('2025-04-25 12:57:00', 'Delivered', 13, 3, 5),
   ('2025-04-25 12:09:00', 'Delivering', 12, 3, 15),
   ('2025-04-25 14:11:00', 'Delivered', 2, 3, 9),
   ('2025-04-27 14:09:00', 'Delivering', 6, 4, 24),
   ('2025-04-23 18:54:00', 'Canceled', 13, 5, 24),
   ('2025-04-21 18:29:00', 'Delivered', 12, 6, 23),
   ('2025-04-21 17:22:00', 'Canceled', 23, 6, 24),
   ('2025-04-21 19:24:00', 'Canceled', 12, 6, 8),
   ('2025-04-23 04:29:00', 'Delivered', 17, 7, 17),
   ('2025-04-23 06:09:00', 'Delivering', 3, 7, 10),
   ('2025-04-29 13:00:00', 'Prepared', 15, 8, 17),
   ('2025-04-29 13:53:00', 'Canceled', 7, 8, 18),
   ('2025-04-29 13:23:00', 'Delivering', 10, 8, 6),
   ('2025-04-24 22:36:00', 'Prepared', 19, 9, 15),
   ('2025-04-24 23:26:00', 'Pending', 6, 9, 11),
   ('2025-04-24 21:43:00', 'Pending', 10, 9, 24),
   ('2025-04-29 02:17:00', 'Delivered', 18, 10, 3),
   ('2025-04-25 22:50:00', 'Prepared', 11, 11, 4),
   ('2025-04-25 23:58:00', 'Delivered', 12, 11, 11),
   ('2025-04-27 15:11:00', 'Prepared', 16, 12, 19),
   ('2025-04-27 14:57:00', 'Delivering', 16, 12, 22),
   ('2025-04-29 18:03:00', 'Delivered', 4, 13, 8),
   ('2025-04-29 20:12:00', 'Pending', 12, 13, 19),
   ('2025-04-29 17:40:00', 'Delivered', 7, 13, 9),
   ('2025-04-28 13:52:00', 'Canceled', 9, 14, 11),
   ('2025-04-23 21:31:00', 'Canceled', 12, 15, 11),
   ('2025-04-23 23:28:00', 'Prepared', 4, 15, 11),
   ('2025-04-23 21:08:00', 'Delivering', 16, 15, 1),
   ('2025-04-22 10:05:00', 'Prepared', 13, 16, 5),
   ('2025-04-22 07:33:00', 'Pending', 18, 16, 21),
   ('2025-04-22 10:18:00', 'Prepared', 19, 16, 8),
   ('2025-04-27 11:33:00', 'Pending', 11, 17, 22),
   ('2025-04-27 12:17:00', 'Prepared', 16, 17, 1),
   ('2025-04-20 19:51:00', 'Pending', 13, 18, 13),
   ('2025-04-20 15:40:00', 'Pending', 21, 19, 12),
   ('2025-04-20 15:31:00', 'Pending', 22, 19, 5),
   ('2025-04-20 15:26:00', 'Delivering', 17, 19, 17),
   ('2025-04-24 21:32:00', 'Canceled', 24, 20, 14),
   ('2025-04-24 20:20:00', 'Canceled', 11, 20, 23),
   ('2025-04-24 20:45:00', 'Pending', 20, 20, 23),
   ('2025-04-23 18:57:00', 'Pending', 3, 21, 12),
   ('2025-04-23 20:17:00', 'Delivering', 16, 21, 22),
   ('2025-04-28 22:54:00', 'Delivered', 7, 22, 16),
   ('2025-04-24 06:21:00', 'Delivering', 10, 23, 6),
   ('2025-04-24 07:36:00', 'Delivering', 24, 23, 4),
   ('2025-04-21 15:05:00', 'Delivering', 2, 24, 22),
   ('2025-04-24 23:21:00', 'Delivering', 4, 25, 7),
   ('2025-04-24 23:31:00', 'Delivered', 9, 25, 23),
   ('2025-04-24 23:12:00', 'Canceled', 4, 25, 18),
   ('2025-04-21 13:40:00', 'Canceled', 10, 26, 23),
   ('2025-04-24 17:40:00', 'Canceled', 10, 27, 6),
   ('2025-04-24 18:50:00', 'Canceled', 22, 27, 2),
   ('2025-04-24 18:43:00', 'Delivered', 24, 27, 19),
   ('2025-04-25 10:05:00', 'Delivering', 25, 28, 1),
   ('2025-04-25 11:08:00', 'Delivering', 22, 28, 13),
   ('2025-04-25 09:00:00', 'Prepared', 16, 28, 5),
   ('2025-04-20 14:22:00', 'Prepared', 14, 29, 22),
   ('2025-04-27 04:42:00', 'Delivering', 2, 30, 5),
   ('2025-04-27 04:16:00', 'Prepared', 18, 30, 16),
   ('2025-04-27 05:46:00', 'Prepared', 5, 30, 11);

INSERT INTO
   review (
      reviewer_type,
      review_rating,
      comment,
      review_date,
      order_line_id
   )
VALUES
   (
      'chef',
      2.3,
      'Eau certes beaucoup remettre rÃ©ponse.',
      '2025-04-21',
      1
   ),
   (
      'customer',
      4.3,
      'Bien reste crÃ©er avoir prÃ©parer.',
      '2025-04-22',
      2
   ),
   (
      'customer',
      1.9,
      'Propre noire chambre joie souffrir pourquoi depuis intelligence puisque.',
      '2025-04-25',
      3
   ),
   (
      'chef',
      1.8,
      'Robe transformer article palais appeler le.',
      '2025-04-25',
      4
   ),
   (
      'customer',
      3.6,
      'PitiÃ© article Ã©trange comprendre hauteur fortune marquer.',
      '2025-04-25',
      5
   ),
   (
      'customer',
      3.9,
      'Rang Ã©clater connaÃ®tre couche perte.',
      '2025-04-27',
      6
   ),
   (
      'chef',
      4.3,
      'Salut banc regretter aussi anglais satisfaire.',
      '2025-04-23',
      7
   ),
   (
      'customer',
      3.9,
      'Haine genre choix fusil froid colline.',
      '2025-04-21',
      8
   ),
   (
      'customer',
      1.6,
      'Avec maintenir accepter public tellement mari douter troubler seul.',
      '2025-04-21',
      9
   ),
   (
      'customer',
      4,
      'Taire Ã¢gÃ© fil puissance quelque.',
      '2025-04-21',
      10
   ),
   (
      'customer',
      4.9,
      'Avenir miser couper officier promener.',
      '2025-04-23',
      11
   ),
   (
      'customer',
      5,
      'Raconter remonter engager dÃ©sespoir dos ombre tÃ¢che Ã©chapper tendre arracher.',
      '2025-04-23',
      12
   ),
   (
      'chef',
      3.1,
      'Respect chair religion prÃ©cipiter phrase odeur.',
      '2025-04-29',
      13
   ),
   (
      'chef',
      2.3,
      'Profond autrefois dent trÃ©sor Ã©poque table colline rejeter homme.',
      '2025-04-29',
      14
   ),
   (
      'chef',
      1.5,
      'SÃ»r image proposer raison prononcer.',
      '2025-04-29',
      15
   ),
   (
      'chef',
      2.4,
      'Nourrir dÃ©couvrir creuser Ã¢gÃ© haine dÃ©couvrir occuper chute.',
      '2025-04-24',
      16
   ),
   (
      'customer',
      4.1,
      'DÃ©couvrir fond mal chaÃ®ne.',
      '2025-04-24',
      17
   ),
   (
      'customer',
      4.2,
      'Cruel celui importance Ã©troit colon.',
      '2025-04-24',
      18
   ),
   (
      'chef',
      4.4,
      'Essayer blanc art clair longtemps rÃ©veiller rÃ©flexion briser remercier prÃªter.',
      '2025-04-29',
      19
   ),
   (
      'chef',
      4.7,
      'PrÃ©fÃ©rer comment rÃ¨gle roche est million escalier rompre particulier ton.',
      '2025-04-25',
      20
   ),
   (
      'customer',
      4.8,
      'TempÃªte rose spectacle plein profond nature certain.',
      '2025-04-25',
      21
   ),
   (
      'chef',
      1.3,
      'Du gros anglais inquiÃ©ter finir ajouter dÃ©cider.',
      '2025-04-27',
      22
   ),
   (
      'customer',
      2.2,
      'Voir apporter recueillir partie permettre quitter supposer offrir sujet matiÃ¨re.',
      '2025-04-27',
      23
   ),
   (
      'chef',
      4,
      'Ramener troisiÃ¨me parfaitement larme glisser voile Ã©lÃ©ment croiser prÃ©voir accepter.',
      '2025-04-29',
      24
   ),
   (
      'customer',
      1.1,
      'Jamais distance d\'autres sÃ©rieux.',
      '2025-04-29',
      25
   ),
   (
      'chef',
      2,
      'Retirer Ã©tendre observer charge trouver.',
      '2025-04-29',
      26
   ),
   (
      'chef',
      2.4,
      'Grandir valoir conseil allumer argent leur poÃ¨te fou changer obÃ©ir.',
      '2025-04-28',
      27
   ),
   (
      'chef',
      1.7,
      'Ã‰tudier tellement manger attirer prÃ©senter semaine parvenir chaleur.',
      '2025-04-23',
      28
   ),
   (
      'chef',
      4.6,
      'Revoir prÃªt pourquoi parmi habiller content dÃ©part contraire vÃªtir ministre.',
      '2025-04-23',
      29
   ),
   (
      'chef',
      4.9,
      'Social tard odeur contenir toit hiver Ã©touffer rapporter toi troubler dehors.',
      '2025-04-23',
      30
   ),
   (
      'chef',
      4.4,
      'Longtemps accrocher droit sens agiter sens calme aspect.',
      '2025-04-22',
      31
   ),
   (
      'chef',
      4.5,
      'Dent droite charge voie village apercevoir demi.',
      '2025-04-22',
      32
   ),
   (
      'customer',
      3.9,
      'LumiÃ¨re plaisir couper camarade intÃ©resser unique heureux.',
      '2025-04-22',
      33
   ),
   (
      'customer',
      1.6,
      'Aimer Ã©tendue retomber hÃ´tel possible enfin.',
      '2025-04-27',
      34
   ),
   (
      'customer',
      1.8,
      'Trente voici fou prÃ©tendre valeur ligne falloir peine.',
      '2025-04-27',
      35
   ),
   (
      'customer',
      1.4,
      'TraÃ®ner as raison arme facile.',
      '2025-04-20',
      36
   ),
   (
      'customer',
      2.3,
      'Tranquille jouer direction arriver douleur large.',
      '2025-04-20',
      37
   ),
   (
      'customer',
      3.1,
      'Arriver guÃ¨re ombre fort habiter exister faire geste.',
      '2025-04-20',
      38
   ),
   (
      'chef',
      3.2,
      'Respect commander peine partout gens jambe prononcer transformer oÃ¹ muet.',
      '2025-04-20',
      39
   ),
   (
      'customer',
      2.8,
      'Voici siÃ¨cle maison pain notre Ã©viter poser prendre accompagner.',
      '2025-04-24',
      40
   );