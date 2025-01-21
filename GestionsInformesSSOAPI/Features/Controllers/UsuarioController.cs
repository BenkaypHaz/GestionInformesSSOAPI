using GestionsInformesSSOAPI.Features.Services;
using GestionsInformesSSOAPI.Features.Utility;
using GestionsInformesSSOAPI.Infraestructure.Entities;
using GestionsInformesSSOAPI.Infraestructure.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace GestionsInformesSSOAPI.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioServices _usuarioService;

        public UsuarioController(UsuarioServices usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllUsuarios()
        {
            var response = await _usuarioService.GetAllUsuariosAsync();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddUsuario([FromBody] Usuarios user)
        {
            if (user == null)
            {
                return BadRequest(ApiResponse.BadRequest("Invalid user data"));
            }

            var response = await _usuarioService.AddUsuarioAsync(user);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetAllUsuarios), new { id = ((Usuarios)response.Data).IdUsuario }, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetUserById(int id)
        {
            var response = await _usuarioService.GetUserByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateUser(int id, [FromBody] Usuarios user)
        {
            var response = await _usuarioService.UpdateUserAsync(id, user);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("authenticate")]
        public async Task<ActionResult<ApiResponse>> Authenticate([FromBody] Credenciales credentials)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.BadRequest("Invalid request"));

            var response = await _usuarioService.AuthenticateUserAsync(credentials);
            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("GetRoles")]
        public async Task<ActionResult<ApiResponse>> GetAllCuentasDescriptions()
        {
            try
            {
                var roles = await _usuarioService.GetRoles();

                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
