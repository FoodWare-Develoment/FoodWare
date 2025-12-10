using Dapper;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FoodWare.Model.DataAccess
{
    public class ConfiguracionSqlRepository : IConfiguracionRepository
    {
        private readonly string _connectionString;

        public ConfiguracionSqlRepository()
        {
            _connectionString = Controller.Program.Configuration.GetConnectionString("FoodWareDB")!;
        }

        public async Task ActualizarConfiguracionAsync(ConfiguracionSistema config)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = @"
                UPDATE ConfiguracionSistema
                SET NombreRestaurante = @NombreRestaurante,
                    Direccion = @Direccion,
                    PorcentajeImpuesto = @PorcentajeImpuesto,
                    Moneda = @Moneda,
                    MensajeTicket = @MensajeTicket
                WHERE IdConfig = 1;";

            await connection.ExecuteAsync(sql, config);
        }

        public async Task<ConfiguracionSistema> ObtenerConfiguracionAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT * FROM ConfiguracionSistema WHERE IdConfig = 1;";

            return await connection.QuerySingleAsync<ConfiguracionSistema>(sql);
        }
    }
}