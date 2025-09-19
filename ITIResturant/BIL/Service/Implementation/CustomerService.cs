
namespace Restaurant.BLL.Service.Implementation
{
    public class CustomerService:ICustomerService
    {
        private readonly IMapper Mapper;
        private readonly ICustomerRepo CustomerRepo;

        public CustomerService(IMapper mapper, ICustomerRepo customerRepo)
        {
            Mapper = mapper;
            CustomerRepo = customerRepo;
        }

        
    }
}
