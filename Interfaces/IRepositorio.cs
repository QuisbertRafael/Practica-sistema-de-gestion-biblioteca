namespace BibliotecaApp.Interfaces
{
    public interface IRepositorio<T>
    {
        void Agregar(T elemento);
        List<T> Listar();
        T? Buscar(Func<T, bool> predicado);
        bool Eliminar(Func<T, bool> predicado);
    }
}