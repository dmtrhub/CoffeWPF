using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PR482017_Dimitrije_Vukovic
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private bool izmeni = false;
        private int index;
        private string slika;
        public AddWindow()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            cmbFontColor.ItemsSource = typeof(Colors).GetProperties();
        }

        public AddWindow(int index2)
        {
            index = index2;
            izmeni = true;
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            cmbFontColor.ItemsSource = typeof(Colors).GetProperties();

            TextBoxNaziv.Text = MainWindow.kafe[index].Naziv;

            FileStream fileStream = new FileStream(MainWindow.kafe[index].Opis, FileMode.Open);
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            range.Load(fileStream, DataFormats.Rtf);

            fileStream.Close();

            string s = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            char[] delimiters = new char[] { ' ', '\r', '\n' };
            string[] pom = s.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            BrRtextBox.Text = "Word Count: " + pom.Length.ToString();

            slika = MainWindow.kafe[index].Slika;
            CreatePreview(slika);
        }

        private void CreatePreview(String slika)
        {
            BitmapImage bitmap = new BitmapImage();
            try
            {
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(slika, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                ImageDodaj.Source = bitmap;
            }
            catch (Exception)
            {
                ImageDodaj.Source = null;
                ButtonPregledaj.BorderBrush = Brushes.Red;
            }
        }

        private void ButtonPregledaj_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dial = new OpenFileDialog();
            dial.Title = "Izaberite sliku...";
            dial.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";

            if (dial.ShowDialog() == true)
            {
                slika = dial.FileName;
                CreatePreview(slika);
            }
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (izmeni == true)
            {
                if (validate())
                {
                    FileStream fileStream = new FileStream(TextBoxNaziv.Text.Trim() + ".rtf", FileMode.Create);
                    TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                    range.Save(fileStream, DataFormats.Rtf);
                    fileStream.Close();

                    MainWindow.kafe[index] = new Classes.Kafa(MainWindow.kafe[index].Index, slika, TextBoxNaziv.Text.Trim(), DateTime.Now, TextBoxNaziv.Text.Trim() + ".rtf");

                    this.Close();
                }
            }
            else
            {
                if (validate() && validate2())
                {
                    FileStream fileStream = new FileStream(TextBoxNaziv.Text.Trim() + ".rtf", FileMode.Create);
                    TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                    range.Save(fileStream, DataFormats.Rtf);
                    fileStream.Close();

                    MainWindow.kafe.Add(new Classes.Kafa(MainWindow.kafe.Count + 1, slika, TextBoxNaziv.Text.Trim(), DateTime.Now, TextBoxNaziv.Text.Trim() + ".rtf"));

                    this.Close();
                }
            }
        }

        private void ButtonIzlaz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool validate()
        {
            bool rez = true;

            if (TextBoxNaziv.Text.Trim().Equals(String.Empty))
            {
                rez = false;
                TextBoxNaziv.BorderBrush = Brushes.Red;
                TextBoxNaziv.BorderThickness = new Thickness(1);
                MessageBox.Show("Niste uneli ime!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                TextBoxNaziv.BorderBrush = Brushes.Black;
            }

            string opis = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            if (opis.Trim().Equals(string.Empty))
            {
                rez = false;
                rtbEditor.BorderBrush = Brushes.Red;
                rtbEditor.BorderThickness = new Thickness(1);
                MessageBox.Show("Niste uneli opis!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                rtbEditor.BorderBrush = Brushes.Black;
            }

            if (ImageDodaj.Source == null)
            {
                rez = false;
                ButtonPregledaj.BorderBrush = Brushes.Red;
                ButtonPregledaj.BorderThickness = new Thickness(1);
                MessageBox.Show("Niste izabrali sliku!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ButtonPregledaj.BorderBrush = Brushes.Black;
            }

            return rez;
        }

        private bool validate2()
        {
            bool rez = true;
            foreach (Classes.Kafa k in MainWindow.kafe)
            {
                if (TextBoxNaziv.Text.Trim().Equals(k.Naziv))
                {
                    rez = false;
                    MessageBox.Show("Uneli ste već postojeći brend kafe!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return rez;
        }

        private void TextBoxNaziv_GotFocus(object sender, RoutedEventArgs e)
        {
            rtbEditor.BorderBrush = Brushes.Black;
        }

        private void TextBoxNaziv_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxNaziv.Text.Trim().Equals(String.Empty))
            {
                TextBoxNaziv.BorderBrush = Brushes.Red;
                TextBoxNaziv.BorderThickness = new Thickness(1);
            }
            else
            {
                TextBoxNaziv.BorderBrush = Brushes.Black;
            }
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
            }
            rtbEditor.Focus();
        }
        private void TextBoxOpis_LostFocus(object sender, RoutedEventArgs e)
        {
            string opis = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            if (opis.Trim().Equals(String.Empty))
            {
                rtbEditor.BorderBrush = Brushes.Red;
                rtbEditor.BorderThickness = new Thickness(1);
            }
            else
            {
                rtbEditor.BorderBrush = Brushes.Black;
            }
        }

        private void cmbFontColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontColor.SelectedItem != null)
            {
                var selectedItem = (PropertyInfo)cmbFontColor.SelectedItem;
                var color = (Color)selectedItem.GetValue(null, null);

                rtbEditor.Selection.ApplyPropertyValue(Inline.ForegroundProperty, color.ToString());
            }

            rtbEditor.Focus();
        }

        private void ComboBoxSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cmbFontSize.SelectedItem != null)
            {
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
            }
            rtbEditor.Focus();
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object ob = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (ob != DependencyProperty.UnsetValue) && (ob.Equals(FontWeights.Bold));

            ob = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (ob != DependencyProperty.UnsetValue) && (ob.Equals(FontStyles.Italic));

            ob = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (ob != DependencyProperty.UnsetValue) && (ob.Equals(TextDecorations.Underline));

            ob = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = ob;

            ob = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = ob.ToString();

            ob = rtbEditor.Selection.GetPropertyValue(Inline.ForegroundProperty);
            cmbFontColor.SelectedItem = ob;


            string s = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            char[] delimiters = new char[] { ' ', '\r', '\n' };
            string[] pom = s.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            BrRtextBox.Text = "Word Count: " + pom.Length.ToString();
        }
    }
}
