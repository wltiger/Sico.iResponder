using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Sico.iResponder.App.Pages.Question
{
    /// <summary>
    /// Interaction logic for QuestionPage.xaml
    /// </summary>
    public partial class QuestionPage : Page, INotifyParticipantKeyDown
    {

        //private readonly Data.Models.Competition _competition;
        private readonly Data.Models.Question _question;
        private readonly IDictionary<int, CompetitionAnswer> _answers;
        private DateTime _startTime;

        public event Action<CompetitionAnswer> Answered;
        public event Action TimeUp;
        public ICollection<CompetitionAnswer> Answers => _answers.Values;

        public QuestionPage(Data.Models.Competition competition, int questionIdx)
        {
            
            InitializeComponent();

            //_competition = competition;
            _question = competition.QuestionBook.Questions[questionIdx];
            _answers = new Dictionary<int, CompetitionAnswer>();

            var participantElems = new[] { Participant1, Participant2, Participant3, Participant4 };
            var participantScoreElems = new[] { Participant1Score, Participant2Score, Participant3Score, Participant4Score };
            foreach (var participantElem in participantElems)
            {
                participantElem.Visibility = Visibility.Hidden;
            }
            var now = DateTime.Now;
            foreach (var participant in competition.Participants)
            {
                participantElems[participant.No - 1].Visibility = Visibility.Visible;
                participantScoreElems[participant.No - 1].Text = participant.Score.ToString("0.#");
                _answers.Add(participant.No, new CompetitionAnswer()
                {
                    Competition = competition,
                    CompetitionParticipant = participant,
                    Question = _question,
                    CreateTime = now
                });
                participant.Unanswered += 1;
            }
            ParticipantAnswer.Text = "";
            QuestionBoard.SetQuestion(_question);
            QuestionNo.Text = (questionIdx + 1).ToString();
            CountDown.Visibility = Visibility.Hidden;
            QuestionBoard.Choose += choice =>
            {
                OnParticipantKeyDown(this,
                    new ParticipantKey() {ParticipantNo = 1, AnswerNo = _question.Choices.IndexOf(choice) + 1});
            };
            
            _startTime = now;
        }

        public void StartCountDown(int second)
        {
            _startTime = DateTime.Now;
            CountDownSec.Text = second.ToString();
            CountDown.Visibility = Visibility.Visible;
            Task.Run(() =>
            {
                for (var i = second - 1; i >= 0; i--)
                {
                    Task.Delay(1000).Wait();
                    this.Dispatcher.Invoke(() =>
                    {
                        CountDownSec.Text = i.ToString();
                    });
                }
                Task.Delay(100).Wait();
                this.Dispatcher.Invoke(() =>
                {
                    TimeUp?.Invoke();
                });
            });
        }

        public void OnParticipantKeyDown(object sender, ParticipantKey key)
        {
            if (_answers.ContainsKey(key.ParticipantNo) && _answers[key.ParticipantNo].QuestionChoice == null)
            {
                if (_question.Choices.Count >= key.AnswerNo)
                {
                    var answer = _answers[key.ParticipantNo];
                    var choice = _question.Choices[key.AnswerNo - 1];
                    answer.QuestionChoice = choice;
                    answer.CompetitionParticipant.Unanswered -= 1;
                    if (choice.IsAnswer)
                    {
                        answer.Score = choice.Question.Score;
                        answer.CompetitionParticipant.Score += answer.Score;
                        answer.CompetitionParticipant.AnsweredCorrect += 1;
                    }
                    else
                    {
                        answer.CompetitionParticipant.AnsweredIncorrect += 1;
                    }
                    answer.TimeTaken = (DateTime.Now - _startTime).TotalSeconds;

                    Answered?.Invoke(answer);

                    ParticipantAnswer.Text += _answers.Values.Count(t=>t.QuestionChoice != null) > 1
                        ? $"  {key.ParticipantNo}号选手已回答"
                        : $"{key.ParticipantNo}号选手已抢答";
                }
            }
        }
    }
}
