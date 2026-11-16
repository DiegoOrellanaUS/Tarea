# Sistema

API .NET 10 con un CRUD de personas y persistencia en PostgreSQL.

## Ejecutar con Docker

Abre PowerShell en la raíz del proyecto con Docker activo. Ejecuta la última línea después de que la API haya arrancado.

```powershell
Copy-Item .env.example .env
docker compose --env-file .env -f contructor.yml up -d --build
docker compose --env-file .env -f contructor.yml ps
Invoke-RestMethod http://localhost:8080/api/personas
```

Puedes cambiar `API_PORT` en `.env` si el puerto 8080 está ocupado. La API aplica la migración al arrancar y PostgreSQL conserva los datos en el volumen `sistema_postgres_data`.

Los ejemplos de [Sistema.http](Sistema/Sistema.http) cubren las cinco operaciones. Ejecuta primero el POST y usa el ID devuelto en las solicitudes siguientes.

## Arrancar desde la imagen local

Después de construir `sistema-api:1.0`, puedes arrancar los servicios en el mismo equipo con `despliegue.yml`.

```powershell
docker compose --env-file .env -f despliegue.yml up -d --no-build
Invoke-RestMethod http://localhost:8080/api/personas
```

El siguiente comando detiene los servicios y conserva los datos del volumen.

```powershell
docker compose --env-file .env -f contructor.yml down
```

## Verificación

Se consultó a Codex cómo arrancar una imagen local ya construida con PostgreSQL y esperar a que la base de datos acepte conexiones. La configuración usa `depends_on` con `service_healthy`. [Docker Compose y orden de arranque](https://docs.docker.com/compose/how-tos/startup-order/)

La API compiló con .NET 10. Se comprobaron el CRUD y la persistencia tras reiniciar la API con PostgreSQL local. La construcción y el arranque en contenedores se pueden comprobar en un equipo con Docker siguiendo los comandos de arriba.
