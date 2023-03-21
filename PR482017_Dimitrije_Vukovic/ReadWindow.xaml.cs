using System;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace PR482017_Dimitrije_Vukovic
{
    /// <summary>
    /// Interaction logic for ReadWindow.xaml
    /// </summary>
    public partial class ReadWindow : Window
    {
        public ReadWindow(int index)
        {
            InitializeComponent();

            LabelNaziv.Content += MainWindow.kafe[index].Naziv;
            FileStream fileStream = new FileStream(MainWindow.kafe[index].Opis, FileMode.Open);
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            range.Load(fileStream, DataFormats.Rtf);
            string s = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            char[] delimiters = new char[] { ' ', '\r', '\n' };
            string[] x = s.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            BrRtextBox.Text = "Word Count: " + x.Length.ToString();
            fileStream.Close();
            ImageDodaj.Source = new BitmapImage(new Uri(MainWindow.kafe[index].Slika, UriKind.RelativeOrAbsolute));
        }

        private void ButtonIzlaz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
