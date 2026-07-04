# CargaVentasDiaria - Proceso ETL de Carga Transaccional



## Información del proyecto

| Campo | Valor |
|---|---|
| Estudiante | Gregori Evangelista Jimenez |
| Matrícula | 2025-1232 |
| Período | C2 2026 |
| Maestro | Francis Ramirez |
| Entregable | Carga de la base de datos transaccional |
| Proyecto seleccionado | Sistema de Análisis de Ventas |

## Descripción

Aplicación que implementa un proceso **ETL (Extract, Transform, Load)** encargado de leer información desde archivos CSV, validar su estructura y contenido, transformar los datos cuando es necesario, y cargarlos en una base de datos transaccional SQL Server, respetando las relaciones e integridad referencial del modelo relacional.

## Arquitectura del proyecto

La solución está organizada en dos proyectos principales:

```
CargaVentasDiaria/
├── CargaVentasDiaria.Data/       # Modelos, contexto de datos e interfaces
│   ├── Context/
│   ├── Interfaces/
│   ├── Models/
│   │   ├── Csv/                 # DTOs para lectura de CSV
│   │   ├── Category.cs
│   │   ├── City.cs
│   │   ├── Country.cs
│   │   ├── Customer.cs
│   │   ├── Order.cs
│   │   ├── OrderDetail.cs
│   │   ├── OrderStatus.cs
│   │   └── Product.cs
│   ├── Result/
│   └── Services/
└── CargaVentasDiaria.Load/       # Lógica de carga, validaciones y ejecución del ETL
    ├── Interfaces/
    └── Services/
```

## Base de datos

**Motor:** SQL Server
**Nombre:** `SistemaVentasETL`
**Script:** [`ScriptBaseDatos.sql`](./ScriptBaseDatos.sql)

### Tablas (8)

| Tabla | Descripción |
|---|---|
| Categories | Categorías de productos |
| Countries | Países |
| Cities | Ciudades (FK → Countries) |
| Customers | Clientes (FK → Cities) |
| Products | Productos (FK → Categories) |
| OrderStatus | Estados de la orden (Completado, Pendiente, Cancelado) |
| Orders | Órdenes de venta (FK → Customers, OrderStatus) |
| Order_Details | Detalle de la orden (FK → Orders, Products) |

### Vistas

- `vw_ResumenVentasMensual` — resumen de ventas agrupado por año/mes
- `vw_VentasDetalladas` — detalle completo de ventas con cliente, producto, ciudad y país

### Procedimientos almacenados

- `sp_TotalVentasPorProducto`
- `sp_TotalVentasPorCliente`
- `sp_TotalVentasPorMes`
- `sp_Top5ProductosMasVendidos`
- `sp_Top5ClientesConMasCompras`

## Proceso ETL

El programa ejecuta las siguientes etapas:

1. **Extracción:** lectura de los archivos CSV de Productos, Clientes y Ventas.
2. **Validación de estructura:** formato correcto, campos obligatorios y tipos de datos válidos.
3. **Transformación:** limpieza de datos cuando aplica.
4. **Validación de duplicados:** registros repetidos dentro del mismo archivo o ya existentes en base de datos.
5. **Validación de integridad referencial:** verifica que clientes y productos referenciados existan antes de insertar.
6. **Carga:** inserción respetando las relaciones entre tablas (Orders → Order_Details).
7. **Resumen final:** cantidad de registros leídos, insertados y rechazados, junto al detalle de cada rechazo.

### Ejemplo de resumen de ejecución

```
===== RESUMEN DEL PROCESO ETL =====
--- Productos ---
Leídos: 17 | Insertados: 12 | Rechazados: 5
--- Clientes ---
Leídas: 15 | Insertados: 10 | Rechazados: 5
--- Ventas (líneas de detalle) ---
Leídas: 15 | Ordenes creadas: 8 | Líneas insertadas: 9 | Rechazadas: 6
```

## Validaciones implementadas

- ✅ Formato correcto de los datos
- ✅ Campos obligatorios
- ✅ Tipos de datos válidos
- ✅ Registros duplicados (dentro del archivo y contra la base de datos)
- ✅ Integridad referencial (cliente/producto existente)
- ✅ Manejo de errores mediante excepciones

## Consultas de validación de integridad referencial

El repositorio incluye 5 consultas SQL que verifican que no existan registros huérfanos entre las tablas relacionadas por llave foránea (Products-Categories, Cities-Countries, Customers-Cities, Orders-Customers/OrderStatus, Order_Details-Orders/Products). Todas retornan **0 filas**, confirmando la integridad del modelo tras la carga.

## Tecnologías utilizadas

- C# (.NET)
- SQL Server
- Arquitectura por capas (Data / Load, Models, Interfaces, Services)



