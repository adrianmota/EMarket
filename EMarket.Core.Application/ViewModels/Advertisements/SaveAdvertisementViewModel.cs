using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EMarket.Core.Application.ViewModels.Categories;

namespace EMarket.Core.Application.ViewModels.Advertisements
{
    public class SaveAdvertisementViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debe colocar el nombre")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Debe colocar la descripción")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
        public string ImageUrl4 { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile ImageFile1 { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile ImageFile2 { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile ImageFile3 { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile ImageFile4 { get; set; }

        [Required(ErrorMessage = "Debe colocar el precio")]
        public double Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría")]
        public int CategoryId { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
    }
}