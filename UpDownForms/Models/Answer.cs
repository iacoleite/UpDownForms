﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UpDownForms.DTO.AnswersDTOs;
using UpDownForms.DTO.QuestionDTOs;

namespace UpDownForms.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")] // it's lowercase because Json
    [JsonDerivedType(typeof(AnswerMultipleChoice), typeDiscriminator: "MultipleChoice")]
    [JsonDerivedType(typeof(AnswerOpenEnded), typeDiscriminator: "OpenEnded")]
    [Table("Answers")]
    public abstract class Answer
    {
        public int Id { get; set; }
        [Required]
        public int ResponseId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public Response Response { get; set; }
        [Required]
        public Question Question { get; set; }

        public Answer()
        {
        }

        public Answer(CreateAnswerDTO createAnswerDTO)
        {
            //this.ResponseId = createAnswerDTO.ResponseId;
            this.QuestionId = createAnswerDTO.QuestionId;
            //this.AnswerText = createAnswerDTO.AnswerText;
            //this.OptionId = createAnswerDTO.OptionId;
            this.IsDeleted = false;
        }
        public virtual AnswerDTO ToAnswerDTO()
        {
            return new AnswerDTO
            {
                Id = this.Id,
                ResponseId = this.ResponseId,
                QuestionId = this.QuestionId,
                //AnswerText = this.AnswerText,
                //OptionId = this.OptionId,
                IsDeleted = this.IsDeleted
            };
        }


    }
}
