using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace VehicleRandomizer
{
    class DatabaseManager2
    {

        private StreamReader reader;
        private List<WTVehicle> vehicleList;
        Random rand = new Random();
        bool dataLoaded { get; set; } = false;

        private static readonly HttpClient client = new HttpClient();
        public DatabaseManager2()
        {
            vehicleList = new List<WTVehicle>();                                 
            ReadFromFile();
        }

        private static string getHTML(string fullURL)
        {            
            var response = client.GetAsync(fullURL).Result;
            string html = "NULL";
            if (response.IsSuccessStatusCode)
            {
                var dataObjects = response.Content.ReadAsStringAsync().Result;
                html = dataObjects;
            }
            return html;
        }

        private void loadData(string treeURL, int nation, int vehicleType)
        {            
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(getHTML(treeURL));

            // LINQ
            var TDs = htmlDoc.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", "").Contains("tree-item-background")).ToList();
            foreach(var tank in TDs)
            {
                var link = tank.FirstChild;
                string pageURL = "https://wiki.warthunder.com" + link.Attributes[0].Value;
                string name = link.Attributes[1].Value;
                Debug.WriteLine(name + " " + pageURL);
                var vehicle = new WTVehicle(name, pageURL);
                vehicle.Nation = nation;
                vehicle.VehicleType = vehicleType;
                vehicleList.Add(vehicle);
            }

            // load each vehicles page, and scrape their current BRs
            
            Debug.WriteLine("DONE LOAD_DATA");
        }

        private bool loadIndividual(WTVehicle wtv)
        {
            var htmlPage = new HtmlDocument();
            htmlPage.LoadHtml(getHTML(wtv.wikiLink));
            var BR = htmlPage.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", "").Contains("general_info_br")).ToList();
            var text = BR[0].InnerText.Trim().Split('\n');
            wtv.BRArcade = text[6];
            wtv.BRRealistic = text[7];
            wtv.BRSimulator = text[8];
            return true;
        }

        public WTVehicle pickRandom(List<bool> nationFilters, List<bool> typeFilters)
        {
            if (vehicleList.Count == 0)
            {
                WTVehicle noData = new WTVehicle(0, "No Vehicle Data", "File > Update", "File > Update", "File > Update", "File > Update", 0);
                return noData;
            }

            HashSet<WTVehicle> filteredSet = new HashSet<WTVehicle>();

            #region filterReferences
            /*
             * 0 USA
             * 1 Germany
             * 2 USSR
             * 3 Great Britain
             * 4 Japan
             * 5 China
             * 6 Italy
             * 7 France
             * 8 Sweden
             * 9 Israel
             */


            /* 
             * 0 Army
             * 1 Helis
             * 2 Aviation
             * 3 Bluewater
             * 4 Coastal
             */
            #endregion

            // Copy list into set
            foreach(var vehicle in vehicleList)
            {
                filteredSet.Add(vehicle);
            }

            // filter out vehicles from set
            foreach(var vehicle2 in filteredSet)
            {
                #region NationFilters
                // USA
                if (!nationFilters[0])
                {
                    if (vehicle2.Nation == 0) { filteredSet.Remove(vehicle2); }
                }
                // Germany
                if (!nationFilters[1])
                {
                    if (vehicle2.Nation == 1) { filteredSet.Remove(vehicle2); }
                }
                // USSR
                if (!nationFilters[2])
                {
                    if (vehicle2.Nation == 2) { filteredSet.Remove(vehicle2); }
                }
                // Great Britain
                if (!nationFilters[3])
                {
                    if (vehicle2.Nation == 3) { filteredSet.Remove(vehicle2); }
                }
                // Japan
                if (!nationFilters[4])
                {
                    if (vehicle2.Nation == 4) { filteredSet.Remove(vehicle2); }
                }
                // China
                if (!nationFilters[5])
                {
                    if (vehicle2.Nation == 5) { filteredSet.Remove(vehicle2); }
                }
                // Italy
                if (!nationFilters[6])
                {
                    if (vehicle2.Nation == 6) { filteredSet.Remove(vehicle2); }
                }
                // France
                if (!nationFilters[7])
                {
                    if (vehicle2.Nation == 7) { filteredSet.Remove(vehicle2); }
                }
                // Sweden
                if (!nationFilters[8])
                {
                    if (vehicle2.Nation == 8) { filteredSet.Remove(vehicle2); }
                }
                // Israel
                if (!nationFilters[9])
                {
                    if (vehicle2.Nation == 9) { filteredSet.Remove(vehicle2); }
                }
                #endregion
                #region VehicleTypeFilters
                // Army
                if (!typeFilters[0])
                {
                    if (vehicle2.VehicleType == 0) { filteredSet.Remove(vehicle2); }
                }
                // Helis
                if (!typeFilters[1])
                {
                    if (vehicle2.VehicleType == 1) { filteredSet.Remove(vehicle2); }
                }
                // Aviation
                if (!typeFilters[2])
                {
                    if (vehicle2.VehicleType == 2) { filteredSet.Remove(vehicle2); }
                }
                // Bluewater
                if (!typeFilters[3])
                {
                    if (vehicle2.VehicleType == 3) { filteredSet.Remove(vehicle2); }
                }
                // Coastal
                if (!typeFilters[4])
                {
                    if (vehicle2.VehicleType == 4) { filteredSet.Remove(vehicle2); }
                }
                #endregion
            }

            // pick a random vehicle from filtered set
            List<WTVehicle> fitlist = filteredSet.ToList<WTVehicle>();
            WTVehicle chosen = fitlist[rand.Next(fitlist.Count)];     
            return chosen;
        }

        public void saveToFile()
        {
            using (StreamWriter file = File.CreateText("VehicleList.wtdb"))
            {
                foreach(var v in vehicleList)
                {
                    file.WriteLine(v.ToString());
                }
            }
        }

        public void ReadFromFile()
        {
           
            try
            {
                using (reader = new StreamReader("VehicleList.wtdb"))
                {

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split('*');
                        vehicleList.Add(new WTVehicle(int.Parse(parts[0]), parts[1], parts[2], parts[3], parts[4], parts[5], int.Parse(parts[6])));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dataLoaded = false;
                return;
                // updateVehicleList();
            }  
        }

        public void updateVehicleList()
        {
            vehicleList.Clear();
            // URL List
            List<Tuple<string, int, int>> URLs = new List<Tuple<string, int, int>>();
            try
            {
                reader = new StreamReader("TreeURLs.wtdb");

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    URLs.Add(new Tuple<string, int, int>(parts[0], int.Parse(parts[1]), int.Parse(parts[2])));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(-1);
            }
            finally
            {
                reader.Close();
            }

            foreach (var url in URLs)
            {
                loadData(url.Item1, url.Item2, url.Item3);
            }
            Parallel.ForEach(vehicleList, v =>
            {
                loadIndividual(v);
            });
            saveToFile();
        }
    }
}
