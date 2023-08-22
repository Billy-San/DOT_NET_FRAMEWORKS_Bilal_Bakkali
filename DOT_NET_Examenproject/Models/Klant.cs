using System.ComponentModel.DataAnnotations;

namespace DOT_NET_Examenproject.Models
{
    public class Klant
    {
        public int KlantId { get; set; }

        [Display(Name = "Customer Name")]
        public string Name { get; set; }
        [Display(Name = "TVA")]
        public int NrTva { get; set; }
        [Display(Name = "Address")]
        public string Adres { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Tel Number")]
        public int NrTel { get; set; }
        public bool IsDeleted { get; set; } = false;

        public string? user_id { get; set; }

    }
}
