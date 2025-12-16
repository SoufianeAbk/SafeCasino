using System.ComponentModel.DataAnnotations;

namespace SafeCasino.Web.ViewModels
{
    /// <summary>
    /// ViewModel voor het aanmaken van een review
    /// </summary>
    public class CreateReviewViewModel
    {
        [Required]
        public int GameId { get; set; }

        public string GameName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Titel is verplicht")]
        [StringLength(200, ErrorMessage = "Titel mag maximaal 200 karakters zijn")]
        [Display(Name = "Titel")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Review tekst is verplicht")]
        [StringLength(2000, ErrorMessage = "Review mag maximaal 2000 karakters zijn", MinimumLength = 10)]
        [Display(Name = "Jouw review")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Beoordeling is verplicht")]
        [Range(1, 5, ErrorMessage = "Beoordeling moet tussen 1 en 5 sterren zijn")]
        [Display(Name = "Beoordeling")]
        public int Rating { get; set; }
    }
}