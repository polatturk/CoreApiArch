using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; } /* Description olmak zorunda degil o yuzden "?" isareti ile null olabilir diye birakiyorum*/
        public int CountOfPage { get; set; }


        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }
}
