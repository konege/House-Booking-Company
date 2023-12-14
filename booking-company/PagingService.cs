namespace booking_company.Services
{

    public class PagingService
    {
        private const int MAX_PAGE_SIZE = 10;
        private const int MIN_PAGE_SIZE = 1;
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }

        public PagingService()
        {
            this.CurrentPage = 1;
            this.PageSize = MAX_PAGE_SIZE;
        }

        public PagingService(int currentPage, int pageSize)
        {
            this.CurrentPage = currentPage < 1 ? 1 : currentPage;
            this.PageSize = pageSize > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : (pageSize < MIN_PAGE_SIZE ? MIN_PAGE_SIZE : pageSize);
        }
    }

}


