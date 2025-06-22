using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Paciente
{
    public string Cedula { get; set; }
    public string Nombre { get; set; }
    public string Especialidad { get; set; }
    public string Fecha { get; set; }
    public string Hora { get; set; }

    public override string ToString()
    {
        return $"{Nombre} | Cédula: {Cedula} | {Especialidad} | {Fecha} {Hora}";
    }
}

public class AgendaTurnos
{
    private const string ArchivoJson = "turnos.json";
    public List<Paciente> Turnos { get; set; } = new();

    public AgendaTurnos()
    {
        Cargar();
    }

    public bool RegistrarTurno(Paciente paciente)
    {
        bool existe = Turnos.Any(t => t.Fecha == paciente.Fecha && t.Hora == paciente.Hora);
        if (existe) return false;

        Turnos.Add(paciente);
        Guardar();
        return true;
    }

    public List<Paciente> BuscarPorCedula(string cedula) =>
        Turnos.Where(t => t.Cedula == cedula.Trim()).ToList();

    public List<Paciente> BuscarPorFecha(string fecha) =>
        Turnos.Where(t => t.Fecha == fecha.Trim()).OrderBy(t => t.Hora).ToList();

    public List<Paciente> BuscarPorMes(string mes) =>
        Turnos.Where(t => t.Fecha.StartsWith(mes)).OrderBy(t => t.Fecha).ThenBy(t => t.Hora).ToList();

    public bool CancelarTurno(string cedula)
    {
        int antes = Turnos.Count;
        Turnos = Turnos.Where(t => t.Cedula != cedula.Trim()).ToList();
        bool eliminado = Turnos.Count < antes;
        if (eliminado) Guardar();
        return eliminado;
    }

    public Dictionary<string, int> ReportePorEspecialidad()
    {
        return Turnos
            .GroupBy(t => t.Especialidad)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public void Guardar()
    {
        var opciones = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(ArchivoJson, JsonSerializer.Serialize(Turnos, opciones));
    }

    public void Cargar()
    {
        if (File.Exists(ArchivoJson))
        {
            var contenido = File.ReadAllText(ArchivoJson);
            Turnos = JsonSerializer.Deserialize<List<Paciente>>(contenido) ?? new List<Paciente>();
        }
    }
}

public class Program
{
    static AgendaTurnos agenda = new();

    public static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== AGENDA DE TURNOS - CLÍNICA ===");
            Console.WriteLine("1. Registrar nuevo turno");
            Console.WriteLine("2. Consultar por cédula");
            Console.WriteLine("3. Consultar por fecha (AAAA-MM-DD)");
            Console.WriteLine("4. Consultar por mes (AAAA-MM)");
            Console.WriteLine("5. Cancelar turno por cédula");
            Console.WriteLine("6. Reporte por especialidad");
            Console.WriteLine("7. Salir");
            Console.Write("Seleccione una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1": Registrar(); break;
                case "2": ConsultarPorCedula(); break;
                case "3": ConsultarPorFecha(); break;
                case "4": ConsultarPorMes(); break;
                case "5": Cancelar(); break;
                case "6": Reporte(); break;
                case "7": return;
                default:
                    Console.WriteLine(" Opción inválida.");
                    break;
            }
        }
    }

    static void Registrar()
    {
        Console.Write("Cédula: ");
        string cedula = Console.ReadLine();
        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();
        Console.Write("Especialidad: ");
        string especialidad = Console.ReadLine();
        Console.Write("Fecha (AAAA-MM-DD): ");
        string fecha = Console.ReadLine();
        Console.Write("Hora (HH:mm): ");
        string hora = Console.ReadLine();

        if (!DateTime.TryParse($"{fecha} {hora}", out _))
        {
            Console.WriteLine(" Fecha u hora inválida.");
            return;
        }

        var paciente = new Paciente
        {
            Cedula = cedula,
            Nombre = nombre,
            Especialidad = especialidad,
            Fecha = fecha,
            Hora = hora
        };

        if (agenda.RegistrarTurno(paciente))
            Console.WriteLine(" Turno registrado correctamente.");
        else
            Console.WriteLine(" Ya hay un turno en esa fecha y hora.");
    }

    static void ConsultarPorCedula()
    {
        Console.Write("Ingrese la cédula: ");
        var lista = agenda.BuscarPorCedula(Console.ReadLine());
        if (lista.Any())
            lista.ForEach(p => Console.WriteLine(p));
        else
            Console.WriteLine(" No se encontraron turnos.");
    }

    static void ConsultarPorFecha()
    {
        Console.Write("Ingrese la fecha (AAAA-MM-DD): ");
        var lista = agenda.BuscarPorFecha(Console.ReadLine());
        if (lista.Any())
            lista.ForEach(p => Console.WriteLine(p));
        else
            Console.WriteLine(" No hay turnos para esa fecha.");
    }

    static void ConsultarPorMes()
    {
        Console.Write("Ingrese el mes (AAAA-MM): ");
        var lista = agenda.BuscarPorMes(Console.ReadLine());
        if (lista.Any())
            lista.ForEach(p => Console.WriteLine(p));
        else
            Console.WriteLine(" No hay turnos en ese mes.");
    }

    static void Cancelar()
    {
        Console.Write("Ingrese la cédula del turno a cancelar: ");
        if (agenda.CancelarTurno(Console.ReadLine()))
            Console.WriteLine(" Turno cancelado.");
        else
            Console.WriteLine(" No se encontró el turno.");
    }

    static void Reporte()
    {
        Console.WriteLine(" Turnos por especialidad:");
        var reporte = agenda.ReportePorEspecialidad();
        if (reporte.Count == 0)
        {
            Console.WriteLine("No hay turnos registrados.");
            return;
        }

        foreach (var kv in reporte)
        {
            Console.WriteLine($"- {kv.Key}: {kv.Value} turno(s)");
        }
    }
}
