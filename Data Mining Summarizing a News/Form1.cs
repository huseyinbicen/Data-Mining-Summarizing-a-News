using Data_Mining_Summarizing_a_News.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Data_Mining_Summarizing_a_News
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txt_Ozet.Clear();

            try
            {
                SummaryTool summaryTool = new SummaryTool(txt_Makale.Text.ToLower());
                txt_Ozet.Text = summaryTool.GetSummaryText();
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
            
        }
    }
}
