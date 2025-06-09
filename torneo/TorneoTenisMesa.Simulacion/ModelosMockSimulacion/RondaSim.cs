// TorneoTenisMesa.Simulacion/ModelosMockSimulacion/RondaSim.cs
using System.Collections.Generic;
using System.Linq;
using TorneoTenisMesa.GestionParticipantes.Modelos; // Para Participante

namespace TorneoTenisMesa.Simulacion.ModelosMockSimulacion
{
    public class RondaSim
    {
        public int NumeroRonda { get; }
        public List<PartidoSim> Partidos { get; }

        public RondaSim(int numeroRonda)
        {
            NumeroRonda = numeroRonda;
            Partidos = new List<PartidoSim>();
        }

        public void AgregarPartido(PartidoSim partido)
        {
            Partidos.Add(partido);
        }

        public void JugarRonda()
        {
            foreach (var partido in Partidos)
            {
                partido.JugarPartido();
            }
        }

        public List<Participante> GetGanadores()
        {
            return Partidos.Select(p => p.Ganador)
                           .Where(g => g != null)
                           .ToList<Participante>()!;
        }
    }
}