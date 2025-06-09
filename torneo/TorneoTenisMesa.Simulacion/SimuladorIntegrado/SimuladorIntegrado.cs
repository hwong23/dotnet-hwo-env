// TorneoTenisMesa.Simulacion/SimuladorIntegrado.cs
using System;
using System.Collections.Generic;
using System.Linq;

// Usings para el subsistema de Gestión de Torneos
using TorneoTenisMesa.GestionTorneos.Enums;
using TorneoTenisMesa.GestionTorneos.Modelos;

// Usings para el subsistema de Gestión de Participantes
using TorneoTenisMesa.GestionParticipantes.Enums;
using TorneoTenisMesa.GestionParticipantes.Modelos;

// Usings para las clases de Simulación
using TorneoTenisMesa.Simulacion.ImplementacionesSimulacion;
using TorneoTenisMesa.Simulacion.ModelosMockSimulacion;


namespace TorneoTenisMesa.Simulacion
{
    public class SimuladorIntegrado
    {
        public static void Main(string[] args) // Cambia el Main en Program.cs para llamar a este si es necesario
        {
            Console.WriteLine("--- INICIO DE LA SIMULACIÓN INTEGRADA (C#) ---");

            // --- 1. CONFIGURACIÓN INICIAL ---
            Console.WriteLine("\n--- FASE 1: Configuración Inicial ---");

            // Crear Jugadores (Subsistema GestiónParticipantes)
            var jugador1 = new Jugador("Carlos", "Alcaraz", new DateTime(2003, 5, 5), Genero.Masculino);
            var jugador2 = new Jugador("Iga", "Swiatek", new DateTime(2001, 5, 31), Genero.Femenino);
            var jugador3 = new Jugador("Jannik", "Sinner", new DateTime(2001, 8, 16), Genero.Masculino);
            var jugador4 = new Jugador("Aryna", "Sabalenka", new DateTime(1998, 5, 5), Genero.Femenino);
            var jugador5 = new Jugador("Novak", "Djokovic", new DateTime(1987,5,22), Genero.Masculino);
            var jugador6 = new Jugador("Coco", "Gauff", new DateTime(2004,3,13), Genero.Femenino);


            Console.WriteLine($"Jugadores Creados: {jugador1.GetNombreDescriptivo()}, {jugador2.GetNombreDescriptivo()}, {jugador3.GetNombreDescriptivo()}, {jugador4.GetNombreDescriptivo()}, ...");

            // Crear Formato de Torneo (Simulación)
            var formatoSim = new EliminatoriaDirectaFormatoSim();

            // Crear Torneo (Subsistema GestiónTorneos)
            var miTorneo = new Torneo(
                "Gran Slam de Simulación",
                DateTime.Now.AddDays(7),
                "Estadio Virtual",
                TipoTorneo.Individual, // Podríamos usar Categoria si quisiéramos complicarlo
                formatoSim
            );
            Console.WriteLine($"Torneo Creado: {miTorneo.Nombre}, Estado: {miTorneo.Estado}");

            // --- 2. PROCESO DE INSCRIPCIÓN ---
            Console.WriteLine("\n--- FASE 2: Proceso de Inscripción ---");
            miTorneo.AbrirInscripciones();

            // Crear e intentar agregar inscripciones (Subsistema GestiónParticipantes y GestiónTorneos)
            var inscripcion1 = new Inscripcion(jugador1, miTorneo.IdTorneo);
            miTorneo.AgregarInscripcion(inscripcion1);

            var inscripcion2 = new Inscripcion(jugador2, miTorneo.IdTorneo);
            miTorneo.AgregarInscripcion(inscripcion2);

            var inscripcion3 = new Inscripcion(jugador3, miTorneo.IdTorneo);
            miTorneo.AgregarInscripcion(inscripcion3);
            
            var inscripcion4 = new Inscripcion(jugador4, miTorneo.IdTorneo);
            miTorneo.AgregarInscripcion(inscripcion4);

            var inscripcion5 = new Inscripcion(jugador5, miTorneo.IdTorneo);
            miTorneo.AgregarInscripcion(inscripcion5);

            var inscripcion6 = new Inscripcion(jugador6, miTorneo.IdTorneo);
            // No agregamos la inscripción 6 para probar el flujo
            // miTorneo.AgregarInscripcion(inscripcion6);


            // Intentar inscribir a alguien cuando no está abierto (debería fallar)
            // miTorneo.Estado = EstadoTorneo.Planificado; // Forzar estado incorrecto
            // var inscripcionFallida = new Inscripcion(jugador6, miTorneo.IdTorneo);
            // miTorneo.AgregarInscripcion(inscripcionFallida);
            // miTorneo.Estado = EstadoTorneo.InscripcionAbierta; // Restaurar para continuar

            miTorneo.CerrarInscripciones();

            // --- 3. INICIO Y DESARROLLO DEL TORNEO ---
            Console.WriteLine("\n--- FASE 3: Inicio y Desarrollo del Torneo ---");
            miTorneo.IniciarTorneo();

            if (miTorneo.Estado == EstadoTorneo.EnCurso)
            {
                List<Participante> participantesActivos = miTorneo.GetParticipantesInscritos();
                if (!participantesActivos.Any())
                {
                    Console.WriteLine("No hay participantes suficientes para continuar el torneo.");
                }
                else
                {
                    int numeroRonda = 1;
                    while (participantesActivos.Count > 1)
                    {
                         if (participantesActivos.Count % 2 != 0 && participantesActivos.Count > 1)
                        {
                            // Manejo simplificado de "bye": el primer jugador de la lista avanza automáticamente
                            var jugadorConBye = participantesActivos[0];
                            Console.WriteLine($"\n--- JUGANDO RONDA {numeroRonda} (con BYE) ---");
                            Console.WriteLine($"  {jugadorConBye.GetNombreDescriptivo()} obtiene un BYE y avanza automáticamente.");
                            
                            var jugadoresParaEmparejar = participantesActivos.Skip(1).ToList();
                            if (jugadoresParaEmparejar.Count < 2 && jugadoresParaEmparejar.Count !=0) { // Si solo queda uno después del bye, es el otro finalista
                                participantesActivos.Clear();
                                participantesActivos.Add(jugadorConBye);
                                participantesActivos.AddRange(jugadoresParaEmparejar);
                                break; 
                            } else if (jugadoresParaEmparejar.Count == 0) { // Si no quedan más después del bye, el bye es el ganador
                                participantesActivos.Clear();
                                participantesActivos.Add(jugadorConBye);
                                break;
                            }


                            RondaSim rondaActual = formatoSim.GenerarRondaSimulada(jugadoresParaEmparejar, numeroRonda);
                            Console.WriteLine($"Generada Ronda {numeroRonda} con {rondaActual.Partidos.Count} partidos.");
                            rondaActual.JugarRonda();
                            
                            var ganadoresDePartidos = rondaActual.GetGanadores();
                            participantesActivos.Clear();
                            participantesActivos.Add(jugadorConBye); 
                            participantesActivos.AddRange(ganadoresDePartidos);
                        }
                        else
                        {
                            Console.WriteLine($"\n--- JUGANDO RONDA {numeroRonda} ---");
                            // El formato debería usar los participantes del torneo
                            RondaSim rondaActual = formatoSim.GenerarRondaSimulada(participantesActivos, numeroRonda);
                            Console.WriteLine($"Generada Ronda {numeroRonda} con {rondaActual.Partidos.Count} partidos.");
                            rondaActual.JugarRonda();
                            participantesActivos = rondaActual.GetGanadores();
                        }

                        if (!participantesActivos.Any() && numeroRonda > 0)
                        {
                            Console.WriteLine("Error: No quedaron participantes activos después de la ronda.");
                            break;
                        }
                        numeroRonda++;
                    }

                    // --- 4. FINALIZACIÓN ---
                    Console.WriteLine("\n--- FASE 4: Finalización del Torneo ---");
                    miTorneo.FinalizarTorneo();

                    if (participantesActivos.Count == 1)
                    {
                        Participante campeon = participantesActivos[0];
                        Console.WriteLine("\n=============================================");
                        Console.WriteLine($"¡EL CAMPEÓN DEL TORNEO '{miTorneo.Nombre}' ES: {campeon.GetNombreDescriptivo()}!");
                        Console.WriteLine("=============================================");
                    }
                    else
                    {
                        Console.WriteLine("\nEl torneo ha finalizado sin un campeón único claro o con un error.");
                        Console.WriteLine($"Participantes restantes: {string.Join(", ", participantesActivos.Select(p => p.GetNombreDescriptivo()))}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"El torneo '{miTorneo.Nombre}' no pudo iniciarse. Estado actual: {miTorneo.Estado}");
            }


            Console.WriteLine($"\nEstado final del torneo: {miTorneo.Estado}");
            Console.WriteLine("--- FIN DE LA SIMULACIÓN INTEGRADA ---");
        }
    }
}