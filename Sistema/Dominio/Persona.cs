using System.ComponentModel.DataAnnotations;

namespace Sistema.Dominio
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Ci { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}
