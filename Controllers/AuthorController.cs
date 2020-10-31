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
    public class AuthorController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Author>>> Get([FromServices] DataContext context)
        {
            var authors = await context.Authors.AsNoTracking().ToListAsync();
            return authors;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Author>> GetById([FromServices] DataContext context, int id)
        {
            var author = await context.Authors.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
            return author;
        }

        [HttpPost]
        public async Task<ActionResult<Author>> Post([FromServices] DataContext context, [FromBody] Author model)
        {
            if(ModelState.IsValid)
            {
                var author = context.Authors.AsNoTracking().FirstOrDefaultAsync(f => f.Id == model.Id);

                if(author != null) context.Authors.Update(model);
                else context.Authors.Add(model);

                await context.SaveChangesAsync();
                return model;
            }

            return BadRequest(ModelState);
        }
    }
}
