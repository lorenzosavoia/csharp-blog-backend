using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using csharp_blog_backend.Models;

namespace csharp_blog_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly BlogContext _context;

        public PostsController(BlogContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts(string? stringa)
        {
          if (_context.Posts == null)
          {
              return NotFound();
          }
            if (stringa != null) { return await _context.Posts.Where(m => m.Title.Contains(stringa) || m.Description.Contains(stringa)).ToListAsync(); }

            return await _context.Posts.ToListAsync();

        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
          if (_context.Posts == null)
          {
              return NotFound();
          }
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            /*string fileName = "immagine-" + post.Id + "." + post.Image.Substring("FileLocal;".Length);

            string Image = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");

            //string fileNameWithPath = Path.Combine(Image, fileName);
            post.Image = "https://localhost:5000/Files/" + fileName;*/

            return post;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost([FromForm] Post post)
        {
            FileInfo fileInfo = new FileInfo(post.File.FileName);
            // post.Image = $"FileLocal{fileInfo.Extension}"; //questo é quello che viene salvato nel Db
            
            Guid guid = Guid.NewGuid();//instanzio la criptografia di microsoft

            string fileName = guid.ToString() + fileInfo.Extension;

            
            //await _context.SaveChangesAsync();

            //Estrazione File e salvataggio su file system.
            //Agendo su Request ci prendiamo il file e lo salviamo su file system.

            string Image = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");
            if (!Directory.Exists(Image))
                Directory.CreateDirectory(Image);

            string fileNameWithPath = Path.Combine(Image, fileName);

            //implementazione blol inizio

            using (BinaryReader br = new BinaryReader(post.File.OpenReadStream()))
            {
                post.ImageBytes = br.ReadBytes((int)post.File.OpenReadStream().Length);
            }

            //implementazione blol inizio

            // inizio stringa normale
             using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
             {
                 post.File.CopyTo(stream);
             }
             // fine stringa normale


            //controllo che non serve perché siamo sicuri esista la tabella post
            /*if (_context.Posts == null)
                return Problem("Entity set 'BlogContext.Posts'  is null.");*/

            post.Image = "https://localhost:5000/Files/" + fileName;

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();// salviamo le modifiche

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
