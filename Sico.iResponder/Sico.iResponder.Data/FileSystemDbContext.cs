using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Sico.iResponder.Data.Models;

namespace Sico.iResponder.Data
{
    public class FileSystemDbContext
    {
        public IList<QuestionBook> QuestionBooks { get; set; }

        public FileSystemDbContext(string rootPath)
        {
            QuestionBooks = new List<QuestionBook>();
            foreach (var file in Directory.GetFiles(rootPath, "question.xml", SearchOption.AllDirectories))
            {
                var imageFile = Directory.GetFiles(Path.GetDirectoryName(file), "*.png").FirstOrDefault();
                var questionBook = new QuestionBook()
                {
                    Id = Guid.NewGuid(),
                    Name = Path.GetFileNameWithoutExtension(imageFile),
                    ImageUrl = imageFile,
                    TotalScore = 100,
                    Disabled = false,
                    Questions = new List<Question>()
                };
                QuestionBooks.Add(questionBook);

                using (var fstream = File.OpenRead(file))
                {
                    var xdoc = XDocument.Load(fstream);
                    var subjects = xdoc.Root?.XPathSelectElements("subject");
                    if (subjects != null)
                    {
                        var idx = 0;
                        foreach (var subject in subjects)
                        {
                            var ok = subject.Attribute("ok")?.Value.Trim().ToLower();
                            string imageUrl;
                            var title = StripImage(subject.Element("title")?.Value, out imageUrl)?.Trim();
                            var a = subject.Element("A")?.Value.Trim();
                            var b = subject.Element("B")?.Value.Trim();
                            var c = subject.Element("C")?.Value.Trim();
                            var d = subject.Element("D")?.Value.Trim();

                            var question = new Question()
                            {
                                Id = Guid.NewGuid(),
                                Title = title,
                                ImageUrl = string.IsNullOrEmpty(imageUrl) || Path.IsPathRooted(imageUrl) ? imageUrl : Path.Combine(rootPath, imageUrl),
                                Sequence = idx++,
                                QuestionBook = questionBook,
                                QuestionBookId = questionBook.Id,
                                Choices = new List<QuestionChoice>()
                            };
                            questionBook.Questions.Add(question);
                            if (!string.IsNullOrEmpty(a))
                            {
                                question.Choices.Add(new QuestionChoice()
                                {
                                    Text = StripImage(a, out imageUrl)?.Trim(),
                                    ImageUrl = string.IsNullOrEmpty(imageUrl) || Path.IsPathRooted(imageUrl) ? imageUrl : Path.Combine(rootPath, imageUrl),
                                    IsAnswer = ok == "a",
                                    Sequence = 1,
                                    QuestionId = question.Id,
                                    Question = question,
                                    
                                });

                                if (!string.IsNullOrEmpty(b))
                                {
                                    question.Choices.Add(new QuestionChoice()
                                    {
                                        Text = StripImage(b, out imageUrl)?.Trim(),
                                        ImageUrl = string.IsNullOrEmpty(imageUrl) || Path.IsPathRooted(imageUrl) ? imageUrl : Path.Combine(rootPath, imageUrl),
                                        IsAnswer = ok == "b",
                                        Sequence = 2,
                                        QuestionId = question.Id,
                                        Question = question,
                                    });

                                    if (!string.IsNullOrEmpty(c))
                                    {
                                        question.Choices.Add(new QuestionChoice()
                                        {
                                            Text = StripImage(c, out imageUrl)?.Trim(),
                                            ImageUrl = string.IsNullOrEmpty(imageUrl) || Path.IsPathRooted(imageUrl) ? imageUrl : Path.Combine(rootPath, imageUrl),
                                            IsAnswer = ok == "c",
                                            Sequence = 3,
                                            QuestionId = question.Id,
                                            Question = question,
                                        });

                                        if (!string.IsNullOrEmpty(d))
                                        {
                                            question.Choices.Add(new QuestionChoice()
                                            {
                                                Text = StripImage(d, out imageUrl)?.Trim(),
                                                ImageUrl = string.IsNullOrEmpty(imageUrl) || Path.IsPathRooted(imageUrl) ? imageUrl : Path.Combine(rootPath, imageUrl),
                                                IsAnswer = ok == "d",
                                                Sequence = 4,
                                                QuestionId = question.Id,
                                                Question = question,
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (questionBook.Questions.Count > 0)
                {
                    var score = 100M / questionBook.Questions.Count;
                    foreach (var question in questionBook.Questions)
                    {
                        question.Score = score;
                    }
                }
            }
        }

        private string StripImage(string text, out string imageUrl)
        {
            imageUrl = null;
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(text);
            var imgNode = doc.DocumentNode.SelectSingleNode("img");
            if (imgNode != null)
            {
                imageUrl = imgNode.GetAttributeValue("src", null)?.Trim();
                text = doc.DocumentNode.InnerText;
            }
            return text;
        }
    }
}
