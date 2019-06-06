using System.Collections.Generic;

namespace Azure.Func.Books
{
    public partial class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}