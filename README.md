# Sistema de Citas Médicas - Configuración Docker

Este documento contiene instrucciones para ejecutar el Sistema de Citas Médicas utilizando Docker, lo que permite un entorno de desarrollo consistente independientemente del sistema operativo.

## Requisitos previos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado y funcionando
- [Git](https://git-scm.com/downloads) para clonar el repositorio (opcional)

## Ejecución del proyecto

1. Asegúrate de estar en el directorio raíz del proyecto (donde se encuentra el archivo docker-compose.yml)

2. Construye e inicia los contenedores:

   ```
   docker-compose up -d --build
   ```

3. La aplicación estará disponible en:

   - http://localhost:8080

4. La base de datos SQL Server está disponible en:
   - Host: localhost
   - Puerto: 1433
   - Usuario: sa
   - Contraseña: YourStrong!Password123
   - Base de datos: CitaMedica_DB (se crea automáticamente durante la inicialización)

## Notas importantes sobre la configuración

La configuración actual está optimizada para el desarrollo en contenedores Docker:

1. La aplicación está configurada para usar solo HTTP en lugar de HTTPS, para evitar problemas con certificados en el contenedor.
2. Las redirecciones HTTPS solo están habilitadas en el entorno de producción.
3. Si necesitas HTTPS para desarrollo, tendrías que montar un certificado válido en el contenedor.
4. La base de datos se inicializa automáticamente con un script que crea las tablas necesarias y agrega datos de ejemplo.
5. La aplicación está configurada para esperar a que SQL Server esté completamente inicializado antes de intentar conectarse.
6. Se ha habilitado la política de reintentos para manejar errores transitorios en la conexión a la base de datos.

## Detener la aplicación

Para detener la aplicación, ejecuta:

```
docker-compose down
```

Para detener y eliminar volúmenes (borrará los datos de la base de datos):

```
docker-compose down -v
```

## Solución de problemas

Si encuentras algún problema, prueba estos pasos:

1. Verifica que Docker Desktop esté en ejecución
2. Intenta reconstruir los contenedores:
   ```
   docker-compose down
   docker-compose up -d --build
   ```
3. Revisa los logs de Docker:
   ```
   docker-compose logs
   ```
4. Si tienes problemas específicos con SQL Server en Mac con arquitectura ARM64 (Apple Silicon), puedes recibir advertencias sobre la plataforma, pero la imagen debería funcionar correctamente a través de la emulación.

5. Si sigues teniendo problemas con la conexión a la base de datos, puedes verificar que SQL Server esté funcionando correctamente usando:
   ```
   docker-compose exec db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong!Password123 -Q "SELECT name FROM sys.databases"
   ```

# Team_D-P.3

"Este repositorio contiene el código fuente para una plataforma web diseñada para revolucionar la gestión de citas médicas a nivel internacional".
