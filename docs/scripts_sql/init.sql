DROP DATABASE IF EXISTS PSI;
CREATE DATABASE IF NOT EXISTS PSI;

CREATE TABLE COMPTE(
   compte_id VARCHAR(50),
   mot_de_passe VARCHAR(50),
   PRIMARY KEY(compte_id)
);

CREATE TABLE INGREDIENT(
   ingredient_nom VARCHAR(50),
   est_vegetarien BOOLEAN,
   est_vegan BOOLEAN,
   est_sans_gluten BOOLEAN,
   est_halal BOOLEAN,
   est_casher BOOLEAN,
   PRIMARY KEY(ingredient_nom)
);

CREATE TABLE ADRESSE(
   numero INT,
   rue VARCHAR(50),
   metro_le_plus_proche VARCHAR(50),
   PRIMARY KEY(numero, rue)
);

CREATE TABLE MET(
   met_nom VARCHAR(50),
   met_type ENUM('entree', 'plat', 'desert'),
   date_fabrication DATE,
   date_peremption DATE,
   nationalité VARCHAR(50),
   quantité INT,
   prix VARCHAR(50),
   photo TEXT,
   PRIMARY KEY(met_nom)
);

CREATE TABLE CLIENT(
   compte_id VARCHAR(50),
   client_note DECIMAL(2,1),
   montant_achats_cumules DECIMAL(15,2),
   est_radie BOOLEAN,
   PRIMARY KEY(compte_id),
   FOREIGN KEY(compte_id) REFERENCES COMPTE(compte_id)
);

CREATE TABLE CUISINIER(
   compte_id VARCHAR(50),
   cuisinier_note DECIMAL(2,1),
   cuisinier_mange_sur_place BOOLEAN,
   est_radie BOOLEAN,
   numero INT NOT NULL,
   rue VARCHAR(50) NOT NULL,
   PRIMARY KEY(compte_id),
   FOREIGN KEY(compte_id) REFERENCES COMPTE(compte_id),
   FOREIGN KEY(numero, rue) REFERENCES ADRESSE(numero, rue)
);

CREATE TABLE TRANSACTION(
   transaction_id INT,
   transaction_date_heure DATETIME,
   compte_id VARCHAR(50) NOT NULL,
   PRIMARY KEY(transaction_id),
   FOREIGN KEY(compte_id) REFERENCES CLIENT(compte_id)
);

CREATE TABLE ENTREPRISE(
   compte_id VARCHAR(50),
   entreprise_nom VARCHAR(50),
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
   commande_date_heure DATETIME,
   duree VARCHAR(50),
   statut ENUM('en attente', 'preparee', 'en livraison', 'livree'),
   commande_mange_sur_place BOOLEAN,
   numero INT NOT NULL,
   rue VARCHAR(50) NOT NULL,
   transaction_id INT NOT NULL,
   compte_id VARCHAR(50) NOT NULL,
   PRIMARY KEY(commande_id),
   FOREIGN KEY(numero, rue) REFERENCES ADRESSE(numero, rue),
   FOREIGN KEY(transaction_id) REFERENCES TRANSACTION(transaction_id),
   FOREIGN KEY(compte_id) REFERENCES CUISINIER(compte_id)
);

CREATE TABLE AVIS(
   avis_id INT,
   avis_type ENUM('client', 'cuisinier'),
   note DECIMAL(2,1),
   commentaire VARCHAR(500),
   avis_date DATE,
   commande_id INT NOT NULL,
   PRIMARY KEY(avis_id),
   FOREIGN KEY(commande_id) REFERENCES COMMANDE(commande_id)
);

CREATE TABLE PROPOSITION(
   compte_id VARCHAR(50),
   jour DATE,
   met_nom VARCHAR(50) NOT NULL,
   PRIMARY KEY(compte_id, jour),
   FOREIGN KEY(compte_id) REFERENCES CUISINIER(compte_id),
   FOREIGN KEY(met_nom) REFERENCES MET(met_nom)
);

CREATE TABLE CONTIENT(
   ingredient_nom VARCHAR(50),
   met_nom VARCHAR(50),
   PRIMARY KEY(ingredient_nom, met_nom),
   FOREIGN KEY(ingredient_nom) REFERENCES INGREDIENT(ingredient_nom),
   FOREIGN KEY(met_nom) REFERENCES MET(met_nom)
);
