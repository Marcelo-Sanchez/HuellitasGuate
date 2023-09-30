using System.ComponentModel.DataAnnotations;

namespace HuellitasGuate.Models
{
    public class Servicio
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nombre del Servicio")]
        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public float? Precio { get; set; }
    }
}
