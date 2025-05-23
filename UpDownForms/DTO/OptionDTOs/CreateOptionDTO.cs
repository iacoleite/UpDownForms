﻿using System.ComponentModel.DataAnnotations;

namespace UpDownForms.DTO.OptionDTOs
{
    public class CreateOptionDTO
    {
        //public int Id { get; set; }
        //public int QuestionId { get; set; }
        [Required(AllowEmptyStrings = false)]

        public string Text { get; set; }
        public int Order { get; set; }
        public bool IsCorrect { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
