using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class AuthorDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PlaceOfBirth { get; set; }
        public int YearOfBirth { get; set; }
    }

    public class AuthorListDto
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PlaceOfBirth { get; set; }
        public int YearOfBirth { get; set; }
    }

    public class AuthorUpdateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PlaceOfBirth { get; set; }
        public int? YearOfBirth { get; set; }
    }
}
