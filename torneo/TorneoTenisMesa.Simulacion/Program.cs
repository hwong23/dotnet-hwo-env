using System;
using System.Collections.Generic;
using System.Linq;
using TorneoTenisMesa.GestionTorneos.Enums;
using TorneoTenisMesa.GestionTorneos.Modelos;
using TorneoTenisMesa.Simulacion.Implementaciones;
using TorneoTenisMesa.Simulacion.ModelosMock;

namespace TorneoTenisMesa.Simulacion
{
    class Program // O puedes llamarlo SimuladorTorneo si prefieres
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- INICIO DE LA SIMULACIÓN DE TORNEO (C#) ---");

            // 1. CREAR EL TORNEO
            // =========================================================
            Console.WriteLine("\n[CASO DE USO: CREAR TORNEO]");
            var formato = new EliminatoriaDirectaFormato();
            var torneo = new Torneo(
                    "Copa de Invierno C# 2024",
                    DateTime.Now,
                    "Gimnasio Digital",
                    TipoTorneo.Individual,
                    formato
            );
            Console.WriteLine($"Torneo Creado: {torneo}");

            // Simular inscripción de jugadores
            var jugadoresInscritos = new List<Jugador>
            {
                new Jugador("Alicia"),
                new Jugador("Bruno"),
                new Jugador("Clara"),
                new Jugador("Diego"),
                new Jugador("Eva"),
                new Jugador("Felix"),
                new Jugador("Gloria"),
                new Jugador("Hector")
            };
            
            Console.WriteLine($"{jugadoresInscritos.Count} jugadores inscritos.");
            
            // Cambiar estados del torneo
            torneo.AbrirInscripciones();
            torneo.CerrarInscripciones();
            
            // 2. JUGAR EL TORNEO
            // =========================================================
            Console.WriteLine("\n[CASO DE USO: JUGAR TORNEO]");
            torneo.IniciarTorneo();

            var jugadoresActivos = new List<Jugador>(jugadoresInscritos);
            int numeroRonda = 1;

            while (jugadoresActivos.Count > 1)
            {
                if (jugadoresActivos.Count % 2 != 0 && jugadoresActivos.Count > 1)
                {
                    // Manejo simplificado de "bye": el primer jugador de la lista avanza automáticamente
                    // Esto es una simplificación extrema para el caso de uso.
                    // En un sistema real, el formato determinaría cómo manejar los byes.
                    var jugadorConBye = jugadoresActivos[0];
                    Console.WriteLine($"\n--- JUGANDO RONDA {numeroRonda} ---");
                    Console.WriteLine($"  {jugadorConBye.Nombre} obtiene un BYE y avanza automáticamente.");
                    var jugadoresParaEmparejar = jugadoresActivos.Skip(1).ToList();
                    Ronda rondaActual = formato.GenerarRonda(jugadoresParaEmparejar, numeroRonda);
                    rondaActual.JugarRonda();
                    
                    var ganadoresDePartidos = rondaActual.GetGanadores();
                    jugadoresActivos.Clear();
                    jugadoresActivos.Add(jugadorConBye); // El jugador con bye
                    jugadoresActivos.AddRange(ganadoresDePartidos); // Los ganadores de los partidos
                }
                else
                {
                    Console.WriteLine($"\n--- JUGANDO RONDA {numeroRonda} ---");
                    Ronda rondaActual = formato.GenerarRonda(jugadoresActivos, numeroRonda);
                    rondaActual.JugarRonda();
                    jugadoresActivos = rondaActual.GetGanadores();
                }
                
                if (jugadoresActivos.Count == 0 && numeroRonda > 1) { // Evitar loop infinito si algo sale mal
                     Console.WriteLine("Error: No quedaron jugadores activos después de la ronda.");
                     break;
                }
                numeroRonda++;
            }
            
            // 3. FINALIZAR EL TORNEO
            // =========================================================
            Console.WriteLine("\n[CASO DE USO: FINALIZAR TORNEO]");
            torneo.FinalizarTorneo();
            
            if (jugadoresActivos.Count == 1)
            {
                Jugador campeon = jugadoresActivos[0];
                Console.WriteLine("\n=============================================");
                Console.WriteLine($"¡EL CAMPEÓN DEL TORNEO '{torneo.Nombre}' ES: {campeon.Nombre}!");
                Console.WriteLine("=============================================");
            }
            else if (jugadoresActivos.Count == 0)
            {
                 Console.WriteLine("\nEl torneo ha finalizado sin un campeón claro debido a un error en el desarrollo de las rondas.");
            }
            else
            {
                Console.WriteLine($"\nEl torneo ha finalizado. Jugadores restantes: {string.Join(", ", jugadoresActivos.Select(j => j.Nombre))}. Se esperaba 1 campeón.");
            }
            
            Console.WriteLine($"\nEstado final del torneo: {torneo.Estado}");
            Console.WriteLine("--- FIN DE LA SIMULACIÓN ---");
        }
    }
}