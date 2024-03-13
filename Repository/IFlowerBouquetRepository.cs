using BusinessObject;
using BusinessObject.Common.PagedList;
using BusinessObject.Dto;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IFlowerBouquetRepository : IGenericRepository<FlowerBouquet>
    {
        Task<IPagedList<TResult>> SearchAsync<TResult>(SearchFlowerRequest req) where TResult : class;
    }
}
