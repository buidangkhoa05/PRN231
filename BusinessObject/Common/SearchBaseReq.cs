using BusinessObject.Common.PagedList;

namespace BusinessObject.Common
{
    public class SearchBaseReq
    {
        public string? KeySearch { get; set; } = null;
        public PagingQuery PagingQuery { get; set; } = new PagingQuery();
        public string? OrderBy { get; set; } = null;
    }
}
