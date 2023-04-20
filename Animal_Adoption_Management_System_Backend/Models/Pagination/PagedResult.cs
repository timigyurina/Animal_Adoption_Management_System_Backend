namespace Animal_Adoption_Management_System_Backend.Models.Pagination
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set;} // all record (eg. 100)
        public int CurrentPage { get; set; } // what page we are currently at (the client requested)
        public int PageSize { get; set; } // max number of records that can be on the page (eg. 15 out of 100)
        public List<T> Items { get; set; }
    }
}
