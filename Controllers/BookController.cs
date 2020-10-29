using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using booksapi.Data;
using booksapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace booksapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Book>>> Get([FromServices] DataContext context)
        {
            var books = await context.Books.Include(entity => entity.Author).AsNoTracking().ToListAsync();
            return books;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Book>> GetById([FromServices] DataContext context, int id)
        {
            var book = await context.Books.Include(entity => entity.Author).AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
            return book;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Post([FromServices] DataContext context, [FromBody] Book model)
        {
            if(ModelState.IsValid)
            {
                context.Books.Add(model);
                await context.SaveChangesAsync();
                return model;
            }

            return BadRequest(ModelState);
        }
    }
}
