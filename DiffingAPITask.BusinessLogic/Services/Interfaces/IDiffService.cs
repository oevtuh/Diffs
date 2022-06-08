using System.Collections.Generic;
using System.Threading.Tasks;
using DiffingAPITask.BusinessLogic.DTO;
using DiffingAPITask.Data.Entities;

namespace DiffingAPITask.BusinessLogic.Services.Interfaces
{
    public interface IDiffService
    {
        Task<DiffResultDTO> GetDiffResult(int id);
        Task SetRight(int id, string data);
        Task SetLeft(int id, string data);
    }
}