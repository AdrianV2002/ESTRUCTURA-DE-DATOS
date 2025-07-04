using System;

namespace InvertirListaEnlazada
{
    // Clase que representa un Nodo de la lista
    public class Nodo
    {
        public int Valor;
        public Nodo Siguiente;

        public Nodo(int valor)
        {
            Valor = valor;
            Siguiente = null;
        }
    }

    // Clase que representa la Lista Enlazada
    public class ListaEnlazada
    {
        private Nodo cabeza;

        // Método para agregar un nodo al final de la lista
        public void AgregarAlFinal(int valor)
        {
            Nodo nuevoNodo = new Nodo(valor);

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

        // Método para INVERTIR la lista enlazada
        public void Invertir()
        {
            Nodo anterior = null;
            Nodo actual = cabeza;
            Nodo siguiente = null;

            while (actual != null)
            {
                siguiente = actual.Siguiente;
                actual.Siguiente = anterior;
                
                anterior = actual;
                actual = siguiente;
            }

            cabeza = anterior;
        }

        public void Imprimir()
        {
            Nodo actual = cabeza;
            while (actual != null)
            {
                Console.Write(actual.Valor + " -> ");
                actual = actual.Siguiente;
            }
            Console.WriteLine("null");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ListaEnlazada lista = new ListaEnlazada();

            // Agregamos elementos a la lista
            lista.AgregarAlFinal(1);
            lista.AgregarAlFinal(2);
            lista.AgregarAlFinal(3);
            lista.AgregarAlFinal(4);

            Console.WriteLine("Lista original:");
            lista.Imprimir();

            // Invertimos la lista
            lista.Invertir();

            Console.WriteLine("Lista invertida:");
            lista.Imprimir();
        }
    }
}