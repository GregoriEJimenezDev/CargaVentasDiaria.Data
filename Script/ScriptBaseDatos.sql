USE [master]
GO
/****** Object:  Database [SistemaVentasETL]    Script Date: 7/4/2026 2:30:14 PM ******/
CREATE DATABASE [SistemaVentasETL]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SistemaVentasETL', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\DATA\SistemaVentasETL.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SistemaVentasETL_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\DATA\SistemaVentasETL_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [SistemaVentasETL] SET COMPATIBILITY_LEVEL = 170
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SistemaVentasETL].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SistemaVentasETL] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET ARITHABORT OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [SistemaVentasETL] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SistemaVentasETL] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SistemaVentasETL] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SistemaVentasETL] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SistemaVentasETL] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [SistemaVentasETL] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SistemaVentasETL] SET  MULTI_USER 
GO
ALTER DATABASE [SistemaVentasETL] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SistemaVentasETL] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SistemaVentasETL] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SistemaVentasETL] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SistemaVentasETL] SET OPTIMIZED_LOCKING = OFF 
GO
ALTER DATABASE [SistemaVentasETL] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [SistemaVentasETL] SET QUERY_STORE = ON
GO
ALTER DATABASE [SistemaVentasETL] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [SistemaVentasETL]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order_Details]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order_Details](
	[DetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](10, 2) NOT NULL,
	[TotalPrice]  AS ([Quantity]*[UnitPrice]) PERSISTED,
 CONSTRAINT [PK_Order_Details] PRIMARY KEY CLUSTERED 
(
	[DetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_ResumenVentasMensual]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_ResumenVentasMensual] AS
SELECT
    YEAR(o.OrderDate)          AS Anio,
    MONTH(o.OrderDate)         AS Mes,
    COUNT(DISTINCT o.OrderID)  AS CantidadOrdenes,
    SUM(od.Quantity)           AS UnidadesVendidas,
    SUM(od.TotalPrice)         AS TotalVentas
FROM Orders o
INNER JOIN Order_Details od ON od.OrderID = o.OrderID
GROUP BY YEAR(o.OrderDate), MONTH(o.OrderDate);

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatus]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatus](
	[StatusID] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_OrderStatus] PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[Stock] [int] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[CityID] [int] IDENTITY(1,1) NOT NULL,
	[CityName] [nvarchar](50) NOT NULL,
	[CountryID] [int] NOT NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[CityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Phone] [varchar](20) NULL,
	[CityID] [int] NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_VentasDetalladas]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_VentasDetalladas] AS
SELECT
    o.OrderID,
    o.OrderDate,
    c.CustomerID,
    c.FirstName + ' ' + c.LastName    AS NombreCliente,
    c.Email,
    ci.CityName,
    co.CountryName,
    st.StatusName,
    p.ProductID,
    p.ProductName,
    cat.CategoryName,
    od.Quantity,
    od.UnitPrice,
    od.TotalPrice
FROM Order_Details od
INNER JOIN Orders o      ON o.OrderID = od.OrderID
INNER JOIN Customers c   ON c.CustomerID = o.CustomerID
INNER JOIN Cities ci     ON ci.CityID = c.CityID
INNER JOIN Countries co  ON co.CountryID = ci.CountryID
INNER JOIN OrderStatus st ON st.StatusID = o.StatusID
INNER JOIN Products p    ON p.ProductID = od.ProductID
INNER JOIN Categories cat ON cat.CategoryID = p.CategoryID;

