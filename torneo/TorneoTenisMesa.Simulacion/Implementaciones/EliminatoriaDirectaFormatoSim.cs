// TorneoTenisMesa.Simulacion/ImplementacionesSimulacion/EliminatoriaDirectaFormatoSim.cs
using System;
using System.Collections.Generic;
using System.Linq;
using TorneoTenisMesa.GestionTorneos.Modelos;         // Para IFormatoTorneo
using TorneoTenisMesa.GestionParticipantes.Modelos; // Para Participante
using TorneoTenisMesa.Simulacion.ModelosMockSimulacion; // Para RondaSim, PartidoSim

namespace TorneoTenisMesa.Simulacion.ImplementacionesSimulacion
{
    public class EliminatoriaDirectaFormatoSim : IFormatoTorneo
    {
        public Guid IdFormato { get; } = Guid.NewGuid();
        public string NombreFormato => "Eliminatoria Directa (Sim)";
        public string Descripcion => "Los participantes compiten en rondas, el perdedor es eliminado.";

        private static readonly Random rng = new Random();

        // Este método ahora es parte de la simulación y opera con entidades de simulación
        public RondaSim GenerarRondaSimulada(List<Participante> participantes, int numeroRonda)
        {
            var ronda = new RondaSim(numeroRonda);
            var participantesBarajados = participantes.OrderBy(p => rng.Next()).ToList();

            for (int i = 0; i < participantesBarajados.Count - (participantesBarajados.Count % 2); i += 2)
            {
                if (i + 1 < participantesBarajados.Count)
                {
                    Participante p1 = participantesBarajados[i];
                    Participante p2 = participantesBarajados[i + 1];
                    ronda.AgregarPartido(new PartidoSim(p1, p2));
                }
            }
            return ronda;
        }

        // Los métodos de la interfaz que no se usarán directamente por el Torneo
        // podrían ser más abstractos o no implementados en una simulación simple.
        // Aquí los dejamos para cumplir la interfaz, pero no se llamarán desde Torneo directamente
        // en esta simulación simplificada. El torneo llama a GenerarRondaSimulada directamente.
        /*
        public object GenerarCuadro(TorneoTenisMesa.GestionTorneos.Modelos.Torneo torneo, List<Inscripcion> participantesInscritos)
        {
            // Lógica conceptual:
            // var participantes = participantesInscritos.Select(i => i.Participante).ToList();
            // return GenerarRondaSimulada(participantes, 1); // Devuelve la primera ronda
            throw new NotImplementedException("Este método es conceptual para la interfaz IFormatoTorneo.");
        }

        public object DeterminarSiguienteFase(TorneoTenisMesa.GestionTorneos.Modelos.Torneo torneo, object faseActual)
        {
            throw new NotImplementedException("Este método es conceptual para la interfaz IFormatoTorneo.");
        }
        */
    }
}