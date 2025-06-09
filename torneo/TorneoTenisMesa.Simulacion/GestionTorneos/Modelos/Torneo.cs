using System;
using System.Collections.Generic;
using System.Linq;
using TorneoTenisMesa.GestionTorneos.Enums;
using TorneoTenisMesa.GestionParticipantes.Modelos; // Para Inscripcion, Participante
using TorneoTenisMesa.GestionParticipantes.Enums;   // Para EstadoInscripcion

namespace TorneoTenisMesa.GestionTorneos.Modelos
{
    public class Torneo : IEquatable<Torneo>
    {
        public Guid IdTorneo { get; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Lugar { get; set; }
        public TipoTorneo TipoTorneo { get; set; }
        public IFormatoTorneo Formato { get; set; }
        public EstadoTorneo Estado { get; private set; }
        public string? ReglasEspecificas { get; set; }
        public Guid? IdOrganizador { get; set; }

        private readonly List<Inscripcion> _inscripcionesRegistradas;
        public IReadOnlyList<Inscripcion> InscripcionesRegistradas => _inscripcionesRegistradas.AsReadOnly();

        public Torneo(string nombre, DateTime fechaInicio, string lugar, TipoTorneo tipoTorneo, IFormatoTorneo formato)
        {
            IdTorneo = Guid.NewGuid();
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
            FechaInicio = fechaInicio;
            Lugar = lugar;
            TipoTorneo = tipoTorneo;
            Formato = formato ?? throw new ArgumentNullException(nameof(formato));
            Estado = EstadoTorneo.Planificado;
            _inscripcionesRegistradas = new List<Inscripcion>();
        }

        public bool AgregarInscripcion(Inscripcion inscripcion)
        {
            if (Estado != EstadoTorneo.InscripcionAbierta)
            {
                Console.Error.WriteLine($"Error: No se puede agregar inscripción. El torneo '{Nombre}' no está abierto. Estado: {Estado}");
                return false;
            }
            if (inscripcion.IdTorneo != IdTorneo)
            {
                Console.Error.WriteLine("Error: La inscripción no pertenece a este torneo.");
                return false;
            }
            if (_inscripcionesRegistradas.Any(i => i.IdInscripcion == inscripcion.IdInscripcion || (i.Participante != null && i.Participante.IdParticipante == inscripcion.Participante?.IdParticipante)))
            {
                Console.Error.WriteLine($"Error: El participante '{inscripcion.Participante?.GetNombreDescriptivo()}' ya está inscrito o la inscripción ya existe.");
                return false;
            }

            _inscripcionesRegistradas.Add(inscripcion);
            inscripcion.ConfirmarInscripcion();
            Console.WriteLine($"Inscripción de '{inscripcion.Participante?.GetNombreDescriptivo()}' registrada y confirmada para '{Nombre}'.");
            return true;
        }

        public List<Participante> GetParticipantesInscritosConfirmados()
        {
            return _inscripcionesRegistradas
                   .Where(i => i.EstadoInscripcion == GestionParticipantes.Enums.EstadoInscripcion.Confirmada && i.Participante != null)
                   .Select(i => i.Participante!) // Usamos el operador ! porque el Where ya filtró los null
                   .ToList();
        }

        public void AbrirInscripciones()
        {
            if (Estado == EstadoTorneo.Planificado)
            {
                Estado = EstadoTorneo.InscripcionAbierta;
                Console.WriteLine($"Inscripciones abiertas para el torneo: {Nombre}");
            }
            else { Console.Error.WriteLine($"No se pueden abrir inscripciones. Estado actual: {Estado}"); }
        }

        public void CerrarInscripciones()
        {
            if (Estado == EstadoTorneo.InscripcionAbierta)
            {
                Estado = EstadoTorneo.InscripcionCerrada;
                long countConfirmados = _inscripcionesRegistradas.Count(i => i.EstadoInscripcion == GestionParticipantes.Enums.EstadoInscripcion.Confirmada);
                Console.WriteLine($"Inscripciones cerradas para: {Nombre}. Total confirmados: {countConfirmados}");
            }
            else { Console.Error.WriteLine($"No se pueden cerrar inscripciones. Estado actual: {Estado}"); }
        }

        public void IniciarTorneo()
        {
            if (Estado == EstadoTorneo.InscripcionCerrada)
            {
                if (GetParticipantesInscritosConfirmados().Count < 2)
                {
                    Console.Error.WriteLine($"El torneo '{Nombre}' no puede iniciar con menos de 2 participantes confirmados.");
                    return;
                }
                Estado = EstadoTorneo.EnCurso;
                Console.WriteLine($"El torneo '{Nombre}' ha comenzado.");
            }
            else { Console.Error.WriteLine($"El torneo no puede iniciar. Estado actual: {Estado}."); }
        }

        public void FinalizarTorneo()
        {
            if (Estado == EstadoTorneo.EnCurso)
            {
                Estado = EstadoTorneo.Finalizado;
                Console.WriteLine($"El torneo '{Nombre}' ha finalizado.");
            }
            else { Console.Error.WriteLine($"El torneo no puede finalizar. Estado actual: {Estado}."); }
        }

        public bool Equals(Torneo? other) => other != null && IdTorneo.Equals(other.IdTorneo);
        public override bool Equals(object? obj) => Equals(obj as Torneo);
        public override int GetHashCode() => IdTorneo.GetHashCode();
        public override string ToString() => $"Torneo{{Id={IdTorneo}, Nombre='{Nombre}', Estado={Estado}}}";
        public static bool operator ==(Torneo? left, Torneo? right) => EqualityComparer<Torneo>.Default.Equals(left, right);
        public static bool operator !=(Torneo? left, Torneo? right) => !(left == right);
    }
}