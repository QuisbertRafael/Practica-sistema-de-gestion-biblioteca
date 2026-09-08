using BibliotecaApp.Exceptions;
using BibliotecaApp.Services;

var servicio = new BibliotecaService();

// Array para un dato fijo: las opciones del menú no cambian en tiempo de ejecución.
string[] menuPrincipal =
{
    "1. Registrar libro",
    "2. Registrar usuario",
    "3. Listar libros (ordenados por título)",
    "4. Buscar libro por código",
    "5. Buscar libros por autor o categoría",
    "6. Eliminar libro",
    "7. Registrar préstamo",
    "8. Registrar devolución",
    "9. Consultar libros disponibles",
    "10. Consultar préstamos activos",
    "11. Listar usuarios",
    "0. Salir"
};

bool continuar = true;

while (continuar)
{
    Console.WriteLine();
    Console.WriteLine("----- Sistema de gestion de Biblioteca ------");
    foreach (var opcion in menuPrincipal)
    {
        Console.WriteLine(opcion);
    }
    Console.Write("Seleccione una opción: ");

    string? entrada = Console.ReadLine();

    if (!int.TryParse(entrada, out int opcionSeleccionada))
    {
        Console.WriteLine("La entrada es inválida. Ingrese un número dentro de las opciones");
        continue;
    }

    try
    {
        switch (opcionSeleccionada)
        {
            case 1:
                RegistrarLibro();
                break;
            case 2:
                RegistrarUsuario();
                break;
            case 3:
                ListarLibros();
                break;
            case 4:
                BuscarLibroPorCodigo();
                break;
            case 5:
                BuscarLibrosPorAutorOCategoria();
                break;
            case 6:
                EliminarLibro();
                break;
            case 7:
                RegistrarPrestamo();
                break;
            case 8:
                RegistrarDevolucion();
                break;
            case 9:
                ConsultarLibrosDisponibles();
                break;
            case 10:
                ConsultarPrestamosActivos();
                break;
            case 11:
                ListarUsuarios();
                break;
            case 0:
                continuar = false;
                Console.WriteLine("Cerrando el sistema");
                break;
            default:
                Console.WriteLine("La opcion no es valida, intente nuevamente");
                break;
        }
    }
    catch (CodigoDuplicadoException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (LibroNoEncontradoException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (UsuarioNoEncontradoException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (LibroNoDisponibleException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (PrestamoNoEncontradoException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Datos inválidos: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Operación no permitida: {ex.Message}");
    }
    catch (Exception ex)
    {
        
        Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
    }
}

// --------- Funcionalidades del menú ------------

void RegistrarLibro()
{
    Console.Write("Código del libro: ");
    string codigo = Console.ReadLine() ?? string.Empty;
    Console.Write("Título: ");
    string titulo = Console.ReadLine() ?? string.Empty;
    Console.Write("Autor: ");
    string autor = Console.ReadLine() ?? string.Empty;
    Console.Write("Categoría: ");
    string categoria = Console.ReadLine() ?? string.Empty;

    servicio.RegistrarLibro(codigo, titulo, autor, categoria);
    Console.WriteLine("Libro registrado exitosamente.");
}

void RegistrarUsuario()
{
    Console.Write("Id del usuario: ");
    string id = Console.ReadLine() ?? string.Empty;
    Console.Write("Nombre: ");
    string nombre = Console.ReadLine() ?? string.Empty;
    Console.Write("Correo: ");
    string correo = Console.ReadLine() ?? string.Empty;

    servicio.RegistrarUsuario(id, nombre, correo);
    Console.WriteLine("Usuario registrado exitosamente.");
}

void ListarLibros()
{
    var libros = servicio.ListarLibrosOrdenadosPorTitulo();
    if (libros.Count == 0)
    {
        Console.WriteLine("No hay libros registrados.");
        return;
    }
    Console.WriteLine("--- Catálogo de libros ordenados por el titulo ---");
    foreach (var libro in libros)
    {
        Console.WriteLine(libro);
    }
}

void BuscarLibroPorCodigo()
{
    Console.Write("Código del libro a buscar: ");
    string codigo = Console.ReadLine() ?? string.Empty;
    var libro = servicio.BuscarLibroPorCodigo(codigo);
    Console.WriteLine("Libro encontrado:");
    Console.WriteLine(libro);
}

void BuscarLibrosPorAutorOCategoria()
{
    Console.Write("Ingrese autor o categoría a buscar: ");
    string texto = Console.ReadLine() ?? string.Empty;
    var resultados = servicio.BuscarLibrosPorAutorOCategoria(texto);
    if (resultados.Count == 0)
    {
        Console.WriteLine($"No se encontraron libros que coincidan con '{texto}'");
        return;
    }
    Console.WriteLine($"--- Resultados para '{texto}' ---");
    foreach (var libro in resultados)
    {
        Console.WriteLine(libro);
    }
}

void EliminarLibro()
{
    Console.Write("Código del libro a eliminar: ");
    string codigo = Console.ReadLine() ?? string.Empty;
    bool eliminado = servicio.EliminarLibro(codigo);
    Console.WriteLine(eliminado ? "Libro eliminado " : "No se pudo eliminar el libro");
}

void RegistrarPrestamo()
{
    Console.Write("Código del libro: ");
    string codigoLibro = Console.ReadLine() ?? string.Empty;
    Console.Write("Id del usuario: ");
    string idUsuario = Console.ReadLine() ?? string.Empty;

    var prestamo = servicio.RegistrarPrestamo(codigoLibro, idUsuario);
    Console.WriteLine($"Préstamo registrado: libro '{prestamo.CodigoLibro}' a usuario '{prestamo.IdUsuario}' el {prestamo.FechaPrestamo}");
}

void RegistrarDevolucion()
{
    Console.Write("Código del libro a devolver: ");
    string codigoLibro = Console.ReadLine() ?? string.Empty;
    var prestamo = servicio.RegistrarDevolucion(codigoLibro);
    Console.WriteLine($"Devolución registrada para el libro '{prestamo.CodigoLibro}' el {prestamo.FechaDevolucion}");
}

void ConsultarLibrosDisponibles()
{
    var disponibles = servicio.ListarLibrosDisponibles();
    if (disponibles.Count == 0)
    {
        Console.WriteLine("No hay libros disponibles en este momento");
        return;
    }
    Console.WriteLine("--- Libros disponibles ---");
    foreach (var libro in disponibles)
    {
        Console.WriteLine(libro);
    }
}

void ConsultarPrestamosActivos()
{
    var activos = servicio.ListarPrestamosActivos();
    if (activos.Count == 0)
    {
        Console.WriteLine("No hay préstamos activos.");
        return;
    }
    Console.WriteLine("--- Préstamos activos ---");
    foreach (var prestamo in activos)
    {
        Console.WriteLine($"Libro: {prestamo.CodigoLibro} | Usuario: {prestamo.IdUsuario} | Desde: {prestamo.FechaPrestamo}");
    }
}

void ListarUsuarios()
{
    var usuarios = servicio.ListarUsuarios();
    if (usuarios.Count == 0)
    {
        Console.WriteLine("No hay usuarios registrados.");
        return;
    }
    Console.WriteLine("--- Usuarios registrados ---");
    foreach (var usuario in usuarios)
    {
        Console.WriteLine(usuario);
    }
}
