// TorneoTenisMesa.Simulacion/ModelosMockSimulacion/PartidoSim.cs
using System;
using TorneoTenisMesa.GestionParticipantes.Modelos; // Para Participante

namespace TorneoTenisMesa.Simulacion.ModelosMockSimulacion
{
    public class PartidoSim
    {
        private static readonly Random random = new Random();
        public Participante Participante1 { get; }
        public Participante Participante2 { get; }
        public Participante? Ganador { get; private set; }

        public PartidoSim(Participante participante1, Participante participante2)
        {
            Participante1 = participante1;
            Participante2 = participante2;
        }

        public void JugarPartido()
        {
            Console.WriteLine($"  -> Jugando partido: {Participante1.GetNombreDescriptivo()} vs {Participante2.GetNombreDescriptivo()}");
            Ganador = (random.NextDouble() < 0.5) ? Participante1 : Participante2;
            Console.WriteLine($"     Ganador: {Ganador.GetNombreDescriptivo()}");
        }
    }
}