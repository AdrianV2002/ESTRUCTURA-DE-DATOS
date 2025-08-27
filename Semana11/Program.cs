using System;
using System.Collections.Generic;

namespace TraductorBasico
{
    class Traductor
    {
        private Dictionary<string, string> diccionario;

        public Traductor()
        {
            diccionario = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {"tiempo", "time"},
                {"persona", "person"},
                {"año", "year"},
                {"camino", "way"},
                {"día", "day"},
                {"cosa", "thing"},
                {"hombre", "man"},
                {"mundo", "world"},
                {"vida", "life"},
                {"mano", "hand"},
                {"parte", "part"},
                {"niño", "child"},
                {"ojo", "eye"},
                {"mujer", "woman"},
                {"lugar", "place"},
                {"trabajo", "work"},
                {"semana", "week"},
                {"caso", "case"},
                {"punto", "point"},
                {"gobierno", "government"},
                {"empresa", "company"}
            };
        }

        public string TraducirFrase(string frase)
        {
            string[] palabras = frase.Split(' ');
            for (int i = 0; i < palabras.Length; i++)
            {
                string limpia = palabras[i].Trim(',', '.', ';', ':', '!', '?');
                if (diccionario.ContainsKey(limpia.ToLower()))
                {
                    string traduccion = diccionario[limpia.ToLower()];
                    palabras[i] = palabras[i].Replace(limpia, traduccion);
                }
            }
            return string.Join(" ", palabras);
        }

        public void AgregarPalabra(string espanol, string ingles)
        {
            if (!diccionario.ContainsKey(espanol.ToLower()))
            {
                diccionario.Add(espanol.ToLower(), ingles.ToLower());
                Console.WriteLine($"Palabra agregada: {espanol} = {ingles}");
            }
            else
            {
                Console.WriteLine("Esa palabra ya existe en el diccionario.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Traductor traductor = new Traductor();
            int opcion;

            do
            {
                Console.Clear();
                Console.WriteLine("==================== MENÚ ====================");
                Console.WriteLine("1. Traducir una frase");
                Console.WriteLine("2. Agregar palabras al diccionario");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");
                
                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("Opción inválida. Presione una tecla...");
                    Console.ReadKey();
                    continue;
                }

                switch (opcion)
                {
                    case 1:
                        Console.Write("Ingrese una frase en español: ");
                        string frase = Console.ReadLine();
                        string traduccion = traductor.TraducirFrase(frase);
                        Console.WriteLine("\n🔹 Traducción: " + traduccion);
                        Console.WriteLine("\nPresione una tecla para continuar...");
                        Console.ReadKey();
                        break;

                    case 2:
                        Console.Write("Ingrese la palabra en español: ");
                        string espanol = Console.ReadLine();
                        Console.Write("Ingrese la traducción en inglés: ");
                        string ingles = Console.ReadLine();
                        traductor.AgregarPalabra(espanol, ingles);
                        Console.WriteLine("\nPresione una tecla para continuar...");
                        Console.ReadKey();
                        break;

                    case 0:
                        Console.WriteLine("Saliendo del programa...");
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }

            } while (opcion != 0);
        }
    }
}
