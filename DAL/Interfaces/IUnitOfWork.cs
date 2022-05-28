using DAL.Models;

namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IDataRepository<BodyType> BodyTypeRepository { get; }
        IDataRepository<Car> CarRepository { get; }
        IDataRepository<CarsListViewModel> CarsListViewModelRepository { get; }
        IDataRepository<Color> ColorRepository { get; }
        IDataRepository<Customer> CustomerRepository { get; }
        IDataRepository<Drive> DriveRepository { get; }
        IDataRepository<Manager> ManagerRepository { get; }
        IDataRepository<Model> ModelRepository { get; }
        IDataRepository<Order> OrderRepository { get; }
        IDataRepository<OrderType> OrderTypeRepository { get; }
        IDataRepository<Service> ServiceRepository { get; }
        IDataRepository<User> UserRepository { get; }

        Task<bool> ConfirmAsync();
    }
}
