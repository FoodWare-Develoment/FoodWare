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
            // 1. Validaciones de negocio
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                throw new ArgumentException("El nombre completo no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                throw new ArgumentException("La contraseña no puede estar vacía y debe tener al menos 6 caracteres.");
            if (idRol <= 0)
                throw new ArgumentException("Debe seleccionar un rol válido.");

            // 2. Lógica de negocio (Hashing de contraseña)
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // 3. Crear la entidad
            Usuario nuevo = new()
            {
                NombreCompleto = nombreCompleto,
                NombreUsuario = nombreUsuario,
                ContraseñaHash = passwordHash,
                IdRol = idRol,
                Activo = activo
            };

            // 4. Llamar al repositorio
            await _usuarioRepo.AgregarAsync(nuevo);
        }

        public async Task ActualizarEmpleadoAsync(int idUsuario, string nombreCompleto, string nombreUsuario, int idRol, bool activo, string rolDelEditado)
        {

            // 1. Jerarquía
            if (UserSession.NombreRol == "Gerente" && rolDelEditado == "Administrador")
            {
                throw new InvalidOperationException("Un Gerente no tiene permisos para modificar una cuenta de Administrador.");
            }

            // 2. Auto-Desactivación
            if (idUsuario == UserSession.IdUsuario && !activo)
            {
                throw new InvalidOperationException("No puede desactivar su propia cuenta mientras tiene la sesión activa.");
            }

            // 3. Validaciones
            if (idUsuario <= 0)
                throw new ArgumentException("ID de usuario no válido.");
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                throw new ArgumentException("El nombre completo no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("El nombre de usuario no puede estar vacío.");
            if (idRol <= 0)
                throw new ArgumentException("Debe seleccionar un rol válido.");

            // 4. Crear la entidad
            Usuario actualizado = new()
            {
                IdUsuario = idUsuario,
                NombreCompleto = nombreCompleto,
                NombreUsuario = nombreUsuario,
                IdRol = idRol,
                Activo = activo,
                ContraseñaHash = "" // El repo no usará esto
            };

            // 5. Llamar al repositorio
            await _usuarioRepo.ActualizarAsync(actualizado);
        }

        public async Task ResetearPasswordAsync(int idUsuario, string nuevaPassword)
        {
            // 1. Validaciones
            if (idUsuario <= 0)
                throw new ArgumentException("ID de usuario no válido.");
            if (string.IsNullOrWhiteSpace(nuevaPassword) || nuevaPassword.Length < 6)
                throw new ArgumentException("La nueva contraseña no puede estar vacía y debe tener al menos 6 caracteres.");

            // 2. Hashear la nueva contraseña
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(nuevaPassword);

            // 3. Llamar al nuevo método del repositorio
            await _usuarioRepo.ActualizarPasswordAsync(idUsuario, passwordHash);
        }
    }
}