using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Drawing;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Presentation;
using System.Collections;
using Drawing = DocumentFormat.OpenXml.Drawing;
using System.Drawing;
using PracticeTS.Models;

namespace PracticeTS.Services
{

    public class PowerPointTemplate
    {
        public int deleteslide;
        public PowerPointTemplate()
        {
            this.PowerPointParameters = new List<PowerPointParameter>();
            this.SlideDictionary = new Dictionary<SlidePart, SlideId>();
        }

        public List<PowerPointParameter> PowerPointParameters { get; set; }

        public Dictionary<SlidePart, SlideId> SlideDictionary { get; set; }

        public void ParseTemplate(string templatePath, string templateOutputPath)
        {
            using (var templateFile = File.Open(templatePath, FileMode.Open, FileAccess.Read)) //read our template
            {
                using (var stream = new MemoryStream())
                {
                    templateFile.CopyTo(stream); //copy template

                    using (var presentationDocument = PresentationDocument.Open(stream, true)) //open presentation document
                    {
                        // Get the presentation part from the presentation document.
                        var presentationPart = presentationDocument.PresentationPart;

                        // Get the presentation from the presentation part.
                        var presentation = presentationPart.Presentation;

                        var slideList = new List<SlidePart>();

                        //get available slide list
                        foreach (SlideId slideID in presentation.SlideIdList)
                        {
                            var slide = (SlidePart)presentationPart.GetPartById(slideID.RelationshipId);
                            slideList.Add(slide);
                            SlideDictionary.Add(slide, slideID);//add to dictionary to be used when needed
                        }

                        //loop all slides and replace images and texts
                        foreach (var slide in slideList)
                        {
                            ReplaceImages(presentationDocument, slide); //replace images by name

                            var paragraphs = slide.Slide.Descendants<Paragraph>().ToList(); //get all paragraphs in the slide

                            foreach (var paragraph in paragraphs)
                            {
                                ReplaceText(paragraph); //replace text by placeholder name
                            }
                        }

                        var slideCount = presentation.SlideIdList.ToList().Count; //count slides
                        DeleteSlide(presentation, slideList[slidedelete(deleteslide)]); //delete last slide

                        presentation.Save(); //save document changes we've made
                    }
                    stream.Seek(0, SeekOrigin.Begin);//scroll to stream start point

                    //save output file
                    using (var fileStream = File.Create(templateOutputPath))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
            }
        }


        /// <summary>
        /// Deletes slide from presentation
        /// </summary>
        /// <param name="presentation"></param>
        /// <param name="slidePart"></param>
        void DeleteSlide(Presentation presentation, SlidePart slidePart)
        {
            var delSlideID = SlideDictionary[slidePart];
            presentation.SlideIdList.RemoveChild(delSlideID);
        }

        public int slidedelete(int sddelte)
        {
            deleteslide = sddelte;
            return deleteslide;
        }


        /// <summary>
        /// Replaces slidePart images
        /// </summary>
        /// <param name="presentationDocument"></param>
        /// <param name="slidePart"></param>
        void ReplaceImages(PresentationDocument presentationDocument, SlidePart slidePart)
        {
            // get all images in the slide
            var imagesToReplace = slidePart.Slide.Descendants<Blip>().ToList();

            if (imagesToReplace.Any())
            {
                var index = 0;//image index within the slide

                //find all image names in the slide
                var slidePartImageNames = slidePart.Slide.Descendants<DocumentFormat.OpenXml.Presentation.Picture>()
                                        .Where(a => a.NonVisualPictureProperties.NonVisualDrawingProperties.Title.HasValue)
                                        .Select(a => a.NonVisualPictureProperties.NonVisualDrawingProperties.Title.Value).Distinct().ToList();

                //check all images in the slide and replace them if it matches our parameter
                foreach (var imagePlaceHolder in slidePartImageNames)
                {
                    //check if we have image parameter that matches slide part image
                    foreach (var param in PowerPointParameters)
                    {
                        //replace it if found by image name
                        if (param.Image != null && param.Name.ToLower() == imagePlaceHolder.ToLower())
                        {
                            var imagePart = slidePart.AddImagePart(ImagePartType.Jpeg); //add image to document

                            using (FileStream imgStream = new FileStream(param.Image.FullName, FileMode.Open))
                            {
                                imagePart.FeedData(imgStream); //feed it with data
                            }

                            var relID = slidePart.GetIdOfPart(imagePart); // get relationship ID

                            imagesToReplace.Skip(index).First().Embed = relID; //assign new relID, skip if this is another image in one slide part
                        }
                    }
                    index += 1;
                }
            }
        }

        /// <summary>
        /// Replace all text placeholders in paragraph
        /// </summary>
        /// <param name="paragraph"></param>
        void ReplaceText(Paragraph paragraph)
        {
            var parent = paragraph.Parent; //get parent element - to be used when removing placeholder
            var dataParam = new PowerPointParameter();

            if (ContainsParam(paragraph, ref dataParam)) //check if paragraph is on our parameter list
            {
                //insert text list
                if (dataParam.textcolor != null)
                {


                    var newParagraph = CreateStyledParagraph(dataParam.textcolor.Trim(), dataParam.FontSize, dataParam.bold, dataParam.italic, dataParam.color);
                    parent.InsertBefore(newParagraph, paragraph);


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

        /// <summary>
        /// Checks if process parameter to replace with text or image
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        public bool ContainsParam(Paragraph paragraph, ref PowerPointParameter dataParam)
        {
            foreach (var param in this.PowerPointParameters)
            {
                if (!string.IsNullOrEmpty(param.Name))
                {
                    dataParam = param;
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Clones paragraph with styles
        /// </summary>
        /// <param name="sourceParagraph"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Paragraph CloneParaGraphWithStyles(Paragraph sourceParagraph, string paramKey, string text)
        {
            var xmlSource = sourceParagraph.OuterXml;

            xmlSource = xmlSource.Replace(paramKey.Trim(), text.Trim());

            return new Paragraph(xmlSource);
        }
        public Paragraph CreateStyledParagraph(string text, int fontsize, bool bold, bool italic, Color color)
        {
            var run = new Drawing.Run(); var run2 = new Drawing.Run(); var textBody = new Drawing.Text(); var textBody2 = new Drawing.Text();
            var newParagraph = new Paragraph();
            string[] newStringa = text.Split(new string[] { "<", ">" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < newStringa.Count(); i++)
            {
                if (newStringa[i].Contains('('))
                {
                    var runProperties2 = new RunProperties(); //set basic styles for paragraph
                    Run ru11 = new Run();
                    runProperties2.Bold = bold;
                    runProperties2.Italic = italic;
                    runProperties2.FontSize = fontsize;
                    runProperties2.Dirty = false;
                    string rem = newStringa[i].Substring(1);
                    Color col = ColorTranslator.FromHtml("#007AC9");
                    var hexColor = col.R.ToString("X2") + col.G.ToString("X2") + col.B.ToString("X2");//convert color to hex
                    //convert color to hex
                    var solidFill = new SolidFill();
                    var rgbColorModelHex = new RgbColorModelHex() { Val = hexColor };
                    var rgbColorModelHex1 = new LatinFont() { Typeface = "Arial Narrow" };
                    solidFill.Append(rgbColorModelHex1);
                    solidFill.Append(rgbColorModelHex);
                    runProperties2.Append(solidFill);
                    textBody2 = new Drawing.Text();
                    textBody2.Text = rem; //assign text
                    run2 = new Drawing.Run();
                    run2.Append(runProperties2);//append styles
                    run2.Append(textBody2);
                    newParagraph.Append(run2);

                }
                else
                {
                    var runProperties = new RunProperties(); //set basic styles for paragraph
                    runProperties.Bold = false;
                    runProperties.Italic = false;
                    runProperties.FontSize = fontsize;
                    runProperties.Dirty = false;
                    var hexColor = color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");//convert color to hex
                    var solidFill = new SolidFill();
                    var rgbColorModelHex = new RgbColorModelHex() { Val = hexColor };
                    solidFill.Append(rgbColorModelHex);
                    runProperties.Append(solidFill);
                    textBody = new Drawing.Text();
                    textBody.Text = newStringa[i]; //assign text
                    run = new Drawing.Run();
                    run.Append(runProperties);//append styles
                    run.Append(textBody);
                    newParagraph.Append(run);

                }
            }


            return newParagraph;
            //append text


            //append run to paragraph

        }
    }
}
