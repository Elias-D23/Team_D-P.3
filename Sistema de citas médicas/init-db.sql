-- Verificar si la base de datos existe y crearla si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CitaMedica_DB')
BEGIN
    CREATE DATABASE [CitaMedica_DB];
END
GO

USE [CitaMedica_DB];
GO

-- Verificar si ya existen las tablas antes de crear (para evitar errores)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Clientes')
BEGIN
    -- Crear tabla Clientes si no existe
    CREATE TABLE [Clientes] (
        [id] INT PRIMARY KEY IDENTITY(1, 1),
        [Nombre] NVARCHAR(100) NOT NULL,
        [Cedula] NVARCHAR(20) NOT NULL,
        [FechaNacimiento] DATETIME NOT NULL,
        [Telefono] NVARCHAR(20) NULL,
        [CorreoElectronico] NVARCHAR(100) NULL,
        [Contrasena] NVARCHAR(100) NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Citas')
BEGIN
    -- Crear tabla Citas si no existe
    CREATE TABLE [Citas] (
        [id] INT PRIMARY KEY IDENTITY(1, 1),
        [ClienteId] INT NOT NULL,
        [FechaHora] DATETIME NOT NULL,
        [Medico] NVARCHAR(100) NOT NULL,
        [Especialidad] NVARCHAR(100) NOT NULL,
        [RequisitosEspeciales] NVARCHAR(MAX) NULL,
        CONSTRAINT [FK_Citas_Clientes] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes]([id]) ON DELETE CASCADE
    );
END
GO

-- Agregar un cliente de ejemplo si la tabla está vacía
IF NOT EXISTS (SELECT TOP 1 * FROM [Clientes])
BEGIN
    INSERT INTO [Clientes] ([Nombre], [Cedula], [FechaNacimiento], [Telefono], [CorreoElectronico], [Contrasena])
    VALUES ('Paciente Demo', '000-0000000-0', '1990-01-01', '809-123-4567', 'demo@example.com', 'password123');
END
GO

-- Agregar una cita de ejemplo si la tabla está vacía
IF NOT EXISTS (SELECT TOP 1 * FROM [Citas])
BEGIN
    DECLARE @ClienteId INT;
    SELECT TOP 1 @ClienteId = [id] FROM [Clientes];
    
    IF @ClienteId IS NOT NULL
    BEGIN
        INSERT INTO [Citas] ([ClienteId], [FechaHora], [Medico], [Especialidad], [RequisitosEspeciales])
        VALUES (@ClienteId, DATEADD(DAY, 7, GETDATE()), 'Dr. Juan Pérez', 'Medicina General', 'Ninguno');
    END
END
GO 