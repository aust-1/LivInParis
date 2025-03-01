DROP DATABASE IF EXISTS PSI;
CREATE DATABASE IF NOT EXISTS PSI;

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
   est_radie BOOLEAN,
   PRIMARY KEY(compte_id),
   FOREIGN KEY(compte_id) REFERENCES COMPTE(compte_id)
);

CREATE TABLE CUISINIER(
   compte_id VARCHAR(50),
   cuisinier_note DECIMAL(3,2),
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
   commande_mange_sur_place BOOLEAN,
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
   avis_type ENUM('client', 'cuisinier'),
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
