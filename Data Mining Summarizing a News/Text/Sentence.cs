using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Mining_Summarizing_a_News.Text
{
    class Sentence
    {
        public int str_Number;
        public String str_Value;
        public int str_Lenght;
        public double str_Skor;

        public Sentence(int str_Number, String str_Value, int str_Lenght, double str_Skor)
        {
            this.str_Number = str_Number;
            this.str_Value = str_Value;
            this.str_Lenght = str_Lenght;
            this.str_Skor = str_Skor;
        }
    }
}
