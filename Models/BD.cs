using Microsoft.Data.SqlClient;
using Dapper;
using TP06.Models;

namespace TP06.Models;

public class BD{
    private string _connectionString = @"Server=localhost;Database=TP06;
    Integrated Security=True;TrustServerCertificate=True;";

    public void CrearPartida(Partidas partida){
        string query = "INSERT INTO Partidas (NombreParticipante, FechaInicio) VALUES (@pNombreParticipante, @pFechaInicio)";
        using (SqlConnection connection = new SqlConnection(_connectionString)){
            connection.Execute(query, new { pNombreParticipante = partida.NombreParticipante, pFechaInicio = DateTime.Now });
        }
    }

    public Partidas ObtenerPartidaPorNombre(string nombre){
        using (SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "SELECT TOP 1 * FROM Partidas WHERE NombreParticipante = @pNombre ORDER BY Id DESC";
            return connection.QueryFirstOrDefault<Partidas>(query, new { pNombre = nombre });
        }
    }

    public void ReiniciarPartida(int idPartida){
        using (SqlConnection connection = new SqlConnection(_connectionString)){
            connection.Execute("DELETE FROM SalasEstado WHERE IdPartida = @pIdPartida", new { pIdPartida = idPartida });
            connection.Execute("UPDATE Partidas SET FechaInicio = @pFechaInicio WHERE Id = @pIdPartida", new { pIdPartida = idPartida, pFechaInicio = DateTime.Now });
        }
    }

    public void GuardarEstadoSala(int idPartida, int numeroSala, string respuesta, string pista){
        string query = @"
            IF EXISTS (
                SELECT 1 FROM SalasEstado WHERE IdPartida = @pIdPartida AND NumeroSala = @pNumeroSala
            )
                UPDATE SalasEstado
                SET Respuesta = @pRespuesta,
                    Pista = @pPista
                WHERE IdPartida = @pIdPartida AND NumeroSala = @pNumeroSala;
            ELSE
                INSERT INTO SalasEstado (IdPartida, NumeroSala, Respuesta, Pista)
                VALUES (@pIdPartida, @pNumeroSala, @pRespuesta, @pPista);";

        using (SqlConnection connection = new SqlConnection(_connectionString)){
            connection.Execute(query, new {
                pIdPartida = idPartida,
                pNumeroSala = numeroSala,
                pRespuesta = respuesta,
                pPista = pista
            });
        }
    }

    public bool TieneSalaCompletada(int idPartida, int numeroSala){
        using (SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "SELECT COUNT(1) FROM SalasEstado WHERE IdPartida = @pIdPartida AND NumeroSala = @pNumeroSala";
            int cantidad = connection.ExecuteScalar<int>(query, new { pIdPartida = idPartida, pNumeroSala = numeroSala });
            return cantidad > 0;
        }
    }

    public int? ObtenerUltimaSalaCompletada(int idPartida){
        using (SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "SELECT MAX(NumeroSala) FROM SalasEstado WHERE IdPartida = @pIdPartida";
            return connection.ExecuteScalar<int?>(query, new { pIdPartida = idPartida });
        }
    }

    public Partidas ExisteNombre(string nombre){
        return ObtenerPartidaPorNombre(nombre);
    }

    public int CalcularTiempoPartida(string nombre){
        Partidas partida = ObtenerPartidaPorNombre(nombre);
        if (partida == null){
            return 0;
        }

        return (int)(DateTime.Now - partida.FechaInicio).TotalSeconds;
    }
}