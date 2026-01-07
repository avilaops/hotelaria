using Hotelaria.Models;
using System.Security.Cryptography;
using System.Text;

namespace Hotelaria.Services
{
    public class AuthService
    {
        private readonly List<Usuario> _usuarios = new();
        private int _nextId = 1;
        private SessaoUsuario _sessaoAtual = new();

        public event Action? OnAuthStateChanged;

        public AuthService()
        {
            // Criar usuário admin padrão
            var senhaHash = HashSenha("admin123");
            AdicionarUsuario(new Usuario
            {
                Nome = "Administrador",
                Email = "admin@hotelaria.com",
                Username = "admin",
                SenhaHash = senhaHash,
                Perfil = PerfilUsuario.Administrador,
                Ativo = true
            });

            // Criar usuário gerente de exemplo
            AdicionarUsuario(new Usuario
            {
                Nome = "Maria Silva",
                Email = "maria@hotelaria.com",
                Username = "maria",
                SenhaHash = HashSenha("maria123"),
                Perfil = PerfilUsuario.Gerente,
                Ativo = true
            });

            // Criar recepcionista de exemplo
            AdicionarUsuario(new Usuario
            {
                Nome = "João Santos",
                Email = "joao@hotelaria.com",
                Username = "joao",
                SenhaHash = HashSenha("joao123"),
                Perfil = PerfilUsuario.Recepcionista,
                Ativo = true
            });
        }

        // Autenticação
        public bool Login(string username, string senha)
        {
            var usuario = _usuarios.FirstOrDefault(u => 
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
                u.Ativo);

            if (usuario == null)
                return false;

            var senhaHash = HashSenha(senha);
            if (usuario.SenhaHash != senhaHash)
                return false;

            usuario.UltimoAcesso = DateTime.Now;
            _sessaoAtual = new SessaoUsuario
            {
                Usuario = usuario,
                DataLogin = DateTime.Now
            };

            OnAuthStateChanged?.Invoke();
            return true;
        }

        public void Logout()
        {
            _sessaoAtual = new SessaoUsuario();
            OnAuthStateChanged?.Invoke();
        }

        public SessaoUsuario ObterSessaoAtual() => _sessaoAtual;

        public Usuario? ObterUsuarioAtual() => _sessaoAtual.Usuario;

        public bool EstaAutenticado() => _sessaoAtual.EstaAutenticado;

        public bool TemPermissao(PerfilUsuario perfilMinimo)
        {
            if (!EstaAutenticado())
                return false;

            var usuario = ObterUsuarioAtual();
            if (usuario == null)
                return false;

            // Hierarquia: Administrador > Gerente > Recepcionista > Visualizador
            return usuario.Perfil <= perfilMinimo;
        }

        // Gestão de Usuários
        public List<Usuario> ObterTodos() => _usuarios;

        public Usuario? ObterPorId(int id) => _usuarios.FirstOrDefault(u => u.Id == id);

        public Usuario? ObterPorUsername(string username) => 
            _usuarios.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        public bool AdicionarUsuario(Usuario usuario)
        {
            // Verificar se username já existe
            if (ObterPorUsername(usuario.Username) != null)
                return false;

            usuario.Id = _nextId++;
            usuario.DataCriacao = DateTime.Now;
            _usuarios.Add(usuario);
            return true;
        }

        public bool AtualizarUsuario(Usuario usuario)
        {
            var index = _usuarios.FindIndex(u => u.Id == usuario.Id);
            if (index == -1)
                return false;

            // Verificar se o novo username já está em uso por outro usuário
            var usuarioComMesmoUsername = ObterPorUsername(usuario.Username);
            if (usuarioComMesmoUsername != null && usuarioComMesmoUsername.Id != usuario.Id)
                return false;

            _usuarios[index] = usuario;
            return true;
        }

        public bool RemoverUsuario(int id)
        {
            // Não permitir remover o próprio usuário logado
            if (_sessaoAtual.Usuario?.Id == id)
                return false;

            // Não permitir remover o último admin
            var usuario = ObterPorId(id);
            if (usuario?.Perfil == PerfilUsuario.Administrador)
            {
                var admins = _usuarios.Count(u => u.Perfil == PerfilUsuario.Administrador && u.Ativo);
                if (admins <= 1)
                    return false;
            }

            var user = _usuarios.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _usuarios.Remove(user);
                return true;
            }

            return false;
        }

        public bool AlterarSenha(int usuarioId, string senhaAtual, string novaSenha)
        {
            var usuario = ObterPorId(usuarioId);
            if (usuario == null)
                return false;

            var senhaAtualHash = HashSenha(senhaAtual);
            if (usuario.SenhaHash != senhaAtualHash)
                return false;

            usuario.SenhaHash = HashSenha(novaSenha);
            return true;
        }

        public void RedefinirSenha(int usuarioId, string novaSenha)
        {
            var usuario = ObterPorId(usuarioId);
            if (usuario != null)
            {
                usuario.SenhaHash = HashSenha(novaSenha);
            }
        }

        // Utilitários
        public static string HashSenha(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(senha);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public string ObterNomePerfil(PerfilUsuario perfil)
        {
            return perfil switch
            {
                PerfilUsuario.Administrador => "Administrador",
                PerfilUsuario.Gerente => "Gerente",
                PerfilUsuario.Recepcionista => "Recepcionista",
                PerfilUsuario.Visualizador => "Visualizador",
                _ => "Desconhecido"
            };
        }

        public List<Usuario> FiltrarPorPerfil(PerfilUsuario? perfil)
        {
            if (perfil == null)
                return _usuarios;

            return _usuarios.Where(u => u.Perfil == perfil).ToList();
        }

        public List<Usuario> BuscarUsuarios(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return _usuarios;

            termo = termo.ToLower();
            return _usuarios.Where(u =>
                u.Nome.ToLower().Contains(termo) ||
                u.Username.ToLower().Contains(termo) ||
                u.Email.ToLower().Contains(termo)
            ).ToList();
        }
    }
}
