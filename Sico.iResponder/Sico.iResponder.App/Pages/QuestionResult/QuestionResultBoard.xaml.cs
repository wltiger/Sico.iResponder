using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sico.iResponder.Data.Models;

namespace Sico.iResponder.App.Pages.QuestionResult
{
    /// <summary>
    /// Interaction logic for QuestionResultBoard.xaml
    /// </summary>
    public partial class QuestionResultBoard : UserControl
    {
        public QuestionResultBoard()
        {
            InitializeComponent();
        }

        public void SetCompetitionAnswer(CompetitionAnswer answer)
        {
            var participant = answer.CompetitionParticipant;
            ParticipantNo.Text = participant.No.ToString();
            RightCount.Text = participant.AnsweredCorrect.ToString();
            WrongCount.Text = (participant.AnsweredIncorrect + participant.Unanswered).ToString();
            Score.Text = answer.Score.ToString("0.#");
            TotalScore.Text = participant.Score.ToString("0.#");
            Rank.Text = participant.Rank.ToString();
        }
    }
}