GO
/****** Object:  Index [IX_Cities_CountryID]    Script Date: 7/4/2026 2:30:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_Cities_CountryID] ON [dbo].[Cities]
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Customers_CityID]    Script Date: 7/4/2026 2:30:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_Customers_CityID] ON [dbo].[Customers]
(
	[CityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Customers_Email]    Script Date: 7/4/2026 2:30:15 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Customers_Email] ON [dbo].[Customers]
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Order_Details_OrderID]    Script Date: 7/4/2026 2:30:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_Order_Details_OrderID] ON [dbo].[Order_Details]
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Order_Details_ProductID]    Script Date: 7/4/2026 2:30:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_Order_Details_ProductID] ON [dbo].[Order_Details]
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_CustomerID]    Script Date: 7/4/2026 2:30:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_CustomerID] ON [dbo].[Orders]
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_StatusID]    Script Date: 7/4/2026 2:30:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_StatusID] ON [dbo].[Orders]
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_CategoryID]    Script Date: 7/4/2026 2:30:15 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_CategoryID] ON [dbo].[Products]
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cities]  WITH CHECK ADD  CONSTRAINT [FK_Cities_Countries_CountryID] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Countries] ([CountryID])
GO
ALTER TABLE [dbo].[Cities] CHECK CONSTRAINT [FK_Cities_Countries_CountryID]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_Customers_Cities_CityID] FOREIGN KEY([CityID])
REFERENCES [dbo].[Cities] ([CityID])
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_Customers_Cities_CityID]
GO
ALTER TABLE [dbo].[Order_Details]  WITH CHECK ADD  CONSTRAINT [FK_Order_Details_Orders_OrderID] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[Order_Details] CHECK CONSTRAINT [FK_Order_Details_Orders_OrderID]
GO
ALTER TABLE [dbo].[Order_Details]  WITH CHECK ADD  CONSTRAINT [FK_Order_Details_Products_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Order_Details] CHECK CONSTRAINT [FK_Order_Details_Products_ProductID]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Customers_CustomerID] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Customers_CustomerID]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_OrderStatus_StatusID] FOREIGN KEY([StatusID])
REFERENCES [dbo].[OrderStatus] ([StatusID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_OrderStatus_StatusID]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories_CategoryID] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories_CategoryID]
GO
/****** Object:  StoredProcedure [dbo].[sp_Top5ClientesConMasCompras]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Top5ClientesConMasCompras]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 5
        c.CustomerID,
        c.FirstName + ' ' + c.LastName AS NombreCliente,
        c.Email,
        SUM(od.TotalPrice) AS TotalComprado
    FROM Customers c
    INNER JOIN Orders o        ON o.CustomerID = c.CustomerID
    INNER JOIN Order_Details od ON od.OrderID = o.OrderID
    GROUP BY c.CustomerID, c.FirstName, c.LastName, c.Email
    ORDER BY TotalComprado DESC;
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Top5ProductosMasVendidos]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Top5ProductosMasVendidos]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 5
        p.ProductID,
        p.ProductName,
        SUM(od.Quantity)   AS UnidadesVendidas,
        SUM(od.TotalPrice) AS TotalVendido
    FROM Products p
    INNER JOIN Order_Details od ON od.ProductID = p.ProductID
    GROUP BY p.ProductID, p.ProductName
    ORDER BY UnidadesVendidas DESC;
END

GO
/****** Object:  StoredProcedure [dbo].[sp_TotalVentasPorCliente]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TotalVentasPorCliente]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        c.CustomerID,
        c.FirstName + ' ' + c.LastName AS NombreCliente,
        c.Email,
        COUNT(DISTINCT o.OrderID)      AS CantidadOrdenes,
        SUM(od.TotalPrice)             AS TotalComprado
    FROM Customers c
    LEFT JOIN Orders o        ON o.CustomerID = c.CustomerID
    LEFT JOIN Order_Details od ON od.OrderID = o.OrderID
    GROUP BY c.CustomerID, c.FirstName, c.LastName, c.Email
    ORDER BY TotalComprado DESC;
END

GO
/****** Object:  StoredProcedure [dbo].[sp_TotalVentasPorMes]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TotalVentasPorMes]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Anio, Mes, CantidadOrdenes, UnidadesVendidas, TotalVentas
    FROM dbo.vw_ResumenVentasMensual
    ORDER BY Anio, Mes;
END

GO
/****** Object:  StoredProcedure [dbo].[sp_TotalVentasPorProducto]    Script Date: 7/4/2026 2:30:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TotalVentasPorProducto]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        p.ProductID,
        p.ProductName,
        cat.CategoryName,
        SUM(od.Quantity)   AS UnidadesVendidas,
        SUM(od.TotalPrice) AS TotalVendido
    FROM Products p
    INNER JOIN Categories cat ON cat.CategoryID = p.CategoryID
    LEFT JOIN Order_Details od ON od.ProductID = p.ProductID
    GROUP BY p.ProductID, p.ProductName, cat.CategoryName
    ORDER BY TotalVendido DESC;
END

GO
USE [master]
GO
ALTER DATABASE [SistemaVentasETL] SET  READ_WRITE 
GO
