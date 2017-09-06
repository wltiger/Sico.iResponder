using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sico.iResponder.Data.Models
{
    public class Competition
    {
        public Guid Id { get; set; }
        public Guid QuestionBookId { get; set; }
        public virtual QuestionBook QuestionBook { get; set; }

        public virtual IList<CompetitionParticipant> Participants { get; set; }
        public virtual IList<CompetitionAnswer> Answers { get; set; }

        public Guid? WinnerId { get; set; }
        public virtual CompetitionParticipant Winner { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public DateTime CreateTime { get; set; }

        public void UpdateParticipantsRank()
        {
            var rank = 1;
            foreach (var participant in Participants
     .OrderByDescending(t => t.Score)
     .ThenBy(t => t.TimeTaken)
     .ThenBy(t => t.No))
            {
                participant.Rank = rank++;
            }
        }
    }
}
