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
using Sico.iResponder.App.Pages;
using Sico.iResponder.App.Pages.Competition;
using Sico.iResponder.App.Pages.CompetitionReady;
using Sico.iResponder.App.Pages.CompetitionResult;
using Sico.iResponder.App.Pages.Question;
using Sico.iResponder.App.Pages.QuestionAnswer;
using Sico.iResponder.App.Pages.QuestionResult;
using Sico.iResponder.Data;
using Sico.iResponder.Data.Models;

namespace Sico.iResponder.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        private Settings _settings;
        private IList<QuestionBook> _questionBooks;

        public MainWindow()
        {
            _settings = new Settings();
            
            InitializeComponent();
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.KeyDown += MainWindow_KeyDown;
            this.MouseDoubleClick += MainWindow_MouseDoubleClick;

            var path = System.IO.Path.Combine(Environment.CurrentDirectory, "题库");
            var context = new FileSystemDbContext(path);

            _questionBooks = context.QuestionBooks;

            this.Content = InitCompetitionPage(_questionBooks);
        }

        private void MainWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowStyle == WindowStyle.None)
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
            else
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
            }
        }

        private CompetitionPage InitCompetitionPage(IList<QuestionBook> books)
        {
            var competition = new Competition()
            {
                Participants = new List<CompetitionParticipant>(),
                Answers = new List<CompetitionAnswer>()
            };
            var page = new CompetitionPage(books);
            page.QuestionBookSelected += book =>
            {
                if (competition.Participants.Count > 0)
                {
                    competition.QuestionBook = book;
                    OnCompetitionReady(competition);
                }

                //MessageBox.Show(this, $"{book.Name}被选择!", "提示");
            };
            page.ParticipantJoin += no =>
            {
                var participant = competition.Participants.FirstOrDefault(t => t.No == no);
                if (participant == null)
                {
                    competition.Participants.Add(new CompetitionParticipant()
                    {
                        Id = Guid.NewGuid(),
                        No = no,
                    });
                }
            };
            page.OnParticipantKeyDown(this, new ParticipantKey() {AnswerNo = 1, ParticipantNo = 1});
            return page;
        }

        private CompetitionReadyPage InitCompetitionReadyPage(Competition competition)
        {
            var page = new CompetitionReadyPage();
            page.TimeUp += () =>
            {
                competition.StartTime = DateTime.Now;
                OnCompetitionStart(competition);
                //MessageBox.Show(this, $"{_currentBook.Name}开始!", "提示");
            };
            page.StartCountDown();
            return page;
        }

        private QuestionPage InitQuestionPage(Competition competition, int questionIdx)
        {
            var page = new QuestionPage(competition, questionIdx);
            var isCompleted = false;
            page.Answered += answer =>
            {
                if (page.Answers.All(t => t.QuestionChoice != null))
                {
                    lock (page)
                    {
                        if (!isCompleted)
                        {
                            isCompleted = true;
                            OnCompetitionQuestionCompleted(competition, questionIdx, page.Answers.OrderBy(t => t.CompetitionParticipant.No).ToList());
                        }
                    }
                }
            };
            if (_settings.QuestionCountDown > 0)
            {
                page.TimeUp += () =>
                {
                    lock (page)
                    {
                        if (!isCompleted)
                        {
                            isCompleted = true;
                            OnCompetitionQuestionCompleted(competition, questionIdx, page.Answers.OrderBy(t => t.CompetitionParticipant.No).ToList());
                        }
                    }
                };
                page.StartCountDown(_settings.QuestionCountDown);
            }

            return page;
        }

        private QuestionAnswerPage InitQuestionAnswerPage(Competition competition, int questionIdx, IList<CompetitionAnswer> answers)
        {
            var question = competition.QuestionBook.Questions[questionIdx];
            var page = new QuestionAnswerPage(question.Choices.FirstOrDefault(t=>t.IsAnswer));
            var isCompleted = false;
            page.Next += () =>
            {
                lock (page)
                {
                    if (!isCompleted)
                    {
                        isCompleted = true;
                        OnCompetitionQuestionCompleted(competition, questionIdx, answers);
                    }
                }
            };

            if (_settings.QuestionAnswerCountDown > 0)
            {
                Task.Run(() =>
                {
                    Task.Delay(TimeSpan.FromSeconds(_settings.QuestionAnswerCountDown)).Wait();
                    this.Dispatcher.Invoke(() =>
                    {
                        lock (page)
                        {
                            if (!isCompleted)
                            {
                                isCompleted = true;
                                OnCompetitionQuestionCompleted(competition, questionIdx, answers);
                            }
                        }
                    });
                });
            }

            return page;
        }

        private QuestionResultPage InitQuestionResultPage(Competition competition, int questionIdx, IList<CompetitionAnswer> answers)
        {
            var page = new QuestionResultPage(answers);
            var isCompleted = false;
            page.Next += () =>
            {
                lock (page)
                {
                    if (!isCompleted)
                    {
                        isCompleted = true;
                        OnCompetitionQuestionCompleted(competition, questionIdx, answers);
                    }
                }
            };

            if (_settings.QuestionResultCountDown > 0)
            {
                Task.Run(() =>
                {
                    Task.Delay(TimeSpan.FromSeconds(_settings.QuestionResultCountDown)).Wait();
                    this.Dispatcher.Invoke(() =>
                    {
                        lock (page)
                        {
                            if (!isCompleted)
                            {
                                isCompleted = true;
                                OnCompetitionQuestionCompleted(competition, questionIdx, answers);
                            }
                        }
                    });
                });
            }
            return page;
        }

        private CompetitionResultPage InitCompetitionResultPage(Competition competition)
        {
            var page = new CompetitionResultPage(competition.Participants);
            page.Next += () =>
            {
                OnCompetitionCompleted(competition);
            };
            return page;
        }

        private void OnCompetitionReady(Competition competition)
        {
            this.Content = InitCompetitionReadyPage(competition);
        }

        private void OnCompetitionStart(Competition competition)
        {
            this.Content = InitQuestionPage(competition, 0);
        }

        private void OnCompetitionQuestionCompleted(Competition competition, int questionIdx, IList<CompetitionAnswer> answers)
        {
            if (this.Content is QuestionAnswerPage)
            {
                competition.UpdateParticipantsRank();
                this.Content = InitQuestionResultPage(competition, questionIdx, answers);
            }
            else if (this.Content is QuestionResultPage)
            {
                if (competition.QuestionBook.Questions.Count <= questionIdx + 1)
                {
                    OnCompetitionCompleted(competition);
                    return;
                }
                // next question
                this.Content = InitQuestionPage(competition, questionIdx + 1);
            }
            else
            {
                this.Content = InitQuestionAnswerPage(competition, questionIdx, answers);
            }
        }

        private void OnCompetitionCompleted(Competition competition)
        {
            if (this.Content is CompetitionResultPage)
            {
                this.Content = InitCompetitionPage(_questionBooks);
            }
            else
            {
                var page = InitCompetitionResultPage(competition);
                this.Content = page;
                //page.StartCountDown();
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (_settings.ParticipantKeyMappings.ContainsKey(e.Key))
            {
                (this.Content as INotifyParticipantKeyDown)?.OnParticipantKeyDown(sender, _settings.ParticipantKeyMappings[e.Key]);
            }
            (this.Content as INotifyKeyDown)?.OnKeyDown(sender, e);
        }


    }
}
