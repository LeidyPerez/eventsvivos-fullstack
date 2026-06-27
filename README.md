# EventosVivos - Sistema de Reservas

Sistema fullstack para gestión de eventos y reservas desarrollado con .NET Core y Angular.

## Tecnologías Utilizadas

**Backend:**
- .NET 10 (ASP.NET Core Web API)
- C#
- Base de datos en memoria (colecciones List<T>)
- Swagger / OpenAPI
- xUnit (pruebas unitarias)

**Frontend:**
- Angular 22
- TypeScript
- SCSS

## Arquitectura

Se implementó una arquitectura en capas con separación de responsabilidades:

- **Models:** Entidades del dominio (Evento, Reserva, Venue)
- **DTOs:** Objetos de transferencia de datos con validaciones
- **Interfaces:** Contratos que definen el comportamiento de los servicios
- **Services:** Lógica de negocio e implementación de las reglas
- **Controllers:** Exposición de endpoints REST

Esta arquitectura fue elegida porque:
- Facilita el mantenimiento y escalabilidad
- Permite reemplazar la base de datos en memoria por una real sin cambiar la lógica de negocio
- Aplica principios SOLID, especialmente el principio de inversión de dependencias

## Requisitos Previos

- .NET 10 SDK
- Node.js 24+
- Angular CLI 22+

## Cómo Ejecutar el Proyecto

### Backend

```bash
cd EventosVivos.API
dotnet run
```

La API quedará disponible en:
- http://localhost:5023
- Swagger: http://localhost:5023/swagger

### Frontend

```bash
cd eventsvivos-frontend
npm install
ng serve
```

El frontend quedará disponible en:
- http://localhost:4200

### Pruebas Unitarias

```bash
cd EventosVivos.Tests
dotnet test
```

## Endpoints de la API

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | /api/eventos | Crear evento |
| GET | /api/eventos | Listar eventos con filtros |
| POST | /api/eventos/reservas | Crear reserva |
| PUT | /api/eventos/reservas/{id}/confirmar | Confirmar pago |
| PUT | /api/eventos/reservas/{id}/cancelar | Cancelar reserva |
| GET | /api/eventos/{id}/reporte | Reporte de ocupación |

## Venues Disponibles

| ID | Nombre | Capacidad | Ciudad |
|----|--------|-----------|--------|
| 1 | Auditorio Central | 200 | Bogotá |
| 2 | Sala Norte | 50 | Bogotá |
| 3 | Arena Sur | 500 | Medellín |

## Reglas de Negocio Implementadas

- Un evento no puede exceder la capacidad del venue
- Dos eventos activos no pueden compartir venue con horarios superpuestos
- Eventos en fin de semana no pueden iniciar después de las 22:00
- No se permiten reservas para eventos que inician en menos de 1 hora
- Eventos con precio mayor a $100 limitan a 10 entradas por transacción
- Si faltan menos de 24 horas, máximo 5 entradas por transacción
- Un evento se marca como completado automáticamente cuando supera su hora de fin
- Si se cancela una reserva confirmada con menos de 48 horas del evento, las entradas se marcan como perdidas

## Seguridad

La API está protegida con autenticación **JWT (JSON Web Tokens)**.

### Credenciales de prueba
- **Admin:** usuario `admin` / contraseña `Ev3nt0s@2026`
- **Usuario:** usuario `user` / contraseña `Us3r@Vivos!`

### Flujo de autenticación
1. Hacer login en `POST /api/auth/login` con las credenciales
2. Copiar el token JWT de la respuesta
3. Incluir el token en cada petición: `Authorization: Bearer {token}`

El frontend maneja el token automáticamente mediante un interceptor HTTP.