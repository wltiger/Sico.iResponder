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

namespace Sico.iResponder.App.Pages.Competition
{
    /// <summary>
    /// Interaction logic for QuestionBookTile.xaml
    /// </summary>
    public partial class QuestionBookTile : UserControl
    {
        public QuestionBook Book { get; private set; }
        public QuestionBookTile()
        {
            InitializeComponent();
        }

        public void SetQuestionBook(QuestionBook book)
        {
            Book = book;
            Title.Content = book.Name;
            if (!string.IsNullOrEmpty(book.ImageUrl))
            {
                Image.Source = new BitmapImage(new Uri(book.ImageUrl));
            }
        }
    }
}
