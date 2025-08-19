CREATE DATABASE AquaSort;
GO

USE AquaSort;
GO

-- Pendiente de agregar el rol de Admin o Cliente
CREATE TABLE Usuarios (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario VARCHAR(50) NOT NULL,
    contraseña VARCHAR(200) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE
);


-- Productos del inventario
CREATE TABLE Producto (
	Id_producto INT IDENTITY(1,1) PRIMARY KEY,
	Descripcion VARCHAR(75) NULL,
	Cantidad INT NULL,
	Precio DECIMAL(7, 2) NULL
);

CREATE TABLE TarjetaAlmacen (
    IdMovimiento INT PRIMARY KEY IDENTITY(1,1),
    Fecha DATE NOT NULL,
    ProductoId INT NOT NULL,
    Entrada INT DEFAULT 0,
    Salida INT DEFAULT 0,
    Costo DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_TarjetaAlmacen_Producto FOREIGN KEY (ProductoId)
    REFERENCES Producto(Id_Producto)
);



CREATE TABLE Componentes (
Id_componente INT IDENTITY(1,1) PRIMARY KEY,
Nombre VARCHAR(75) NULL,

);
		
		
CREATE TABLE Componentes_producto (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdProducto INT NULL,
    Id_Componente INT NULL,
    Cantidad INT NULL,
    CONSTRAINT FK_Componentes_Producto FOREIGN KEY (IdProducto)
        REFERENCES Producto(Id_producto),
    CONSTRAINT FK_ComponentesProducto_Componentes FOREIGN KEY (Id_Componente)
        REFERENCES Componentes(Id_componente)
);


SELECT * FROM Usuarios

ALTER TABLE Usuarios ADD COLUMN Rol INT NULL;









select * from Usuarios
-- Ejemplo del usuario

INSERT INTO Usuarios (usuario, contraseña, email) -- campos que llevara la tabla al momento de insertarlo.
VALUES (
    'admin',
    '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', -- encriprado con el metodo sha256
    'admin@aquasort.com'
);

-- Solicitudes de carrito por parte del cliente
CREATE TABLE Solicitudes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CodigoPedido VARCHAR(10),        -- Ej: PED001
    ClienteNombre NVARCHAR(100), -- Quiza se relacione con Id usuario tipo cliente que inicio sesion
    Telefono NVARCHAR(20),
    Direccion NVARCHAR(255),
    Referencia NVARCHAR(255),
    Fecha DATETIME DEFAULT GETDATE(),
    Cantidad INT,
    Total DECIMAL(10,2),              -- Cantidad * PrecioUnitario (precio unitario ya vendra definido en el front)
    MetodoPago NVARCHAR(50),          -- Ej: Efectivo al recibir
    Estado NVARCHAR(20) DEFAULT 'Pendiente' -- Pendiente / Aceptado / Rechazado
);




SELECT * FROM Solicitudes




