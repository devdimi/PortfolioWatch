using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortfolioWatch
{
   

    public partial class PortfolioWatch : Form
    {
        public PortfolioWatch()
        {
            InitializeComponent();
        }

        List<Transaction> transactions;
        
        private void btnLoad_Click(object sender, EventArgs e)
        {
            PortfolioDao dao = new PortfolioDao();
            if(String.IsNullOrEmpty(this.txtTransactions.Text) || String.IsNullOrEmpty(this.txtPortfolio.Text))
            {
                return;
            }

            var records = dao.GetData(this.txtTransactions.Text, this.txtPortfolio.Text);
            this.dataGridView1.DataSource = records;
            this.transactions = records as List<Transaction>;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String path = GetSettingsPath();
            if (!File.Exists(path)) { return; }
            String[] settings = File.ReadAllLines(path);
            this.txtTransactions.Text = GetSetting(settings, 0);
            this.txtPortfolio.Text = GetSetting(settings, 1);

            this.btnLoad_Click(sender, e);
        }

        public static String GetSetting(String[] array, Int32 index)
        {
            if (array.Length <= index) { return String.Empty; }
            return array[index];
        }

        public const Int32 SetttingsVersion = 1;
        public static string GetSettingsPath()
        {
            string commonData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string path = Path.Combine(commonData,
                "devdimi",
                "PortfolioWatch.v" + SetttingsVersion + ".settings.txt");
            EnsureDirExists(Path.GetDirectoryName(path));
            return path;
        }

        public static void EnsureDirExists(String dir)
        {
            if(Directory.Exists(dir)){ return;}

            DirectoryInfo baseDir = Directory.GetParent(dir);
            if(!baseDir.Exists) { EnsureDirExists(baseDir.FullName);}
            Directory.CreateDirectory(dir);
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            String path = GetSettingsPath();
            if (File.Exists(path)) { File.Delete(path); }
            File.WriteAllLines(path, new[]
            {
                this.txtTransactions.Text,
                this.txtPortfolio.Text
            });

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Form1_Leave(sender, e);
            e.Cancel = false;
        }

        private void PortfolioWatch_Load(object sender, EventArgs e)
        {
            this.Form1_Load(sender, e);
        }

        private void btnAggregate_Click(object sender, EventArgs e)
        {
            if(this.transactions == null)
            {
                return;
            }

            if(this.transactions == null) { return; }
            var groups = this.transactions.GroupBy(x => x.ISIN).ToList();
            List<PortfolioAggregate> list = new List<PortfolioAggregate>();
            foreach (var group in groups)
            {
                int sumNum = group.Sum(x => x.Anzahl);
                decimal sumValue = group.Sum(x => x.CurrentValue * x.Anzahl);
                decimal averagePrice = sumValue / sumNum;
                PortfolioAggregate record = new PortfolioAggregate()
                {
                    ISIN = group.First().ISIN,
                    Produkt = group.First().Produkt,
                    CurrentPrice = group.First().Price,
                    AveragePrice = averagePrice
                };

                list.Add(record);
            }
            this.dataGridView1.DataSource = list;
        }

        private void btnSingleTransactions_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = this.transactions;
        }
    }
}
