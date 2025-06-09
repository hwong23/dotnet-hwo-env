using System;

namespace TorneoTenisMesa.GestionParticipantes.Modelos
{
    public abstract class Participante : IEquatable<Participante>
    {
        public Guid IdParticipante { get; }

        protected Participante()
        {
            IdParticipante = Guid.NewGuid();
        }

        public abstract string GetNombreDescriptivo();

        public bool Equals(Participante? other) => other != null && IdParticipante.Equals(other.IdParticipante);
        public override bool Equals(object? obj) => Equals(obj as Participante);
        public override int GetHashCode() => IdParticipante.GetHashCode();
        public override string ToString() => $"Participante{{Id={IdParticipante}}}";
    }
}