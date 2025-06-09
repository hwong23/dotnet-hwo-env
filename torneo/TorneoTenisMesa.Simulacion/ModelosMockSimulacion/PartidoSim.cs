using System;
using TorneoTenisMesa.GestionParticipantes.Modelos;

namespace TorneoTenisMesa.Simulacion.ModelosMockSimulacion
{
    public class PartidoSim
    {
        private static readonly Random random = new();
        public Participante Participante1 { get; }
        public Participante Participante2 { get; }
        public Participante? Ganador { get; private set; }

        public PartidoSim(Participante participante1, Participante participante2)
        {
            Participante1 = participante1 ?? throw new ArgumentNullException(nameof(participante1));
            Participante2 = participante2 ?? throw new ArgumentNullException(nameof(participante2));
        }

        public void JugarPartido()
        {
            Console.WriteLine($"  -> Jugando partido: {Participante1.GetNombreDescriptivo()} vs {Participante2.GetNombreDescriptivo()}");
            Ganador = (random.NextDouble() < 0.5) ? Participante1 : Participante2;
            Console.WriteLine($"     Ganador: {Ganador.GetNombreDescriptivo()}");
        }
    }
}