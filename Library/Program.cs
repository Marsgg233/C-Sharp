using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public string ISBN { get; set; }

    public Book(string title, string author, int year, string isbn)
    {
        Title = title;
        Author = author;
        Year = year;
        ISBN = isbn;
    }

    public override string ToString()
    {
        return $"Название: {Title}, Автор: {Author}, Год: {Year}, ISBN: {ISBN}";
    }
}

class Library
{
    private List<Book> books = new List<Book>();
    private const string DataFilePath = "library_data.json";

    public Library()
    {
        LoadData();
    }

    public void AddBook(Book book)
    {
        if (books.Any(b => b.ISBN == book.ISBN))
        {
            Console.WriteLine("Книга с таким ISBN уже есть ");
            return;
        }
        books.Add(book);
        Console.WriteLine("Книга добавлена");
        SaveBooks();
    }

    public void RemoveBook(string isbn)
    {
        var bookToRemove = books.FirstOrDefault(b => b.ISBN == isbn);
        if (bookToRemove != null)
        {
            books.Remove(bookToRemove);
            Console.WriteLine("Книга удалена ");
            SaveBooks();
        }
        else
        {
            Console.WriteLine("Книга с таким ISBN нет ");
        }
    }

    public List<Book> FindBooksByTitle(string title)
    {
        return books.Where(b => b.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
    }

    public void DisplayAllBooks()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("В библиотеке нет книг ");
            return;
        }

        Console.WriteLine("Список книг: ");
        foreach (var book in books)
        {
            Console.WriteLine(book);
        }
    }

    public void SaveBooks()
    {
        try
        {
            string json = JsonConvert.SerializeObject(books, Formatting.Indented);
            File.WriteAllText(DataFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Пупупу чето не сохранилось: {ex.Message}");
        }
    }

    private void LoadData()
    {
        if (File.Exists(DataFilePath))
        {
            try
            {
                string json = File.ReadAllText(DataFilePath);
                books = JsonConvert.DeserializeObject<List<Book>>(json) ?? new List<Book>();
                Console.WriteLine("Ура книги загруженны");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Пупупу чето не получилось загрузить: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Файл с книгами не обнаружен Ошибка Ошибка ПЕРЕЗАГРУЗИТЬЬЬЬ СИСТЕ М У ....");
        }
    }
}

class Program
{
    static void Main()
    {
        Library library = new Library();

        while (true)
        {
            DisplayMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddNewBook(library);
                    break;
                case "2":
                    RemoveBook(library);
                    break;
                case "3":
                    FindBooks(library);
                    break;
                case "4":
                    library.DisplayAllBooks();
                    break;
                case "5":
                    library.SaveBooks();
                    Console.WriteLine("До встречи мой дорогой читатель");
                    return;
                default:
                    Console.WriteLine("Неправильно, попробуй еще разок");
                    break;
            }

            Console.WriteLine("\nНажми куда-нибудь уже, что бы продолжить . . . ");
            Console.ReadKey();
        }
    }

    static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("(((✞ Моя Библиотека ✞)))");
        Console.WriteLine("1) Добавить книгу");
        Console.WriteLine("2) Удаление книг по ISBN");
        Console.WriteLine("3) Поиск книг по названию");
        Console.WriteLine("4) Показать твои книги");
        Console.WriteLine("5) До скорых встреч (Exit)");
        Console.Write("Введите номер действия: ");
    }

    static void AddNewBook(Library library)
    {
        Console.Write("Введите название: ");
        string title = Console.ReadLine();

        Console.Write("Введите автора: ");
        string author = Console.ReadLine();

        int year;
        while (true)
        {
            Console.Write("Введите год: ");
            if (int.TryParse(Console.ReadLine(), out year))
            {
                break;
            }
            Console.WriteLine("Неправильно - введите целое число");
        }

        Console.Write("Введите ISBN: ");
        string isbn = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(isbn))
        {
            Console.WriteLine("Неправильно - заполниет все поля");
            return;
        }

        Book newBook = new Book(title, author, year, isbn);
        library.AddBook(newBook);
    }

    static void RemoveBook(Library library)
    {
        Console.Write("Введите ISBN для удаления книги из библиотеки: ");
        string isbn = Console.ReadLine();
        library.RemoveBook(isbn);
    }

    static void FindBooks(Library library)
    {
        Console.Write("Введите название: ");
        string title = Console.ReadLine();

        var foundBooks = library.FindBooksByTitle(title);
        if (foundBooks.Count == 0)
        {
            Console.WriteLine("Я не нашел книг с таким названием");
        }
        else
        {
            Console.WriteLine("Книги, которые я нашел:");
            foreach (var book in foundBooks)
            {
                Console.WriteLine(book);
            }
        }
    }
}