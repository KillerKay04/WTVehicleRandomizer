namespace VehicleRandomizer
{
    public class WTVehicle
    {

        public string nationString()
        {
            switch (this.Nation)
            {
                case 0:
                    return "USA";
                case 1:
                    return "Germany";
                case 2:
                    return "USSR";
                case 3:
                    return "Great Britain";
                case 4:
                    return "Japan";
                case 5:
                    return "China";
                case 6:
                    return "Italy";
                case 7:
                    return "France";
                case 8:
                    return "Sweden";
                case 9:
                    return "Israel";
                default:
                    return "Nation";
            }
        }

        public string vehicleTypeString()
        {
            switch (this.VehicleType)
            {
                case 0:
                    return "Army";
                case 1:
                    return "Helicopter";
                case 2:
                    return "Aviation";
                case 3:
                    return "Bluewater";
                case 4:
                    return "Coastal";
                default:
                    return "Vehicle Type";
            }
        }

        /// <summary>
        /// Constructor, assigns paramaters to instance variables
        /// </summary>
        /// <param name="nation">the nation of the vehicle</param>
        /// <param name="name">the name of the vehicle</param>
        /// <param name="BR">the vehicles current realisitc battles BR</param>
        public WTVehicle(string name, string wikiLink)
        {
            Name = name;
            this.wikiLink = wikiLink;
        }
        public WTVehicle(int nation, string name, string BRArcade, string BRRealistic, string BRSimulator, string wikiLink, int vehicleType)
        {
            this.Nation = nation;
            this.Name = name;
            this.BRArcade = BRArcade;
            this.BRRealistic = BRRealistic;
            this.BRSimulator = BRSimulator;
            this.wikiLink = wikiLink;
            this.VehicleType = vehicleType;
        }
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
        public int Nation { get; set; } = -1;
        public string Name { get; set; } = "null";
        public string BRArcade { get; set; } = "null";
        public string BRRealistic { get; set; } = "null";
        public string BRSimulator { get; set; } = "null";
        public string wikiLink { get; set; } = "null";
        /* 
             * 0 Army
             * 1 Helis
             * 2 Aviation
             * 3 Bluewater
             * 4 Coastal
             */
        public int VehicleType { get; set; } = -1;

        public override string ToString()
        {
            string output = "";
            output += Nation;
            output += "*" + Name + "*" + BRArcade + "*" + BRRealistic + "*" + BRSimulator + "*" + wikiLink + "*";
            output += VehicleType;            
            return output;
        }
    }
}