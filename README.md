# Backend – Proyecto Prueba Técnica

Este proyecto es el **backend** de la prueba técnica, desarrollado con **.NET 8, Dapper y PostgreSQL**.

Incluye:

- CRUD de usuarios
- Autenticación JWT
- Seed automático de usuarios
- Triggers de fechas (`fechaCreacion` y `fechaModificacion`)
- Constraint de estado (`'A'` o `'I'`)

---

## Requisitos previos

- .NET 8 SDK
- PostgreSQL 15 o superior
- Visual Studio 2022 / Visual Studio Code (opcional)

---

## Pasos para levantar el proyecto

### 1 Clonar el repositorio

```bash
git clone https://github.com/usuario/proyecto-prueba-tecnica.git
cd proyecto-prueba-tecnica/backend

```

### 2 Crear Base de Datos

#### 2.1 Ingresar usuario

```bash
psql -U postgres -h localhost -d ptcnica
```

#### 2.2 Crear la base de datos y conectarse a ella

```bash
CREATE DATABASE ptecnica;

--ejecutar el comando aparte
\c ptecnica
```

#### 2.3 Ejecutar el archivo dentro de postgresql script.sql que se encuentra en la carpeta "Base de Datos"

```bash
\i '/Base_de_Datos/script.sql'
```

### 3 Editar el archivo appsettings.json de la carpeta

```bash
{
  "ConnectionStrings": {
    "ConnectionPostgresql": "Host=localhost;Database=ptecnica;Username=postgres;Password=1234"
  },
  "Jwt": {
    "key": "SecretKey"
  }
}
```

### 4 Restaurar paquetes nuget

--en consola desde powershell dentro de la carpeta del proyecto

```bash
cd Backend/Prueba_tecnica

dotnet restore
```

### 5 Ejecutar la aplicación

```bash
dotner run
```

### 6 (Al ejecutar el proyecto se hace la incersión de 4 usuarios, si solo si no hay registros en la BD)

#### para ingresar a login son los siguientes

##### - telefono: 71234567, password: AnaPass123

##### - telefono: 77771111, password: LuisPass456

##### - telefono: 77770000, password: MariaPass789

##### - telefono: 77773241, password: CarlosPass321
