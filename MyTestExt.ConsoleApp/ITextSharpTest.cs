using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.util;
//using System.Web.Compilation;
//using iTextSharp.awt.geom;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.parser;


namespace MyTestExt.ConsoleApp
{
    //public  class ITextSharpTest
    //{
    //    public static void Do()
    //    {
    //       var t =  PdfWordPositionHelper.Handler(@"D:\0.Work\10月302019_1955341.pdf", "c6b34ee0edcff8de6d2c13dace057be6");


    //    }


    //}


    //public class PdfWordPositionHelper
    //{
    //    public static List<PdfWordRectange> Handler(string fileName, string text)
    //    {
    //        // PdfWordPositionHelper.Handler(@"D:\0.Work\10月302019_1955341.pdf", "c6b34ee0edcff8de6d2c13dace057be6");
    //        using (var reader = new PdfReader(fileName))
    //        {
    //            return HandlerBase(text, reader);
    //        }
    //    }

    //    public static List<PdfWordRectange> Handler(byte [] bytes, string text)
    //    {
    //        using (var reader = new PdfReader(bytes))
    //        {
    //            return HandlerBase(text, reader);
    //        }
    //    }

    //    private static List<PdfWordRectange> HandlerBase(string text, PdfReader reader)
    //    {
    //        var pageNum = reader.NumberOfPages;
    //        var parser = new PdfReaderContentParser(reader);

    //        var t = new PdfWordRenderListener();
    //        t.SearchText = text;

    //        for (var i = 1; i <= pageNum; i++)
    //        {
    //            t.Page = i;
    //            t.Width = reader.GetPageSize(i).Width;
    //            t.Height = reader.GetPageSize(i).Height;
    //            parser.ProcessContent(i, t);
    //        }

    //        return t.Points;
    //    }
    //}


    //public class PdfWordRenderListener : IRenderListener
    //{
    //    //Hold each coordinate
    //    public List<PdfWordRectange> Points = new List<PdfWordRectange>();

    //    public string SearchText { get; set; }

    //    public float Width { get; set; }

    //    public float Height { get; set; }

    //    public int Page { get; set; }

    //    public void RenderText(TextRenderInfo renderInfo)
    //    {
    //        // Get text
    //        var text = renderInfo.GetText();
    //        if (null != text && text.Contains(SearchText))
    //        {
    //            var boundingRectange = renderInfo.GetBaseline().GetBoundingRectange();
    //            var x = boundingRectange.X / Width;
    //            var y = boundingRectange.Y / Height;
    //            Points.Add(new PdfWordRectange(Page, x, y));
    //        }
    //    }

    //    #region MyRegion
    //    public void BeginTextBlock()
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public void EndTextBlock()
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public void RenderImage(ImageRenderInfo renderInfo)
    //    {
    //        //throw new NotImplementedException();
    //    } 
    //    #endregion
    //}

    //public class PdfWordRectange
    //{
    //    public PdfWordRectange(int page, float x, float y)
    //    {
    //        Page = page;
    //        X = x;
    //        Y = y;
    //    }

    //    public int Page { get; set; }

    //    public float X { get; set; }

    //    public float Y { get; set; }
    //}


}
