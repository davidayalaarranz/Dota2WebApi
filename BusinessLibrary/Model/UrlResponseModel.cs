using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLibrary.Model
{
    public class Url
    {
        public long Id { get; set; }
        public string Value { get; set; }
    }
    public class UrlResponseModel
    {
        public IEnumerable<Url> heroUrls { get; set; }
    }
}
