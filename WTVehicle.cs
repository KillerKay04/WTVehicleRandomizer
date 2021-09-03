namespace VehicleRandomizer
{
    public class WTVehicle
    {
        private string nation;
        private string name;
        private string BR;

        /// <summary>
        /// Constructor, assigns paramaters to instance variables
        /// </summary>
        /// <param name="nation">the nation of the vehicle</param>
        /// <param name="name">the name of the vehicle</param>
        /// <param name="BR">the vehicles current realisitc battles BR</param>
        public WTVehicle(string nation, string name, string BR)
        {
            this.nation = nation;
            this.name = name;
            this.BR = BR;
        }

        public string getNation() { return nation; }
        public string getName() { return name; }
        public string getBR() { return BR; }

        public override string ToString()
        {
            return nation + " " + name + " " + BR;
        }
    }
}