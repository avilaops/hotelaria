using Hotelaria.Models;
using System.Security.Cryptography;
using System.Text;

namespace Hotelaria.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private SessaoUsuario _sessaoAtual = new();
        
        // Rate limiting (por sessão)
        private readonly Dictionary<string, (int attempts, DateTime lockUntil)> _loginAttempts = new();
        private const int MaxLoginAttempts = 5;
        private const int LockoutMinutes = 15;

        public event Action? OnAuthStateChanged;

        public AuthService(UserRepository userRepository)
        {
            _userRepository = userRepository;
            // IMPORTANTE: Iniciar sem sessão ativa
            _sessaoAtual = new SessaoUsuario();
        }

        // Autenticação com Rate Limiting
        public bool Login(string username, string senha)
        {
            // Verificar bloqueio por tentativas excessivas
            if (IsAccountLocked(username))
            {
                return false;
            }

            var usuario = _userRepository.ObterPorUsername(username);

            if (usuario == null || !usuario.Ativo)
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

            // Atualizar último acesso
            usuario.UltimoAcesso = DateTime.Now;
            _userRepository.Atualizar(usuario);

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

        public bool EstaAutenticado() 
        {
            // IMPORTANTE: Validar que a sessão existe E tem usuário válido
            return _sessaoAtual != null && _sessaoAtual.Usuario != null && _sessaoAtual.Usuario.Ativo;
        }

        public bool TemPermissao(PerfilUsuario perfilMinimo)
        {
            if (!EstaAutenticado())
                return false;

            var usuario = ObterUsuarioAtual();
            if (usuario == null)
                return false;

            // Desenvolvedor tem permissão total sempre
            if (usuario.Perfil == PerfilUsuario.Desenvolvedor)
                return true;

            // Hierarquia: Desenvolvedor > Administrador > Gerente > Recepcionista > Visualizador
            return usuario.Perfil <= perfilMinimo;
        }

        // Gestão de Usuários (delegação para UserRepository)
        public List<Usuario> ObterTodos() => _userRepository.ObterTodos();

        public Usuario? ObterPorId(int id) => _userRepository.ObterPorId(id);

        public Usuario? ObterPorUsername(string username) => _userRepository.ObterPorUsername(username);

        public bool AdicionarUsuario(Usuario usuario)
        {
            return _userRepository.Adicionar(usuario);
        }

        public bool AtualizarUsuario(Usuario usuario)
        {
            return _userRepository.Atualizar(usuario);
        }

        public bool RemoverUsuario(int id)
        {
            return _userRepository.Remover(id, _sessaoAtual.Usuario?.Id);
        }

        public bool AlterarSenha(int usuarioId, string senhaAtual, string novaSenha)
        {
            var usuario = _userRepository.ObterPorId(usuarioId);
            if (usuario == null)
                return false;

            if (!VerificarSenha(senhaAtual, usuario.SenhaHash))
                return false;

            usuario.SenhaHash = HashSenha(novaSenha);
            return _userRepository.Atualizar(usuario);
        }

        public void RedefinirSenha(int usuarioId, string novaSenha)
        {
            var usuario = _userRepository.ObterPorId(usuarioId);
            if (usuario != null)
            {
                usuario.SenhaHash = HashSenha(novaSenha);
                _userRepository.Atualizar(usuario);
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
                PerfilUsuario.Desenvolvedor => "Desenvolvedor",
                PerfilUsuario.Administrador => "Administrador",
                PerfilUsuario.Gerente => "Gerente",
                PerfilUsuario.Recepcionista => "Recepcionista",
                PerfilUsuario.Visualizador => "Visualizador",
                _ => "Desconhecido"
            };
        }

        public List<Usuario> FiltrarPorPerfil(PerfilUsuario? perfil)
        {
            return _userRepository.FiltrarPorPerfil(perfil);
        }

        public List<Usuario> BuscarUsuarios(string termo)
        {
            return _userRepository.Buscar(termo);
        }
    }
}
