using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CategoryListDto
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CategoryUpdateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
