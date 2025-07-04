using System;

namespace ListasConPromedio
{
    // Clase para el Nodo de la lista enlazada
    public class Nodo
    {
        public double Dato;
        public Nodo Siguiente;

        public Nodo(double dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }

    // Clase para la Lista Enlazada
    public class ListaEnlazada
    {
        private Nodo cabeza;

        // Agrega un nodo al final de la lista
        public void Agregar(double dato)
        {
            Nodo nuevoNodo = new Nodo(dato);

            if (cabeza == null)
            {
                cabeza = nuevoNodo;
            }
            else
            {
                Nodo actual = cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevoNodo;
            }
        }

        // Calcula el promedio de los datos en la lista
        public double CalcularPromedio()
        {
            if (cabeza == null) return 0; // Evita división por cero

            double suma = 0;
            int contador = 0;
            Nodo actual = cabeza;

            while (actual != null)
            {
                suma += actual.Dato;
                contador++;
                actual = actual.Siguiente;
            }

            return suma / contador;
        }

        // Filtra los datos según una condición (predicado)
        public ListaEnlazada Filtrar(Func<double, bool> condicion)
        {
            ListaEnlazada listaFiltrada = new ListaEnlazada();
            Nodo actual = cabeza;

            while (actual != null)
            {
                if (condicion(actual.Dato))
                {
                    listaFiltrada.Agregar(actual.Dato);
                }
                actual = actual.Siguiente;
            }

            return listaFiltrada;
        }

        // Imprime los datos de la lista
        public void Imprimir(string mensaje)
        {
            Console.Write(mensaje);
            Nodo actual = cabeza;

            while (actual != null)
            {
                Console.Write(actual.Dato + " ");
                actual = actual.Siguiente;
            }
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ListaEnlazada listaPrincipal = new ListaEnlazada();
            
            // Cargar datos
            listaPrincipal.Agregar(10.5);
            listaPrincipal.Agregar(20.3);
            listaPrincipal.Agregar(5.7);
            listaPrincipal.Agregar(30.2);
            listaPrincipal.Agregar(15.0);

            listaPrincipal.Imprimir("Datos en la lista principal: ");

            double promedio = listaPrincipal.CalcularPromedio();
            Console.WriteLine($"Promedio: {promedio}");

            ListaEnlazada menoresIguales = listaPrincipal.Filtrar(d => d <= promedio);
            menoresIguales.Imprimir("Datos menores o iguales al promedio: ");

            ListaEnlazada mayores = listaPrincipal.Filtrar(d => d > promedio);
            mayores.Imprimir("Datos mayores al promedio: ");
        }
    }
}