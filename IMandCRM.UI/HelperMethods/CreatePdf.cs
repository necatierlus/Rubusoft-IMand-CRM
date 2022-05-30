using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMandCRM.UI.HelperMethods
{
    public static class CreatePdf
    {
        public static bool SelectCreatePdf(string fileName, string pageUrl, string footer)
        {
            try
            {
                // instantiate a html to pdf converter object
                HtmlToPdf converter = new HtmlToPdf();

                // set converter rendering engine
                converter.Options.RenderingEngine = RenderingEngine.Blink;
                // set converter options
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

                // set timeout
                converter.Options.MinPageLoadTime = 10;
                converter.Options.MaxPageLoadTime = 20;

                // footer settings
                converter.Options.DisplayFooter = true;
                converter.Footer.DisplayOnFirstPage = true;
                converter.Footer.DisplayOnOddPages = true;
                converter.Footer.DisplayOnEvenPages = true;
                converter.Footer.Height = 40;



                // page numbers can be added using a PdfTextSection object
                PdfTextSection text1 = new PdfTextSection(0, 0, footer, new System.Drawing.Font("Arial", 9));
                PdfTextSection text2 = new PdfTextSection(0, 3, "Sayfa: {page_number} / {total_pages}  ", new System.Drawing.Font("Arial", 7));
                text1.HorizontalAlign = PdfTextHorizontalAlign.Center;
                text1.VerticalAlign = PdfTextVerticalAlign.Middle;
                text1.BackColor = System.Drawing.ColorTranslator.FromHtml("#fee9e9");
                text1.Height = 35;

                text2.HorizontalAlign = PdfTextHorizontalAlign.Right;

                converter.Footer.Add(text1);
                converter.Footer.Add(text2);

                // create a new pdf document converting an url
                string url = Constants.Constants.Url + pageUrl;
                SelectPdf.PdfDocument doc = converter.ConvertUrl(url);


                if (System.IO.File.Exists("wwwroot\\assets\\media\\bid\\pdf\\" + fileName + ".pdf"))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete("wwwroot\\assets\\media\\bid\\pdf\\" + fileName + ".pdf");
                }

                // save pdf document
                doc.Save("wwwroot\\assets\\media\\bid\\pdf\\" + fileName + ".pdf");

                // close pdf document
                doc.Close();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}
