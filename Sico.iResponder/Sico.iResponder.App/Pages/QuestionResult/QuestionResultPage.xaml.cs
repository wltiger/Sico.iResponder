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
using Sico.iResponder.App.Common;
using Sico.iResponder.Data.Models;

namespace Sico.iResponder.App.Pages.QuestionResult
{
    /// <summary>
    /// Interaction logic for QuestionResult.xaml
    /// </summary>
    public partial class QuestionResultPage : Page, INotifyKeyDown
    {
        public event Action Next;
        public QuestionResultPage(IList<CompetitionAnswer> answers)
        {
            InitializeComponent();

            SetAnswers(answers);

            this.MouseDown += (sender, e) =>
            {
                Next?.Invoke();
            };
        }

        private void SetAnswers(IList<CompetitionAnswer> answers)
        {
            answers = answers.OrderBy(t => t.CompetitionParticipant.No).ToList();
;           var count = answers.Count;
            var participants = new[] { Participant1, Participant2, Participant3, Participant4 };
            double paddingLeftRight = 20;
            double paddingTopBottom = 50;
            var scale = (double)900 / 600;
            var width = (1920 - paddingLeftRight * 2 - paddingLeftRight * (count - 1)) / count;
            var height = width * scale;
            if (height > 1080 - paddingTopBottom * 2)
            {
                height = 1080 - paddingTopBottom * 2;
                width = (int)(height / scale);
            }

            paddingTopBottom = ((1080 - height) / 2);
            paddingLeftRight = (1920 - width * count) / (count + 1);

            for (var i = 0; i < participants.Length; i++)
            {
                var participant = participants[i];
                var answer = answers.Skip(i).FirstOrDefault();
                if (answer == null)
                {
                    participant.Visibility = Visibility.Hidden;
                    continue;
                }
                
                participant.SetValue(Canvas.LeftProperty, (double)(paddingLeftRight + (paddingLeftRight + width) * i));
                participant.SetValue(Canvas.TopProperty, (double)paddingTopBottom);
                participant.Width = width;
                participant.Height = height;
                participant.SetCompetitionAnswer(answer);
            }
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                Next?.Invoke();
            }
        }
    }
}
