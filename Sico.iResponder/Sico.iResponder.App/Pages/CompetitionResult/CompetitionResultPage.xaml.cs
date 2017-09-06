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

namespace Sico.iResponder.App.Pages.CompetitionResult
{
    /// <summary>
    /// Interaction logic for CompetitionResultPage.xaml
    /// </summary>
    public partial class CompetitionResultPage : Page, INotifyKeyDown
    {
        public event Action Next;
        public CompetitionResultPage(IList<CompetitionParticipant> participants)
        {
            InitializeComponent();

            SetParticipants(participants);

            this.MouseDown += (sender, e) =>
            {
                Next?.Invoke();
            };
        }

        private void SetParticipants(IList<CompetitionParticipant> participants)
        {
            participants = participants.OrderBy(t => t.No).ToList();
            ; var count = participants.Count;
            var participantCtrls = new[] { Participant1, Participant2, Participant3, Participant4 };
            double paddingLeftRight = 100;
            double paddingTopBottom = 100;
            double panelMargin = 20;
            var scale = (double)900 / 600;
            var width = (1920 - paddingLeftRight * 2 - panelMargin * (count + 1)) / count;
            var height = width * scale;
            if (height > 1080 - paddingTopBottom * 2)
            {
                height = 1080 - paddingTopBottom * 2;
                width = (int)(height / scale);
            }

            paddingTopBottom = ((1080 - height) / 2);
            panelMargin = (1920 - width * count - paddingLeftRight * 2) / (count + 1);

            for (var i = 0; i < participantCtrls.Length; i++)
            {
                var participantCtrl = participantCtrls[i];
                var participant = participants.Skip(i).FirstOrDefault();
                if (participant == null)
                {
                    participantCtrl.Visibility = Visibility.Hidden;
                    continue;
                }

                participantCtrl.SetValue(Canvas.LeftProperty, (double)(paddingLeftRight + panelMargin + (panelMargin + width) * i));
                participantCtrl.SetValue(Canvas.TopProperty, (double)paddingTopBottom);
                participantCtrl.Width = width;
                participantCtrl.Height = height;
                participantCtrl.SetCompetitionParticipant(participant);
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
