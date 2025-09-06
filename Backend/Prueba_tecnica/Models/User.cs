namespace Prueba_tecnica.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Estado { get; set; } = "A"; 

        //campos para el retorno de fecha de creacion y modificacion
        public DateTime? FechaCreacion { get; }
        public DateTime? FechaModificacion { get; }
    }
}
