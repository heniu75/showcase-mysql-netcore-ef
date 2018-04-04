using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;

namespace MySqlEfCoreConsole
{
    // see https://www.learnentityframeworkcore.com/

    // see example      - https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core-example.html
    // see ef core docs - https://docs.microsoft.com/en-us/ef/core/

    // ef core Package Manager Console (PMC), dont use CLI (as at 04/04)
    // https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell

    //  current orale mysql ef provider (as at 04/04) does not fully support ef core migrations
    // therefore am using https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql

    class Program
    {
        static void Main(string[] args)
        {
            EnsureCreated();
            DeleteData();
            InsertData();
            PrintData();
        }

        private static void EnsureCreated()
        {
            using (var context = new LibraryContext())
            {
                // Creates the database if not exists ... DONT USE THIS WHEN USING MIGRATIONS!
                //context.Database.EnsureCreated();
                context.Database.Migrate();
            }
        }

        private static void DeleteData()
        {
            using (var context = new LibraryContext())
            {
                var meta = context.MetaData.FirstOrDefault();
                if (meta != null)
                {
                    if (meta.DataSeeded)
                    {
                        var lord = context.Book.Where(b => b.ISBN == "978-0544003415").FirstOrDefault();
                        if (lord != null)
                            context.Book.Remove(lord);
                        var letter = context.Book.Where(b => b.ISBN == "978-0547247762").FirstOrDefault();
                        if (letter != null)
                            context.Book.Remove(letter);
                        var mariner = context.Publisher.Where(m => m.Name == "Mariner Books").FirstOrDefault();
                        if (mariner != null)
                            context.Publisher.Remove(mariner);

                        meta.DataSeeded = false;
                        meta.StatusAt = DateTime.Now;

                        // Saves changes
                        context.SaveChanges();
                    }
                }
            }
        }

        private static void InsertData()
        {
            using (var context = new LibraryContext())
            {
                var meta = context.MetaData.FirstOrDefault();
                if ((meta == null) || (!meta.DataSeeded))
                {
                    // Adds a publisher
                    var publisher = new Publisher { Name = "Mariner Books" };
                    context.Publisher.Add(publisher);

                    // Adds some books
                    context.Book.Add(new Book
                    {
                        ISBN = "978-0544003415",
                        Title = "The Lord of the Rings",
                        Author = "J.R.R. Tolkien",
                        Language = "English",
                        Pages = 1216,
                        Publisher = publisher
                    });
                    context.Book.Add(new Book
                    {
                        ISBN = "978-0547247762",
                        Title = "The Sealed Letter",
                        Author = "Emma Donoghue",
                        Language = "English",
                        Pages = 416,
                        Publisher = publisher
                    });

                    if (meta == null)
                    {
                        meta = new MetaData();
                        context.MetaData.Add(meta);
                    }

                    meta.DataSeeded = true;
                    meta.StatusAt = DateTime.Now;

                    // Saves changes
                    context.SaveChanges();
                }
            }
        }

        private static void PrintData()
        {
            // Gets and prints all books in database
            using (var context = new LibraryContext())
            {
                var books = context.Book
                  .Include(p => p.Publisher);
                foreach (var book in books)
                {
                    var data = new StringBuilder();
                    data.AppendLine($"ISBN: {book.ISBN}");
                    data.AppendLine($"Title: {book.Title}");
                    data.AppendLine($"Publisher: {book.Publisher.Name}");
                    Console.WriteLine(data.ToString());
                }
            }
        }
    }
}