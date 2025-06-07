using System;

namespace Figuras
{
    public class Circulo
    {
        private double radio;

        public Circulo(double r)
        {
            radio = r;
        }

        public double CalcularArea()
        {
            return Math.PI * radio * radio;
        }

        public double CalcularPerimetro()
        {
            return 2 * Math.PI * radio;
        }
    }

    public class Rectangulo
    {
        private double baseRectangulo;
        private double altura;

        public Rectangulo(double b, double h)
        {
            baseRectangulo = b;
            altura = h;
        }

        public double CalcularArea()
        {
            return baseRectangulo * altura;
        }

        public double CalcularPerimetro()
        {
            return 2 * (baseRectangulo + altura);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Circulo c = new Circulo(3);
            Console.WriteLine("Área del círculo: " + c.CalcularArea());
            Console.WriteLine("Perímetro del círculo: " + c.CalcularPerimetro());

            Rectangulo r = new Rectangulo(4, 2);
            Console.WriteLine("Área del rectángulo: " + r.CalcularArea());
            Console.WriteLine("Perímetro del rectángulo: " + r.CalcularPerimetro());
        }
    }
}
