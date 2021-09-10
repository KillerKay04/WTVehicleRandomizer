using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace VehicleRandomizer
{
    /// <summary>
    /// This class manages the data handling of the app
    /// </summary>
    class DatabaseManager
    {

        private StreamReader reader;
        private List<WTVehicle> vehicleList;

        private Random rand;

        // constructor
        public DatabaseManager()
        {
                    
            vehicleList = new List<WTVehicle>();
            rand = new Random();            
        }

        // read file into vector, file is specified by passed filename
        private void loadVector(string filename)
        {
            try
            {
                reader = new StreamReader(filename);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Add support for comments in wtdb files in the form of // before single line comment
                    if(line.Substring(0,2).Equals("//"))
                    {
                        // If line is a comment, do nothing with this line, read next line.
                    }
                    else
                    {
                        string[] items = line.Split(',');
                        vehicleList.Add(new WTVehicle(items[0], items[1], items[2]));
                    }                    
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
        }

        private void unloadVector()
        {
            vehicleList.Clear();
        }

        // pick random upon request with filters
        public WTVehicle pickRandom(List<bool> nationFilters, List<bool> typeFilters)
        {

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
             */


            /* 
             * 0 Army
             * 1 Helis
             * 2 Aviation
             * 3 Bluewater
             * 4 Coastal
             */
            #endregion

            // load vector according to filters

            // USA
            if (nationFilters[0])
            {
                // Army
                if (typeFilters[0]) { loadVector("VehicleData/USA/USAArmy.wtdb"); }
                // helis
                if (typeFilters[1]) { loadVector("VehicleData/USA/USAHelicopters.wtdb"); }
                // aviation
                if (typeFilters[2]) { loadVector("VehicleData/USA/USAAviation.wtdb"); }
                // bluewater
                if (typeFilters[3]) { loadVector("VehicleData/USA/USABluewater.wtdb"); }
                // coastal
                if (typeFilters[4]) { loadVector("VehicleData/USA/USACoastal.wtdb"); }
            }
            // Germany
            if (nationFilters[1])
            {
                // Army
                if (typeFilters[0]) { loadVector("VehicleData/Germany/GermanyArmy.wtdb"); }
                // helis                                      
                if (typeFilters[1]) { loadVector("VehicleData/Germany/GermanyHelicopters.wtdb"); }
                // aviation                                  
                if (typeFilters[2]) { loadVector("VehicleData/Germany/GermanyAviation.wtdb"); }
                // bluewater                                  
                if (typeFilters[3]) { loadVector("VehicleData/Germany/GermanyBluewater.wtdb"); }
                // coastal                                    
                if (typeFilters[4]) { loadVector("VehicleData/Germany/GermanyCoastal.wtdb"); }
            }
            // USSR
            if (nationFilters[2])
            {
                // Army
                if (typeFilters[0]) { loadVector("VehicleData/USSR/USSRArmy.wtdb"); }
                // helis
                if (typeFilters[1]) { loadVector("VehicleData/USSR/USSRHelicopters.wtdb"); }
                // aviation 
                if (typeFilters[2]) { loadVector("VehicleData/USSR/USSRAviation.wtdb"); }
                // bluewater
                if (typeFilters[3]) { loadVector("VehicleData/USSR/USSRBluewater.wtdb"); }
                // coastal
                if (typeFilters[4]) { loadVector("VehicleData/USSR/USSRCoastal.wtdb"); }
            }
            // Great Britain
            if (nationFilters[3])
            {
                // Army
                if (typeFilters[0]) { loadVector("VehicleData/GB/GBArmy.wtdb"); }
                // helis
                if (typeFilters[1]) { loadVector("VehicleData/GB/GBHelicopters.wtdb"); }
                // aviation 
                if (typeFilters[2]) { loadVector("VehicleData/GB/GBAviation.wtdb"); }
                // bluewater
                if (typeFilters[3]) { loadVector("VehicleData/GB/GBBluewater.wtdb"); }
                // coastal
                if (typeFilters[4]) { loadVector("VehicleData/GB/GBCoastal.wtdb"); }
            }

            // Sweden
            if (nationFilters[8])
            {
                // Army
                if (typeFilters[0]) { loadVector("VehicleData/Sweden/SwedenArmy.wtdb"); }
                // helis
                if (typeFilters[1]) { /* Do nothing, no swedish helis in game. */ }
                // aviation 
                if (typeFilters[2]) { loadVector("VehicleData/Sweden/SwedenAviation.wtdb"); }
                // bluewater
                if (typeFilters[3]) { /* Do nothing, no swedish ships in game. */ }
                // coastal
                if (typeFilters[4]) { /* Do nothing, no swedish ships in game. */ }
            }

            // TODO Need to handle case for empty vector.
            // will be caused by selecting a nation, and a vehicle type that that nation
            // does not have.
            // Example, Swedish Helicopters, will bring up nothing, and should be handled
            if (vehicleList.Count  > 0)
            {
                // pick randomly from generated vector
                WTVehicle chosen = vehicleList[rand.Next(vehicleList.Count)];
                unloadVector();
                return chosen;
            }
            else
            {
                return null;
            }
            
        }
    }
}
