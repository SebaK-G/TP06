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


}