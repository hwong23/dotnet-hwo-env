using System;

namespace TorneoTenisMesa.Simulacion.ModelosMock
{
    // Mock de la clase Jugador del subsistema de Participantes
    public class Jugador : IEquatable<Jugador>
    {
        public Guid Id { get; }
        public string Nombre { get; }

        public Jugador(string nombre)
        {
            Id = Guid.NewGuid();
            Nombre = nombre;
        }

        public override string ToString() => Nombre;

        public bool Equals(Jugador? other)
        {
            if (other is null) return false;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object? obj) => Equals(obj as Jugador);
        public override int GetHashCode() => Id.GetHashCode();
    }
}