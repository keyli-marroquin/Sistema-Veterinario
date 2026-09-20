using System;
using System.Collections.Generic;

namespace SistemaClinicaVeterinaria
{
    // Clase Cliente (Dueño de la mascota)
    public class Cliente
    {
        public string IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }

        public Cliente(string idCliente, string nombre, string telefono)
        {
            IdCliente = idCliente;
            Nombre = nombre;
            Telefono = telefono;
        }

        public void MostrarDatos()
        {
            Console.WriteLine($"ID: {IdCliente} | Nombre: {Nombre} | Teléfono: {Telefono}");
        }
    }

    // Clase Paciente (Mascota)
    public class Paciente
    {
        public string NombreMascota { get; set; }
        public string Especie { get; set; }
        public string Raza { get; set; }
        public int Edad { get; set; }
        public Cliente Duenio { get; set; } // Asociación con Cliente

        public Paciente(string nombreMascota, string especie, string raza, int edad, Cliente duenio)
        {
            NombreMascota = nombreMascota;
            Especie = especie;
            Raza = raza;
            Edad = edad;
            Duenio = duenio;
        }

        public void MostrarPaciente()
        {
            Console.WriteLine($"Mascota: {NombreMascota} ({Especie} - {Raza}), {Edad} años");
            Console.WriteLine($"   Dueño: {Duenio.Nombre} (Tel: {Duenio.Telefono})");
        }
    }

    // Clase Cita
    public class Cita
    {
        public string Fecha { get; set; }
        public string Motivo { get; set; }
        public Paciente PacienteAsignado { get; set; }

        public Cita(string fecha, string motivo, Paciente pacienteAsignado)
        {
            Fecha = fecha;
            Motivo = motivo;
            PacienteAsignado = pacienteAsignado;
        }

        public void MostrarCita()
        {
            Console.WriteLine($"Fecha: {Fecha} | Motivo: {Motivo}");
            Console.WriteLine($"   Paciente: {PacienteAsignado.NombreMascota} (Dueño: {PacienteAsignado.Duenio.Nombre})");
        }
    }

    // Clase Principal con el Menú Interactivo
    class Program
    {
        static List<Cliente> listaClientes = new List<Cliente>();
        static List<Paciente> listaPacientes = new List<Paciente>();
        static List<Cita> listaCitas = new List<Cita>();

        static void Main(string[] args)
        {
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
                            Console.WriteLine("\nSaliendo del sistema...");
                            break;
                        default:
                            Console.WriteLine("\nOpción inválida. Intente de nuevo.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\nPor favor, ingrese un número válido.");
                }

                if (opcion != 7)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 7);
        }

        static void RegistrarCliente()
        {
            Console.Clear();
            Console.WriteLine("--- REGISTRAR CLIENTE ---");
            Console.Write("Ingrese ID o DPI del cliente: ");
            string id = Console.ReadLine();
            Console.Write("Ingrese Nombre completo: ");
            string nombre = Console.ReadLine();
            Console.Write("Ingrese Teléfono: ");
            string telefono = Console.ReadLine();

            listaClientes.Add(new Cliente(id, nombre, telefono));
            Console.WriteLine("\n¡Cliente registrado con éxito!");
        }

        static void RegistrarPaciente()
        {
            Console.Clear();
            Console.WriteLine("--- REGISTRAR PACIENTE (MASCOTA) ---");
            if (listaClientes.Count == 0)
            {
                Console.WriteLine("¡Error! Debe registrar al menos un cliente antes de agregar una mascota.");
                return;
            }

            Console.Write("Nombre de la mascota: ");
            string nombreMascota = Console.ReadLine();
            Console.Write("Especie (Perro, Gato, etc.): ");
            string especie = Console.ReadLine();
            Console.Write("Raza: ");
            string raza = Console.ReadLine();
            Console.Write("Edad: ");
            int edad = int.Parse(Console.ReadLine());

            Console.WriteLine("\nSeleccione el ID del dueño para esta mascota:");
            foreach (var c in listaClientes)
            {
                c.MostrarDatos();
            }
            Console.Write("ID del dueño: ");
            string idDuenio = Console.ReadLine();

            Cliente duenioEncontrado = listaClientes.Find(c => c.IdCliente == idDuenio);

            if (duenioEncontrado != null)
            {
                listaPacientes.Add(new Paciente(nombreMascota, especie, raza, edad, duenioEncontrado));
                Console.WriteLine("\n¡Paciente registrado con éxito!");
            }
            else
            {
                Console.WriteLine("\nNo se encontró un cliente con ese ID.");
            }
        }

        static void AgendarCita()
        {
            Console.Clear();
            Console.WriteLine("--- AGENDAR CITA ---");
            if (listaPacientes.Count == 0)
            {
                Console.WriteLine("¡Error! No hay pacientes registrados para agendar una cita.");
                return;
            }

            Console.Write("Fecha y hora de la cita (ej. 20/09/2026 10:00 AM): ");
            string fecha = Console.ReadLine();
            Console.Write("Motivo de consulta: ");
            string motivo = Console.ReadLine();

            Console.WriteLine("\nPacientes disponibles:");
            for (int i = 0; i < listaPacientes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {listaPacientes[i].NombreMascota} (Dueño: {listaPacientes[i].Duenio.Nombre})");
            }
            Console.Write("Seleccione el número del paciente: ");
            int indice = int.Parse(Console.ReadLine()) - 1;

            if (indice >= 0 && indice < listaPacientes.Count)
            {
                listaCitas.Add(new Cita(fecha, motivo, listaPacientes[indice]));
                Console.WriteLine("\n¡Cita agendada con éxito!");
            }
            else
            {
                Console.WriteLine("\nSelección inválida.");
            }
        }

        static void MostrarClientes()
        {
            Console.Clear();
            Console.WriteLine("--- LISTA DE CLIENTES ---");
            if (listaClientes.Count == 0)
            {
                Console.WriteLine("No hay clientes registrados.");
                return;
            }
            foreach (var c in listaClientes)
            {
                c.MostrarDatos();
                Console.WriteLine("------------------------");
            }
        }

        static void MostrarPacientes()
        {
            Console.Clear();
            Console.WriteLine("--- LISTA DE PACIENTES ---");
            if (listaPacientes.Count == 0)
            {
                Console.WriteLine("No hay pacientes registrados.");
                return;
            }
            foreach (var p in listaPacientes)
            {
                p.MostrarPaciente();
                Console.WriteLine("------------------------");
            }
        }

        static void MostrarCitas()
        {
            Console.Clear();
            Console.WriteLine("--- CITAS PROGRAMADAS ---");
            if (listaCitas.Count == 0)
            {
                Console.WriteLine("No hay citas registradas.");
                return;
            }
            foreach (var ci in listaCitas)
            {
                ci.MostrarCita();
                Console.WriteLine("------------------------");
            }
        }
    }
}