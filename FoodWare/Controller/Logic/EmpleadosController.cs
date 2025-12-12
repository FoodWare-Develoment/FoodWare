using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodWare.Shared.Entities;
using FoodWare.Shared.Interfaces;
using BCrypt.Net;

namespace FoodWare.Controller.Logic
{
    public class EmpleadosController(IUsuarioRepository usuarioRepo, IRolRepository rolRepo)
    {
        private readonly IUsuarioRepository _usuarioRepo = usuarioRepo;
        private readonly IRolRepository _rolRepo = rolRepo;

        public async Task<List<UsuarioDto>> CargarEmpleadosAsync()
        {
            return await _usuarioRepo.ObtenerTodosDtoAsync();
        }

        public async Task<List<Rol>> CargarRolesAsync()
        {
            return await _rolRepo.ObtenerTodosAsync();
        }

        public async Task GuardarNuevoEmpleadoAsync(string nombreCompleto, string nombreUsuario, string password, int idRol, bool activo)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                throw new ArgumentException("El nombre completo no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                throw new ArgumentException("La contraseña no puede estar vacía y debe tener al menos 6 caracteres.");

            if (idRol <= 0)
                throw new ArgumentException("Debe seleccionar un rol válido.");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            Usuario nuevo = new()
            {
                NombreCompleto = nombreCompleto,
                NombreUsuario = nombreUsuario,
                ContraseñaHash = passwordHash,
                IdRol = idRol,
                Activo = activo
            };

            await _usuarioRepo.AgregarAsync(nuevo);
        }

        public async Task ActualizarEmpleadoAsync(int idUsuario, string nombreCompleto, string nombreUsuario, int idRol, bool activo, string rolDelEditado)
        {
            if (UserSession.NombreRol == "Gerente" && rolDelEditado == "Administrador")
            {
                throw new InvalidOperationException("Un Gerente no tiene permisos para modificar una cuenta de Administrador.");
            }

            if (idUsuario == UserSession.IdUsuario && !activo)
            {
                throw new InvalidOperationException("No puede desactivar su propia cuenta mientras tiene la sesión activa.");
            }

            if (idUsuario <= 0) throw new ArgumentException("ID de usuario no válido.");
            if (string.IsNullOrWhiteSpace(nombreCompleto)) throw new ArgumentException("El nombre completo no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(nombreUsuario)) throw new ArgumentException("El nombre de usuario no puede estar vacío.");
            if (idRol <= 0) throw new ArgumentException("Debe seleccionar un rol válido.");

            Usuario actualizado = new()
            {
                IdUsuario = idUsuario,
                NombreCompleto = nombreCompleto,
                NombreUsuario = nombreUsuario,
                IdRol = idRol,
                Activo = activo,
                ContraseñaHash = ""
            };

            await _usuarioRepo.ActualizarAsync(actualizado);
        }

        public async Task ResetearPasswordAsync(int idUsuario, string nuevaPassword)
        {
            if (idUsuario <= 0) throw new ArgumentException("ID de usuario no válido.");

            if (string.IsNullOrWhiteSpace(nuevaPassword) || nuevaPassword.Length < 6)
                throw new ArgumentException("La nueva contraseña no puede estar vacía y debe tener al menos 6 caracteres.");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(nuevaPassword);
            await _usuarioRepo.ActualizarPasswordAsync(idUsuario, passwordHash);
        }

        public async Task EliminarEmpleadoAsync(int idUsuario, string nombreRol)
        {
            if (idUsuario == UserSession.IdUsuario)
            {
                throw new InvalidOperationException("No puede eliminar su propia cuenta mientras está en uso.");
            }

            if (UserSession.NombreRol == "Gerente" && nombreRol == "Administrador")
            {
                throw new InvalidOperationException("Un Gerente no puede eliminar a un Administrador.");
            }

            await _usuarioRepo.EliminarAsync(idUsuario);
        }
    }
}