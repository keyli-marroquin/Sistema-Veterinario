using System;
using Microsoft.Data.Sqlite;

class Program
{
    private static string connectionString = "Data Source=veterinaria.db";

    static void Main(string[] args)
    {
        InicializarBaseDeDatos();

        int opcion = 0;
        do
        {
            Console.Clear();
            Console.WriteLine("=== SISTEMA DE CONTROL CLÍNICO VETERINARIO ===");
            Console.WriteLine("1. Registrar Cliente");
            Console.WriteLine("2. Registrar Paciente (Mascota)");
            Console.WriteLine("3. Agendar Cita");
            Console.WriteLine("4. Mostrar Clientes Registrados");
            Console.WriteLine("5. Mostrar Pacientes Registrados");
            Console.WriteLine("6. Mostrar Citas Programadas");
            Console.WriteLine("7. Salir");
            Console.Write("\nSeleccione una opción: ");

            if (int.TryParse(Console.ReadLine(), out opcion))
            {
                switch (opcion)
                {
                    case 1:
                        RegistrarCliente();
                        break;
                    case 2:
                        RegistrarPaciente();
                        break;
                    case 3:
                        AgendarCita();
                        break;
                    case 4:
                        MostrarClientes();
                        break;
                    case 5:
                        MostrarPacientes();
                        break;
                    case 6:
                        MostrarCitas();
                        break;
                    case 7:
                        Console.WriteLine("Saliendo del sistema...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione una tecla para continuar.");
                        Console.ReadKey();
                        break;
                }
            }
            else
            {
                Console.WriteLine("Por favor, ingrese un número válido.");
                Console.ReadKey();
            }

        } while (opcion != 7);
    }

    private static void InicializarBaseDeDatos()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string query = @"
                CREATE TABLE IF NOT EXISTS Clientes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT NOT NULL,
                    Telefono TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Pacientes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    NombreMascota TEXT NOT NULL,
                    Especie TEXT NOT NULL,
                    ClienteId INTEGER,
                    FOREIGN KEY(ClienteId) REFERENCES Clientes(Id)
                );

                CREATE TABLE IF NOT EXISTS Citas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PacienteId INTEGER,
                    FechaHora TEXT NOT NULL,
                    Motivo TEXT NOT NULL,
                    FOREIGN KEY(PacienteId) REFERENCES Pacientes(Id)
                );
            ";

            using (var command = new SqliteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    private static void RegistrarCliente()
    {
        Console.Clear();
        Console.WriteLine("=== REGISTRAR CLIENTE ===");
        Console.Write("Ingrese el nombre del cliente: ");
        string nombre = Console.ReadLine() ?? "";
        Console.Write("Ingrese el teléfono: ");
        string telefono = Console.ReadLine() ?? "";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Clientes (Nombre, Telefono) VALUES (@nombre, @telefono)";
            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nombre", nombre);
                command.Parameters.AddWithValue("@telefono", telefono);
                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("\n¡Cliente registrado con éxito! Presione una tecla para continuar.");
        Console.ReadKey();
    }

    private static void RegistrarPaciente()
    {
        Console.Clear();
        Console.WriteLine("=== REGISTRAR PACIENTE (MASCOTA) ===");
        Console.Write("Ingrese el nombre de la mascota: ");
        string nombreMascota = Console.ReadLine() ?? "";
        Console.Write("Ingrese la especie (Ej. Perro, Gato): ");
        string especie = Console.ReadLine() ?? "";
        Console.Write("Ingrese el ID del cliente dueño: ");
        if (int.TryParse(Console.ReadLine(), out int clienteId))
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Pacientes (NombreMascota, Especie, ClienteId) VALUES (@nombre, @especie, @clienteId)";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", nombreMascota);
                    command.Parameters.AddWithValue("@especie", especie);
                    command.Parameters.AddWithValue("@clienteId", clienteId);
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine("\n¡Paciente registrado con éxito!");
        }
        else
        {
            Console.WriteLine("\nID de cliente no válido.");
        }
        Console.ReadKey();
    }

    private static void AgendarCita()
    {
        Console.Clear();
        Console.WriteLine("=== AGENDAR CITA ===");
        Console.Write("Ingrese el ID del paciente (mascota): ");
        if (int.TryParse(Console.ReadLine(), out int pacienteId))
        {
            Console.Write("Ingrese la fecha y hora (Ej. 2026-10-25 10:00 AM): ");
            string fechaHora = Console.ReadLine() ?? "";
            Console.Write("Ingrese el motivo de la cita: ");
            string motivo = Console.ReadLine() ?? "";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Citas (PacienteId, FechaHora, Motivo) VALUES (@pacienteId, @fechaHora, @motivo)";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@pacienteId", pacienteId);
                    command.Parameters.AddWithValue("@fechaHora", fechaHora);
                    command.Parameters.AddWithValue("@motivo", motivo);
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine("\n¡Cita agendada con éxito!");
        }
        else
        {
            Console.WriteLine("\nID de paciente no válido.");
        }
        Console.ReadKey();
    }

    private static void MostrarClientes()
    {
        Console.Clear();
        Console.WriteLine("=== CLIENTES REGISTRADOS ===");
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT Id, Nombre, Telefono FROM Clientes";
            using (var command = new SqliteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Id"]} | Nombre: {reader["Nombre"]} | Teléfono: {reader["Telefono"]}");
                    }
                }
            }
        }
        Console.WriteLine("\nPresione una tecla para volver al menú.");
        Console.ReadKey();
    }

    private static void MostrarPacientes()
    {
        Console.Clear();
        Console.WriteLine("=== PACIENTES REGISTRADOS ===");
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT p.Id, p.NombreMascota, p.Especie, c.Nombre AS Dueno FROM Pacientes p LEFT JOIN Clientes c ON p.ClienteId = c.Id";
            using (var command = new SqliteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["Id"]} | Mascota: {reader["NombreMascota"]} ({reader["Especie"]}) | Dueño: {reader["Dueno"]}");
                    }
                }
            }
        }
        Console.WriteLine("\nPresione una tecla para volver al menú.");
        Console.ReadKey();
    }

    private static void MostrarCitas()
    {
        Console.Clear();
        Console.WriteLine("=== CITAS PROGRAMADAS ===");
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT ci.Id, p.NombreMascota, ci.FechaHora, ci.Motivo FROM Citas ci LEFT JOIN Pacientes p ON ci.PacienteId = p.Id";
            using (var command = new SqliteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID Cita: {reader["Id"]} | Mascota: {reader["NombreMascota"]} | Fecha: {reader["FechaHora"]} | Motivo: {reader["Motivo"]}");
                    }
                }
            }
        }
        Console.WriteLine("\nPresione una tecla para volver al menú.");
        Console.ReadKey();
    }
}