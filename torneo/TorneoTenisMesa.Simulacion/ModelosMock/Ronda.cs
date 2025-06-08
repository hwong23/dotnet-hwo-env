using System.Collections.Generic;
using System.Linq;

namespace TorneoTenisMesa.Simulacion.ModelosMock
{
    // Mock de la clase Ronda del subsistema de Partidos y Resultados
    public class Ronda
    {
        public int NumeroRonda { get; }
        public List<Partido> Partidos { get; }

        public Ronda(int numeroRonda)
        {
            NumeroRonda = numeroRonda;
            Partidos = new List<Partido>();
        }

        public void AgregarPartido(Partido partido)
        {
            Partidos.Add(partido);
        }

        /// <summary>
        /// Simula jugar todos los partidos de la ronda.
        /// </summary>
        public void JugarRonda()
        {
            foreach (var partido in Partidos)
            {
                partido.JugarPartido();
            }
        }

        /// <summary>
        /// Recolecta los ganadores de todos los partidos de esta ronda.
        /// </summary>
        /// <returns>Lista de jugadores ganadores.</returns>
        public List<Jugador> GetGanadores()
        {
            return Partidos.Select(p => p.Ganador)
                           .Where(g => g != null) // Asegurarse de que el ganador no sea null
                           .ToList<Jugador>()!; // El '!' es un operador de anulación de null, asumiendo que si un partido se jugó, tiene ganador.
        }
    }
}