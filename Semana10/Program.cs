using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        // Crear los 500 ciudadanos
        List<string> todos = new List<string>();
        for (int i = 1; i <= 500; i++)
        {
            todos.Add($"Ciudadano {i}");
        }

        // Generar vacunados Pfizer y AstraZeneca (aleatoriamente, pero sin repetidos y cada uno con 75)
        Random random = new Random();

        HashSet<string> pfizer = new HashSet<string>();
        HashSet<string> astrazeneca = new HashSet<string>();

        while (pfizer.Count < 75)
            pfizer.Add(todos[random.Next(todos.Count)]);

        while (astrazeneca.Count < 75)
            astrazeneca.Add(todos[random.Next(todos.Count)]);

        // Operaciones de teoría de conjuntos

        // No vacunados = Todos - (Pfizer ∪ AstraZeneca)
        HashSet<string> noVacunados = new HashSet<string>(todos);
        noVacunados.ExceptWith(pfizer);
        noVacunados.ExceptWith(astrazeneca);

        // Ambas dosis = Pfizer ∩ AstraZeneca
        HashSet<string> ambasDosis = new HashSet<string>(pfizer);
        ambasDosis.IntersectWith(astrazeneca);

        // Solo Pfizer = Pfizer - AstraZeneca
        HashSet<string> soloPfizer = new HashSet<string>(pfizer);
        soloPfizer.ExceptWith(astrazeneca);

        // Solo AstraZeneca = AstraZeneca - Pfizer
        HashSet<string> soloAstrazeneca = new HashSet<string>(astrazeneca);
        soloAstrazeneca.ExceptWith(pfizer);

        // Mostrar resultados
        Console.WriteLine("=== Ciudadanos que NO se han vacunado ===\n\n");
        Console.WriteLine(string.Join(", ", noVacunados));

        Console.WriteLine("\n\n=== Ciudadanos con AMBAS dosis ===\n\n");
        Console.WriteLine(string.Join(", ", ambasDosis));

        Console.WriteLine("\n\n=== Ciudadanos con SOLO Pfizer ===\n\n");
        Console.WriteLine(string.Join(", ", soloPfizer));

        Console.WriteLine("\n\n=== Ciudadanos con SOLO AstraZeneca ===\n\n");
        Console.WriteLine(string.Join(", ", soloAstrazeneca));
    }
}
