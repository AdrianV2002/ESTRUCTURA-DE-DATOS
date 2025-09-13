using System;
using System.Collections.Generic;

namespace CatalogoRevistas
{
    class Program
    {
        // Estructura de datos para almacenar el catálogo de revistas
        private static List<string> catalogoRevistas = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DE GESTIÓN DE CATÁLOGO DE REVISTAS ===");
            
            // Inicializar el catálogo con al menos 10 revistas
            InicializarCatalogo();
            
            bool continuar = true;
            
            while (continuar)
            {
                MostrarMenu();
                string opcion = Console.ReadLine();
                
                switch (opcion)
                {
                    case "1":
                        BuscarRevista();
                        break;
                    case "2":
                        MostrarCatalogo();
                        break;
                    case "3":
                        AgregarRevista();
                        break;
                    case "4":
                        Console.WriteLine("¡Gracias por usar el sistema! Hasta pronto.");
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Por favor, seleccione una opción del 1 al 4.");
                        break;
                }
                
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        }
        
        /// <summary>
        /// Inicializa el catálogo con al menos 10 revistas de ejemplo
        /// </summary>
        private static void InicializarCatalogo()
        {
            catalogoRevistas.Add("National Geographic");
            catalogoRevistas.Add("Economia");
            catalogoRevistas.Add("Naturaleza");
            catalogoRevistas.Add("Personas");
            catalogoRevistas.Add("Ciencia Hoy");
            catalogoRevistas.Add("Viajes");
            catalogoRevistas.Add("Tecnología");
            catalogoRevistas.Add("Historia");
            catalogoRevistas.Add("Arte y Cultura");
            catalogoRevistas.Add("Deportes");
            
            // Ordenar el catálogo para facilitar la búsqueda binaria.
            catalogoRevistas.Sort();
            
            Console.WriteLine("Catálogo inicializado con " + catalogoRevistas.Count + " revistas.");
        }
        
        /// <summary>
        /// Muestra el menú principal de opciones
        /// </summary>
        private static void MostrarMenu()
        {
            Console.WriteLine("\n=== MENÚ PRINCIPAL ===");
            Console.WriteLine("1. Buscar revista por título");
            Console.WriteLine("2. Mostrar catálogo completo");
            Console.WriteLine("3. Agregar nueva revista");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");
        }
        
        /// <summary>
        /// Permite al usuario buscar una revista en el catálogo
        /// </summary>
        private static void BuscarRevista()
        {
            Console.WriteLine("\n=== BÚSQUEDA DE REVISTA ===");
            Console.Write("Ingrese el título de la revista a buscar: ");
            string tituloBuscado = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(tituloBuscado))
            {
                Console.WriteLine("Error: Debe ingresar un título válido.");
                return;
            }
            
            // Realizar búsqueda binaria iterativa (más eficiente para listas ordenadas)
            bool encontrado = BusquedaBinariaIterativa(tituloBuscado);
            
            // Alternativa: búsqueda lineal recursiva (comentada)
            // bool encontrado = BusquedaLinealRecursiva(tituloBuscado, 0);
            
            // Mostrar resultado
            if (encontrado)
            {
                Console.WriteLine($"Resultado: ENCONTRADO - '{tituloBuscado}' está en el catálogo.");
            }
            else
            {
                Console.WriteLine($"Resultado: NO ENCONTRADO - '{tituloBuscado}' no está en el catálogo.");
            }
        }
        
        /// <summary>
        /// Implementación de búsqueda binaria iterativa
        /// Más eficiente para listas ordenadas - Complejidad O(log n)
        /// </summary>
        /// <param name="titulo">Título a buscar</param>
        /// <returns>True si se encuentra, False en caso contrario</returns>
        private static bool BusquedaBinariaIterativa(string titulo)
        {
            int izquierda = 0;
            int derecha = catalogoRevistas.Count - 1;
            
            while (izquierda <= derecha)
            {
                int medio = (izquierda + derecha) / 2;
                int comparacion = string.Compare(catalogoRevistas[medio], titulo, StringComparison.OrdinalIgnoreCase);
                
                if (comparacion == 0)
                {
                    return true; // Encontrado
                }
                else if (comparacion < 0)
                {
                    izquierda = medio + 1; // Buscar en la mitad derecha
                }
                else
                {
                    derecha = medio - 1; // Buscar en la mitad izquierda
                }
            }
            
            return false; // No encontrado
        }
        
        /// <summary>
        /// Implementación de búsqueda lineal recursiva
        /// Alternativa menos eficiente pero que demuestra recursividad - Complejidad O(n)
        /// </summary>
        /// <param name="titulo">Título a buscar</param>
        /// <param name="indice">Índice actual para la búsqueda recursiva</param>
        /// <returns>True si se encuentra, False en caso contrario</returns>
        private static bool BusquedaLinealRecursiva(string titulo, int indice)
        {
            // Caso base: llegamos al final de la lista
            if (indice >= catalogoRevistas.Count)
            {
                return false;
            }
            
            // Caso base: encontramos el título
            if (string.Equals(catalogoRevistas[indice], titulo, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            
            // Llamada recursiva: buscar en el siguiente elemento
            return BusquedaLinealRecursiva(titulo, indice + 1);
        }
        
        /// <summary>
        /// Muestra todo el catálogo de revistas
        /// </summary>
        private static void MostrarCatalogo()
        {
            Console.WriteLine("\n=== CATÁLOGO COMPLETO DE REVISTAS ===");
            
            if (catalogoRevistas.Count == 0)
            {
                Console.WriteLine("El catálogo está vacío.");
                return;
            }
            
            for (int i = 0; i < catalogoRevistas.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {catalogoRevistas[i]}");
            }
            
            Console.WriteLine($"\nTotal de revistas: {catalogoRevistas.Count}");
        }
        
        /// <summary>
        /// Permite al usuario agregar una nueva revista al catálogo
        /// </summary>
        private static void AgregarRevista()
        {
            Console.WriteLine("\n=== AGREGAR NUEVA REVISTA ===");
            Console.Write("Ingrese el título de la nueva revista: ");
            string nuevoTitulo = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(nuevoTitulo))
            {
                Console.WriteLine("Error: El título no puede estar vacío.");
                return;
            }
            
            // Verificar si ya existe
            if (BusquedaBinariaIterativa(nuevoTitulo))
            {
                Console.WriteLine("Error: Esta revista ya existe en el catálogo.");
                return;
            }
            
            // Agregar y mantener ordenado
            catalogoRevistas.Add(nuevoTitulo);
            catalogoRevistas.Sort();
            
            Console.WriteLine($"Revista '{nuevoTitulo}' agregada exitosamente al catálogo.");
        }
    }
}