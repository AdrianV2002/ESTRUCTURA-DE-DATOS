using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Vectores iniciales
        List<int> v1 = new List<int> { 1, 2, 3 };
        List<int> v2 = new List<int> { -1, 0, 2 };

        try
        {
            int resultado = ProductoEscalar(v1, v2);
            Console.WriteLine($"El producto escalar es: {resultado}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static int ProductoEscalar(List<int> a, List<int> b)
    {
        if (a.Count != b.Count)
            throw new ArgumentException("Los vectores deben tener la misma dimensión");

        int suma = 0;
        for (int i = 0; i < a.Count; i++)
        {
            suma += a[i] * b[i];
        }
        return suma;
    }
}