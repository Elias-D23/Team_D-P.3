#!/bin/bash

# Esperar a que SQL Server inicie
echo "Esperando a que SQL Server inicie..."
sleep 30s

# Ejecutar el script de inicialización
echo "Ejecutando script de inicialización..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong!Password123 -i /docker-entrypoint-initdb.d/init-db.sql

echo "Inicialización de base de datos completada" 