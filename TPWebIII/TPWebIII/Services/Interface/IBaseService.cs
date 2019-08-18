
namespace TPWebIII.Services.Interface
{
    public interface IBaseService<T> : IGetterService<T>
    {
        int Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}