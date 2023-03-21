using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PR482017_Dimitrije_Vukovic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static BindingList<Classes.Kafa> kafe { get; set; }
        private Classes.DataIO serializer = new Classes.DataIO();

        public MainWindow()
        {
            kafe = serializer.DeSerializeObject<BindingList<Classes.Kafa>>("brendovi.xml");
            if (kafe == null)
            {
                LoadCoffee();
            }

            DataContext = this;
            InitializeComponent();
        }

        private void LoadCoffee()
        {
            kafe = new BindingList<Classes.Kafa>();

            kafe.Add(new Classes.Kafa(kafe.Count + 1, "lavazza.jpg", "Lavazza", DateTime.Now, "Lavazza.rtf"));
            FileStream fileStream = new FileStream("Lavazza.rtf", FileMode.Create);
            StreamWriter sw = new StreamWriter(fileStream);
            sw.WriteLine("Kompanija Lavazza je osnovana 1895. godine od strane Luigi Lavazza-e. Danas je Lavazza neprikosnoveni lider na tržištu espresso kafe u Italiji sa 45% udela na tržištu." +
                " Podatak da 75% italijanskih porodica kupuje Lavazza proizvode dovoljno govori o privrženosti i jačini brenda.");
            sw.Close();
            fileStream.Close();

            kafe.Add(new Classes.Kafa(kafe.Count + 1, "Julius.png", "Julius", DateTime.Now, "Julius.rtf"));
            FileStream fileStream2 = new FileStream("Julius.rtf", FileMode.Create);
            StreamWriter sw2 = new StreamWriter(fileStream2);
            sw2.WriteLine("Julius Meinl je vodeća kompanija za preradu kafe u Austriji, Italiji, Srednjoj i Istočnoj Evropi. Svoje poslovanje fokusira na kafi, čaju i konzerviranom voću, s kafom kao najvažnijom kategorijom." +
                " Julius Meinl svaki dan usluži više od 40.000 kupaca širom sveta.");
            sw2.Close();
            fileStream2.Close();

            kafe.Add(new Classes.Kafa(kafe.Count + 1, "Grand.png", "Grand", DateTime.Now, "Grand.rtf"));
            FileStream fileStream3 = new FileStream("Grand.rtf", FileMode.Create);
            StreamWriter sw3 = new StreamWriter(fileStream3);
            sw3.WriteLine("Poštujući želje potrošača i nudeći im više od ukusa, Grand kafa je izgradila prepoznatljiv koncept uživanja u kafi –" +
                " uvek najbolji kvalitet, bogata lepeza proizvoda, prepoznatljiva aroma, vrhunski estetski užitak, inovativnost i dostupnost u svakom trenutku i na svakom mestu.");
            sw3.Close();
            fileStream3.Close();

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void buttonDodaj_Click(object sender, RoutedEventArgs e)
        {
            AddWindow dw = new AddWindow();
            dw.ShowDialog();
        }

        private void buttonIzlaz_Click(object sender, RoutedEventArgs e)
        {
            serializer.SerializeObject<BindingList<Classes.Kafa>>(kafe, "brendovi.xml");
            foreach (Classes.Kafa k in kafe)
            {
                File.Delete(k.Opis);
            }
            this.Close();
        }

        private void ButtonProcitaj_Click(object sender, RoutedEventArgs e)
        {
            ReadWindow rw = new ReadWindow(Tabela.SelectedIndex);
            rw.ShowDialog();
        }

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            AddWindow aw = new AddWindow(Tabela.SelectedIndex);
            aw.ShowDialog();
        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (Tabela.SelectedIndex >= 0)
            {
                for (int i = 0; i < kafe.Count(); i++)
                {
                    if (Tabela.SelectedIndex + 1 < kafe[i].Index)
                    {
                        kafe[i] = new Classes.Kafa(kafe[i].Index - 1, kafe[i].Slika, kafe[i].Naziv, kafe[i].Datum, kafe[i].Opis);
                    }
                }
                File.Delete(kafe[Tabela.SelectedIndex].Opis);
                kafe.RemoveAt(Tabela.SelectedIndex);
            }
        }
    }
}
