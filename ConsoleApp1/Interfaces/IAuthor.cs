using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Interfaces
{
    public interface IAuthor
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        Task<Author> GetAuthorWithBookAsync(int id);
        Task<Author> GetAuthorAsync(ICollection<Author> authors);
        Task<IEnumerable<Author>> GetAuthorsByNameAsync(string name);

        Task AddAuthorAsync(Author author);
        Task DeleteAuthorAsync(Author author);
        Task UpdateAuthorAsync(Author author);
        Task<Author> GetAuthorAsync(int result);
    }
}
