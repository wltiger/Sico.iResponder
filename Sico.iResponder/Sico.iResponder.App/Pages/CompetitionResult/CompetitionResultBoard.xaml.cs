using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sico.iResponder.Data.Models;

namespace Sico.iResponder.App.Pages.CompetitionResult
{
    /// <summary>
    /// Interaction logic for CompetitionResultBoard.xaml
    /// </summary>
    public partial class CompetitionResultBoard : UserControl
    {
        public CompetitionResultBoard()
        {
            InitializeComponent();
        }

        public void SetCompetitionParticipant(CompetitionParticipant participant)
        {
            ParticipantNo.Text = participant.No.ToString();
            RightCount.Text = participant.AnsweredCorrect.ToString();
            WrongCount.Text = (participant.AnsweredIncorrect + participant.Unanswered).ToString();
            TotalScore.Text = participant.Score.ToString("0.#");
            Rank.Text = participant.Rank == 1 ? "太棒了第一名!" : "再接再厉";
        }
    }
}
