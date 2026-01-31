using System;
using System.Collections.Generic;
using System.Text;

namespace NewNews.MAUI.Dto
{
    public class CountryDto
    {
        public string Name { get; set; } = string.Empty; 
        public string Code { get; set; } = string.Empty;  

        public override string ToString() => Name; 

        public override bool Equals(object? obj)
        {
            return obj is CountryDto dto &&
                   Code == dto.Code;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }
    }
}
