# Prueba Técnica del Broker PPI

Este repositorio contiene la Prueba Técnica del Broker PPI, que es una API web desarrollada con ASP.NET Core. El proyecto utiliza Entity Framework para trabajar con una base de datos SQL Server y se incluyen pruebas unitarias realizadas con Xunit. Además, se encuentra Dockerizado para facilitar la implementación.

## Instalación de Entity Framework Core (EF Core)

Antes de comenzar, asegúrate de tener instalada la herramienta global de Entity Framework Core (EF Core) ejecutando el siguiente comando:

`dotnet tool install --global dotnet-ef`

Una vez que tengas la herramienta instalada, dirígete a la carpeta raíz del proyecto Tecnico_PPI y ejecuta el siguiente comando para asegurarte de que la base de datos esté actualizada o creada:

`dotnet ef database update`


## Autenticación y Autorización

Para acceder a los controladores de la API, es necesario utilizar un token de autenticación. Puedes obtener un token ejecutando el método POST en el AuthController. Los detalles del usuario y contraseña válidos se encuentran en el archivo `appsettings.Development.json`.

Una vez que tengas el token, si estás utilizando la interfaz de Swagger, sigue estos pasos:

1. Haz clic en el botón "Authorize".
2. Ingresa la palabra 'Bearer' seguida del token (deja un espacio en blanco entre ellos).

Esto permitirá que accedas a los recursos protegidos de la API.
