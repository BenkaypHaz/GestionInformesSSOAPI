using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.DataBases;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionsInformesSSOAPI.Features.Repository
{
    public class UsuarioRepository
    {
        private readonly GestionInformesSSO _context;
        public UsuarioRepository(GestionInformesSSO context)
        {
            _context = context;
        }

        public async Task<List<Usuarios>> GetAllUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuarios> GetUserByIdAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task<List<Roles>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Usuarios> AddUsuarioAsync(Usuarios user)
        {
            string salt;
            user.ClaveEncriptada = Security.HashPassword(user.ClaveEncriptada, out salt);
            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(int id, Usuarios userToUpdate) 
        {
            var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (existingUser != null)
            {
                existingUser.Usuario = userToUpdate.Usuario;
                existingUser.Nombre = userToUpdate.Nombre;
                existingUser.Correo = userToUpdate.Correo;
                existingUser.IdRol = userToUpdate.IdRol;

                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (user != null)
            {
                user.activo = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Usuarios> GetUserByUsernameAsync(string username)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Usuario == username);
        }

    }
}
