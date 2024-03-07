using BusinessObject;
using BusinessObject.Common.Enums;
using BusinessObject.Common.PagedList;
using Microsoft.EntityFrameworkCore;
using Repository.Common;
using Repository.Helper;

namespace Repository
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(DbContext context) : base(context)
        {
        }

        public override async Task<IPagedList<Account>> SearchAsync(string keySearch, PagingQuery pagingQuery, string orderBy)
        {
            return await _dbSet.AsNoTracking()
                                .Where(a => a.Role != Role.Admin.ToString())
                                .AddOrderByString(orderBy)
                                .Include(a => a.Orders)
                                    .ThenInclude(a => a.OrderDetails)
                                .ToPagedListAsync(pagingQuery);
        }

        public override Task<IPagedList<TResult>> SearchAsync<TResult>(string keySearch, PagingQuery pagingQuery, string orderBy)
        {
            throw new NotImplementedException();
        }
    }
}
