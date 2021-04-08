using curso.api.Models.Usuarios;

namespace curso.api.Configuration
{
    public interface IAuthenticationService
    {
        string GerarToken(UsuarioViewModelOutput usuarioViewModelOutput);
    }
}
