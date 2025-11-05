using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class BookDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CountOfPage { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }

    public class BookListDto
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CountOfPage { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }
}
