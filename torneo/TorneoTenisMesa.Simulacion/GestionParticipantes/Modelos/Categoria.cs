using System;
using TorneoTenisMesa.GestionParticipantes.Enums;

namespace TorneoTenisMesa.GestionParticipantes.Modelos
{
    public class Categoria : IEquatable<Categoria>
    {
        public Guid IdCategoria { get; }
        public string NombreCategoria { get; set; }
        public int? RestriccionEdadMin { get; set; }
        public int? RestriccionEdadMax { get; set; }
        public Genero? RestriccionGenero { get; set; } // Nullable si no hay restricción de género

        public Categoria(string nombreCategoria)
        {
            IdCategoria = Guid.NewGuid();
            NombreCategoria = nombreCategoria ?? throw new ArgumentNullException(nameof(nombreCategoria));
        }

        public bool EsJugadorElegible(Jugador jugador, DateTime fechaReferenciaTorneo)
        {
            if (jugador == null) return false;

            if (RestriccionGenero.HasValue && RestriccionGenero.Value != Genero.Mixto)
            {
                if (jugador.Genero != RestriccionGenero.Value) return false;
            }

            if (jugador.FechaNacimiento.HasValue)
            {
                int edad = fechaReferenciaTorneo.Year - jugador.FechaNacimiento.Value.Year;
                if (jugador.FechaNacimiento.Value.Date > fechaReferenciaTorneo.AddYears(-edad)) edad--;

                if (RestriccionEdadMin.HasValue && edad < RestriccionEdadMin.Value) return false;
                if (RestriccionEdadMax.HasValue && edad > RestriccionEdadMax.Value) return false;
            }
            else // Si el jugador no tiene fecha de nacimiento, no podemos verificar restricciones de edad
            {
                if (RestriccionEdadMin.HasValue || RestriccionEdadMax.HasValue) return false; 
            }
            return true;
        }

        public bool Equals(Categoria? other) => other != null && IdCategoria.Equals(other.IdCategoria);
        public override bool Equals(object? obj) => Equals(obj as Categoria);
        public override int GetHashCode() => IdCategoria.GetHashCode();
        public override string ToString() => $"Categoria{{Id={IdCategoria}, Nombre='{NombreCategoria}'}}";
    }
}