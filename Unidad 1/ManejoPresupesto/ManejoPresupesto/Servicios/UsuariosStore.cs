using ManejoPresupesto.Models;
using Microsoft.AspNetCore.Identity;

namespace ManejoPresupesto.Servicios;

public class UsuariosStore :
    IUserStore<Usuario>,
    IUserEmailStore<Usuario>,
    IUserPasswordStore<Usuario>
{
    private readonly IRepositorioUsuarios _repositorioUsuarios;

    public UsuariosStore(IRepositorioUsuarios repositorioUsuarios)
    {
        _repositorioUsuarios = repositorioUsuarios;
    }

    public void Dispose()
    {
    }

    public Task<string> GetUserIdAsync(Usuario user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(Usuario user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Email);
    }

    public Task SetUserNameAsync(Usuario user, string userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetNormalizedUserNameAsync(Usuario user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedUserNameAsync(Usuario user, string normalizedName, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> CreateAsync(Usuario user, CancellationToken cancellationToken)
    {
        user.Id = await _repositorioUsuarios.CrearUsuarioAsync(user);
        return IdentityResult.Success;
    }

    public Task<IdentityResult> UpdateAsync(Usuario user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(Usuario user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Usuario> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Usuario> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return await _repositorioUsuarios.BuscarUsuarioPorEmail(normalizedUserName);
    }

    public Task SetEmailAsync(Usuario user, string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetEmailAsync(Usuario user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(Usuario user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetEmailConfirmedAsync(Usuario user, bool confirmed, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Usuario> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        return await _repositorioUsuarios.BuscarUsuarioPorEmail(normalizedEmail);
    }

    public Task<string> GetNormalizedEmailAsync(Usuario user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedEmailAsync(Usuario user, string normalizedEmail, CancellationToken cancellationToken)
    {
        user.EmailNormalizado = normalizedEmail;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(Usuario user, string passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string> GetPasswordHashAsync(Usuario user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(Usuario user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}