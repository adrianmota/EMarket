using System.ComponentModel.DataAnnotations;

namespace EMarket.Core.Application.ViewModels.Categories
{
    public class SaveCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe colocar el nombre")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe colocar la descripción")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
    }
}