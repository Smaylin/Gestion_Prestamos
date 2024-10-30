
create database PruebaTecnica;

use PruebaTecnica;

CREATE TABLE tabla_cliente (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    Identificacion NVARCHAR(50) NOT NULL UNIQUE,
    Genero CHAR(1) CHECK (Genero IN ('F', 'M')),
    Es_empleado CHAR(1) CHECK (Es_empleado IN ('S', 'N')),
    Telefono NVARCHAR(20),
    Direccion NVARCHAR(255),
    Estado CHAR(1) CHECK (Estado IN ('A', 'I')),
    PerfilEmpleado INT CHECK (PerfilEmpleado IN (1, 2, 3)),
    Oficial INT,
    Fecha_creacion DATETIME DEFAULT GETDATE(),
    Fecha_modificacion DATETIME DEFAULT GETDATE()
);

GO
---------------------------------------------------------------------------
CREATE TABLE tabla_prestamos (
    Num_prestamos INT PRIMARY KEY IDENTITY(1,1),
    Tipo_prestamos INT CHECK (Tipo_prestamos IN (1, 2, 3)),
    Cliente INT NOT NULL,
    Tasa DECIMAL(5, 2) NOT NULL,
    Monto DECIMAL(18, 2) NOT NULL,
    Estado CHAR(1) CHECK (Estado IN ('A', 'I')),
    Fecha_creacion DATETIME DEFAULT GETDATE(),
    Fecha_modificacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Cliente) REFERENCES tabla_cliente(Id)
);

GO
---------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_InsertarActualizarCliente
    @Id INT = NULL,
    @Nombres NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @Identificacion NVARCHAR(50),
    @Genero CHAR(1),
    @Es_empleado CHAR(1),
    @Telefono NVARCHAR(20),
    @Direccion NVARCHAR(255),
    @Estado CHAR(1),
    @PerfilEmpleado INT,
    @Oficial INT
AS
BEGIN
    IF @Id IS NULL
    BEGIN
        -- Insertar nuevo cliente
        INSERT INTO tabla_cliente (Nombres, Apellidos, Identificacion, Genero, Es_empleado, Telefono, Direccion, Estado, PerfilEmpleado, Oficial, Fecha_creacion, Fecha_modificacion)
        VALUES (@Nombres, @Apellidos, @Identificacion, @Genero, @Es_empleado, @Telefono, @Direccion, @Estado, @PerfilEmpleado, @Oficial, GETDATE(), GETDATE());
    END
    ELSE
    BEGIN
        -- Actualizar cliente existente
        UPDATE tabla_cliente
        SET Nombres = @Nombres,
            Apellidos = @Apellidos,
            Identificacion = @Identificacion,
            Genero = @Genero,
            Es_empleado = @Es_empleado,
            Telefono = @Telefono,
            Direccion = @Direccion,
            Estado = @Estado,
            PerfilEmpleado = @PerfilEmpleado,
            Oficial = @Oficial,
            Fecha_modificacion = GETDATE()
        WHERE Id = @Id;
    END
END;
---------------------------------------------------------------------------
GO
CREATE OR ALTER PROCEDURE sp_ConsultarCliente
    @Identificacion NVARCHAR(50) = NULL,
    @CodigoCliente INT = NULL
AS
BEGIN
    IF @Identificacion IS NOT NULL
    BEGIN
        SELECT * FROM tabla_cliente WHERE Identificacion = @Identificacion;
    END
    ELSE IF @CodigoCliente IS NOT NULL
    BEGIN
        SELECT * FROM tabla_cliente WHERE Id = @CodigoCliente;
    END
END;
---------------------------------------------------------------------------
GO
CREATE OR ALTER PROCEDURE sp_InsertarActualizarPrestamo
    @Num_prestamos INT = NULL,
    @Tipo_prestamos INT,
    @Cliente INT,
    @Tasa DECIMAL(5, 2),
    @Monto DECIMAL(18, 2),
    @Estado CHAR(1)
AS
BEGIN
    IF @Num_prestamos IS NULL
    BEGIN
        -- Insertar nuevo préstamo
        INSERT INTO tabla_prestamos (Tipo_prestamos, Cliente, Tasa, Monto, Estado, Fecha_creacion, Fecha_modificacion)
        VALUES (@Tipo_prestamos, @Cliente, @Tasa, @Monto, @Estado, GETDATE(), GETDATE());
    END
    ELSE
    BEGIN
        -- Actualizar préstamo existente
        UPDATE tabla_prestamos
        SET Tipo_prestamos = @Tipo_prestamos,
            Cliente = @Cliente,
            Tasa = @Tasa,
            Monto = @Monto,
            Estado = @Estado,
            Fecha_modificacion = GETDATE()
        WHERE Num_prestamos = @Num_prestamos;
    END
END;
---------------------------------------------------------------------------
GO
CREATE OR ALTER PROCEDURE sp_ConsultarPrestamo
    @Num_prestamos INT = NULL,
    @CodigoCliente INT = NULL
AS
BEGIN
    IF @Num_prestamos IS NOT NULL
    BEGIN
        SELECT * FROM tabla_prestamos WHERE Num_prestamos = @Num_prestamos;
    END
    ELSE IF @CodigoCliente IS NOT NULL
    BEGIN
        SELECT * FROM tabla_prestamos WHERE Cliente = @CodigoCliente;
    END
END;
---------------------------------------------------------------------------


