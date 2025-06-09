using System;
using System.Collections.Generic;
using System.Linq;
using TorneoTenisMesa.GestionTorneos.Modelos;
using TorneoTenisMesa.GestionParticipantes.Modelos;
using TorneoTenisMesa.Simulacion.ModelosMockSimulacion;

namespace TorneoTenisMesa.Simulacion.ImplementacionesSimulacion
{
    public class EliminatoriaDirectaFormatoSim : IFormatoTorneo
    {
        public Guid IdFormato { get; } = Guid.NewGuid();
        public string NombreFormato => "Eliminatoria Directa (Sim)";
        public string Descripcion => "Los participantes compiten en rondas, el perdedor es eliminado.";
        private static readonly Random rng = new();

        public RondaSim GenerarRondaSimulada(List<Participante> participantes, int numeroRonda)
        {
            var ronda = new RondaSim(numeroRonda);
            var participantesBarajados = participantes.OrderBy(p => rng.Next()).ToList();

            for (int i = 0; i < participantesBarajados.Count - (participantesBarajados.Count % 2); i += 2)
            {
                if (i + 1 < participantesBarajados.Count)
                {
                    ronda.AgregarPartido(new PartidoSim(participantesBarajados[i], participantesBarajados[i + 1]));
                }
            }
            return ronda;
        }
    }
}