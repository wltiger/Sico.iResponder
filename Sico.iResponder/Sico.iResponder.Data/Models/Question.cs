using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sico.iResponder.Data.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public Guid QuestionBookId { get; set; }
        public virtual QuestionBook QuestionBook { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public decimal Score { get; set; }
        public IList<QuestionChoice> Choices { get; set; }
        public int Sequence { get; set; }
    }
}
