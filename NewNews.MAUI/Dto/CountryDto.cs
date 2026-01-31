using System;
using System.Collections.Generic;
using System.Text;

namespace NewNews.MAUI.Dto
{
    public class CountryDto
    {
        public string Name { get; set; } = string.Empty;  // t.ex. "Sverige"
        public string Code { get; set; } = string.Empty;  // t.ex. "se"

        public override string ToString() => Name; // gör att combobox/collectionview visar Name
    }
}
