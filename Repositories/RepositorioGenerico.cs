using BibliotecaApp.Interfaces;

namespace BibliotecaApp.Repositories
{
    public class RepositorioGenerico<T> : IRepositorio<T>
    {
        private readonly List<T> _elementos = new List<T>();

        public void Agregar(T elemento)
        {
            _elementos.Add(elemento);
        }

        public List<T> Listar()
        {
            return _elementos;
        }

        public T? Buscar(Func<T, bool> predicado)
        {
            return _elementos.FirstOrDefault(predicado);
        }

        public bool Eliminar(Func<T, bool> predicado)
        {
            var elemento = _elementos.FirstOrDefault(predicado);
            if (elemento == null)
            {
                return false;
            }
            return _elementos.Remove(elemento);
        }

    }


}