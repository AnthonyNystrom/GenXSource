using System;
using System.Collections.Generic;
using System.Text;

namespace Next2Friends.Data
{
    public partial class Category
    {
        public Category(int CategoryID, string Name)
        {
            this.CategoryID = CategoryID;
            this.Name = Name;
        }


        public static string[] GetTagsFromCategory(int CategoryID)
        {
            //2	Comedy
            //3	Education
            //4	Entertainment
            //5	Film & Animation
            //6	Howto & Style
            //7	News & Politics
            //8	Nonprofits & Activism
            //9	People & Blogs
            //10	Pets & Animals
            //11	Science & Technology
            //12	Sports
            //13	Travel & Events
            //14 Autos & Vehicles
            //15 Music

            string[] Tags = new string[0];

            switch (CategoryID)
            {
                case 2: // Comedy
                    Tags = new string[] { "Animation", "Blooper", "Improv", "Parody", "Pranks", "Series", "Short Film", "Sketch", "Spoof", "Stand-up", "Video Blog" };
                    break;
                case 3://Education
                    Tags = new string[] { "Athletics",  "Business",  "Communications",  "Computer Science",  "Economics",  "Engineering",  "Health",  "Humanities",  "Language",  "Math",  "Media",  "Medicine",  "Performing Arts",  "Physical Science",  "Social Science",  "Visual Arts"};
                    break;
                case 4:// Entertainment
                    Tags = new string[] { "Advertising", "Commercials", "Entertainment News", "Performing Arts", "Short Film", "Trailer", "TV", "Video Game", "Web Series" };
                    break;
                case 5://File and Animation
                    Tags = new string[] { "Commentary & Analysis", "Documentary", "Gotcha!", "Grassroots Outreach", "News", "Political Commercial" };
                    break;
                case 6://Howto & Style
                    Tags = new string[] { "Advice", "Community", "Dating", "Personals", "Random", "Video Blog", "Wisdom" };
                    break;
                case 7://News & Politics
                    Tags = new string[] {"Commentary & Analysis",  "Documentary",  "Gotcha!",  "Grassroots Outreach",  "News",  "Political Commercial" };
                    break;
                case 8://Nonprofits & Activism
                    Tags = new string[] {"Federal Government",  "Grassroots Outreach",  "Local Government",  "Nonprofit",  "Public Service Announcements",  "Regional Government", "State Government" };
                    break;
                case 9:
                    Tags = new string[] { "Aviation & Space", "Computer", "DIY", "Electronics", "Environment", "Gadget", "Mechanics", "Medicine", "Video Game" };
                    break;
                case 10://Pets & Animals
                    Tags = new string[] { "quatic", "Bird", "Cat", "Dog", "Hamster", "Insect", "Rabbit", "Reptile", "Wildlife" };
                    break;
                case 11://Science & Technology
                    Tags = new string[] { "Adventure", "Cityscape", "Cruise", "Culture", "Destination", "Events", "Landmark", "Museum", "Nature", "Travel Log" };
                    break;
                case 12:// Sports
                    Tags = new string[] {  "Action", "American Football", "Baseball", "Basketball", "Combat Sports", "Extreme", "Golf", "Hockey", "Martial Arts", "Motor Sport", "Soccer", "Sports Talk", "Tennis", "Track & Field", "Water Sport", "Winter Sports" };
                    break;
                case 13://Travel & Events
                    Tags = new string[] { "Adventure", "Cityscape", "Cruise", "Culture", "Destination", "Events", "Landmark", "Museum", "Nature", "Travel Log" };
                    break;
                case 14://Autos & Vehicles
                    Tags = new string[] { "Airplane", "Auto", "Boat", "Motorcycle", "Motor Sport", "Train" };
                    break;
                case 15: // Music
                    Tags = new string[] { "Alternative", "Blues", "Classical", "Country", "Electronic", "Folk", "Hip-Hop", "Indie", "Jazz", "Pop", "R&B", "Rap", "Religious", "Rock", "Soul", "Unsigned", "World Music" };
                    break;
  

            }

            return Tags;

        }
    }
}