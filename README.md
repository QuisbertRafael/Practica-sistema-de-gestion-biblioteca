# BibliotecaApp

Aplicación de consola en C# (.NET 10) para gestionar una biblioteca: libros, usuarios y préstamos.

## ¿Qué hace?

- Registrar libros y usuarios
- Listar, buscar y eliminar libros
- Registrar préstamos y devoluciones
- Consultar libros disponibles y préstamos activos

## Cómo ejecutarlo

Necesitas tener instalado el SDK de .NET 10.

```bash
git clone <url-del-repositorio>
cd BibliotecaApp
dotnet run
```

Se abrirá un menú en la consola donde eliges la opción escribiendo el número correspondiente.

## Estructura del proyecto

- `Models/` → clases Libro, Usuario y el record Prestamo
- `Interfaces/` y `Repositories/` → repositorio genérico para guardar los datos
- `Services/` → lógica de negocio (registrar, buscar, prestar, devolver)
- `Exceptions/` → errores personalizados (libro no encontrado, no disponible, etc.)
- `Program.cs` → menú principal