namespace SewingFactory.Core
{
    public class PaginatedList<T>
    {
        //Thuộc tính lưu trữ danh sách phần tử
        public IEnumerable<T> Items { get; private set; }

        // Thuộc tính để lưu trữ tổng số phần tử
        public int TotalItems { get; private set; }

        // Thuộc tính để lưu trữ số trang hiện tại
        public int CurrentPage { get; private set; }

        // Thuộc tính để lưu trữ tổng số trang
        public int TotalPages { get; private set; }

        // Thuộc tính để lưu trữ số phần tử trên mỗi trang
        public int PageSize { get; private set; }

        public PaginatedList(IEnumerable<T> items, int pageNumber, int pageSize)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (pageNumber <= 0) throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than zero.");
            if (pageSize <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than zero.");

            Items = items;
            TotalItems = items.Count();
            TotalPages = (int)Math.Ceiling((decimal)TotalItems / pageSize);
            CurrentPage = pageNumber;
            PageSize = pageSize;

            if (pageNumber > TotalPages) throw new ArgumentOutOfRangeException("The requested page number exceeds the total number of pages.");
        }

        //Phương thức để trả ra danh sách object theo pagination
        public IEnumerable<T> GetPaginatedItems()
        {
            return Items
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList()
                ?? throw new ArgumentNullException(nameof(Items));
        }

        // Phương thức để kiểm tra nếu có trang trước đó
        public bool HasPreviousPage => CurrentPage > 1;

        // Phương thức để kiểm tra nếu có trang kế tiếp
        public bool HasNextPage => CurrentPage < TotalPages;


    }
}
