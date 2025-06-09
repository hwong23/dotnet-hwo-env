// TorneoTenisMesa.Simulacion/GestionTorneos/Modelos/Torneo.cs
using System;
using System.Collections.Generic;
using System.Linq; // Para Select y ToList
using TorneoTenisMesa.GestionTorneos.Enums;
using TorneoTenisMesa.GestionParticipantes.Modelos; // <--- AÑADIR ESTE USING

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

        // --- NUEVAS ADICIONES PARA GESTIONAR INSCRIPCIONES ---
        private readonly List<Inscripcion> _inscripcionesRegistradas;
        public IReadOnlyList<Inscripcion> InscripcionesRegistradas => _inscripcionesRegistradas.AsReadOnly();

        public Torneo(string nombre, DateTime fechaInicio, string lugar, TipoTorneo tipoTorneo, IFormatoTorneo formato)
        {
            IdTorneo = Guid.NewGuid();
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre), "El nombre del torneo no puede ser nulo.");
            FechaInicio = fechaInicio;
            Lugar = lugar;
            TipoTorneo = tipoTorneo;
            Formato = formato ?? throw new ArgumentNullException(nameof(formato), "El formato del torneo no puede ser nulo.");
            Estado = EstadoTorneo.Planificado;
            _inscripcionesRegistradas = new List<Inscripcion>(); // <--- INICIALIZAR LISTA
        }

        // --- NUEVO MÉTODO PARA AGREGAR INSCRIPCIÓN ---
        public bool AgregarInscripcion(Inscripcion inscripcion)
        {
            if (Estado != EstadoTorneo.InscripcionAbierta)
            {
                Console.Error.WriteLine($"Error: No se puede agregar inscripción. El torneo '{Nombre}' no está abierto para inscripciones. Estado actual: {Estado}");
                return false;
            }
            if (inscripcion.IdTorneo != this.IdTorneo)
            {
                Console.Error.WriteLine("Error: La inscripción no pertenece a este torneo.");
                return false;
            }
            if (_inscripcionesRegistradas.Any(i => i.IdInscripcion == inscripcion.IdInscripcion || i.Participante.IdParticipante == inscripcion.Participante.IdParticipante))
            {
                Console.Error.WriteLine($"Error: El participante '{inscripcion.Participante.GetNombreDescriptivo()}' ya está inscrito o la inscripción ya existe.");
                return false;
            }

            _inscripcionesRegistradas.Add(inscripcion);
            inscripcion.ConfirmarInscripcion(); // Marcar la inscripción como confirmada
            Console.WriteLine($"Inscripción de '{inscripcion.Participante.GetNombreDescriptivo()}' registrada y confirmada para el torneo '{Nombre}'.");
            return true;
        }

        // --- NUEVO MÉTODO PARA OBTENER PARTICIPANTES INSCRITOS ---
        public List<Participante> GetParticipantesInscritos()
        {
            return _inscripcionesRegistradas
                   .Where(i => i.EstadoInscripcion == GestionParticipantes.Enums.EstadoInscripcion.Confirmada)
                   .Select(i => i.Participante)
                   .ToList();
        }


        public void AbrirInscripciones()
        {
            if (this.Estado == EstadoTorneo.Planificado)
            {
                this.Estado = EstadoTorneo.InscripcionAbierta;
                Console.WriteLine($"Inscripciones abiertas para el torneo: {Nombre}");
            }
            else
            {
                Console.Error.WriteLine($"No se pueden abrir inscripciones. Estado actual: {Estado}");
            }
        }

        public void CerrarInscripciones()
        {
            if (this.Estado == EstadoTorneo.InscripcionAbierta)
            {
                this.Estado = EstadoTorneo.InscripcionCerrada;
                Console.WriteLine($"Inscripciones cerradas para el torneo: {Nombre}. Total inscritos: {_inscripcionesRegistradas.Count(i => i.EstadoInscripcion == GestionParticipantes.Enums.EstadoInscripcion.Confirmada)}");
            }
            else
            {
                Console.Error.WriteLine($"No se pueden cerrar inscripciones. Estado actual: {Estado}");
            }
        }

        public void IniciarTorneo()
        {
            if (this.Estado == EstadoTorneo.InscripcionCerrada)
            {
                if (GetParticipantesInscritos().Count < 2) {
                     Console.Error.WriteLine($"El torneo '{Nombre}' no puede iniciar con menos de 2 participantes.");
                     return;
                }
                this.Estado = EstadoTorneo.EnCurso;
                Console.WriteLine($"El torneo '{Nombre}' ha comenzado.");
            }
            else
            {
                Console.Error.WriteLine($"El torneo no puede iniciar. Estado actual: {Estado}. Asegúrese que las inscripciones estén cerradas.");
            }
        }

        public void FinalizarTorneo()
        {
            if (this.Estado == EstadoTorneo.EnCurso)
            {
                this.Estado = EstadoTorneo.Finalizado;
                Console.WriteLine($"El torneo '{Nombre}' ha finalizado.");
            }
            else
            {
                Console.Error.WriteLine($"El torneo no puede finalizar. Estado actual: {Estado}.");
            }
        }

        public override bool Equals(object? obj) => Equals(obj as Torneo);
        public bool Equals(Torneo? other) => other != null && IdTorneo.Equals(other.IdTorneo);
        public override int GetHashCode() => HashCode.Combine(IdTorneo);
        public override string ToString() => $"Torneo{{Id={IdTorneo}, Nombre='{Nombre}', Estado={Estado}}}";
        public static bool operator ==(Torneo? left, Torneo? right) => EqualityComparer<Torneo>.Default.Equals(left, right);
        public static bool operator !=(Torneo? left, Torneo? right) => !(left == right);
    }
}