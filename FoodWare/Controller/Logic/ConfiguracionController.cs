using FoodWare.Model.DataAccess;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace FoodWare.Controller.Logic
{
    public class ConfiguracionController(IConfiguracionRepository repository)
    {
        private readonly IConfiguracionRepository _repository = repository;

        public async Task<ConfiguracionSistema> CargarConfiguracionAsync()
        {
            return await _repository.ObtenerConfiguracionAsync();
        }

        public async Task GuardarConfiguracionAsync(string nombre, string direccion,
            decimal impuesto, string moneda, string mensaje)
        {
            // 1. Validaciones
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del restaurante es obligatorio.");

            if (impuesto < 0 || impuesto > 1)
                throw new ArgumentException("El impuesto debe ser un valor decimal entre 0.00 y 1.00 (ej. 0.16 para 16%).");

            if (string.IsNullOrWhiteSpace(moneda))
                throw new ArgumentException("El símbolo de moneda es obligatorio.");

            // 2. Crear entidad
            var config = new ConfiguracionSistema
            {
                IdConfig = 1, // Siempre es 1
                NombreRestaurante = nombre,
                Direccion = direccion,
                PorcentajeImpuesto = impuesto,
                Moneda = moneda,
                MensajeTicket = mensaje
            };

            // 3. Persistir
            await _repository.ActualizarConfiguracionAsync(config);
        }
    }
}