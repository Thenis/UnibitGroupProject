using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace UnibitGroupProject
{
    public partial class MainForm_JSON_XML : MetroForm
    {
        public MainForm_JSON_XML()
        {
            InitializeComponent();
        }

        private void MainForm_JSON_XML_Load(object sender, EventArgs e)
        {
            
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            AddDataForm newform = new AddDataForm();
            newform.Show();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            ExportForm exportF = new ExportForm();
            exportF.Show();
        }
    }
}
