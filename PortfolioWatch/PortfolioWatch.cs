﻿using CsvHelper;
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

        private void btnLoad_Click(object sender, EventArgs e)
        {
            String transactions = this.txtTransactions.Text;
            List<TransactionCsv> list = null;
            List<PortfolioRecord> portfolioList = null;
            using (StreamReader reader = new StreamReader(transactions))
            {
                var csv = new CsvReader(reader);
                list = csv.GetRecords<TransactionCsv>().ToList();
            }

            if(!String.IsNullOrEmpty(this.txtPortfolio.Text) && File.Exists(this.txtPortfolio.Text))
            {
                using (StreamReader reader = new StreamReader(this.txtPortfolio.Text))
                {
                    var csv = new CsvReader(reader);
                    portfolioList = csv.GetRecords<PortfolioRecord>().ToList();
                }

                List<Transaction> finalList = new List<Transaction>();
                foreach(var trans in list)
                {
                    PortfolioRecord match = portfolioList.FirstOrDefault(x => x.ISIN == trans.ISIN);
                    if(match != null)
                    {
                        finalList.Add(new Transaction(trans) { CurrentValue = match.WertDecimal / match.Anzahl });
                    }
                }

                finalList.Sort((x, y) => x.Perf.CompareTo(y.Perf));
                this.dataGridView1.DataSource = finalList;
            }
            else
            {
                this.dataGridView1.DataSource = list;
            }

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String path = GetSettingsPath();
            if (!File.Exists(path)) { return; }
            String[] settings = File.ReadAllLines(path);
            this.txtTransactions.Text = GetSetting(settings, 0);
            this.txtPortfolio.Text = GetSetting(settings, 1);
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
    }
}