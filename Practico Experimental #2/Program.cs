using System;
using System.Collections.Generic;

namespace ParqueDiversiones
{
    class Persona
    {
        public string Nombre { get; set; }
        public int NumeroAsiento { get; set; }

        public Persona(string nombre, int numeroAsiento)
        {
            Nombre = nombre;
            NumeroAsiento = numeroAsiento;
        }
    }

    class AsignacionAsientos
    {
        private Queue<Persona> cola = new Queue<Persona>();
        private int capacidad = 30;
        private int contadorAsientos = 0;

        public void LlegadaPersona(string nombre)
        {
            if (contadorAsientos < capacidad)
            {
                contadorAsientos++;
                Persona nuevaPersona = new Persona(nombre, contadorAsientos);
                cola.Enqueue(nuevaPersona);
                Console.WriteLine($"{nombre} ha llegado y se le asignó el asiento #{contadorAsientos}.");
            }
            else
            {
                Console.WriteLine($"No hay asientos disponibles para {nombre}. La atracción está llena.");
            }
        }

        public void MostrarCola()
        {
            Console.WriteLine("\nPersonas en la cola:");
            foreach (var persona in cola)
            {
                Console.WriteLine($"Asiento #{persona.NumeroAsiento}: {persona.Nombre}");
            }
        }

        public void SubirAtraccion()
        {
            Console.WriteLine("\nSubida de las personas en orden de llegada:");
            while (cola.Count > 0)
            {
                Persona persona = cola.Dequeue();
                Console.WriteLine($"{persona.Nombre} ocupa el asiento #{persona.NumeroAsiento}.");
            }
            Console.WriteLine("\nTodos los asientos han sido ocupados.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            AsignacionAsientos asignacion = new AsignacionAsientos();
            Console.WriteLine("Bienvenido al sistema de asignación de asientos (30 asientos)");

            while (true)
            {
                Console.WriteLine("\nSeleccione una opción:");
                Console.WriteLine("1. Llegada de persona");
                Console.WriteLine("2. Mostrar cola actual");
                Console.WriteLine("3. Subir a la atracción");
                Console.WriteLine("4. Salir");
                Console.Write("Opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.Write("Ingrese el nombre de la persona: ");
                        string nombre = Console.ReadLine();
                        asignacion.LlegadaPersona(nombre);
                        break;
                    case "2":
                        asignacion.MostrarCola();
                        break;
                    case "3":
                        asignacion.SubirAtraccion();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Opción no válida, intente de nuevo.");
                        break;
                }
            }
        }
    }
}
