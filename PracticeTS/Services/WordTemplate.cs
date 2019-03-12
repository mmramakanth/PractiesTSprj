

using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PracticeTS.Models;
using System.Collections;
using DocumentFormat.OpenXml.Wordprocessing;
using A= DocumentFormat.OpenXml.Drawing;

namespace PracticeTS.Services
{
    public class WordTemplate
    {
        public WordTemplate()
        {
            this.WordParameters = new List<WordParameter>();
           
        }
        public List<WordParameter> WordParameters { get; set; }
        public void ParseTemplate(string templatePath, string templateOutputPath)
        {
            using (var templateFile = File.Open(templatePath, FileMode.Open, FileAccess.Read)) //read our template
            {
                using (var stream = new MemoryStream())
                {
                    templateFile.CopyTo(stream); //copy template

                    using (var wordDoc = WordprocessingDocument.Open(stream, true)) //open word document
                    {

                        foreach (var paragraph in wordDoc.MainDocumentPart.Document.Descendants<Paragraph>().ToList()) //loop through all paragraphs 
                        {
                            ReplaceImages(wordDoc, paragraph); //replace images

                            ReplaceText(paragraph); //replace text
                        }

                        wordDoc.MainDocumentPart.Document.Save(); //save document changes we've made
                    }
                    stream.Seek(0, SeekOrigin.Begin);//scroll to stream start point

                    //save file or overwrite it
                   // var outPath = WordTemplate.GetRootPath() + @"\Output\DocumentOutput.docx";

                    using (var fileStream = File.Create(templateOutputPath))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
            }
        }
        void ReplaceImages(WordprocessingDocument wordDoc, Paragraph paragraph)
        {
            // get all images in paragraph
            var imagesToReplace = paragraph.Descendants<A.Blip>().ToList();

            if (imagesToReplace.Any())
            {
                var index = 0;//image index within paragraph

                //find all original image names in paragraph
                var paragraphImageNames = paragraph.Descendants<DocumentFormat.OpenXml.Drawing.Pictures.NonVisualDrawingProperties>().ToList();

                //check all images in the paragraph and replace them if it matches our parameter
                foreach (var imagePlaceHolder in paragraphImageNames)
                {
                    //check if we have image parameter that matches paragraph image
                    foreach (var param in WordParameters)
                    {
                        //replace it if found by original image name
                        if (param.Image != null && param.Name.ToLower() == imagePlaceHolder.Name.Value.ToLower())
                        {
                            var imagePart = wordDoc.MainDocumentPart.AddImagePart(ImagePartType.Jpeg); //add image to document
                            using (FileStream imgStream = new FileStream(param.Image.FullName, FileMode.Open))
                            {
                                imagePart.FeedData(imgStream); //feed it with data
                            }

                            var relID = wordDoc.MainDocumentPart.GetIdOfPart(imagePart); // get relationship ID

                            imagesToReplace.Skip(index).First().Embed = relID; //assign new relID, skip if this is another image in one paragraph
                        }
                    }
                    index += 1;
                }
            }
        }
        void ReplaceText(Paragraph paragraph)
        {
            var parent = paragraph.Parent; //get parent element - to be used when removing placeholder
            var dataParam = new WordParameter();

            if (ContainsParam(paragraph, ref dataParam)) //check if paragraph is on our parameter list
            {
                //insert text list
                if (dataParam.Name.Contains("string[]")) //check if param is a list
                {
                    var arrayText = dataParam.Text.Split(Environment.NewLine.ToCharArray()); //in our case we split it into lines

                    if (arrayText is IEnumerable) //enumerate if we can
                    {
                        foreach (var itemData in arrayText)
                        {
                            Paragraph bullet = CloneParaGraphWithStyles(paragraph, dataParam.Name, itemData);// create new param - preserve styles
                            parent.InsertBefore(bullet, paragraph); //insert new element
                        }
                    }
                    paragraph.Remove();//delete placeholder
                }
                else
                {
                    //insert text line
                    var param = CloneParaGraphWithStyles(paragraph, dataParam.Name, dataParam.Text); // create new param - preserve styles
                    parent.InsertBefore(param, paragraph);//insert new element

                    paragraph.Remove();//delete placeholder
                }
            }
        }
        public static Paragraph CloneParaGraphWithStyles(Paragraph sourceParagraph, string paramKey, string text)
        {
            var xmlSource = sourceParagraph.OuterXml;

            xmlSource = xmlSource.Replace(paramKey.Trim(), text.Trim());

            return new Paragraph(xmlSource);
        }
        public bool ContainsParam(Paragraph paragraph, ref WordParameter dataParam)
        {
            foreach (var param in this.WordParameters)
            {
                if (!string.IsNullOrEmpty(param.Name) && paragraph.InnerText.ToLower().Contains(param.Name.ToLower()))
                {
                    dataParam = param;
                    return true;
                }
            }

            return false;
        }

    }
}