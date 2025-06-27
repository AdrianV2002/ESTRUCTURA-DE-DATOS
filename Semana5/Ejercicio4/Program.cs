using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // 1. Creamos una lista con todas las letras del abecedario
        List<char> abecedario = new List<char>();
        for (char letra = 'a'; letra <= 'z'; letra++)
        {
            abecedario.Add(letra);
        }

        Console.WriteLine("Abecedario completo:");
        MostrarLista(abecedario);

        // 2. Eliminamos las letras en posiciones múltiplos de 3
        // Nota: Trabajamos con índices empezando en 0
        for (int i = abecedario.Count - 1; i >= 0; i--)
        {
            // i+1 porque las posiciones humanas empiezan en 1
            if ((i + 1) % 3 == 0)
            {
                abecedario.RemoveAt(i);
            }
        }

        Console.WriteLine("\nAbecedario sin letras en posiciones múltiplo de 3:");
        MostrarLista(abecedario);

        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }

    // Método auxiliar para mostrar la lista
    static void MostrarLista(List<char> lista)
    {
        foreach (char letra in lista)
        {
            Console.Write(letra + " ");
        }
    }
}