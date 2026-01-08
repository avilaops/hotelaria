namespace Hotelaria.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public PerfilUsuario Perfil { get; set; } = PerfilUsuario.Recepcionista;
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? UltimoAcesso { get; set; }
        public string? Foto { get; set; }
    }

    public enum PerfilUsuario
    {
        Desenvolvedor,      // Permissão suprema - controle total
        Administrador,      // Gerencia sistema e usuários
        Gerente,           // Gerencia operações do hotel
        Recepcionista,     // Operações diárias
        Visualizador       // Apenas visualização
    }

    public class LoginModel
    {
        public string Username { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public bool LembrarMe { get; set; }
    }

    public class SessaoUsuario
    {
        public Usuario? Usuario { get; set; }
        public DateTime DataLogin { get; set; }
        public bool EstaAutenticado => Usuario != null;
    }
}
