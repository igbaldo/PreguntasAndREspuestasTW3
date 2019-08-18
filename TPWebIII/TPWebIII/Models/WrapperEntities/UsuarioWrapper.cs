using TPWebIII.Models.Enums;

namespace TPWebIII.Models.WrapperEntities
{
    public class UsuarioWrapper
    {
        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string Nombre { get; set; }
        public EnumPerfilUsuario PerfilUsuario { get; set; }
    }
}