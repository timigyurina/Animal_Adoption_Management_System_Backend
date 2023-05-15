using System.ComponentModel.DataAnnotations;

namespace Animal_Adoption_Management_System_Backend.Models.Pagination
{
    public class QueryParameters
    {
        private int _pageSize = 10;
        private int _pageNumber = 1;

        public int StartIndex { get => PageSize * (PageNumber - 1); }

        [Range(1, int.MaxValue)]
        public int PageNumber // what page the client wants to get
        { 
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }  

        [Range(1, int.MaxValue)]
        public int PageSize // how many items should be on page (default 10)
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

    }
}
