namespace TorneoTenisMesa.GestionTorneos.Enums
{
    /// <summary>
    /// Define los posibles estados de un torneo a lo largo de su ciclo de vida.
    /// Ej: PLANIFICADO, INSCRIPCION_ABIERTA, EN_CURSO, FINALIZADO, CANCELADO.
    /// </summary>
    public enum EstadoTorneo
    {
        Planificado,
        InscripcionAbierta,
        InscripcionCerrada,
        EnCurso,
        Finalizado,
        Cancelado
    }
}