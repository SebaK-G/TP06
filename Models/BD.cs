using Microsoft.Data.SqlClient;
using Dapper;
using TP06.Models;

namespace TP06.Models;

public class BD{
    private string _connectionString = @"Server=localhost\SQLEXPRESS;Database=TP06;
    Integrated Security=True;TrustServerCertificate=True;";

    public void CrearPartida(Partidas partida){
        string query = "INSERT INTO Partidas (NombreParticipante, FechaInicio) VALUES (@pNombreParticipante, @pFechaInicio)";
        using (SqlConnection connection = new SqlConnection(_connectionString)){
            connection.Execute(query, new { pNombreParticipante = partida.NombreParticipante, pFechaInicio = DateTime.Now });
        }
    }    
    public Partidas ExisteNombre(string nombre){
        Partidas partida = null;
        using (SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "SELECT TOP 1 * FROM Partidas WHERE NombreParticipante = @pNombre";
            partida = connection.QueryFirstOrDefault<Partidas>(query, new { pNombre = nombre });
        }
        return partida;
    }
    public int CalcularTiempoPartida(string nombre){
        using (SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "SELECT TOP 1 FechaInicio FROM Partidas WHERE NombreParticipante = @pNombre";
            DateTime fechaInicio = connection.QueryFirstOrDefault<DateTime>(query, new { pNombre = nombre });
            return (int)(DateTime.Now - fechaInicio).TotalSeconds;
        }
    }






}