using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // 1. Creamos una lista con las asignaturas
        List<string> materias = new List<string>();
        materias.Add("Matemáticas");
        materias.Add("Física");
        materias.Add("Química");
        materias.Add("Historia");
        materias.Add("Lengua");

        // 2. Mostramos cada asignatura con el mensaje
        Console.WriteLine("Mis asignaturas del curso:");
        
        // Recorremos la lista con un bucle foreach
        foreach (string asignatura in materias)
        {
            Console.WriteLine("Yo estudio " + asignatura);
        }

        // 3. Esperamos a que el usuario presione una tecla para cerrar
        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}