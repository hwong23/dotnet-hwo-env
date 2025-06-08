using System;
using System.Collections.Generic;
using TorneoTenisMesa.GestionTorneos.Enums;

// --- Usings para clases de OTROS SUBSISTEMAS (conceptuales por ahora) ---
// using TorneoTenisMesa.GestionParticipantes.Modelos;
// using TorneoTenisMesa.GestionPartidos.Modelos;
// using TorneoTenisMesa.GestionUsuarios.Modelos;

namespace TorneoTenisMesa.GestionTorneos.Modelos
{
    /// <summary>
    /// Entidad principal que representa un torneo de tenis de mesa.
    /// Contiene la información general del torneo, su estado, formato y
    /// referencias a las entidades relacionadas.
    /// </summary>
    public class Torneo : IEquatable<Torneo>
    {
        public Guid IdTorneo { get; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; } // Puede ser nulo si aún no ha terminado
        public string Lugar { get; set; }
        public TipoTorneo TipoTorneo { get; set; }
        public IFormatoTorneo Formato { get; set; } // Referencia a la interfaz del formato
        public EstadoTorneo Estado { get; private set; } // El estado se modifica mediante métodos
        public string? ReglasEspecificas { get; set; } // Puede ser nulo

        // --- Relaciones con otros subsistemas (Tipos conceptuales) ---
        // public Usuario Organizador { get; set; }
        // public List<Inscripcion> Inscripciones { get; private set; }
        // public List<object> FasesTorneo { get; private set; } // Podría ser List<Ronda> o List<Grupo> o una abstracción común

        // Atributo de ejemplo para el organizador (se reemplazaría con la entidad Usuario real)
        public Guid? IdOrganizador { get; set; } // Temporalmente, hasta integrar con subsistema de Usuarios

        public Torneo(string nombre, DateTime fechaInicio, string lugar, TipoTorneo tipoTorneo, IFormatoTorneo formato)
        {
            IdTorneo = Guid.NewGuid();
            Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre), "El nombre del torneo no puede ser nulo.");
            FechaInicio = fechaInicio; // DateTime es un struct, no puede ser null por defecto. Se podría usar DateTime? si fuera opcional.
            Lugar = lugar; // Puede ser nulo o vacío si se permite
            TipoTorneo = tipoTorneo; // Enum, no puede ser nulo
            Formato = formato ?? throw new ArgumentNullException(nameof(formato), "El formato del torneo no puede ser nulo.");
            Estado = EstadoTorneo.Planificado; // Estado inicial por defecto

            // Inscripciones = new List<Inscripcion>();
            // FasesTorneo = new List<object>();
        }

        // --- Métodos para gestionar relaciones (conceptuales por ahora) ---

        // public void AgregarInscripcion(Inscripcion inscripcion)
        // {
        //     this.Inscripciones.Add(inscripcion);
        // }

        // public IReadOnlyList<Inscripcion> GetInscripciones()
        // {
        //     return Inscripciones.AsReadOnly(); // Devuelve vista de solo lectura
        // }

        // public void AgregarFase(object fase) // Fase puede ser Ronda o Grupo
        // {
        //     this.FasesTorneo.Add(fase);
        // }

        // public IReadOnlyList<object> GetFasesTorneo()
        // {
        //     return FasesTorneo.AsReadOnly();
        // }

        // --- Métodos de Lógica de Negocio (ejemplos) ---

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
                Console.WriteLine($"Inscripciones cerradas para el torneo: {Nombre}");
                // Aquí se podría disparar la lógica para generar cuadros/grupos vía el FormatoTorneo
            }
            else
            {
                Console.Error.WriteLine($"No se pueden cerrar inscripciones. Estado actual: {Estado}");
            }
        }

        public void IniciarTorneo()
        {
            // En un sistema real: && cuadrosGenerados
            if (this.Estado == EstadoTorneo.InscripcionCerrada)
            {
                this.Estado = EstadoTorneo.EnCurso;
                Console.WriteLine($"El torneo '{Nombre}' ha comenzado.");
            }
            else
            {
                Console.Error.WriteLine($"El torneo no puede iniciar. Estado actual: {Estado}. Asegúrese que las inscripciones estén cerradas y los cuadros generados.");
            }
        }

        public void FinalizarTorneo()
        {
            // En un sistema real: && todosLosPartidosFinalizados
            if (this.Estado == EstadoTorneo.EnCurso)
            {
                this.Estado = EstadoTorneo.Finalizado;
                Console.WriteLine($"El torneo '{Nombre}' ha finalizado.");
            }
            else
            {
                Console.Error.WriteLine($"El torneo no puede finalizar. Estado actual: {Estado}. Asegúrese que todos los partidos hayan concluido.");
            }
        }


        // --- Sobrescritura de Equals, GetHashCode y ToString ---

        public override bool Equals(object? obj)
        {
            return Equals(obj as Torneo);
        }

        public bool Equals(Torneo? other)
        {
            return other != null && IdTorneo.Equals(other.IdTorneo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdTorneo);
        }

        public override string ToString()
        {
            return $"Torneo{{IdTorneo={IdTorneo}, Nombre='{Nombre}', FechaInicio={FechaInicio:yyyy-MM-dd}, Estado={Estado}, TipoTorneo={TipoTorneo}, Formato={(Formato != null ? Formato.NombreFormato : "N/A")}}}";
        }

        public static bool operator ==(Torneo? left, Torneo? right)
        {
            return EqualityComparer<Torneo>.Default.Equals(left, right);
        }

        public static bool operator !=(Torneo? left, Torneo? right)
        {
            return !(left == right);
        }
    }
}