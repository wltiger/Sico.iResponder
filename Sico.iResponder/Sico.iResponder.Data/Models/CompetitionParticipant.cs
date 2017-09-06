using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Sico.iResponder.Data.Models
{
    public class CompetitionParticipant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int No { get; set; }
        public decimal Score { get; set; }
        public int AnsweredCorrect { get; set; }
        public int AnsweredIncorrect { get; set; }
        public int Unanswered { get; set; }
        public int Rank { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
