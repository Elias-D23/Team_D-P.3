# Sistema de Citas Médicas - Configuración Docker

Este proyecto implementa un sistema de gestión de citas médicas utilizando ASP.NET Core y SQLite.

## Requisitos previos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado y en ejecución
- [Git](https://git-scm.com/downloads) instalado

## Ejecución del proyecto

1. Clonar el repositorio:

   ```bash
   git clone <URL_DEL_REPOSITORIO>
   cd <DIRECTORIO_DEL_REPOSITORIO>/Sistema\ de\ citas\ médicas
   ```

2. Construir y ejecutar los contenedores Docker:

   ```bash
   docker-compose up -d --build
   ```

3. Acceder a la aplicación:

   - Abrir un navegador web y visitar: `http://localhost:8080`

4. Para detener la aplicación:
   ```bash
   docker-compose down
   ```

## Características

- **Base de datos SQLite**: La aplicación utiliza SQLite como base de datos, lo que elimina la necesidad de un servidor de base de datos separado.
- **Inicialización automática**: La base de datos se crea automáticamente al iniciar la aplicación.
- **Persistencia de datos**: Los datos se almacenan en un volumen Docker para mantener la persistencia entre reinicios.

## Solución de problemas

- **Error al iniciar la aplicación**: Verifique los logs de Docker con `docker-compose logs app`
- **Problemas de permisos**: Asegúrese de que la carpeta `data` tenga los permisos adecuados
- **Cambios en el código**: Después de realizar cambios en el código, reconstruya los contenedores con `docker-compose up -d --build`

## Notas técnicas

- La aplicación está configurada para ejecutarse en el puerto 8080
- Se utiliza SQLite para la base de datos, lo que mejora la compatibilidad con diferentes plataformas
- Los datos se almacenan en la carpeta `data` que está montada como un volumen en el contenedor
