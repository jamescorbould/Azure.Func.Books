using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Azure.Func.Books
{
    public partial class Blog
    {
        public class BloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext>
        {
            public BloggingContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<BloggingContext>();
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnectionString"));

                return new BloggingContext(optionsBuilder.Options);
            }
        }
    }
}