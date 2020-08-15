using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Generator_SVG_u_HTML
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Pokrecem ucitavanje slika iz fajla koji se nalazi u ovom folderu sa imenom sprite.svg...");
            Thread.Sleep(3000);
            List<string> ListaImena = Generator.LoadSVG("sprite.svg");
            if(ListaImena == null || ListaImena.Count == 0)
            {
                Console.WriteLine("Doslo je do greske! Nisam pronasao ikonice.");
                return;
            }
            Console.WriteLine($"Pronasao sam {ListaImena.Count} ikonica!\n\n\n");
            
            Console.WriteLine("Sledi generisanje images.html fajla...");
            Thread.Sleep(3000);
            Generator.GenerateHTML("images.html", ListaImena);

            Console.WriteLine("Generisanje uspesno zavrseno!");

            Console.ReadLine();
        }
    }
    static class Generator
    {
        public static List<string> LoadSVG(string ImeFajla)
        {
            try
            {
                List<string> Lista = new List<string>();
            
                using (StreamReader reader = new StreamReader(ImeFajla))
                {
                    while (reader.EndOfStream != true)
                    {
                        string linija = reader.ReadLine();
                        if (linija.Contains("id=\""))
                        {
                            int start, end;
                            start = linija.IndexOf("id=\"", 0) + 4;
                            end = linija.IndexOf("\"", start);
                            string ime = linija[start..end];
                            Lista.Add(ime);
                            Console.WriteLine("Pronasao sam ime! " + ime);
                        }
                    }
                }
                return Lista;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("Exception: " + ex.StackTrace);
            }
            return null;
        }
        public static void GenerateHTML(string ImeFajla, List<string> Ikonice)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(ImeFajla))
                {
                    sw.WriteLine("<!DOCTYPE html>\n<html lang=\"en\">\n\n");
                    sw.WriteLine("<head>\n<meta charset=\"UTF-8\">\n<title>Icons</title>\n<link href=\"https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css\" rel=\"stylesheet\"\nintegrity=\"sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh\" crossorigin=\"anonymous\">\n");

                    sw.WriteLine("<style>\nsvg {\nheight: 200px;\nwidth: 200px;}\n.category__body {\ndisplay: flex;\nflex-wrap: wrap;\njustify-content: space-around;\n}\n");
                    sw.WriteLine("p { \nmargin-top: 15px;\nfont-weight: 700;\ntext-align: center;\n}\n");
                    sw.WriteLine("p::before { \n content: \"ID: \"; \nuser-select: none;\nfont-weight: 400;\n}\n</style>\n</head>\n\n\n");
                    sw.WriteLine("<body>\n<div class=\"container\">\n\t<div class=\"category p-3\">\n\t\t<div class=\"category__body\">\n\t\t\t");

                    foreach (string icon in Ikonice)
                    {
                        sw.WriteLine($"<!--   {icon}  -->");
                        sw.WriteLine("<div class=\"icon p-4\">");
                        sw.WriteLine($"  <svg><use xlink:href=\"sprite.svg#{icon}\" href=\"sprite.svg#{icon}\"></use></svg>");
                        sw.WriteLine($"  <p class=\"text-primary\">{icon}</p>");
                        sw.WriteLine("</div>");
                        sw.WriteLine("");
                    }

                    sw.WriteLine("\n\t\t\t</div>\n\t\t</div>\n\t</div>\n</body>\n</html>\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("Exception: " + ex.StackTrace);
            }
        }
    }
}
