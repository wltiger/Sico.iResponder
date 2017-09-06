using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
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

namespace Sico.iResponder.App.Pages.Question
{
    /// <summary>
    /// Interaction logic for QuestionBoard.xaml
    /// </summary>
    public partial class QuestionBoard : UserControl
    {
        public event Action<QuestionChoice> Choose;
        public QuestionBoard()
        {
            InitializeComponent();
            Title.LayoutUpdated += (sender, e) =>
            {
                if (Title.ActualHeight > 320 && Title.FontSize > 10)
                {
                    Title.FontSize = Title.FontSize - 2;
                }
            };
        }

        public void SetQuestion(Data.Models.Question question)
        {
            Title.Text = question.Title;
            if (string.IsNullOrEmpty(question.ImageUrl))
            {
                TitleImage.Visibility = Visibility.Hidden;
            }
            else
            {
                TitleImage.Source = new BitmapImage(new Uri(question.ImageUrl));
                Title.Margin = new Thickness(TitleImage.Margin.Left + TitleImage.Width + 20, Title.Margin.Top, 0, 0);
                Title.Width = Title.Width - TitleImage.Width - 20;
            }
            SetChoice(Choice1, question.Choices.Skip(0).FirstOrDefault());
            SetChoice(Choice2, question.Choices.Skip(1).FirstOrDefault());
            SetChoice(Choice3, question.Choices.Skip(2).FirstOrDefault());
            SetChoice(Choice4, question.Choices.Skip(3).FirstOrDefault());
        }

        private void SetChoice(TextBlock textBlock, QuestionChoice choice)
        {

            if (choice == null)
            {
                textBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                textBlock.Inlines.Clear();
                if (!string.IsNullOrEmpty(choice.ImageUrl))
                {
                    textBlock.Inlines.Add(new Run(choice.Text)
                    {
                        BaselineAlignment = BaselineAlignment.Center
                    });
                    textBlock.Inlines.Add(new Image()
                    {
                        Source = new BitmapImage(new Uri(choice.ImageUrl)),
                        Stretch = Stretch.Uniform
                    });
                }
                else
                {
                    textBlock.Text = choice.Text;
                }
                
                textBlock.Visibility = Visibility.Visible;
                textBlock.MouseDown += (sender, e) =>
                {
                    Choose?.Invoke(choice);
                };
            }
        }
    }
}
