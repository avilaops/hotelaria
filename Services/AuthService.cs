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
        
        // Rate limiting
        private readonly Dictionary<string, (int attempts, DateTime lockUntil)> _loginAttempts = new();
        private const int MaxLoginAttempts = 5;
        private const int LockoutMinutes = 15;

        public event Action? OnAuthStateChanged;

        public AuthService()
        {
            // Criar usuário admin padrão
            AdicionarUsuario(new Usuario
            {
                Nome = "Administrador",
                Email = "admin@hotelaria.com",
                Username = "admin",
                SenhaHash = HashSenha("admin123"),
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

        // Autenticação com Rate Limiting
        public bool Login(string username, string senha)
        {
            // Verificar bloqueio por tentativas excessivas
            if (IsAccountLocked(username))
            {
                return false;
            }

            var usuario = _usuarios.FirstOrDefault(u => 
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
                u.Ativo);

            if (usuario == null)
            {
                RegisterFailedAttempt(username);
                return false;
            }

            if (!VerificarSenha(senha, usuario.SenhaHash))
            {
                RegisterFailedAttempt(username);
                return false;
            }

            // Login bem-sucedido - limpar tentativas
            if (_loginAttempts.ContainsKey(username))
            {
                _loginAttempts.Remove(username);
            }

            usuario.UltimoAcesso = DateTime.Now;
            _sessaoAtual = new SessaoUsuario
            {
                Usuario = usuario,
                DataLogin = DateTime.Now
            };

            OnAuthStateChanged?.Invoke();
            return true;
        }

        private bool IsAccountLocked(string username)
        {
            if (!_loginAttempts.ContainsKey(username))
                return false;

            var (attempts, lockUntil) = _loginAttempts[username];
            
            if (attempts >= MaxLoginAttempts && DateTime.Now < lockUntil)
            {
                return true;
            }

            // Limpar bloqueio expirado
            if (DateTime.Now >= lockUntil)
            {
                _loginAttempts.Remove(username);
                return false;
            }

            return false;
        }

        private void RegisterFailedAttempt(string username)
        {
            if (_loginAttempts.ContainsKey(username))
            {
                var (attempts, lockUntil) = _loginAttempts[username];
                attempts++;
                
                if (attempts >= MaxLoginAttempts)
                {
                    _loginAttempts[username] = (attempts, DateTime.Now.AddMinutes(LockoutMinutes));
                }
                else
                {
                    _loginAttempts[username] = (attempts, lockUntil);
                }
            }
            else
            {
                _loginAttempts[username] = (1, DateTime.MinValue);
            }
        }

        public int GetRemainingAttempts(string username)
        {
            if (!_loginAttempts.ContainsKey(username))
                return MaxLoginAttempts;

            var (attempts, _) = _loginAttempts[username];
            return Math.Max(0, MaxLoginAttempts - attempts);
        }

        public DateTime? GetLockoutTime(string username)
        {
            if (!_loginAttempts.ContainsKey(username))
                return null;

            var (attempts, lockUntil) = _loginAttempts[username];
            
            if (attempts >= MaxLoginAttempts && lockUntil > DateTime.Now)
                return lockUntil;

            return null;
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

            if (!VerificarSenha(senhaAtual, usuario.SenhaHash))
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

        // Hash de Senha com PBKDF2 (SEGURO)
        public static string HashSenha(string senha)
        {
            // Gerar salt aleatório de 128 bits
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derivar hash com PBKDF2
            // 100.000 iterações = recomendação OWASP 2023
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(senha),
                salt,
                100000,
                HashAlgorithmName.SHA256,
                256 / 8
            );

            // Retornar salt:hash em Base64
            return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        }

        // Verificar senha com PBKDF2
        private static bool VerificarSenha(string senha, string hashArmazenado)
        {
            try
            {
                var parts = hashArmazenado.Split(':');
                if (parts.Length != 2)
                {
                    // Fallback para SHA256 antigo (compatibilidade)
                    using (var sha256 = SHA256.Create())
                    {
                        var bytes = Encoding.UTF8.GetBytes(senha);
                        var hash = sha256.ComputeHash(bytes);
                        return hashArmazenado == Convert.ToBase64String(hash);
                    }
                }

                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] hashOriginal = Convert.FromBase64String(parts[1]);

                byte[] hashNovo = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(senha),
                    salt,
                    100000,
                    HashAlgorithmName.SHA256,
                    256 / 8
                );

                return CryptographicOperations.FixedTimeEquals(hashOriginal, hashNovo);
            }
            catch
            {
                return false;
            }
        }

        // Utilitários
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
