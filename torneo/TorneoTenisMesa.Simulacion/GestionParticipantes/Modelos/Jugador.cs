using System;
using TorneoTenisMesa.GestionParticipantes.Enums;

namespace TorneoTenisMesa.GestionParticipantes.Modelos
{
    public class Jugador : Participante
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? Ranking { get; set; }
        public string? Club { get; set; }
        public Genero Genero { get; set; }

        public Jugador(string nombre, string apellido, Genero genero, DateTime? fechaNacimiento = null) : base()
        {
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            Apellido = apellido ?? throw new ArgumentNullException(nameof(apellido));
            Genero = genero;
            FechaNacimiento = fechaNacimiento;
        }

        public override string GetNombreDescriptivo() => $"{Nombre} {Apellido}";

        public override string ToString() => $"Jugador{{Id={IdParticipante}, Nombre='{GetNombreDescriptivo()}', Genero={Genero}}}";
    }
}