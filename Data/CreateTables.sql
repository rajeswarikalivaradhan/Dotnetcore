-- ============================================
-- MySQL Workbench: Create tables from EF models
-- Run this script in MySQL Workbench (File > Open SQL Script, then Execute)
-- ============================================

-- Create database (optional - skip if you already have one)
-- CREATE DATABASE IF NOT EXISTS YourDatabaseName;
-- USE YourDatabaseName;

-- --------------------------------------------
-- Table: Users (from WebApi.Models.User)
-- --------------------------------------------
CREATE TABLE IF NOT EXISTS Users (
    Id              INT             NOT NULL AUTO_INCREMENT,
    Name            VARCHAR(100)    NOT NULL,
    Email           VARCHAR(255)    NOT NULL,
    PasswordHash    VARCHAR(255)    NOT NULL,
    IsActive        TINYINT(1)      NOT NULL DEFAULT 1,
    CreatedAt       DATETIME(6)     NOT NULL,
    PasswordResetToken       VARCHAR(500)    NULL,
    PasswordResetTokenExpiry DATETIME(6)     NULL,
    PRIMARY KEY (Id),
    UNIQUE INDEX IX_Users_Email (Email)
);

-- --------------------------------------------
-- Table: Categories (from WebApi.Models.Category)
-- --------------------------------------------
CREATE TABLE IF NOT EXISTS Categories (
    Id       INT           NOT NULL AUTO_INCREMENT,
    Name     VARCHAR(200)  NOT NULL,
    IsActive TINYINT(1)    NOT NULL DEFAULT 1,
    PRIMARY KEY (Id)
);

-- --------------------------------------------
-- Table: Products (from WebApi.Models.Product)
-- --------------------------------------------
CREATE TABLE IF NOT EXISTS Products (
    Id          INT             NOT NULL AUTO_INCREMENT,
    Name        VARCHAR(200)    NOT NULL,
    Price       DECIMAL(18, 2)  NOT NULL,
    Description VARCHAR(1000)   NULL,
    PRIMARY KEY (Id)
);

-- --------------------------------------------
-- Table: Customers (from WebApi.Models.Customer)
-- --------------------------------------------
CREATE TABLE IF NOT EXISTS Customers (
    Id      INT           NOT NULL AUTO_INCREMENT,
    Name    VARCHAR(200)  NOT NULL,
    Email   VARCHAR(255)  NOT NULL,
    Mobile  VARCHAR(20)   NULL,
    PRIMARY KEY (Id)
);
