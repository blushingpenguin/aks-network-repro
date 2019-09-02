using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace TestClient
{
    class TestDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public int Number { get; set; }
    }

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public async Task HandleRequest(HttpContext context)
        {
            var client = new MongoClient("mongodb://mongo-server");
            var db = client.GetDatabase("test");
            var coll = db.GetCollection<TestDocument>("test");

            for (int i = 0; i < 10; ++i)
            {
                await coll.InsertOneAsync(new TestDocument
                {
                    Text = $"test document {i}",
                    Number = i + 42
                });
            }
            var docs = await coll.Find(x => true).ToListAsync();
            context.Response.ContentType = "text/plain";
            foreach (var doc in docs)
            {
                await context.Response.WriteAsync($"id = {doc.Id}, text = {doc.Text}, number = {doc.Number}\n");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(HandleRequest);
        }
    }
}
