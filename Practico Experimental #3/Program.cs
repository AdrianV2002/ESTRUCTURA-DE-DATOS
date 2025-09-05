using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BibliotecaApp
{
    // ----------------------------------------
    // Clase Book (modelo de datos)
    // ----------------------------------------
    public sealed class Book
    {
        public string Isbn { get; }
        public string Title { get; }
        public string Author { get; }
        public int Year { get; }
        public string Category { get; }

        public Book(string isbn, string title, string author, int year, string category)
        {
            if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("ISBN no puede estar vacío.", nameof(isbn));
            Isbn = isbn;
            Title = title ?? string.Empty;
            Author = author ?? string.Empty;
            Year = year;
            Category = category ?? string.Empty;
        }

        public override string ToString() => $"[{Isbn}] {Title} — {Author} ({Year}) | {Category}";
    }

    // ----------------------------------------
    // Clase Library (estructura con Dictionary y HashSet)
    // ----------------------------------------
    public class Library
    {
        private readonly Dictionary<string, Book> isbnToBook = new(StringComparer.Ordinal);
        private readonly Dictionary<string, HashSet<string>> authorToIsbns = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, HashSet<string>> categoryToIsbns = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> allIsbns = new(StringComparer.Ordinal);

        // Agrega un libro si el ISBN no existe. Devuelve true si se agregó.
        public bool AddBook(Book book)
        {
            if (book == null) throw new ArgumentNullException(nameof(book));
            if (allIsbns.Contains(book.Isbn)) return false;

            isbnToBook[book.Isbn] = book;
            allIsbns.Add(book.Isbn);

            if (!authorToIsbns.TryGetValue(book.Author, out var aSet))
            {
                aSet = new HashSet<string>(StringComparer.Ordinal);
                authorToIsbns[book.Author] = aSet;
            }
            aSet.Add(book.Isbn);

            if (!categoryToIsbns.TryGetValue(book.Category, out var cSet))
            {
                cSet = new HashSet<string>(StringComparer.Ordinal);
                categoryToIsbns[book.Category] = cSet;
            }
            cSet.Add(book.Isbn);

            return true;
        }

        // Elimina un libro por ISBN. Devuelve true si existía.
        public bool RemoveBook(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn)) return false;
            if (!isbnToBook.Remove(isbn, out var book)) return false;

            allIsbns.Remove(isbn);

            if (book.Author != null && authorToIsbns.TryGetValue(book.Author, out var aSet))
            {
                aSet.Remove(isbn);
                if (aSet.Count == 0) authorToIsbns.Remove(book.Author);
            }

            if (book.Category != null && categoryToIsbns.TryGetValue(book.Category, out var cSet))
            {
                cSet.Remove(isbn);
                if (cSet.Count == 0) categoryToIsbns.Remove(book.Category);
            }

            return true;
        }

        // Obtener por ISBN
        public Book? GetBook(string isbn)
            => string.IsNullOrWhiteSpace(isbn) ? null : (isbnToBook.TryGetValue(isbn, out var b) ? b : null);

        // Listar todos (ordenado por título)
        public IEnumerable<Book> ListBooks()
            => isbnToBook.Values.OrderBy(b => b.Title, StringComparer.OrdinalIgnoreCase);

        // Buscar por autor (exacto, insensible a mayúsculas)
        public IEnumerable<Book> FindByAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author)) return Enumerable.Empty<Book>();
            if (!authorToIsbns.TryGetValue(author, out var set)) return Enumerable.Empty<Book>();
            return set.Select(i => isbnToBook[i]);
        }

        // Buscar por categoría (exacto, insensible a mayúsculas)
        public IEnumerable<Book> FindByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category)) return Enumerable.Empty<Book>();
            if (!categoryToIsbns.TryGetValue(category, out var set)) return Enumerable.Empty<Book>();
            return set.Select(i => isbnToBook[i]);
        }

        // Buscar por texto en título (subcadena, insensible a mayúsculas)
        public IEnumerable<Book> SearchByTitle(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return Enumerable.Empty<Book>();
            var t = text.ToLowerInvariant();
            return isbnToBook.Values.Where(b => (b.Title ?? string.Empty).ToLowerInvariant().Contains(t));
        }

        public int Count() => allIsbns.Count;

        // Exporta a JSON y devuelve el texto (opcionalmente también puede guardarse en archivo)
        public string ExportJson()
        {
            var list = isbnToBook.Values.Select(b => new
            {
                isbn = b.Isbn,
                title = b.Title,
                author = b.Author,
                year = b.Year,
                category = b.Category
            }).ToList();

            var opts = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(list, opts);
        }

        // Guarda el JSON en ruta dada. Retorna true si escrito correctamente.
        public bool ExportJsonToFile(string path)
        {
            try
            {
                var json = ExportJson();
                File.WriteAllText(path, json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    // ----------------------------------------
    // Program (aplicación de consola)
    // ----------------------------------------
    public static class Program
    {
        public static void Main()
        {
            var lib = new Library();
            SeedSample(lib);

            while (true)
            {
                PrintHeader("Sistema de Registro de Libros — Conjuntos y Mapas");
                Console.WriteLine("1. Agregar libro");
                Console.WriteLine("2. Listar libros");
                Console.WriteLine("3. Buscar por autor");
                Console.WriteLine("4. Buscar por categoría");
                Console.WriteLine("5. Buscar por texto en título");
                Console.WriteLine("6. Eliminar libro");
                Console.WriteLine("7. Exportar a JSON");
                Console.WriteLine("8. Ejecutar benchmark");
                Console.WriteLine("9. Mostrar JSON en pantalla");
                Console.WriteLine("0. Salir");
                Console.Write("Selecciona una opción: ");
                var choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": AddBookFlow(lib); break;
                    case "2": ListBooksFlow(lib); break;
                    case "3": SearchByAuthorFlow(lib); break;
                    case "4": SearchByCategoryFlow(lib); break;
                    case "5": SearchByTitleFlow(lib); break;
                    case "6": RemoveBookFlow(lib); break;
                    case "7": ExportToFileFlow(lib); break;
                    case "8": RunBenchmark(); break;
                    case "9": ShowJsonFlow(lib); break;
                    case "0": Console.WriteLine("¡Hasta luego!"); return;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
        }

        // --- Flujos de UI ---
        private static void AddBookFlow(Library lib)
        {
            PrintHeader("Agregar libro");
            Console.Write("ISBN: "); var isbn = Console.ReadLine() ?? string.Empty;
            Console.Write("Título: "); var title = Console.ReadLine() ?? string.Empty;
            Console.Write("Autor: "); var author = Console.ReadLine() ?? string.Empty;
            Console.Write("Año (ej. 2022): ");
            var yearText = Console.ReadLine();
            if (!int.TryParse(yearText, out var year)) year = 0;
            Console.Write("Categoría: "); var category = Console.ReadLine() ?? string.Empty;

            try
            {
                var added = lib.AddBook(new Book(isbn, title, author, year, category));
                Console.WriteLine(added ? "     Libro agregado." : "     Ya existe un libro con ese ISBN.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private static void ListBooksFlow(Library lib)
        {
            PrintHeader($"Listado de libros (total: {lib.Count()})");
            foreach (var b in lib.ListBooks()) Console.WriteLine("- " + b);
        }

        private static void SearchByAuthorFlow(Library lib)
        {
            PrintHeader("Buscar por autor");
            Console.Write("Autor: "); var author = Console.ReadLine() ?? string.Empty;
            var res = lib.FindByAuthor(author).ToList();
            if (!res.Any()) Console.WriteLine("Sin resultados.");
            else res.ForEach(b => Console.WriteLine("- " + b));
        }

        private static void SearchByCategoryFlow(Library lib)
        {
            PrintHeader("Buscar por categoría");
            Console.Write("Categoría: "); var cat = Console.ReadLine() ?? string.Empty;
            var res = lib.FindByCategory(cat).ToList();
            if (!res.Any()) Console.WriteLine("Sin resultados.");
            else res.ForEach(b => Console.WriteLine("- " + b));
        }

        private static void SearchByTitleFlow(Library lib)
        {
            PrintHeader("Buscar por texto en título");
            Console.Write("Texto: "); var t = Console.ReadLine() ?? string.Empty;
            var res = lib.SearchByTitle(t).ToList();
            if (!res.Any()) Console.WriteLine("Sin resultados.");
            else res.ForEach(b => Console.WriteLine("- " + b));
        }

        private static void RemoveBookFlow(Library lib)
        {
            PrintHeader("Eliminar libro");
            Console.Write("ISBN a eliminar: "); var isbn = Console.ReadLine() ?? string.Empty;
            var ok = lib.RemoveBook(isbn);
            Console.WriteLine(ok ? "     Eliminado." : "     No existe ese ISBN.");
        }

        private static void ExportToFileFlow(Library lib)
        {
            PrintHeader("Exportar a JSON (archivo)");
            Console.Write("Ruta del archivo (ej. reporte_biblioteca.json): "); var path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path)) path = "reporte_biblioteca.json";
            var ok = lib.ExportJsonToFile(path);
            Console.WriteLine(ok ? $"     Exportado a {path}" : "     Error al escribir el archivo.");
        }

        private static void ShowJsonFlow(Library lib)
        {
            PrintHeader("JSON (vista rápida)");
            Console.WriteLine(lib.ExportJson());
        }

        // --- Benchmark simple ---
        private static void RunBenchmark()
        {
            PrintHeader("Benchmark (inserción, búsquedas y eliminaciones) — demo");
            const int n = 50_000;
            var lib = new Library();
            var sw = new Stopwatch();

            // Inserción
            sw.Restart();
            for (int i = 0; i < n; i++)
            {
                lib.AddBook(new Book($"ISBN{i:00000000}", $"Título {i}", $"Autor {i % 1000}", 2000 + (i % 25), $"Cat{i % 50}"));
            }
            sw.Stop();
            var insertSec = sw.Elapsed.TotalSeconds;

            // Búsquedas muestreadas
            sw.Restart();
            int hits = 0;
            var step = Math.Max(n / 1000, 1);
            for (int i = 0; i < n; i += step)
            {
                if (lib.GetBook($"ISBN{i:00000000}") != null) hits++;
            }
            sw.Stop();
            var lookupSec = sw.Elapsed.TotalSeconds;

            // Eliminaciones (la mitad)
            sw.Restart();
            for (int i = 0; i < n; i += 2)
            {
                lib.RemoveBook($"ISBN{i:00000000}");
            }
            sw.Stop();
            var removeSec = sw.Elapsed.TotalSeconds;

            Console.WriteLine($"n: {n}");
            Console.WriteLine($"insert_seconds: {insertSec:F4}");
            Console.WriteLine($"lookup_seconds: {lookupSec:F4}");
            Console.WriteLine($"remove_seconds: {removeSec:F4}");
            Console.WriteLine($"remaining: {lib.Count()}");
            Console.WriteLine($"hits: {hits}");
        }

        // --- Utilidades ---
        private static void SeedSample(Library lib)
        {
            var samples = new[]
            {
                new Book("9780001", "Estructuras de Datos con C#", "A. Turing", 2022, "Computación"),
                new Book("9780002", "Algoritmos Modernos", "G. Hopper", 2023, "Computación"),
                new Book("9780003", "Fundamentos de Redes", "L. Torvalds", 2021, "Redes"),
                new Book("9780004", "Bases de Datos Prácticas", "E. F. Codd", 2020, "Bases de Datos"),
                new Book("9780005", "Introducción a la IA", "A. Ng", 2024, "Inteligencia Artificial")
            };

            foreach (var b in samples) lib.AddBook(b);
        }

        private static void PrintHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine(new string('=', 70));
            Console.WriteLine(title);
            Console.WriteLine(new string('=', 70));
        }
    }
}
