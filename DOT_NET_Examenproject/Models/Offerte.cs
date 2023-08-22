using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOT_NET_Examenproject.Models
{
    public class Offerte
    {
        public int OfferteId { get; set; }

        [Display(Name = "Invoice Title")]
        public string TitelOfferte { get; set; }
        [Display(Name = "Total Amount")]
        public float TotaalBedrag { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Display(Name = "Customer")]
        public int KlantId { get; set; }

        [Display(Name = "Customer")]
        [ForeignKey("KlantId")]
        public Klant? Klant { get; set; }


        [Display(Name = "Company")]
        public int BedrijfId { get; set; }

        [Display(Name = "Company")]
        [ForeignKey("BedrijfId")]
        public Bedrijf? Bedrijf { get; set; }

        public string? user_id { get; set; }


    }

  
}
