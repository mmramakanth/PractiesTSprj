using PracticeTS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PracticeTS.Services
{
    public  class WordGent
    {
        public static void BindWord()
        {
            var templ = new WordTemplate();
            templ.WordParameters.Add(new WordParameter() { Name = "##Text1##", Text = "This is success" });
            templ.WordParameters.Add(new WordParameter() { Name = "home-2.PNG", Image = new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/Document/Koala.jpg")) });
            templ.WordParameters.Add(new WordParameter() { Name = "home-1.PNG", Image = new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/Document/Penguins.jpg")) });

            var templatePath = System.Web.HttpContext.Current.Server.MapPath("~/Document/templateword.docx");
            var outputPath = System.Web.HttpContext.Current.Server.MapPath("~/Document/Outputemp.docx");
            templ.ParseTemplate(templatePath, outputPath);



            //var temp2 = new PowerPointTemplate();
            //temp2.PowerPointParameters.Add(new PowerPointParameter() { Name = "Text_1", Text = "This is success" });
            //temp2.PowerPointParameters.Add(new PowerPointParameter() { Name = "img_2", Image = new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/Document/Koala.jpg")) });

            //var templatePath2 = System.Web.HttpContext.Current.Server.MapPath("~/Document/Presentation1.pptx");
            //var outputPath2 = System.Web.HttpContext.Current.Server.MapPath("~/Document/output2.pptx");
            //temp2.ParseTemplate(templatePath2, outputPath2);


        }
    }
}