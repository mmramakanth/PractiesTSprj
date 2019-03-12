using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PracticeTS.Services;
using Spire.Doc;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Xml;
using SendGrid;
using System.Net.Mail;
using System.Net;

namespace PracticeTS.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            //WordGent.BindWord();
            //Document doc = new Document();
            ////Pass path of Word Document in LoadFromFile method  
            //doc.LoadFromFile(Server.MapPath("~/Document/Outputemp.docx"));
            ////Pass Document Name and FileFormat of Document as Parameter in SaveToFile Method  
            //doc.SaveToFile(Server.MapPath("~/Document/Convert.PDF"), FileFormat.PDF);
            ////Launch Document  
            //System.Diagnostics.Process.Start(Server.MapPath("~/Document/Convert.PDF"));

            // var latlong = getLatLong("", "AI-2640");

           try
            {
                SendGridMessage myMessage = new SendGridMessage();
                myMessage.From = new MailAddress("info@idearosolutions.com");
                myMessage.Subject = "New job request from website";
                //  myMessage.Text = "Hello World! ";

                myMessage.Html = "<table width='600' border='0' cellspacing='0' cellpadding='0' style='border:5px solid #3D899F;'><tr><td align='center' valign='middle'><table width='520' border ='0' cellspacing ='0' cellpadding ='0'><tr><td>&nbsp;  </td></tr><tr><td>&nbsp;  </td></tr><tr><td style='font-size:16px;font-family:Arial,Helvetica,sans-serif;color:#595959;line-height:140%;'><p><span style='font-size:20px;font-weight:bold;color:#3D899F'> New job request from website</span></p><p> Name: <span style='font-size:18px;font-weight:bold;color:#000'></span><br>Email: <span style='font-size:18px;font-weight:bold;color:#000'></span><br>Phone: <span style='font-size:18px;font-weight:bold;color:#000'> </span><br>Position Applying for: <span style='font-size:18px;font-weight:bold;color:#000'></span><br>Comments: <span style='font-size:18px;font-weight:bold;color:#000'> </span><br></p></td></tr><tr><td align='center'> &nbsp;</td></tr><tr><td align='center'><a href='https://idaerosolutions.com' target='_blank'><img src='https://idaerosolutions.com/images/logo-idaero.png' title='Idaero' alt='Idaero'></a></td></tr><tr><td>&nbsp;</td></tr></table></td></tr></table>";
              //  myMessage.AddAttachment(path);
                var credentials = new NetworkCredential("iDaerosol", "25Email13??");
                myMessage.AddTo("mmramakanth@gmail.com");
                //  myMessage.AddTo("venky@theweb.agency");
                // myMessage.Bcc("venky@theweb.agency");
                var transportWeb = new Web(credentials);
                transportWeb.DeliverAsync(myMessage);

            }

            catch (Exception ex)
            {


            }



            return View();
        }

        public string getLatLong(string Address, string Zip)
        {
            string latlong = "", address = "";
            if (Address != string.Empty)
            {
                address = "http://maps.googleapis.com/maps/api/geocode/xml?address=" + Address + "&sensor=false";
            }
            else
            {
                address = "http://maps.googleapis.com/maps/api/geocode/xml?components=postal_code:" + Zip.Trim() + "&sensor=false";
            }
            var result = new System.Net.WebClient().DownloadString(address);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            XmlNodeList parentNode = doc.GetElementsByTagName("location");
            var lat = "";
            var lng = "";
            foreach (XmlNode childrenNode in parentNode)
            {
                lat = childrenNode.SelectSingleNode("lat").InnerText;
                lng = childrenNode.SelectSingleNode("lng").InnerText;
            }
            latlong = Convert.ToString(lat) + "," + Convert.ToString(lng);
            return latlong;
        }

        public ActionResult About()
        {


            //    new Chart(width: 500, height: 300)

            //.AddTitle("Chart for languages")

            //     .AddSeries(

            //          chartType: "Doughnut",

            //       xValue: new[] { "ASP.NET", "HTML5", "C Language", "C++" },

            //         yValues: new[] { "90", "100", "80", "70" }). AddLegend("Summary")

            //       .Write("bmp").Save(Server.MapPath("~/Document/test2.jpeg"));

            var xvals = new[]
                {
                   "J","F","M","A","M","J","J","A","S","O","N","D"
                };
            //var xvals = new[]
            //     {
            //        new DateTime(2012, 4, 4),
            //        new DateTime(2012, 4, 5),
            //        new DateTime(2012, 4, 6),
            //        new DateTime(2012, 4, 7)
            //    };
            var yvals = new[] { 1, 3, 7, 12, 1, 3, 7, 12, 7, 12,5,4 };
            var yvals2 = new[] { 1, 3, 7, 12, 1, 3, 7,4,0,0,0,0 };
            // create the chart
            var chart = new Chart();
            chart.Width = 600;chart.Height = 250;
          //  chart.Size = new Size(600, 250);

        
            var chartArea = new ChartArea();
            
            chartArea.AxisX.LabelStyle.Format = "dd/MMM\nhh:mm";
            //chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            //chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.MajorGrid.LineWidth = 0;
            chartArea.AxisY.MajorGrid.LineWidth = 0;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisX.Interval = 1;
            chart.ChartAreas.Add(chartArea);

            var series = new Series();
            series.Name = "Series1";
            series.ChartType = SeriesChartType.FastLine;
            series.XValueType = ChartValueType.DateTime;

            chart.Series.Add(series);

            var series2= new Series();
            series.Name = "Series2";
            series.ChartType = SeriesChartType.FastLine;
            series.XValueType = ChartValueType.DateTime;

            chart.Series.Add(series2);
            // bind the datapoints
            chart.Series["Series1"].Points.DataBindXY(xvals, yvals);
            chart.Series["Series2"].Points.DataBindXY(xvals, yvals2);
            chart.Series["Series1"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            chart.Legends.Add(new Legend("Legend1"));

            // Set title
            chart.Legends["Legend1"].Title = "My legend";
            chart.Legends["Legend1"].Docking = Docking.Bottom;
            chart.Legends["Legend1"].Alignment = StringAlignment.Center;
            chart.Legends["Legend1"].LegendStyle = LegendStyle.Column;
            chart.Legends["Legend1"].MaximumAutoSize = 100;

            // Assign the legend to Series1.
            chart.Series["Series1"].Legend = "Legend1";
            chart.Series["Series1"].IsVisibleInLegend = true;
            // copy the series and manipulate the copy
            // chart.AlignDataPointsByAxisLabel();

            // draw!
            // chart.Invalidate();

            // write out a file
            chart.SaveImage(Server.MapPath("~/Document/test2.jpeg"));

            return View();



            // return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}