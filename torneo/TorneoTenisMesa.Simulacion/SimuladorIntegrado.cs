using System;
using System.Collections.Generic;
using System.Linq;
using TorneoTenisMesa.GestionTorneos.Enums;
using TorneoTenisMesa.GestionTorneos.Modelos;
using TorneoTenisMesa.GestionParticipantes.Enums;
using TorneoTenisMesa.GestionParticipantes.Modelos;
using TorneoTenisMesa.Simulacion.ImplementacionesSimulacion;
using TorneoTenisMesa.Simulacion.ModelosMockSimulacion;

namespace TorneoTenisMesa.Simulacion
{
    public class SimuladorIntegrado
    {
        public static void Ejecutar() // Renombrado para evitar conflicto con Main si Program.cs lo llama
        {
            Console.WriteLine("--- INICIO DE LA SIMULACIÓN INTEGRADA (C#) ---");

            Console.WriteLine("\n--- FASE 1: Configuración Inicial ---");
            var jugador1 = new Jugador("Carlos", "Alcaraz", Genero.Masculino, new DateTime(2003, 5, 5));
            var jugador2 = new Jugador("Iga", "Swiatek", Genero.Femenino, new DateTime(2001, 5, 31));
            var jugador3 = new Jugador("Jannik", "Sinner", Genero.Masculino, new DateTime(2001, 8, 16));
            var jugador4 = new Jugador("Aryna", "Sabalenka", Genero.Femenino, new DateTime(1998, 5, 5));
            var jugador5 = new Jugador("Novak", "Djokovic", Genero.Masculino, new DateTime(1987, 5, 22));
            Console.WriteLine($"Jugadores Creados: {jugador1.GetNombreDescriptivo()}, {jugador2.GetNombreDescriptivo()}, ...");

            var formatoSim = new EliminatoriaDirectaFormatoSim();
            var miTorneo = new Torneo("Gran Slam C# Sim", DateTime.Now.AddDays(7), "Estadio Digital", TipoTorneo.Individual, formatoSim);
            Console.WriteLine($"Torneo Creado: {miTorneo.Nombre}, Estado: {miTorneo.Estado}");

            Console.WriteLine("\n--- FASE 2: Proceso de Inscripción ---");
            miTorneo.AbrirInscripciones();
            miTorneo.AgregarInscripcion(new Inscripcion(jugador1, miTorneo.IdTorneo));
            miTorneo.AgregarInscripcion(new Inscripcion(jugador2, miTorneo.IdTorneo));
            miTorneo.AgregarInscripcion(new Inscripcion(jugador3, miTorneo.IdTorneo));
            miTorneo.AgregarInscripcion(new Inscripcion(jugador4, miTorneo.IdTorneo));
            // No inscribimos a jugador5 para tener un número par o para probar bye
            miTorneo.AgregarInscripcion(new Inscripcion(jugador5, miTorneo.IdTorneo));
            miTorneo.CerrarInscripciones();

            Console.WriteLine("\n--- FASE 3: Inicio y Desarrollo del Torneo ---");
            miTorneo.IniciarTorneo();

            if (miTorneo.Estado == EstadoTorneo.EnCurso)
            {
                List<Participante> participantesActivos = miTorneo.GetParticipantesInscritosConfirmados();
                if (!participantesActivos.Any())
                {
                    Console.WriteLine("No hay participantes para continuar.");
                }
                else
                {
                    int numeroRonda = 1;
                    while (participantesActivos.Count > 1)
                    {
                        if (participantesActivos.Count % 2 != 0 && participantesActivos.Count > 1)
                        {
                            var jugadorConBye = participantesActivos[0];
                            Console.WriteLine($"\n--- JUGANDO RONDA {numeroRonda} (con BYE) ---");
                            Console.WriteLine($"  {jugadorConBye.GetNombreDescriptivo()} obtiene un BYE.");
                            var jugadoresParaEmparejar = participantesActivos.Skip(1).ToList();
                            if (!jugadoresParaEmparejar.Any()) { participantesActivos = new List<Participante> { jugadorConBye }; break; }
                            if (jugadoresParaEmparejar.Count == 1) { participantesActivos = new List<Participante> { jugadorConBye, jugadoresParaEmparejar[0] }; break; }


                            RondaSim rondaActual = formatoSim.GenerarRondaSimulada(jugadoresParaEmparejar, numeroRonda);
                            Console.WriteLine($"Generada Ronda {numeroRonda} con {rondaActual.Partidos.Count} partidos.");
                            rondaActual.JugarRonda();
                            var ganadoresDePartidos = rondaActual.GetGanadores();
                            participantesActivos = new List<Participante> { jugadorConBye };
                            participantesActivos.AddRange(ganadoresDePartidos);
                        }
                        else
                        {
                            Console.WriteLine($"\n--- JUGANDO RONDA {numeroRonda} ---");
                            RondaSim rondaActual = formatoSim.GenerarRondaSimulada(participantesActivos, numeroRonda);
                            Console.WriteLine($"Generada Ronda {numeroRonda} con {rondaActual.Partidos.Count} partidos.");
                            rondaActual.JugarRonda();
                            participantesActivos = rondaActual.GetGanadores();
                        }
                        if (!participantesActivos.Any() && numeroRonda > 0) { Console.Error.WriteLine("Error: No quedaron participantes activos."); break; }
                        numeroRonda++;
                    }

                    Console.WriteLine("\n--- FASE 4: Finalización del Torneo ---");
                    miTorneo.FinalizarTorneo();
                    if (participantesActivos.Count == 1)
                    {
                        Console.WriteLine($"\n=============================================\n¡EL CAMPEÓN DE '{miTorneo.Nombre}' ES: {participantesActivos[0].GetNombreDescriptivo()}!\n=============================================");
                    }
                    else { Console.WriteLine($"\nEl torneo finalizó sin un campeón único. Restantes: {string.Join(", ", participantesActivos.Select(p => p.GetNombreDescriptivo()))}"); }
                }
            }
            else { Console.WriteLine($"El torneo '{miTorneo.Nombre}' no pudo iniciarse. Estado: {miTorneo.Estado}"); }

            Console.WriteLine($"\nEstado final del torneo: {miTorneo.Estado}");
            Console.WriteLine("--- FIN DE LA SIMULACIÓN INTEGRADA (C#) ---");
        }
    }
}