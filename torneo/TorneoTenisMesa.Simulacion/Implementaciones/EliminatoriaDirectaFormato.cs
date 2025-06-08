using System;
using System.Collections.Generic;
using System.Linq;
using TorneoTenisMesa.GestionTorneos.Modelos; // Para IFormatoTorneo
using TorneoTenisMesa.Simulacion.ModelosMock;  // Para Jugador, Partido, Ronda

namespace TorneoTenisMesa.Simulacion.Implementaciones
{
    public class EliminatoriaDirectaFormato : IFormatoTorneo
    {
        public Guid IdFormato { get; } = Guid.NewGuid();
        public string NombreFormato => "Eliminatoria Directa";
        public string Descripcion => "Los jugadores compiten en rondas, el perdedor es eliminado.";

        private static readonly Random rng = new Random();

        /// <summary>
        /// Genera la ronda inicial a partir de una lista de jugadores.
        /// Para simplificar, asume que el número de jugadores es una potencia de 2.
        /// </summary>
        /// <param name="jugadores">Lista de jugadores inscritos.</param>
        /// <param name="numeroRonda">El número de la ronda a generar (ej. 1 para la primera).</param>
        /// <returns>La ronda generada con sus partidos.</returns>
        public Ronda GenerarRonda(List<Jugador> jugadores, int numeroRonda)
        {
            var ronda = new Ronda(numeroRonda);

            // Barajamos a los jugadores para que los emparejamientos sean aleatorios
            var jugadoresBarajados = jugadores.OrderBy(j => rng.Next()).ToList();

            if (jugadoresBarajados.Count % 2 != 0 && jugadoresBarajados.Count > 1)
            {
                // Manejo simple de impares: el último "pasa" (bye) si no es la final.
                // En un sistema real, esto sería más complejo (asignar byes estratégicamente).
                // Por ahora, para esta simulación, asumiremos que siempre llegan en pares a las rondas internas.
                // O podríamos lanzar una excepción si no es potencia de 2.
                Console.WriteLine($"Advertencia: Número impar de jugadores ({jugadoresBarajados.Count}) para la ronda {numeroRonda}. Se requiere un número par para emparejamientos simples.");
                 // Opcional: si es crítico, lanzar una excepción
                // throw new InvalidOperationException("Se requiere un número par de jugadores para generar la ronda en este formato simple.");
            }


            for (int i = 0; i < jugadoresBarajados.Count - (jugadoresBarajados.Count % 2) ; i += 2) // Aseguramos que no accedamos fuera de rango si hay impares
            {
                if (i + 1 < jugadoresBarajados.Count)
                {
                    Jugador jugador1 = jugadoresBarajados[i];
                    Jugador jugador2 = jugadoresBarajados[i + 1];
                    ronda.AgregarPartido(new Partido(jugador1, jugador2));
                }
            }
            return ronda;
        }

        // Los métodos conceptuales de IFormatoTorneo que no se usan en esta simulación básica
        // podrían lanzar NotImplementedException o dejarse como están si solo se necesitan las propiedades.
        // public object GenerarCuadro(GestionTorneos.Modelos.Torneo torneo, List<object> participantesInscritos) => throw new NotImplementedException();
        // public object DeterminarSiguienteFase(GestionTorneos.Modelos.Torneo torneo, object faseActual) => throw new NotImplementedException();
    }
}