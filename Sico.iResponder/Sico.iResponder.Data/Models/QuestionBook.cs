using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sico.iResponder.Data.Models
{
    public class QuestionBook
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal TotalScore { get; set; }
        public string ImageUrl { get; set; }
        public bool Disabled { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual IList<Question> Questions { get; set; }
    }
}
