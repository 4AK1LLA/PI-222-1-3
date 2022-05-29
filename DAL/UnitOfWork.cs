using DAL.Models;
using DAL.Contexts;
using DAL.Interfaces;
using Ninject;
using Ninject.Web.Common;
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
            IKernel ninjectKernel = new StandardKernel();

            ninjectKernel.Bind<IDataRepository<BodyType>>().To<DataRepository<BodyType>>()
                .InRequestScope().WithConstructorArgument("context", context);
            BodyTypeRepository = ninjectKernel.Get<IDataRepository<BodyType>>();

            ninjectKernel.Bind<IDataRepository<Car>>().To<DataRepository<Car>>()
                .InRequestScope().WithConstructorArgument("context", context);
            CarRepository = ninjectKernel.Get<IDataRepository<Car>>();

            ninjectKernel.Bind<IDataRepository<CarsListViewModel>>().To<DataRepository<CarsListViewModel>>()
                .InRequestScope().WithConstructorArgument("context", context);
            CarsListViewModelRepository = ninjectKernel.Get<IDataRepository<CarsListViewModel>>();

            ninjectKernel.Bind<IDataRepository<Color>>().To<DataRepository<Color>>()
                .InRequestScope().WithConstructorArgument("context", context);
            ColorRepository = ninjectKernel.Get<IDataRepository<Color>>();

            ninjectKernel.Bind<IDataRepository<Customer>>().To<DataRepository<Customer>>()
                .InRequestScope().WithConstructorArgument("context", context);
            CustomerRepository = ninjectKernel.Get<IDataRepository<Customer>>();

            ninjectKernel.Bind<IDataRepository<Drive>>().To<DataRepository<Drive>>()
                .InRequestScope().WithConstructorArgument("context", context);
            DriveRepository = ninjectKernel.Get<IDataRepository<Drive>>();

            ninjectKernel.Bind<IDataRepository<Manager>>().To<DataRepository<Manager>>()
                .InRequestScope().WithConstructorArgument("context", context);
            ManagerRepository = ninjectKernel.Get<IDataRepository<Manager>>();

            ninjectKernel.Bind<IDataRepository<Model>>().To<DataRepository<Model>>()
                .InRequestScope().WithConstructorArgument("context", context);
            ModelRepository = ninjectKernel.Get<IDataRepository<Model>>();

            ninjectKernel.Bind<IDataRepository<Order>>().To<DataRepository<Order>>()
                .InRequestScope().WithConstructorArgument("context", context);
            OrderRepository = ninjectKernel.Get<IDataRepository<Order>>();

            ninjectKernel.Bind<IDataRepository<OrderType>>().To<DataRepository<OrderType>>()
                .InRequestScope().WithConstructorArgument("context", context);
            OrderTypeRepository = ninjectKernel.Get<IDataRepository<OrderType>>();

            ninjectKernel.Bind<IDataRepository<Service>>().To<DataRepository<Service>>()
                .InRequestScope().WithConstructorArgument("context", context);
            ServiceRepository = ninjectKernel.Get<IDataRepository<Service>>();

            ninjectKernel.Bind<IDataRepository<User>>().To<DataRepository<User>>()
                .InRequestScope().WithConstructorArgument("context", context);
            UserRepository = ninjectKernel.Get<IDataRepository<User>>();
        }

        public async Task<bool> ConfirmAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}