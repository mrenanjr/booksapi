using System;
using System.Collections.Generic;
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

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Book>> DeleteById([FromServices] DataContext context, int id)
        {
            context.Books.Remove(new Book() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult<Book>> Post([FromServices] DataContext context, [FromBody] Book model)
        {
            if(ModelState.IsValid)
            {
                var book = await context.Books.AsNoTracking().FirstOrDefaultAsync(f => f.Id == model.Id);

                if(book != null) context.Books.Update(model);
                else {
                    book = await context.Books.AsNoTracking().FirstOrDefaultAsync(f =>
                        f.Title.Equals(model.Title) &&
                        f.Year.Equals(model.Year) &&
                        f.AuthorId.Equals(model.AuthorId)
                    );

                    if(book != null) return StatusCode(403, "Dados já existem na base!");

                    context.Books.Add(model);
                }

                await context.SaveChangesAsync();
                return model;
            }

            return BadRequest(ModelState);
        }
    }
}
