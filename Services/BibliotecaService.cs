using BibliotecaApp.Exceptions;
using BibliotecaApp.Interfaces;
using BibliotecaApp.Models;
using BibliotecaApp.Repositories;

namespace BibliotecaApp.Services
{
    // Aqui entran las reglas del negocio, prestamos, devoluciones y el uso de consultas con Linq
    public class BibliotecaService
    {
        private readonly IRepositorio<Libro> _repositorioLibros = new RepositorioGenerico<Libro>();
        private readonly IRepositorio<Usuario> _repositorioUsuarios = new RepositorioGenerico<Usuario>();
        private readonly List<Prestamo> _prestamos = new List<Prestamo>();

        // Libros

        public void RegistrarLibro(string codigo, string titulo, string autor, string categoria)
        {
            if (string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("El código y el título del libro son obligatorios.");
            }

            if (_repositorioLibros.Buscar(l => l.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase)) != null)
            {
                throw new CodigoDuplicadoException($"Ya existe un libro registrado con el código '{codigo}'.");
            }

            var libro = new Libro(codigo, titulo, autor, categoria);
            _repositorioLibros.Agregar(libro);
        }

        public Libro BuscarLibroPorCodigo(string codigo)
        {
            var libro = _repositorioLibros.Buscar(l => l.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
            if (libro == null)
            {
                throw new LibroNoEncontradoException($"No se encontró ningún libro con el código '{codigo}'.");
            }
            return libro;
        }

        public List<Libro> BuscarLibrosPorAutorOCategoria(string texto)
        {
            return _repositorioLibros.Listar()
                .Where(l => l.Autor.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                            l.Categoria.Contains(texto, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Libro> ListarLibrosDisponibles()
        {
            return _repositorioLibros.Listar()
                .Where(l => l.Disponible)
                .ToList();
        }

        public List<Libro> ListarLibrosOrdenadosPorTitulo()
        {
            return _repositorioLibros.Listar()
                .OrderBy(l => l.Titulo)
                .ToList();
        }

        public bool EliminarLibro(string codigo)
        {
            var libro = BuscarLibroPorCodigo(codigo);
            if (!libro.Disponible)
            {
                throw new InvalidOperationException("No se puede eliminar un libro que actualmente está prestado.");
            }
            return _repositorioLibros.Eliminar(l => l.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
        }

        // Usuarios

        public void RegistrarUsuario(string id, string nombre, string correo)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El id y el nombre del usuario son obligatorios.");
            }

            if (_repositorioUsuarios.Buscar(u => u.Id.Equals(id, StringComparison.OrdinalIgnoreCase)) != null)
            {
                throw new CodigoDuplicadoException($"Ya existe un usuario registrado con el id '{id}'.");
            }

            var usuario = new Usuario(id, nombre, correo);
            _repositorioUsuarios.Agregar(usuario);
        }

        public List<Usuario> ListarUsuarios()
        {
            return _repositorioUsuarios.Listar();
        }

        public Usuario BuscarUsuarioPorId(string id)
        {
            var usuario = _repositorioUsuarios.Buscar(u => u.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (usuario == null)
            {
                throw new UsuarioNoEncontradoException($"No se encontró ningún usuario con el id '{id}'.");
            }
            return usuario;
        }

        // Seccion de prestamos

        public Prestamo RegistrarPrestamo(string codigoLibro, string idUsuario)
        {
            var libro = BuscarLibroPorCodigo(codigoLibro);
            var usuario = BuscarUsuarioPorId(idUsuario);

            if (!libro.Disponible)
            {
                throw new LibroNoDisponibleException($"El libro \"{libro.Titulo}\" no está disponible para préstamo.");
            }

            libro.Disponible = false;
            var prestamo = new Prestamo(libro.Codigo, usuario.Id, DateTime.Now, null);
            _prestamos.Add(prestamo);
            return prestamo;
        }

        public Prestamo RegistrarDevolucion(string codigoLibro)
        {
            var prestamo = _prestamos.FirstOrDefault(p =>
                p.CodigoLibro.Equals(codigoLibro, StringComparison.OrdinalIgnoreCase) && p.Activo);

            if (prestamo == null)
            {
                throw new PrestamoNoEncontradoException(
                    $"No hay un préstamo activo para el libro con código '{codigoLibro}'.");
            }

            var libro = BuscarLibroPorCodigo(codigoLibro);
            libro.Disponible = true;

            int indice = _prestamos.IndexOf(prestamo);
            var prestamoActualizado = prestamo with { FechaDevolucion = DateTime.Now };
            _prestamos[indice] = prestamoActualizado;

            return prestamoActualizado;
        }

        public List<Prestamo> ListarPrestamosActivos()
        {
            return _prestamos
                .Where(p => p.Activo)
                .Select(p => p)
                .ToList();
        }
    }
}
