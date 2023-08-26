using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{

    
    public class PaginationHeader
    {
        public PaginationHeader(int currentPage , int itemsPerPage , int totalItems, int totalPages){
            Currentpage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }
    

        public int Currentpage { get; set; }
        public int ItemsPerPage { get; set; }

        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}