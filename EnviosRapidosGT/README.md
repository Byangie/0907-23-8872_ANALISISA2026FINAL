## Autor

Angelica María Mejía Tzoc

# EnviosRapidosGT

API REST desarrollada en C# para la empresa ficticia Envios Rapidos GT. El sistema permite registrar envíos, calcular tarifas, aplicar descuentos, generar códigos de rastreo, actualizar estados, registrar intentos fallidos y consultar historial.

## Tecnologías utilizadas

* C#
* ASP.NET Core Web API
* .NET 8
* SQLite
* Entity Framework Core
* Swagger / OpenAPI
* xUnit
* Visual Studio 2022

## Historias de usuario

1. Como cliente, quiero registrar un envío para poder enviar paquetes a nivel nacional.
2. Como cliente, quiero obtener un código de rastreo para consultar mi paquete.
3. Como cliente, quiero consultar el estado de mi envío para saber dónde se encuentra.
4. Como empleado, quiero actualizar el estado del envío para mantener informado al cliente.
5. Como empleado, quiero registrar la oficina donde se actualiza el paquete.
6. Como sistema, quiero calcular automáticamente la tarifa según el peso del paquete.
7. Como sistema, quiero aplicar un 5% de descuento si el remitente o destinatario tiene NIT válido.
8. Como repartidor, quiero registrar intentos fallidos de entrega.
9. Como sistema, quiero cambiar el envío a EnDevolucion al tercer intento fallido.
10. Como administrador, quiero consultar el historial del envío para auditar los cambios realizados.

## Reglas de negocio

* Peso menor o igual a 1 kg: Q25.00
* Peso de 1.01 kg a 5 kg: Q45.00
* Peso de 5.01 kg a 10 kg: Q75.00
* Peso mayor a 10 kg: Q100.00
* Si el remitente o destinatario tiene NIT válido, se aplica 5% de descuento.
* Máximo 3 intentos de entrega fallidos.
* Al tercer intento fallido, el envío cambia automáticamente a EnDevolucion.
* Los estados solo avanzan en una dirección:

  * Registrado → EnTransito → EnReparto → Entregado
  * EnReparto → Devuelto
  * EnReparto → EnDevolucion → Devuelto
* Cada cambio de estado registra historial con estado, ubicación, fecha/hora y notas.

## Endpoints principales

* GET /api/Envios
* POST /api/Envios
* GET /api/Envios/{codigo}
* PUT /api/Envios/{codigo}/estado
* POST /api/Envios/{codigo}/intento-fallido
* GET /api/Envios/{codigo}/historial

## Cómo ejecutar el proyecto

1. Clonar o descargar el proyecto.
2. Abrir la solución en Visual Studio 2022.
3. Restaurar paquetes NuGet.
4. Ejecutar el proyecto con HTTPS.
5. Abrir Swagger en el navegador.

También puede ejecutarse desde consola:

```bash
dotnet restore
dotnet run
```

## Base de datos

El proyecto utiliza SQLite con Entity Framework Core.

Comandos utilizados:

```powershell
Add-Migration InitialCreate
Update-Database
```

La base de datos generada es:

```txt
envios.db
```

## Cómo ejecutar pruebas unitarias

Desde Visual Studio:

```txt
Prueba → Ejecutar todas las pruebas
```

Desde consola:

```bash
dotnet test
```

Se realizaron pruebas unitarias con xUnit para validar tarifas, descuentos, cambios de estado, intentos fallidos, historial y código de rastreo.

## Prompts utilizados con IA

1. Analiza el caso de Envios Rapidos GT y sus reglas de negocio.
2. Genera historias de usuario para un sistema de rastreo de paquetes.
3. Ayúdame a crear una API REST en C# con SQLite.
4. Ayúdame a implementar las reglas de negocio en un Service.
5. Crea pruebas unitarias con xUnit para validar tarifas, descuentos y estados.

## Reflexión sobre el uso de IA

La inteligencia artificial fue utilizada como apoyo para organizar los requerimientos, definir historias de usuario, estructurar la API REST, crear reglas de negocio y proponer pruebas unitarias. La IA ayudó a reducir errores y acelerar el desarrollo, pero fue necesario revisar y ajustar las respuestas según el enunciado del examen.

## Correcciones realizadas

* Se corrigió la transición de estados permitida.
* Se agregó la regla de máximo 3 intentos fallidos.
* Se agregó el cambio automático a EnDevolucion.
* Se configuró SQLite con Entity Framework Core.
* Se agregó una llave primaria a HistorialEstado para permitir migraciones.
* Se implementaron pruebas unitarias con xUnit.
* Se conectó el Controller con la base de datos usando AppDbContext.

## Diagrama de flujo del proceso

```txt
Inicio
 ↓
Analizar el caso del examen
 ↓
Identificar problemas y reglas de negocio
 ↓
Crear historias de usuario
 ↓
Diseñar modelos del sistema
 ↓
Implementar API REST en C#
 ↓
Configurar SQLite y Entity Framework
 ↓
Crear migraciones
 ↓
Implementar reglas en EnvioService
 ↓
Crear endpoints en EnviosController
 ↓
Realizar pruebas unitarias con xUnit
 ↓
Ejecutar pruebas y corregir errores
 ↓
Documentar proyecto en README.md
 ↓
Preparar despliegue en Render
 ↓
Fin
```

## Autor

Angelica María Mejía Tzoc
