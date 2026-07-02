-- ============================================================
-- Script de creación de base de datos - Sistema de Ventas
-- ============================================================

-- Crear base de datos (si no existe)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'VentasDB')
BEGIN
    CREATE DATABASE VentasDB;
END
GO

USE VentasDB;
GO

-- ============================================================
-- TABLAS
-- ============================================================

-- Tabla de categorías
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Categorias') AND type = 'U')
BEGIN
    CREATE TABLE Categorias (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Nombre NVARCHAR(150) NOT NULL,
        CONSTRAINT UQ_Categorias_Nombre UNIQUE (Nombre)
    );
END
GO

-- Tabla de clientes
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Clientes') AND type = 'U')
BEGIN
    CREATE TABLE Clientes (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Codigo NVARCHAR(50) NOT NULL,
        Nombre NVARCHAR(200) NOT NULL,
        CONSTRAINT UQ_Clientes_Codigo UNIQUE (Codigo)
    );
END
GO

-- Tabla de productos
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Productos') AND type = 'U')
BEGIN
    CREATE TABLE Productos (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Codigo NVARCHAR(50) NOT NULL,
        Nombre NVARCHAR(200) NOT NULL,
        CategoriaId INT NOT NULL,
        CONSTRAINT UQ_Productos_Codigo UNIQUE (Codigo),
        CONSTRAINT FK_Productos_Categorias FOREIGN KEY (CategoriaId)
            REFERENCES Categorias(Id)
    );
END
GO

-- Tabla de ventas
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Ventas') AND type = 'U')
BEGIN
    CREATE TABLE Ventas (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Fecha DATE NOT NULL,
        ClienteId INT NOT NULL,
        ProductoId INT NOT NULL,
        Cantidad DECIMAL(18,2) NOT NULL,
        PrecioUnitario DECIMAL(18,2) NOT NULL,
        Total AS (Cantidad * PrecioUnitario) PERSISTED,
        CONSTRAINT FK_Ventas_Clientes FOREIGN KEY (ClienteId)
            REFERENCES Clientes(Id),
        CONSTRAINT FK_Ventas_Productos FOREIGN KEY (ProductoId)
            REFERENCES Productos(Id)
    );
END
GO

-- Índice único para evitar duplicados en Ventas
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Ventas_Fecha_Cliente_Producto')
BEGIN
    CREATE UNIQUE INDEX UQ_Ventas_Fecha_Cliente_Producto
        ON Ventas(Fecha, ClienteId, ProductoId);
END
GO

-- ============================================================
-- VISTAS
-- ============================================================

-- Vista: Resumen de ventas mensual
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_ResumenVentasMensual')
    DROP VIEW vw_ResumenVentasMensual;
GO

CREATE VIEW vw_ResumenVentasMensual
AS
SELECT
    YEAR(Fecha) AS Anio,
    MONTH(Fecha) AS Mes,
    SUM(Cantidad * PrecioUnitario) AS TotalVendido
FROM Ventas
GROUP BY YEAR(Fecha), MONTH(Fecha);
GO

-- ============================================================
-- PROCEDIMIENTOS ALMACENADOS
-- ============================================================

-- SP: Total de ventas por producto
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_TotalVentasPorProducto')
    DROP PROCEDURE sp_TotalVentasPorProducto;
GO

CREATE PROCEDURE sp_TotalVentasPorProducto
AS
BEGIN
    SELECT
        p.Id AS ProductoId,
        p.Codigo,
        p.Nombre AS NombreProducto,
        SUM(v.Cantidad * v.PrecioUnitario) AS TotalVendido
    FROM Productos p
    INNER JOIN Ventas v ON v.ProductoId = p.Id
    GROUP BY p.Id, p.Codigo, p.Nombre
    ORDER BY TotalVendido DESC;
END;
GO

-- SP: Total de ventas por cliente
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_TotalVentasPorCliente')
    DROP PROCEDURE sp_TotalVentasPorCliente;
GO

CREATE PROCEDURE sp_TotalVentasPorCliente
AS
BEGIN
    SELECT
        c.Id AS ClienteId,
        c.Codigo,
        c.Nombre AS NombreCliente,
        SUM(v.Cantidad * v.PrecioUnitario) AS TotalVendido
    FROM Clientes c
    INNER JOIN Ventas v ON v.ClienteId = c.Id
    GROUP BY c.Id, c.Codigo, c.Nombre
    ORDER BY TotalVendido DESC;
END;
GO

-- ============================================================
-- CONSULTAS DE VALIDACIÓN
-- ============================================================

-- 1. Total de ventas por producto
-- SELECT p.Nombre AS Producto, SUM(v.Cantidad * v.PrecioUnitario) AS Total
-- FROM Productos p
-- INNER JOIN Ventas v ON v.ProductoId = p.Id
-- GROUP BY p.Nombre
-- ORDER BY Total DESC;

-- 2. Total de ventas por cliente
-- SELECT c.Nombre AS Cliente, SUM(v.Cantidad * v.PrecioUnitario) AS Total
-- FROM Clientes c
-- INNER JOIN Ventas v ON v.ClienteId = c.Id
-- GROUP BY c.Nombre
-- ORDER BY Total DESC;

-- 3. Total de ventas por mes
-- SELECT YEAR(Fecha) AS Anio, MONTH(Fecha) AS Mes, SUM(Cantidad * PrecioUnitario) AS Total
-- FROM Ventas
-- GROUP BY YEAR(Fecha), MONTH(Fecha)
-- ORDER BY Anio DESC, Mes DESC;

-- 4. Top 5 productos más vendidos
-- SELECT TOP 5 p.Nombre, SUM(v.Cantidad) AS UnidadesVendidas, SUM(v.Cantidad * v.PrecioUnitario) AS Total
-- FROM Productos p
-- INNER JOIN Ventas v ON v.ProductoId = p.Id
-- GROUP BY p.Nombre
-- ORDER BY UnidadesVendidas DESC;

-- 5. Top 5 clientes con más compras
-- SELECT TOP 5 c.Nombre, COUNT(v.Id) AS CantidadCompras, SUM(v.Cantidad * v.PrecioUnitario) AS TotalGastado
-- FROM Clientes c
-- INNER JOIN Ventas v ON v.ClienteId = c.Id
-- GROUP BY c.Nombre
-- ORDER BY TotalGastado DESC;
