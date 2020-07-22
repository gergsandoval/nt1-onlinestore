# OnlineStore
Hola! Este fue un proyecto del 2do cuatrimestre para la carrera Analista de Sistemas en ORT Argentina.

### La Materia (Programacion en Nuevas Tecnologias 1) se enfocaba en aprender
- Arquitectura Cliente-Servidor
- Codigos de respuesta HTTP
- C#
- MVC
- ASP.NET
- Entity Framework

### Modelos
- Contenian la estructura de las entidades
- Estas entidades se utilizaron para realizar el scaffolding con Entity Framework
- Se utilizaron DataAnnotations para realizar validaciones

### Controllers
- Contenian toda la logica de negocio
- Se utilizaron los controllers provistos por el scaffolding de ASP.NET para
  - Registrar un usuario
  - Loguear un usuario
- Se utilizaron autorizaciones en algunos metodos o Controllers enteros ya que la aplicacion tiene 3 Roles
  - Invitado
  - Registrado
  - Administrador

### Vistas
- Se uso el HTML basico que brinda el scaffolding de ASP.NET
- Se agregaron estilos utilizando clases de Bootstrap
