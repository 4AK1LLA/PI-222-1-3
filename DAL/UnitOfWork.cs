using DAL.Models;
using DAL.Contexts;
using DAL.Interfaces;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AutoShowContext _context;
        public IDataRepository<BodyType> BodyTypeRepository { get; private set; }
        public IDataRepository<Car> CarRepository { get; private set; }
        public IDataRepository<CarsListViewModel> CarsListViewModelRepository { get; private set; }
        public IDataRepository<Color> ColorRepository { get; private set; }
        public IDataRepository<Customer> CustomerRepository { get; private set; }
        public IDataRepository<Drive> DriveRepository { get; private set; }
        public IDataRepository<Manager> ManagerRepository { get; private set; }
        public IDataRepository<Model> ModelRepository { get; private set; }
        public IDataRepository<Order> OrderRepository { get; private set; }
        public IDataRepository<OrderType> OrderTypeRepository { get; private set; }
        public IDataRepository<Service> ServiceRepository { get; private set; }
        public IDataRepository<User> UserRepository { get; private set; }

        public UnitOfWork(AutoShowContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
            
            BodyTypeRepository = new DataRepository<BodyType>(context);
            CarRepository = new DataRepository<Car>(context);
            CarsListViewModelRepository = new DataRepository<CarsListViewModel>(context);
            ColorRepository = new DataRepository<Color>(context);
            CustomerRepository = new DataRepository<Customer>(context);
            DriveRepository = new DataRepository<Drive>(context);
            ManagerRepository = new DataRepository<Manager>(context);
            ModelRepository = new DataRepository<Model>(context);
            OrderRepository = new DataRepository<Order>(context);
            OrderTypeRepository = new DataRepository<OrderType>(context);
            ServiceRepository = new DataRepository<Service>(context);
            UserRepository = new DataRepository<User>(context);
        }

        public async Task<bool> ConfirmAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}