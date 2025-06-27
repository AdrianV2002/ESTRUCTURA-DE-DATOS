using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("Introduce los números ganadores de la Lotería Primitiva (6 números del 1 al 49):");
        
        // Creamos una lista para almacenar los números
        List<int> numeros = new List<int>();
        
        // Pedimos 6 números al usuario
        for (int i = 1; i <= 6; i++)
        {
            Console.Write($"Número {i}: ");
            string entrada = Console.ReadLine();
            
            // Verificamos que sea un número válido
            if (int.TryParse(entrada, out int numero) && numero >= 1 && numero <= 49)
            {
                numeros.Add(numero);
            }
            else
            {
                Console.WriteLine("Por favor, introduce un número válido entre 1 y 49.");
                i--; // Repetimos esta iteración
            }
        }
        
        // Ordenamos los números de menor a mayor
        numeros.Sort();
        
        // Mostramos los números ordenados
        Console.WriteLine("\nNúmeros ganadores ordenados:");
        foreach (int num in numeros)
        {
            Console.Write(num + " ");
        }
        
        Console.WriteLine("\n\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}