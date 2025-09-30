using System;
using System.IO;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Estructuras: Árboles y Grafos - Implementación C#");
        Console.WriteLine("Opciones:");
        Console.WriteLine("1 - Cargar grafo desde data/ejemplo_grafo1.txt");
        Console.WriteLine("2 - Cargar grafo desde data/ejemplo_grafo2.txt");
        Console.WriteLine("3 - Cargar árbol desde data/ejemplo_arbol1.txt");
        Console.Write("Selecciona opción: ");
        var opt = Console.ReadLine();

        Stopwatch sw = new Stopwatch();
        sw.Start();

        if (opt == "1" || opt == "2")
        {
            string filename = opt == "1" ? "data/ejemplo_grafo1.txt" : "data/ejemplo_grafo2.txt";
            var g = Graph.LoadFromFile(filename);
            Console.WriteLine($"Grafo cargado desde {filename}");
            g.PrintSummary();
            Console.WriteLine("Grados (degree centrality):");
            foreach (var kv in g.DegreeCentrality())
                Console.WriteLine($"Nodo {kv.Key} => grado {kv.Value}");
            g.ExportDot("outputs/grafo_export.dot");
            Console.WriteLine("DOT exportado a outputs/grafo_export.dot (usa Graphviz para generar imagen)");
            Console.WriteLine("Recorrido BFS desde primer nodo:");
            var first = g.FirstNode();
            var bfs = g.BFS(first);
            Console.WriteLine(string.Join(" -> ", bfs));
        }
        else if (opt == "3")
        {
            string filename = "data/ejemplo_arbol1.txt";
            var t = Tree.LoadFromFile(filename);
            Console.WriteLine($"Árbol cargado desde {filename}");
            t.PrintSummary();
            t.ExportDot("outputs/arbol_export.dot");
            Console.WriteLine("DOT exportado a outputs/arbol_export.dot (usa Graphviz para generar imagen)");
            Console.WriteLine("Recorridos:");
            Console.WriteLine("PreOrder: " + string.Join(" ", t.PreOrder()));
            Console.WriteLine("InOrder:  " + string.Join(" ", t.InOrder()));
            Console.WriteLine("PostOrder:" + string.Join(" ", t.PostOrder()));
        }
        else
        {
            Console.WriteLine("Opción no válida.");
        }

        sw.Stop();
        Console.WriteLine($"Tiempo total ejecución: {sw.ElapsedMilliseconds} ms");
        Console.WriteLine("Fin.");
    }
}
