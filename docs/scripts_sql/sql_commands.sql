INSERT INTO compte VALUES(000, 'password');
INSERT INTO compte VALUES(001, 'hellopeople');
INSERT INTO compte VALUES(002, 'motdepasse');

ALTER TABLE ingredient DROP regime;
ALTER TABLE ingredient DROP est_vegetarien;
ALTER TABLE ingredient ADD(est_vegetarien BOOLEAN);
ALTER TABLE ingredient ADD(est_vegan BOOLEAN);
ALTER TABLE ingredient ADD(est_sans_gluten BOOLEAN);
ALTER TABLE ingredient ADD(est_halal BOOLEAN);
ALTER TABLE ingredient ADD(est_casher BOOLEAN);

INSERT INTO ingredient VALUES ('jambon', false, false, false, false, false);
INSERT INTO ingredient VALUES ('riz', false, false, true, false, false);
INSERT INTO ingredient VALUES ('salade', true, true, true, false, false);

INSERT INTO adresse VALUES (1, 'bellevue', 'pont de sèvre');
INSERT INTO adresse VALUES (4, 'charles de gaulle', 'étoile');
INSERT INTO adresse VALUES (54, 'champs élysées', 'champs-élysées-clemanceau');

INSERT INTO met VALUES('salade de riz avec saucisse', 'plat', STR_TO_DATE('01-03-2025', '%m-%d-%Y'), STR_TO_DATE('03-03-2025', '%m-%d-%Y'), 'français', 5, 20, 'photo.txt');

INSERT INTO client VALUES (000, 4, 0, false);
INSERT INTO client VALUES (001, 3, 0, true);

INSERT INTO cuisinier VALUES(002, 4.5, true, false, 1, 'bellevue');

INSERT INTO transaction VALUES(0, 20, 001);

INSERT INTO entreprise VALUES(001, 'delicious_diner', 'Marc', 'lafix');

INSERT INTO particulier VALUES(000, 'eliott', 'roussille', 'eliottroussille@gmail.com', 0657923710, 4, 'charles de gaulle');

INSERT INTO commande VALUES(0000, curdate(), '2H', 'en attente', true, 1, 'bellevue', 4, 'charles de gaulle', 0, 002);

INSERT INTO avis VALUES(0, 'le repas', 5, 'excellent', curdate(), 0000);

INSERT INTO proposition VALUES(002, curdate(), 'salade de riz avec saucisse');

INSERT INTO contient VALUES('saucisse', 'salade de riz avec saucisse', 4);
INSERT INTO contient VALUES('riz', 'salade de riz avec saucisse', 500);
INSERT INTO contient VALUES('salade', 'salade de riz avec saucisse', 100);

SELECT nom FROM Particulier WHERE compte_id = 000;

UPDATE particulier SET nom = 'françois' WHERE compte_id = 000;

INSERT INTO compte VALUES(003, 'securepass');
INSERT INTO compte VALUES(004, 'anotherpass');

INSERT INTO ingredient VALUES ('tomate', true, true, true, false, false);
INSERT INTO ingredient VALUES ('poulet', false, false, false, true, false);

INSERT INTO adresse VALUES (5, 'rue de la paix', 'opéra');
INSERT INTO adresse VALUES (6, 'avenue des champs', 'george v');

INSERT INTO met VALUES('poulet rôti', 'plat', STR_TO_DATE('02-28-2025', '%m-%d-%Y'), STR_TO_DATE('03-05-2025', '%m-%d-%Y'), 'français', 10, 50, 'poulet_roti.jpg');

INSERT INTO client VALUES (002, 5, 100, false);
INSERT INTO client VALUES (003, 2, 50, true);

INSERT INTO cuisinier VALUES(003, 4.8, true, false, 5, 'rue de la paix');

INSERT INTO transaction VALUES(1, 30, 002);
INSERT INTO transaction VALUES(2, 45, 003);

INSERT INTO entreprise VALUES(002, 'gourmet_galley', 'Alice', 'Wonderland');

INSERT INTO particulier VALUES(001, 'john', 'doe', 'john.doe@example.com', 1234567890, 5, 'rue de la paix');

INSERT INTO commande VALUES(0001, curdate(), '1H', 'préparée', false, 5, 'rue de la paix', 6, 'avenue des champs', 1, 003);

INSERT INTO avis VALUES(1, 'le dessert', 4, 'très bon', curdate(), 0001);

INSERT INTO proposition VALUES(003, curdate(), 'poulet rôti');

INSERT INTO contient VALUES('poulet', 'poulet rôti', 1);
INSERT INTO contient VALUES('tomate', 'poulet rôti', 2);

SELECT nom FROM Particulier WHERE compte_id = 001;

UPDATE particulier SET nom = 'jean' WHERE compte_id = 001;