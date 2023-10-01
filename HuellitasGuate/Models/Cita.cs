using System.ComponentModel.DataAnnotations;

namespace HuellitasGuate.Models
{
    public partial class Cita
    {
        [Key]
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public string? Mascota { get; set; }

        [Display(Name = "Servicio")]
        public int ServicioId { get; set; }
        public virtual Servicio Servicio { get; set; }

        public string? Telefono { get; set; }

        public DateTime? Fecha { get; set; }

        public string Dpi { get; set; }

        public string? Correo { get; set; }

        public string? Descripcion { get; set; }


    }
}
