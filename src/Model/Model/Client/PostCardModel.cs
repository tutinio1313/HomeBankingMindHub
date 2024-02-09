﻿using System.ComponentModel.DataAnnotations;

namespace HomeBankingMindHub.Model.Model.Client
{
    public class PostCardModel
    {
        [Required(ErrorMessage = "El color de la tarjeta es requerida.")]
        public string Color { get; set; }
        [Required(ErrorMessage = "El tipo de la tarjeta es requerida.")]
        public string Type { get; set; }
    }
}
