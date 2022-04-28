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
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Drawing;
using ControlzEx.Standard;

namespace lab7
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Student> Students = new List<Student> {
            new Student() { Nazwisko = "Han", Ocena = 4.0 },
            new Student() { Nazwisko = "Seo", Ocena = 4.5 },
            new Student() { Nazwisko = "Bang", Ocena = 5.0 },
        };
        public MainWindow()
        {
            InitializeComponent();

        }

        private void btnMelduj_Click(object sender, RoutedEventArgs e)
        {
            FileStream fs = new FileStream(@"F:\rejestr.txt", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(DateTime.Now);
            sw.Close();
            fs.Close();
        }
        
        private void btnCzytaj_Click(object sender, RoutedEventArgs e)
        {
            StreamReader sr = new StreamReader(@"F:\dane.txt");
            List<double> lista = new List<double>();
            while (sr.Peek() != -1)
            {
                string linia = sr.ReadLine();
                lista.Add(Convert.ToDouble(linia));
            }
            foreach (var s in lista)
            {
                lbxWys.Items.Add(s.ToString("F03"));
            }
            lblCzytaj.Content = "Min: "+ lista.Min() + ", Max: " + lista.Max() + ", Średnia: " + lista.Average();
            sr.Close();
        }
        //zad C
      [Serializable]
        public class Student
        {
            public string Nazwisko { get; set; }  
            public double Ocena { get; set; }
        }
       [Serializable]
        public class Grupa
        {
            [JsonPropertyName("Name")]
            [XmlElement("NazwaGrupy")]
            public string Nazwa { get; set; }
            [JsonIgnore]
            [XmlIgnore]
            
            public List<Student> Studenci { get; set; }
            public int LiczbaStudentów { get { return Studenci.Count; }}
            [XmlAttribute("srd")]
            public double SredniaOcen { get { return Studenci.Average(t => t.Ocena); }}

            public void Wyśletl(ListBox list)
            {
                string wys = "";
                wys += "Grupa: " + Nazwa + "\n";
                foreach (var student in Studenci)
                {
                    wys += "\t" + student.Nazwisko + " " + student.Ocena + "\n";
                }
                wys += $"Liczba studentów: {LiczbaStudentów}\nŚrednia ocen: {SredniaOcen.ToString("F02")}";
                list.Items.Add(wys);
            }
            public void ZapiszDoPliku(string name)
            {
                string formed = "";
                formed += "Grupa: " + Nazwa + "\n";

                foreach (var student in Studenci)
                {
                    formed += "\t" + student.Nazwisko + " " + student.Ocena + "\n";

                }
                formed += $"Liczba sttudentów: {LiczbaStudentów}\n Srednia ocen: {SredniaOcen.ToString("F02")}";
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.AppendAllText(saveFileDialog.FileName, $"{Convert.ToString(formed)}\n");
                }
                FileStream fs = new FileStream(name + ".txt", FileMode.Create);
                fs.Close();
            }
        }


        private void btnWczytaj_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                lbxWys.Items.Add(File.ReadAllText(openFileDialog.FileName));
            }
        }

        private void btnZapisz_Click(object sender, RoutedEventArgs e)
        {
            Grupa gr = new Grupa() { Nazwa = "IPpp", Studenci = Students };
            gr.Wyśletl(lbxWys);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                foreach (var item in lbxWys.Items)
                {
                    File.AppendAllText(saveFileDialog.FileName, $"{Convert.ToString(item)}\n");
                }
            }
            FileStream fs = new FileStream("grupa.xml", FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(Grupa));
            
            serializer.Serialize(fs, gr);
            fs.Close();
        }

        private void btnZapiszBin_Click(object sender, RoutedEventArgs e)
        {
            Grupa gr = new Grupa() { Nazwa = "Koks", Studenci = Students };
            gr.Wyśletl(lbxWys);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                foreach (var item in lbxWys.Items)
                {
                    File.AppendAllText(saveFileDialog.FileName, $"{Convert.ToString(item)}\n");
                }
            }
            FileStream fs = new FileStream("grupa.bin", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, gr);
            fs.Close();
        }

        private void btnWczytajBin_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                lbxWys.Items.Add(File.ReadAllText(openFileDialog.FileName));
            }
        }

        private void btnZapiszJSON_Click(object sender, RoutedEventArgs e)
        {
            Grupa gr = new Grupa() { Nazwa = "json", Studenci = Students };
            gr.Wyśletl(lbxWys);
            string json =JsonSerializer.Serialize<Grupa>(gr);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                foreach (var item in lbxWys.Items)
                {
                    File.AppendAllText(saveFileDialog.FileName, $"{Convert.ToString(item)}\n");
                }
            }
            FileStream fs = new FileStream("grupa.json", FileMode.Create);
            fs.Close();
        }

        private void btnWczytajJSON_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                lbxWys.Items.Add(File.ReadAllText(openFileDialog.FileName));
            }
        }

        private void btnZapisDoPliku_Click(object sender, RoutedEventArgs e)
        {
            Grupa gr = new Grupa() { Nazwa = "zapisik", Studenci = Students };
            gr.ZapiszDoPliku("koksik");
        }

        private void btnWybierz_Click(object sender, RoutedEventArgs e)
        {
            var filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "G:\\";
            openFileDialog.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
               
            }

            Bitmap image = new Bitmap(filePath, true);
            BITMAPINFOHEADER bi = new BITMAPINFOHEADER();
           
            lblSzerokosc.Content = image.Width;
            lblWysokosc.Content = image.Height;
            lblKolor.Content = bi.biBitCount;

        }
    }
}
