using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Azure.Func.Books
{
    public class HttpTrigger
    {
        private readonly BloggingContext _context;
        public HttpTrigger(BloggingContext context)
        {
            _context = context;
        }

        [FunctionName("GetPosts")]
        public async Task<HttpResponseMessage> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "posts")] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP GET/posts trigger function processed a request.");

            var postsArray = _context.Posts.OrderBy(p => p.Title).ToArray();
            
            return req.CreateResponse(HttpStatusCode.OK, postsArray);
        }

        [FunctionName("PostPost")]
        public async Task<HttpResponseMessage> Post([HttpTrigger(AuthorizationLevel.Function, "post", Route = "blog/{blogId}/post")] HttpRequestMessage req,
            int blogId,
            CancellationToken cts,
            ILogger log)
        {
            log.LogInformation("C# HTTP POST/posts trigger function processed a request.");
            
            // Get request body
            string requestBody = await req.Content.ReadAsStringAsync();
            Post data = JsonConvert.DeserializeObject<Post>(requestBody);

            Post p = new Post
            {
                BlogId = blogId,
                Content = data.Content,
                Title = data.Title
            };

            var entity = await _context.Posts.AddAsync(p, cts);
            await _context.SaveChangesAsync(cts);

            return req.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(entity.Entity));
        }

        [FunctionName("PostBlog")]
        public async Task<HttpResponseMessage> PostBlogAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "blog")] HttpRequestMessage req,
            CancellationToken cts,
            ILogger log)
        {
            log.LogInformation("C# HTTP POST/blog trigger function processed a request.");

            // Get request body
            string requestBody = await req.Content.ReadAsStringAsync();
            Blog data = JsonConvert.DeserializeObject<Blog>(requestBody);

            Blog b = new Blog
            {
                Url = data.Url
            };

            var entity = await _context.Blogs.AddAsync(b, cts);
            await _context.SaveChangesAsync(cts);

            return req.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(entity.Entity));
        }
    }
}
