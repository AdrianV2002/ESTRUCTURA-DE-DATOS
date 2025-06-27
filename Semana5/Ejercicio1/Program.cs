using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Crear lista de asignaturas
        List<string> asignaturas = new List<string> 
        { 
            "Matemáticas", 
            "Física", 
            "Química", 
            "Historia", 
            "Lengua" 
        };
        
        // Lista para almacenar las notas
        List<string> notas = new List<string>();
        
        // Pedir al usuario la nota para cada asignatura
        foreach (string asignatura in asignaturas)
        {
            Console.Write($"¿Qué nota has sacado en {asignatura}? ");
            string nota = Console.ReadLine();
            notas.Add(nota);
        }
        
        Console.WriteLine("\nResultados:");
        
        // Mostrar los resultados
        for (int i = 0; i < asignaturas.Count; i++)
        {
            Console.WriteLine($"En {asignaturas[i]} has sacado {notas[i]}");
        }
        
        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}