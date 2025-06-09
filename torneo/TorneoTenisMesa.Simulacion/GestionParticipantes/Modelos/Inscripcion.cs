using System;
using TorneoTenisMesa.GestionParticipantes.Enums;

namespace TorneoTenisMesa.GestionParticipantes.Modelos
{
    public class Inscripcion : IEquatable<Inscripcion>
    {
        public Guid IdInscripcion { get; }
        public DateTime FechaInscripcion { get; }
        public EstadoInscripcion EstadoInscripcion { get; private set; }
        public Participante? Participante { get; } // Nullable si la inscripción se crea antes de asignar participante
        public Guid IdTorneo { get; }
        public Categoria? Categoria { get; set; }

        public Inscripcion(Participante participante, Guid idTorneo, Categoria? categoria = null)
        {
            IdInscripcion = Guid.NewGuid();
            FechaInscripcion = DateTime.Now;
            EstadoInscripcion = EstadoInscripcion.Pendiente;
            Participante = participante ?? throw new ArgumentNullException(nameof(participante));
            IdTorneo = idTorneo;
            Categoria = categoria;
        }

        public void ConfirmarInscripcion() => EstadoInscripcion = EstadoInscripcion.Confirmada;
        public void RechazarInscripcion() => EstadoInscripcion = EstadoInscripcion.Rechazada;
        public void PonerEnListaEspera() => EstadoInscripcion = EstadoInscripcion.ListaEspera;

        public bool Equals(Inscripcion? other) => other != null && IdInscripcion.Equals(other.IdInscripcion);
        public override bool Equals(object? obj) => Equals(obj as Inscripcion);
        public override int GetHashCode() => IdInscripcion.GetHashCode();
        public override string ToString() => $"Inscripcion{{Id={IdInscripcion}, Participante='{Participante?.GetNombreDescriptivo()}', TorneoId={IdTorneo}, Estado={EstadoInscripcion}}}";
    }
}
