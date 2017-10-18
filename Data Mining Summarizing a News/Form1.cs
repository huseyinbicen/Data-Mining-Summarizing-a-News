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
        String makele = "";
        String ozet = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txt_Ozet.Clear();

            makele = txt_Makale.Text;
            char[] karakter = { '.', ',', ' ', ':',';','!', '+' , '-','*','/','&','%','(',')','?','{','}','[',']','<','>','|'};
            List<String> dizi = makele.Split(karakter).ToList();

            foreach (String item in dizi)
            {
                ozet += item + " ";
            }

            txt_Ozet.Text = ozet ;
        }

      
    }
}
