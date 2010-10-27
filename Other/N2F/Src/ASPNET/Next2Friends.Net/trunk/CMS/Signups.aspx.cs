using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using dotnetCHARTING;
using Next2Friends.Data;

namespace CMS
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(Object sender,EventArgs e)
        {

            Chart.Type = ChartType.Combo;//Horizontal;
            Chart.Size = "640s480";
            Chart.TempDirectory = @"temp";
            Chart.Mentor = false;
            Chart.Debug = false;
            Chart.Title = "New Signups";


            // This sample demonstrates using a shadow axis with calculated ticks.
            Axis replicaAxis = Chart.YAxis.Calculate("", true, dotnetCHARTING.Orientation.Right);

            replicaAxis.AddCalculatedTick("Average: %Value", Calculation.Average);
            replicaAxis.AddCalculatedTick("Max: %Value", Calculation.Maximum);
            replicaAxis.AddCalculatedTick("Running Number: %Value", Calculation.RunningSum);

            Chart.AxisCollection.Add(replicaAxis);

            SeriesCollection mySC = DaysSignup();

            // Add the random data.
            Chart.SeriesCollection.Add(mySC);


            Chart2.Type = ChartType.Scatter;//Horizontal;
            Chart2.Size = "640x480";
            Chart2.TempDirectory = @"temp";
            Chart2.Mentor = false;
            Chart2.Debug = false;
            Chart2.Title = "New Signups";


            // This sample demonstrates using a shadow axis with calculated ticks.
            Axis replicaAxis1 = Chart2.YAxis.Calculate("", true, dotnetCHARTING.Orientation.Right);

            replicaAxis1.AddCalculatedTick("Average: %Value", Calculation.Average);
            replicaAxis1.AddCalculatedTick("Max: %Value", Calculation.Maximum);
            replicaAxis1.AddCalculatedTick("Running Number: %Value", Calculation.RunningSum);

            Chart2.AxisCollection.Add(replicaAxis);

            SeriesCollection mySC1 = HourSignup();

            // Add the random data.
            Chart2.SeriesCollection.Add(mySC1);
              
        }

        public SeriesCollection DaysSignup()
        {
              SeriesCollection SC = new SeriesCollection();

              List<Signups> signups = Reporting.GetSignups();


              Series series = new Series();

              for (int i = 0; i < signups.Count; i++)
              {
                  Element e = new Element();
                  if (signups[i].Date.DayOfYear == DateTime.Now.DayOfYear)
                  {
                      e.Name = "Today";
                  }
                  else if (signups[i].Date.DayOfYear == DateTime.Now.AddDays(-1).DayOfYear)
                  {
                      e.Name = "Yesterday";
                  }
                  else
                  {
                    e.Name = signups[i].Date.ToShortDateString();
                  }

                  e.ShowValue = true;
                  e.Color = Color.CornflowerBlue;
                  e.YValue = signups[i].NumberOfSignups;
                  series.Elements.Add(e);

              }

              SC.Add(series);

              // Set Different Colors for our Series
              SC[0].DefaultElement.Color = Color.CornflowerBlue;
              SC[0].Background.Color= Color.CornflowerBlue;
              //SC[1].DefaultElement.Color = Color.FromArgb(255,255,0);
              //SC[2].DefaultElement.Color = Color.FromArgb(255,99,49);
              //SC[3].DefaultElement.Color = Color.FromArgb(0,156,255);

              return SC;
        }


        
        public SeriesCollection HourSignup()
        {
              SeriesCollection SC = new SeriesCollection();

              List<Signups> signups = Reporting.GetSignupsByHour();

              Series series = new Series();

              for (int i = 0; i < signups.Count; i++)
              {
                  Element e = new Element();
                  e.Name = signups[i].Date.Hour + ":00";

                  e.ShowValue = true;
                  e.YValue = signups[i].NumberOfSignups;
                  series.Elements.Add(e);

                 
              }

              SC.Add(series);

              // Set Different Colors for our Series
              SC[0].DefaultElement.Color = Color.CornflowerBlue;
              //SC[1].DefaultElement.Color = Color.FromArgb(255,255,0);
              //SC[2].DefaultElement.Color = Color.FromArgb(255,99,49);
              //SC[3].DefaultElement.Color = Color.FromArgb(0,156,255);

              return SC;
        }


        
    }
}
