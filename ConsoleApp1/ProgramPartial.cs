using ConsoleApp1.Helpers;
using ConsoleApp1.Interfaces;
using ConsoleApp1.Models;
using ConsoleApp1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public partial class Program
    {
        //Authors
        static async Task ReviewAuthors()
        {
            var allAuthors = await _authors.GetAllAuthorsAsync();
            var authors = allAuthors.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
            int result = ItemsHelper.MultipleChoice(true, authors, true);
            if (result != 0)
            {
                var currentAuthor = await _authors.GetAuthorAsync(result);
                await AuthorInfo(currentAuthor);
            }
        }
        static async Task AuthorInfo(Author currentAuthor)
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
                {
                 new ItemView { Id = 1, Value = "Browse books"},
                 new ItemView { Id = 2, Value = "Edit author"},
                 new ItemView { Id = 3, Value = "Delete author"},
                },
                IsMenu: true, message: String.Format("{0}\n", currentAuthor), startY: 5, optionsPerLine: 1);

            switch (result)
            {
                case 1:
                    {
                        //<---------- Вызов метода с выводом книг данного автора
                        break;
                    }
                case 2:
                    {
                        await EditAuthor(currentAuthor);
                        Console.ReadLine();
                        break;
                    }
                case 3:
                    {
                        await RemoveAuthor(currentAuthor);
                        Console.ReadLine();
                        break;
                    }
            }
            await ReviewAuthors();
        }
        static async Task AddAuthor()
        {
            string authorName = InputHelper.GetString("author 'Name' with 'Surname'");
            await _authors.AddAuthorAsync(new Author
            {
                Name = authorName
            });
            Console.WriteLine("Author successfully added.");
        }
        static async Task EditAuthor(Author currentAuthor)
        {
            Console.WriteLine("Changing: {0}", currentAuthor.Name);
            currentAuthor.Name = InputHelper.GetString("author 'Name' with 'SurName'");
            await _authors.UpdateAuthorAsync(currentAuthor);
            Console.WriteLine("Author successfully changed.");
        }
        static async Task RemoveAuthor(Author currentAuthor)
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
                {
                 new ItemView { Id = 1, Value = "Yes"},
                 new ItemView { Id = 0, Value = "No"},
                }, message: String.Format("[Are you sure you want to delete the author {0} ?]\n", currentAuthor.Name), startY: 2);

            if (result == 1)
            {
                await _authors.DeleteAuthorAsync(currentAuthor);
                Console.WriteLine("The author has been successfully deleted.");
            }
            else
            {
                Console.WriteLine("Press any key to continue...");
            }
        }
        static async Task SearchAuthors()
        {
            string authorName = InputHelper.GetString("author name or surname");
            var currentAuthors = await _authors.GetAuthorsByNameAsync(authorName);
            if (currentAuthors.Count() > 0)
            {
                var authors = currentAuthors.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
                int result = ItemsHelper.MultipleChoice(true, authors, true);
                if (result != 0)
                {
                    var currentAuthor = await _authors.GetAuthorAsync(result);
                    await AuthorInfo(currentAuthor);
                }
            }
            else
            {
                Console.WriteLine("No authors were found by this attribute.");
            }
        }

        //Categories
        static async Task ReviewCategories()
        {
            var allCategories = await _categories.GetAllCategoriesAsync();
            var categories = allCategories.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
            int result = ItemsHelper.MultipleChoice(true, categories, true);
            if (result != 0)
            {
                var currentCategory = await _categories.GetCategoryAsync(result);
                await CategoryInfo(currentCategory);
            }
        }

        static async Task CategoryInfo(Category currentCategory)
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
    {
        new ItemView { Id = 1, Value = "Browse books in this category"},
        new ItemView { Id = 2, Value = "Edit category"},
        new ItemView { Id = 3, Value = "Delete category"},
    },
            IsMenu: true, message: String.Format("{0}\n", currentCategory), startY: 5, optionsPerLine: 1);

            switch (result)
            {
                case 1:
                    {
                        
                        await BrowseBooksInCategory(currentCategory);
                        break;
                    }
                case 2:
                    {
                        await EditCategory(currentCategory);
                        Console.ReadLine();
                        break;
                    }
                case 3:
                    {
                        await RemoveCategory(currentCategory);
                        Console.ReadLine();
                        break;
                    }
            }
            await ReviewCategories();
        }

        static async Task BrowseBooksInCategory(Category currentCategory)
        {
           
            var categoryWithBooks = await _categories.GetCategoryWithBooksAsync(currentCategory.Id);
            foreach (var book in categoryWithBooks.Books)
            {
                Console.WriteLine(book.ToString());
                Console.WriteLine();
            }
        }

        static async Task AddCategory()
        {
            string categoryName = InputHelper.GetString("Enter category name");
            string categoryDescription = InputHelper.GetString("Enter category description"); 
            await _categories.AddCategoryAsync(new Category
            {
                Name = categoryName,
                Description = categoryDescription 
            });
            Console.WriteLine("Category successfully added.");
        }

        static async Task EditCategory(Category currentCategory)
        {
            Console.WriteLine("Changing: {0}", currentCategory.Name);
            string newCategoryName = InputHelper.GetString("Enter new category name");

            currentCategory.Name = newCategoryName;

            await _categories.UpdateCategoryAsync(currentCategory);
            Console.WriteLine("Category successfully changed.");
        }

        static async Task RemoveCategory(Category currentCategory)
        {
            int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
    {
        new ItemView { Id = 1, Value = "Yes"},
        new ItemView { Id = 0, Value = "No"},
    }, message: String.Format("[Are you sure you want to delete the category {0} ?]\n", currentCategory.Name), startY: 2);

            if (result == 1)
            {
                await _categories.DeleteCategoryAsync(currentCategory);
                Console.WriteLine("The category has been successfully deleted.");
            }
            else
            {
                Console.WriteLine("Press any key to continue...");
            }
        }
        //Books
        static async Task AddBook()
        {
            string bookTitle = InputHelper.GetString("Enter book title");
            string bookDescription = InputHelper.GetString("Enter book description");
            DateTime publishedDate = InputHelper.GetDateTime("Enter published date");
            string publisher = InputHelper.GetString("Enter publisher");
            decimal price = InputHelper.GetDecimal("Enter price");
            var allCategories = await _categories.GetAllCategoriesAsync();
            var categories = allCategories.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
            int categoryId = ItemsHelper.MultipleChoice(true, categories, true);

            
            var selectedCategory = await _categories.GetCategoryAsync(categoryId);

            
            var newBook = new Book
            {
                Title = bookTitle,
                Description = bookDescription,
                PublishedOn = publishedDate,
                Publisher = publisher,
                Price = price,
                CategoryId = selectedCategory.Id, 
                Category = selectedCategory
            };

            
            await _books.AddBookAsync(newBook);
            Console.WriteLine("Book successfully added.");
        }
        static async Task ReviewBooks()
        {
            var allBooks = await _books.GetAllBooksAsync();

            if (allBooks.Any())
            {
                Console.WriteLine("Books available in the store:\n");

                foreach (var book in allBooks)
                {
                    var author = await _authors.GetAuthorAsync(book.Authors);
                    Console.WriteLine($"Title: {book.Title}");
                    Console.WriteLine($"Author: {author.Name}");
                    Console.WriteLine($"Price: {book.Price}\n");
                }
            }
            else
            {
                Console.WriteLine("No books available in the store.");
            }
        }
        //Reviews
        static async Task AddReview(Book currentBook)
        {
            string reviewContent = InputHelper.GetString("Enter review content");
            int rating = InputHelper.GetInt("Enter rating");

            
            var newReview = new Review
            {
                Content = reviewContent,
                Rating = rating,
                BookId = currentBook.Id, 
                Book = currentBook
            };

            
            await _review.AddReviewAsync(newReview);
            Console.WriteLine("Review successfully added.");
        }
        //Orders
        static async Task AddOrder()
        {
            string customerName = InputHelper.GetString("Enter customer name");
            DateTime orderDate = InputHelper.GetDateTime("Enter order date");

            var allBooks = await _books.GetAllBooksAsync();
            var books = allBooks.Select(e => new ItemView { Id = e.Id, Value = e.Title }).ToList();
            int bookId = ItemsHelper.MultipleChoice(true, books, true);
            var selectedBook = await _books.GetBookAsync(bookId);
            var newOrder = new Order
            {
                CustomerName = customerName,
                OrderDate = orderDate,
                BookId = selectedBook.Id, 
                
            };

            await _order.AddOrderAsync(newOrder);
            Console.WriteLine("Order successfully added.");
        }
        static async Task SearchOrders()
        {
            string searchCriteria = InputHelper.GetString("Enter search criteria for orders");

            var orders = await _order.SearchOrdersAsync(searchCriteria);

            if (orders.Any())
            {
                Console.WriteLine("Found orders matching the search criteria:\n");

               
            }
            else
            {
                Console.WriteLine("No orders found matching the search criteria.");
            }
        }
        static async Task ReviewOrders()
        {
            var allOrders = await _order.GetAllOrdersAsync();

            if (allOrders.Any())
            {
                Console.WriteLine("All Orders:\n");

                foreach (var order in allOrders)
                {
                    Console.WriteLine(order.ToString());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No orders available.");
            }
        }

        static async Task SearchBooks()
        {
            string searchTerm = InputHelper.GetString("Enter search term for books:");

            var foundBooks = await _books.SearchBooksAsync(searchTerm);

            if (foundBooks.Any())
            {
                Console.WriteLine("Found Books:\n");

                foreach (var book in foundBooks)
                {
                    Console.WriteLine(book.ToString());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"No books found with the search term '{searchTerm}'.");
            }
        }

        static async Task SearchCategories()
        {
            string searchTerm = InputHelper.GetString("Enter search term for categories:");

            var foundCategories = await _categories.SearchCategoriesAsync(searchTerm);

            if (foundCategories.Any())
            {
                Console.WriteLine("Found Categories:\n");

                foreach (var category in foundCategories)
                {
                    Console.WriteLine(category.ToString());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"No categories found with the search term '{searchTerm}'.");
            }
        }
    }
}
    

