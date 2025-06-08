using System;
// --- Usings para clases de OTROS SUBSISTEMAS (conceptuales por ahora) ---
// using TorneoTenisMesa.GestionParticipantes.Modelos;
// using TorneoTenisMesa.GestionPartidos.Modelos;

namespace TorneoTenisMesa.GestionTorneos.Modelos
{
    /// <summary>
    /// Interfaz que define el contrato para los diferentes formatos de torneo.
    /// Un formato determina cómo se estructura la competición (ej. Eliminatoria Directa, Round Robin).
    /// Sus implementaciones concretas se encargarán de la lógica de generación de cuadros
    /// y determinación de las siguientes fases.
    /// </summary>
    public interface IFormatoTorneo
    {
        /// <summary>
        /// Obtiene el identificador único del formato.
        /// </summary>
        Guid IdFormato { get; }

        /// <summary>
        /// Obtiene el nombre descriptivo del formato.
        /// </summary>
        string NombreFormato { get; }

        /// <summary>
        /// Obtiene una descripción más detallada del formato.
        /// </summary>
        string Descripcion { get; }

        /// <summary>
        /// Método conceptual para generar la estructura inicial de partidos (cuadro/grupos)
        /// para un torneo y una lista de participantes.
        /// La implementación real dependerá de las entidades de Partidos y Participantes.
        /// </summary>
        /// <param name="torneo">El torneo para el cual generar el cuadro.</param>
        /// <param name="participantesInscritos">Lista de participantes inscritos.</param>
        /// <returns>Una representación de la estructura de partidos (Ej: Cuadro, List<Grupo>).
        /// (Tipo de retorno es conceptual y dependerá de las clases del subsistema de Partidos)</returns>
        // object GenerarCuadro(Torneo torneo, List<ParticipanteInscrito> participantesInscritos);
        // Por ejemplo, podría ser:
        // TorneoTenisMesa.GestionPartidos.Modelos.Cuadro GenerarCuadro(
        //      Torneo torneo,
        //      List<TorneoTenisMesa.GestionParticipantes.Modelos.ParticipanteInscrito> participantesInscritos
        // );


        /// <summary>
        /// Método conceptual para determinar la siguiente fase o ronda del torneo.
        /// La implementación real dependerá de las entidades de Partidos y Resultados.
        /// </summary>
        /// <param name="torneo">El torneo en curso.</param>
        /// <param name="faseActual">La fase/ronda/grupo actual que ha concluido.</param>
        /// <returns>La siguiente fase del torneo.
        /// (Tipo de retorno es conceptual y dependerá de las clases del subsistema de Partidos)</returns>
        // object DeterminarSiguienteFase(Torneo torneo, object faseActual);
        // Por ejemplo, podría ser:
        // TorneoTenisMesa.GestionPartidos.Modelos.FaseTorneo DeterminarSiguienteFase(
        //      Torneo torneo,
        //      TorneoTenisMesa.GestionPartidos.Modelos.FaseTorneo faseActual
        // );
    }
}