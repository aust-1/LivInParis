CREATE database if not exists PSI;


CREATE TABLE COMPTE(
   compte_id VARCHAR(50),
   mot_de_passe VARCHAR(50),
   PRIMARY KEY(compte_id)
);

CREATE TABLE INGREDIENT(
   nom VARCHAR(50),
   regimes VARCHAR(50),
   PRIMARY KEY(nom)
);

CREATE TABLE ADRESSE(
   numero INT,
   rue VARCHAR(50),
   metro_le_plus_proche VARCHAR(50),
   PRIMARY KEY(numero, rue)
);

CREATE TABLE MET(
   nom_met VARCHAR(50),
   type VARCHAR(50),
   date_fabrication DATE,
   date_peremption DATE,
   nationalité VARCHAR(50),
   quantité INT,
   prix VARCHAR(50),
   photo TEXT,
   PRIMARY KEY(nom_met)
);

CREATE TABLE CLIENT(
   compte_id VARCHAR(50),
   client_note DECIMAL(3,2),
   montant_achats_cumules DECIMAL(15,2),
   est_radie boolean,
   PRIMARY KEY(compte_id),
   FOREIGN KEY(compte_id) REFERENCES COMPTE(compte_id)
);

CREATE TABLE CUISINIER(
   compte_id VARCHAR(50),
   cuisinier_note DECIMAL(3,2),
   cuisinier_mange_sur_place boolean,
   est_radie boolean,
   numero INT NOT NULL,
   rue VARCHAR(50) NOT NULL,
   PRIMARY KEY(compte_id),
   FOREIGN KEY(compte_id) REFERENCES COMPTE(compte_id),
   FOREIGN KEY(numero, rue) REFERENCES ADRESSE(numero, rue)
);

CREATE TABLE TRANSACTION(
   transaction_id INT,
   prix_tot DECIMAL(6,2),
   compte_id VARCHAR(50) NOT NULL,
   PRIMARY KEY(transaction_id),
   FOREIGN KEY(compte_id) REFERENCES CLIENT(compte_id)
);

CREATE TABLE ENTREPRISE(
   compte_id VARCHAR(50),
   nom VARCHAR(50),
   prenom_referent VARCHAR(50),
   nom_referent VARCHAR(50),
   PRIMARY KEY(compte_id),
   FOREIGN KEY(compte_id) REFERENCES CLIENT(compte_id)
);

CREATE TABLE PARTICULIER(
   compte_id VARCHAR(50),
   nom VARCHAR(50),
   prenom VARCHAR(50),
   email VARCHAR(100),
   telephone INT,
   numero INT NOT NULL,
   rue VARCHAR(50) NOT NULL,
   PRIMARY KEY(compte_id),
   FOREIGN KEY(compte_id) REFERENCES CLIENT(compte_id),
   FOREIGN KEY(numero, rue) REFERENCES ADRESSE(numero, rue)
);

CREATE TABLE COMMANDE(
   commande_id INT,
   date_heure DATETIME,
   duree VARCHAR(50),
   statut ENUM('en attente', 'préparée', 'en livraison', 'livrée'),
   commande_mange_sur_place boolean,
   numero INT NOT NULL,
   rue VARCHAR(50) NOT NULL,
   numero_1 INT NOT NULL,
   rue_1 VARCHAR(50) NOT NULL,
   transaction_id INT NOT NULL,
   compte_id VARCHAR(50) NOT NULL,
   PRIMARY KEY(commande_id),
   FOREIGN KEY(numero, rue) REFERENCES ADRESSE(numero, rue),
   FOREIGN KEY(numero_1, rue_1) REFERENCES ADRESSE(numero, rue),
   FOREIGN KEY(transaction_id) REFERENCES TRANSACTION(transaction_id),
   FOREIGN KEY(compte_id) REFERENCES CUISINIER(compte_id)
);

CREATE TABLE AVIS(
   avis_id INT,
   avis_type VARCHAR(50),
   note DECIMAL(3,2),
   commentaire VARCHAR(500),
   avis_date DATE,
   commande_id INT NOT NULL,
   PRIMARY KEY(avis_id),
   FOREIGN KEY(commande_id) REFERENCES COMMANDE(commande_id)
);

CREATE TABLE PROPOSITION(
   compte_id VARCHAR(50),
   jour DATE,
   nom_met VARCHAR(50) NOT NULL,
   PRIMARY KEY(compte_id, jour),
   FOREIGN KEY(compte_id) REFERENCES CUISINIER(compte_id),
   FOREIGN KEY(nom_met) REFERENCES MET(nom_met)
);

CREATE TABLE CONTIENT(
   nom VARCHAR(50),
   nom_met VARCHAR(50),
   quantité DOUBLE,
   PRIMARY KEY(nom, nom_met),
   FOREIGN KEY(nom) REFERENCES INGREDIENT(nom),
   FOREIGN KEY(nom_met) REFERENCES MET(nom_met)
);

Insert into compte values(000, 'password');
Insert into compte values(001, 'hellopeople');
Insert into compte values(002, 'motdepasse');

Alter table ingredient drop regime;
Alter table ingredient drop est_vegetarien;
Alter table ingredient add(est_vegetarien boolean);
Alter table ingredient add(est_vegan boolean);
Alter table ingredient add(est_sans_gluten boolean);
Alter table ingredient add(est_halal boolean);
Alter table ingredient add(est_casher boolean);


Insert into ingredient values ('jambon', false, false, false, false, false);
Insert into ingredient values ('riz', false, false, true, false, false);
Insert into ingredient values ('salade', true, true, true, false, false);

Insert into adresse values (1, 'bellevue', 'pont de sèvre');
Insert into adresse values (4, 'charles de gaulle', 'étoile');
Insert into adresse values (54, 'champs élysées', 'champs-élysées-clemanceau');

insert into met values('salade de riz avec saucisse', 'plat', STR_TO_DATE('01-03-2025', '%m-%d-%Y'), STR_TO_DATE('03-03-2025', '%m-%d-%Y'), 'français', 5, 20, 'photo.txt');

insert into client values (000, 4, 0, false);
insert into client values (001, 3, 0, false);

insert into cuisinier values(002, 4.5, true, false, 1, 'bellevue');

insert into transaction values(0, 20, 001);

insert into entreprise values(001, 'delicious_diner', 'Marc', 'lafix');

insert into particulier values(000, 'eliott', 'roussille', 'eliottroussille@orange.fr', 0657923710, 4, 'charles de gaulle');

insert into commande values(0000, curdate(), '2H', 'en attente', true, 1, 'bellevue', 4, 'charles de gaulle', 0, 002);

insert into avis values(0, 'le repas', 5, 'excellent', curdate(), 0000);

insert into proposition values(002, curdate(), 'salade de riz avec saucisse');

insert into contient values('saucisse', 'salade de riz avec saucisse', 4);
insert into contient values('riz', 'salade de riz avec saucisse', 500);
insert into contient values('salade', 'salade de riz avec saucisse', 100);

Select nom from Particulier Where compte_id = 000;

Update particulier set nom = 'françois' where compte_id = 000;