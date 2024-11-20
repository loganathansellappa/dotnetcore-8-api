IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Restaurants')
    BEGIN
        CREATE DATABASE Restaurants;
    END
GO