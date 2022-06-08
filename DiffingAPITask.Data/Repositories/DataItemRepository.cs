using DiffingAPITask.Data.EF;
using DiffingAPITask.Data.Entities;
using DiffingAPITask.Data.Repositories.Interfaces;

namespace DiffingAPITask.Data.Repositories
{
    public class DataItemRepository : EfRepository<DataItem>, IDataItemRepository
    {
        public DataItemRepository(ApplicationContext context) : base(context)
        {
        }
    }
}