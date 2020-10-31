using System.Collections.Generic;
using System.IO;
using booksapi.Data;
using booksapi.Middleware;
using booksapi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace booksapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration().WriteTo.RollingFile(Path.Combine("Logs", "log-{Date}.txt")).CreateLogger();
            Log.Logger.Information("Logger init");
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("BooksDatabase"), ServiceLifetime.Scoped);

            var context = services.BuildServiceProvider().GetService<DataContext>();
            
            InitContext(context);
            
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("MyPolicy");

            app.UseRouting();

            app.UseAuthorization();
            app.UseHandlerExceptions();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static void InitContext(DataContext context)
        {
            var authors = new List<Author>()
            {
                new Author() { Id = 1, Name = "Miguel de Cervantes" },
                new Author() { Id = 2, Name = "Liev Tolstói" },
                new Author() { Id = 3, Name = "Thomas Mann" },
                new Author() { Id = 4, Name = "Gabriel García Márquez" },
                new Author() { Id = 5, Name = "James Joyce" },
                new Author() { Id = 6, Name = "Marcel Proust" },
                new Author() { Id = 7, Name = "Dante Alighieri" },
                new Author() { Id = 8, Name = "Robert Musil" },
                new Author() { Id = 9, Name = "Franz Kafka" },
                new Author() { Id = 10, Name = "William Faulkner" }
            };

            context.Authors.AddRange(authors);

            var books = new List<Book>()
            {
                new Book() { Title = "Dom Quixote", AuthorId = 1, Year = 1605 },
                new Book() { Title = "Guerra e Paz", AuthorId = 2, Year = 1869 },
                new Book() { Title = "A Montanha Mágica", AuthorId = 3, Year = 1924 },
                new Book() { Title = "Cem Anos de Solidão", AuthorId = 4, Year = 1967 },
                new Book() { Title = "Ulisses", AuthorId = 5, Year = 1922 },
                new Book() { Title = "Em Busca do Tempo Perdido", AuthorId = 6, Year = 1913 },
                new Book() { Title = "A Divina Comédia", AuthorId = 7, Year = 1321 },
                new Book() { Title = "O Homem sem Qualidades", AuthorId = 8, Year = 1943 },
                new Book() { Title = "O Processo", AuthorId = 9, Year = 1925 },
                new Book() { Title = "O Som e a Fúria", AuthorId = 10, Year = 1929 },
            };
        
            context.Books.AddRange(books);
        
            context.SaveChanges();
        }
    }
}
