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