using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data_Mining_Summarizing_a_News.Text
{
    class SummaryTool
    {
        String Org_Text;
        //String Sum_Text = "Numara ... Uzunluk ... Skor ... Cümle \n";
        String Sum_Text = "";
        List<Sentence> list_Cumle = new List<Sentence>();
        String FirstSentence, LastSentence;

        public SummaryTool(String Org_Text)
        {
            this.Org_Text = BoslukSil(Org_Text);
            DenemeTextSplit(Org_Text);
            metod1();
        }



        /// <summary>
        /// Burada iki algoritmayı birleştiriyorum.
        /// </summary>
        private void metod1()
        {
            List<String> list_EnCokKullanilanKelimeler = enCokKullanilanKelimeler(Org_Text);

            Sum_Text += "\n\n\n.................." + FirstSentence;

            for (int i = 0; i < list_EnCokKullanilanKelimeler.Count; i++)
            {
                for (int k = 0; k < list_Cumle.Count; k++)
                {
                    var x = list_Cumle[k];
                    if (x.str_Value.Contains(list_EnCokKullanilanKelimeler[i]))
                    {
                        //Sum_Text += "\n" + x.str_Number + " ... " + x.str_Lenght + " ... " + x.str_Skor.ToString("0.00") + " ... " + x.str_Value;
                        Sum_Text += "\n\n\n.................." + ilkHarfBuyukYaz(x.str_Value);
                        list_Cumle.RemoveAt(k);
                        break;
                    }
                }
            }

            Sum_Text += "\n\n\n.................." + LastSentence;
        }






        /// <summary>
        /// Genel Olarak Algoritma bu metodda. Sentence Listesini Dolduruyor.
        /// </summary>
        /// <param name="text">Verilen Makale</param>
        private void DenemeTextSplit(String text)
        {
            Dictionary<String, double> dictionary = new Dictionary<string, double>();
            text = text.Replace(":", ".");
            //text = text.Replace("   " , ".");
            List<String> SentenceList = Regex.Split(text, @"(?<=[\.!\?])\s+").ToList();
            SentenceList.Add(" ");

            // Burada Cümle uzunluğu 10 dan küçük olan cümleleri listeden siliyorum.
            for (int i = 0; i < SentenceList.Count; i++)
            {
                if (SentenceList[i].Length < 10)
                {
                    SentenceList.RemoveAt(i);
                }
            }

            FirstSentence = ilkHarfBuyukYaz(SentenceList[0].TrimStart()) + ilkHarfBuyukYaz(SentenceList[1].TrimStart());
            LastSentence = SentenceList[SentenceList.Count - 1];

            // Burada Her bir cümle, Bir sonraki cümle ile bir skor hesaplıyor. Skor Formül:  (Lenght(s1) + Lenght(s2)) / ((s1.KelimeSayisi + s2.KelimeSayisi)/2)
            for (int i = 0; i < SentenceList.Count - 1; i++)
            {
                String s1 = SentenceList[i];
                s1 = s1.Replace(",", ""); //Just cleaning up a bit
                s1 = s1.Replace(".", ""); //Just cleaning up a bit

                String s2 = SentenceList[i];
                s2 = s2.Replace(",", ""); //Just cleaning up a bit
                s2 = s2.Replace(".", ""); //Just cleaning up a bit

                double result = 1.0 * (SentenceList[i].Length + SentenceList[i + 1].Length) / ((s1.Split(' ').Length + s2.Split(' ').Length) / 2);

                if (!dictionary.ContainsKey(SentenceList[i]))
                {
                    dictionary.Add(SentenceList[i], result);
                }
                
            }


            // Sentence Listesini dolduruyorum.
            int count = 1;
            foreach (var item in dictionary)
            {
                list_Cumle.Add(new Sentence(count, item.Key, item.Key.Length, item.Value));
                count++;
            }



            list_Cumle = list_Cumle.OrderBy(o => o.str_Skor).ToList();

        }


        /// <summary>
        /// Makalede geçen kelimeler kaç defa tekrarlanmış, Bunları bir sözlükte tutan metoddur
        /// </summary>
        /// <param name="text">Verilen makaledir</param>
        /// <returns></returns>
        private List<string> enCokKullanilanKelimeler(String text)
        {

            List<string> list_enCokKullanilanKelimeler = new List<string>();

            Dictionary<String, int> dictionary = new Dictionary<string, int>();

            string words = text.ToLower();

            var results = words.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Where(x => x.Length > 3)
                                          .GroupBy(x => x)
                                          .Select(x => new { Count = x.Count(), Word = x.Key })
                                          .OrderByDescending(x => x.Count);

            foreach (var item in results)
            {
                dictionary.Add(item.Word, item.Count);
            }

            dictionary = DictionaryOptimize(dictionary);

            for (int i = 0; i < OzetUzunluk(); i++)
            {
                list_enCokKullanilanKelimeler.Add(dictionary.Keys.ElementAt(i));
            }

            return list_enCokKullanilanKelimeler;
        }




        /// <summary>
        /// Verilecek özet metnin toplam cümle sayısıdır.
        /// </summary>
        /// <returns>Oluşacak özet metnin toplam cümle sayısı</returns>
        private int OzetUzunluk()
        {
            //if (list_Cumle.Count < 34)
            //{
                return Convert.ToInt32(Math.Ceiling(1.0* list_Cumle.Count * 17 / 100));
            //}
            //return 7;
        }



        /// <summary>
        /// Özetlenen veriyi veren metod.
        /// </summary>
        /// <returns>Özet makale</returns>
        public String GetSummaryText()
        {
            return Sum_Text;
        }



        /// <summary>
        /// Algoritma gereği metindeki bütün harfleri küçük harfe dönüştürdüm.
        /// Çıktının güzel gözükmesi için her cümlenin ilk harfini büyük harfe çeviren metoddur.
        /// </summary>
        /// <param name="s">Bütün harfleri küçük olan cümle</param>
        /// <returns>ilk harfi büyük harfe çeviren cümle</returns>
        public string ilkHarfBuyukYaz(string s)
        {
            String ss = s;
            if (ss.IndexOf(' ') == 0)
            {
                ss = ss.Remove(0, 1);
            }
            if (String.IsNullOrEmpty(ss))
                return ss;
            if (ss.Length == 1)
                return ss.ToUpper();
            return ss.Remove(1).ToUpper() + ss.Substring(1);
        }




        /// <summary>
        /// Metindeki 2 karakterden fazla olan boşlukları sil.
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Boşlukları silmiş halini dönderir.</returns>
        private string BoslukSil(string str)
        {
            return Regex.Replace(str, " {2,}", string.Empty);
        }





        /// <summary>
        /// En çok tekrar eden kelimeler arasında MyWords Listesinde tükçede en çok kelimler olup olmadığını kontrol ediyorum.
        /// Listenin içinde varsa o dictionary kelimesini siliyorum.
        /// Listenin içinde yoksa bir şey yapmıyorum.
        /// </summary>
        /// <param name="dictionary">Fazla kelimeler olan dictioary</param>
        /// <returns>Yeni dictionary listem</returns>
        private Dictionary<String, int> DictionaryOptimize(Dictionary<String, int> dictionary)
        {
            Dictionary<String, int> New_dictionary = new Dictionary<string, int>();

            List<string> list = new MyWords().GetList();

            foreach (var item in dictionary)
            {
                if (!list.Contains(item.Key))
                {
                    New_dictionary.Add(item.Key,item.Value);
                }
            }

            return New_dictionary;
        }
    }
}

