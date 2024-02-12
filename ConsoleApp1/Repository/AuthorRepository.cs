using ConsoleApp1.Data;
using ConsoleApp1.Interfaces;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ConsoleApp1.Repository
{
    public class AuthorRepository : IAuthor
    {
        public async Task AddAuthorAsync(Author author)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                context.Authors.Add(author);
                await context.SaveChangesAsync();   
            }
        }

        public async Task DeleteAuthorAsync(Author author)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                context.Authors.Remove(author);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            using (ApplicationContext context = Program.DbContext())
            {
                return await context.Authors.ToListAsync();
            }
        }

        public async Task<Author> GetAuthorAsync(ICollection<Author> authors)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                // Получаем идентификаторы всех авторов
                var authorIds = authors.Select(a => a.Id).ToList();

                // Ищем автора в базе данных по списку идентификаторов
                var author = await context.Authors
                    .Include(a => a.Books) // Включаем книги автора
                    .FirstOrDefaultAsync(a => authorIds.Contains(a.Id));

                return author;
            }
        }

        public async Task<Author> GetAuthorAsync(int id)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                return await context.Authors.FirstOrDefaultAsync(e => e.Id == id);
            }
        }

        public async Task<IEnumerable<Author>> GetAuthorsByNameAsync(string name)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                return await context.Authors.Where(e=>e.Name.Contains(name)).ToListAsync();
            }
        }

        public async Task<Author> GetAuthorWithBookAsync(int id)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                return await context.Authors.Include(e=>e.Books).FirstOrDefaultAsync(e => e.Id == id);
            }
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                context.Authors.Update(author);
                await context.SaveChangesAsync();
            }
        }
       
    }
}
