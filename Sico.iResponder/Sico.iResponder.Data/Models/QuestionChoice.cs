using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sico.iResponder.Data.Models
{
    public class QuestionChoice
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAnswer { get; set; }
        public int Sequence { get; set; }
    }
}
