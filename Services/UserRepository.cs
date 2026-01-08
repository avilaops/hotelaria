using Hotelaria.Models;

namespace Hotelaria.Services
{
    /// <summary>
    /// Repositório Singleton para armazenar usuários compartilhados entre todas as sessões
    /// </summary>
    public class UserRepository
    {
        private readonly List<Usuario> _usuarios = new();
        private int _nextId = 1;
        private readonly object _lock = new();

        public UserRepository()
        {
            InicializarUsuariosPadrao();
        }

        private void InicializarUsuariosPadrao()
        {
            // Criar usuário desenvolvedor (permissão suprema)
            Adicionar(new Usuario
            {
                Nome = "Nicolas Rosa",
                Email = "dev@avila.inc",
                Username = "nicolasrosaab",
                SenhaHash = AuthService.HashSenha("7Aciqgr7@"),
                Perfil = PerfilUsuario.Desenvolvedor,
                Ativo = true
            });

            // Criar usuário admin padrão
            Adicionar(new Usuario
            {
                Nome = "Administrador",
                Email = "admin@hotelaria.com",
                Username = "admin",
                SenhaHash = AuthService.HashSenha("admin123"),
                Perfil = PerfilUsuario.Administrador,
                Ativo = true
            });

            // Criar usuário gerente de exemplo
            Adicionar(new Usuario
            {
                Nome = "Maria Silva",
                Email = "maria@hotelaria.com",
                Username = "maria",
                SenhaHash = AuthService.HashSenha("maria123"),
                Perfil = PerfilUsuario.Gerente,
                Ativo = true
            });

            // Criar recepcionista de exemplo
            Adicionar(new Usuario
            {
                Nome = "João Santos",
                Email = "joao@hotelaria.com",
                Username = "joao",
                SenhaHash = AuthService.HashSenha("joao123"),
                Perfil = PerfilUsuario.Recepcionista,
                Ativo = true
            });
        }

        public List<Usuario> ObterTodos()
        {
            lock (_lock)
            {
                return _usuarios.ToList(); // Retorna cópia
            }
        }

        public Usuario? ObterPorId(int id)
        {
            lock (_lock)
            {
                return _usuarios.FirstOrDefault(u => u.Id == id);
            }
        }

        public Usuario? ObterPorUsername(string username)
        {
            lock (_lock)
            {
                return _usuarios.FirstOrDefault(u => 
                    u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool Adicionar(Usuario usuario)
        {
            lock (_lock)
            {
                // Verificar se username já existe
                if (ObterPorUsername(usuario.Username) != null)
                    return false;

                usuario.Id = _nextId++;
                usuario.DataCriacao = DateTime.Now;
                _usuarios.Add(usuario);
                return true;
            }
        }

        public bool Atualizar(Usuario usuario)
        {
            lock (_lock)
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
        }

        public bool Remover(int id, int? usuarioAtualId = null)
        {
            lock (_lock)
            {
                // Não permitir remover o próprio usuário
                if (usuarioAtualId.HasValue && id == usuarioAtualId.Value)
                    return false;

                var usuario = ObterPorId(id);
                
                // NUNCA permitir remover desenvolvedor
                if (usuario?.Perfil == PerfilUsuario.Desenvolvedor)
                    return false;

                // Não permitir remover o último admin
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
        }

        public List<Usuario> FiltrarPorPerfil(PerfilUsuario? perfil)
        {
            lock (_lock)
            {
                if (perfil == null)
                    return _usuarios.ToList();

                return _usuarios.Where(u => u.Perfil == perfil).ToList();
            }
        }

        public List<Usuario> Buscar(string termo)
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(termo))
                    return _usuarios.ToList();

                termo = termo.ToLower();
                return _usuarios.Where(u =>
                    u.Nome.ToLower().Contains(termo) ||
                    u.Username.ToLower().Contains(termo) ||
                    u.Email.ToLower().Contains(termo)
                ).ToList();
            }
        }
    }
}
