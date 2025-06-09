using System;
using System.Collections.Generic;
using System.Linq;

namespace TorneoTenisMesa.GestionParticipantes.Modelos
{
    public class Equipo : Participante
    {
        public string NombreEquipo { get; set; }
        private readonly List<Jugador> _miembros;
        public IReadOnlyList<Jugador> Miembros => _miembros.AsReadOnly();

        public Equipo(string nombreEquipo) : base()
        {
            NombreEquipo = nombreEquipo ?? throw new ArgumentNullException(nameof(nombreEquipo));
            _miembros = new List<Jugador>();
        }

        public void AgregarMiembro(Jugador jugador)
        {
            if (jugador == null) throw new ArgumentNullException(nameof(jugador));
            if (!_miembros.Contains(jugador))
            {
                _miembros.Add(jugador);
            }
        }

        public bool RemoverMiembro(Jugador jugador) => _miembros.Remove(jugador);

        public bool ValidarMinimoMiembros(int minimoRequerido) => _miembros.Count >= minimoRequerido;

        public override string GetNombreDescriptivo() => NombreEquipo;

        public override string ToString() => $"Equipo{{Id={IdParticipante}, Nombre='{NombreEquipo}', Miembros={_miembros.Count}}}";
    }
}