using FoodWare.Shared.Entities;
using System.Threading.Tasks;

namespace FoodWare.Shared.Interfaces
{
    /// <summary>
    /// Define las operaciones para gestionar la configuración única del sistema.
    /// </summary>
    public interface IConfiguracionRepository
    {
        /// <summary>
        /// Obtiene la configuración actual.
        /// </summary>
        Task<ConfiguracionSistema> ObtenerConfiguracionAsync();

        /// <summary>
        /// Actualiza los parámetros de configuración.
        /// </summary>
        Task ActualizarConfiguracionAsync(ConfiguracionSistema config);
    }
}