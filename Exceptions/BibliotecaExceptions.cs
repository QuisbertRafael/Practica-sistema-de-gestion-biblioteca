namespace BibliotecaApp.Exceptions
{
    //Cuando no existe un libro con codigo solicitado
    public class LibroNoEncontradoException : Exception
    {
        public LibroNoEncontradoException(string mensaje) : base(mensaje){ }
    }

     //Cuando no existe un usuario con el id solicitado
    public class UsuarioNoEncontradoException : Exception
    {
        public UsuarioNoEncontradoException(string mensaje) : base(mensaje) { }
    }

    // Al intentar prestar un libro que ya está prestado
    public class LibroNoDisponibleException : Exception
    {
        public LibroNoDisponibleException(string mensaje) : base(mensaje) { }
    }

    // al intentar devolver un préstamo que no existe o no está activo
    public class PrestamoNoEncontradoException : Exception
    {
        public PrestamoNoEncontradoException(string mensaje) : base(mensaje) { }
    }

    // al intentar registrar un código/id que ya existe
    public class CodigoDuplicadoException : Exception
    {
        public CodigoDuplicadoException(string mensaje) : base(mensaje) { }
    }


}