using System;

namespace TorneoTenisMesa.Simulacion.ModelosMock
{
    // Mock de la clase Partido del subsistema de Partidos y Resultados
    public class Partido
    {
        private static readonly Random random = new Random();
        public Jugador Jugador1 { get; }
        public Jugador Jugador2 { get; }
        public Jugador? Ganador { get; private set; }

        public Partido(Jugador jugador1, Jugador jugador2)
        {
            Jugador1 = jugador1;
            Jugador2 = jugador2;
        }

        /// <summary>
        /// Simula la ejecución de un partido, eligiendo un ganador al azar.
        /// </summary>
        public void JugarPartido()
        {
            Console.WriteLine($"  -> Jugando partido: {Jugador1} vs {Jugador2}");
            Ganador = (random.NextDouble() < 0.5) ? Jugador1 : Jugador2;
            Console.WriteLine($"     Ganador: {Ganador}");
        }
    }
}