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

namespace Sico.iResponder.App.Pages.Competition
{
    /// <summary>
    /// Interaction logic for Page.xaml
    /// </summary>
    public partial class CompetitionPage : Page, INotifyKeyDown, INotifyParticipantKeyDown
    {
        private TextBlock[] _participantTextBlocks;

        public event Action<QuestionBook> QuestionBookSelected;
        public event Action<int> ParticipantJoin;

        public IList<QuestionBook> Books { get; private set; }
        public CompetitionPage(IList<QuestionBook> books)
        {
            InitializeComponent();

            _participantTextBlocks = new[] {Participant1Ready, Participant2Ready, Participant3Ready, Participant4Ready};

            foreach (var item in _participantTextBlocks)
            {
                item.Visibility = Visibility.Hidden;
            }

            Books = books;
            SetActiveBook(books[0]);
        }

        private void CompetitionPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                SetActiveBook(Book4.Book);
            }
            else if (e.Key == Key.Left)
            {
                SetActiveBook(Book2.Book);
            }
            else if (e.Key == Key.Enter)
            {
                OnQuestionBookSelected(Book3.Book);
            }
        }

        public void SetActiveBook(QuestionBook book)
        {
            var idx = Books.IndexOf(book);
            Book1.SetQuestionBook(Books[idx - 2 < 0 ? Books.Count + idx - 2 : idx - 2]);
            Book2.SetQuestionBook(Books[idx - 1 < 0 ? Books.Count + idx - 1 : idx - 1]);
            Book3.SetQuestionBook(book);
            Book4.SetQuestionBook(Books[(idx + 1) % Books.Count]);
            Book5.SetQuestionBook(Books[(idx + 2) % Books.Count]);
        }

        protected void OnQuestionBookSelected(QuestionBook book)
        {
            QuestionBookSelected?.Invoke(book);
        }

        protected void QuestionBookTile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var book = sender as QuestionBookTile;
            if (book == null)
            {
                return;
            }
            if (book == Book3)
            {
                OnQuestionBookSelected(book.Book);
            }
            else
            {
                SetActiveBook(book.Book);
            }
            
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            CompetitionPage_KeyDown(sender, e);
        }

        public void OnParticipantKeyDown(object sender, ParticipantKey key)
        {
            ParticipantJoin?.Invoke(key.ParticipantNo);

            _participantTextBlocks[key.ParticipantNo - 1].Visibility = Visibility.Visible;
        }
    }
}
