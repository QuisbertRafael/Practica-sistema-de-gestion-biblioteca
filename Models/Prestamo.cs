namespace BibliotecaApp.Models
{
public record Prestamo(string CodigoLibro, string IdUsuario, DateTime FechaPrestamo, DateTime? FechaDevolucion)
{
    public bool Activo => FechaDevolucion == null;
}
}