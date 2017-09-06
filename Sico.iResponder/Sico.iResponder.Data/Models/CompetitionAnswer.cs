using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sico.iResponder.Data.Models
{
    public class CompetitionAnswer
    {
        public Guid Id { get; set; }
        public Guid CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }
        public Guid CompetitionParticipantId { get; set; }
        public virtual CompetitionParticipant CompetitionParticipant { get; set; }
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public Guid? QuestionChoiceId { get; set; }
        public virtual QuestionChoice QuestionChoice { get; set; }
        public decimal Score { get; set; }
        public DateTime CreateTime { get; set; }
        public double TimeTaken { get; set; }
    }
}
