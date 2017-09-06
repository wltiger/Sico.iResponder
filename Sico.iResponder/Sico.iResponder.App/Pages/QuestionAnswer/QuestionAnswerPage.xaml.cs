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

namespace Sico.iResponder.App.Pages.QuestionAnswer
{
    /// <summary>
    /// Interaction logic for QuestionAnswerPage.xaml
    /// </summary>
    public partial class QuestionAnswerPage : Page, INotifyKeyDown
    {
        public event Action Next;

        public QuestionAnswerPage(QuestionChoice choice)
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(choice.ImageUrl))
            {
                Answer.Text = choice.Text;
            }
            else
            {
                Answer.Inlines.Clear();
                Answer.Inlines.Add(new Run(choice.Text)
                {
                    BaselineAlignment = BaselineAlignment.Center
                });
                Answer.Inlines.Add(new Image()
                {
                    Source = new BitmapImage(new Uri(choice.ImageUrl)),
                    Stretch = Stretch.Uniform
                });
            }
            
            AnswerSymbol.Text = choice?.Text.Substring(0, 1).ToUpper();
            this.MouseDown += (sender, e) =>
            {
                Next?.Invoke();
            };
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
