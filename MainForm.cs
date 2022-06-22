using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VehicleRandomizer
{
    public partial class Main : Form
    {

        public const String CURR_VERSION = "Danger Zone";

        DatabaseManager dbm;
        WTVehicle currentVehicle;
        public Main()
        {
            InitializeComponent();
            lblVersion.Text = CURR_VERSION;
            dbm = new DatabaseManager();
        }

        private void Randomize_Click(object sender, EventArgs e)
        {
            // determine filters
            List<bool> nationFilters = new List<bool>();
            List<bool> typeFilters = new List<bool>();

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

            

            // nation filters
            nationFilters.Add(cbUSA.Checked);
            nationFilters.Add(cbGermany.Checked);
            nationFilters.Add(cbUSSR.Checked);
            nationFilters.Add(cbGreatBritain.Checked);
            nationFilters.Add(cbJapan.Checked);
            nationFilters.Add(cbChina.Checked);
            nationFilters.Add(cbItaly.Checked);
            nationFilters.Add(cbFrance.Checked);
            nationFilters.Add(cbSweden.Checked);
            nationFilters.Add(cbIsrael.Checked);

            // detect if any nation filters were selected. if none were selected, treat like all were selected.

            // OR all of the nation filters together, if false, no nation filters selected, dispaly all nations
            bool anyNationSelected = false;
            foreach (bool b in nationFilters)
            {
                anyNationSelected = anyNationSelected || b;
            }
            if (!anyNationSelected)
            {
                for (int i = 0; i < nationFilters.Count; i++)
                {
                    nationFilters[i] = true;
                }
            }

            /* 
             * 0 Army
             * 1 Helis
             * 2 Aviation
             * 3 Bluewater
             * 4 Coastal
             */

            // vehicle type filters
            typeFilters.Add(cbArmy.Checked);
            typeFilters.Add(cbHelicopters.Checked);
            typeFilters.Add(cbAviation.Checked);
            typeFilters.Add(cbBluewater.Checked);
            typeFilters.Add(cbCoastal.Checked);

            // detect if any types were selected. If none were selected, treat like all were selected.
            // same as above, OR all together, if result false, none selected.
            bool anyTypeSelected = false;
            foreach (bool b in typeFilters)
            {
                anyTypeSelected = anyTypeSelected || b;
            }
            if (!anyTypeSelected)
            {
                for (int i = 0; i < typeFilters.Count; i++)
                {
                    typeFilters[i] = true;
                }
            }

            // pick random
            currentVehicle = dbm.pickRandom(nationFilters, typeFilters);

            // empty vehicleList test
            // if not empty list proceed as normal
            if (currentVehicle != null)
            {
                // set resulting text
                rtbNation.Text = currentVehicle.getNation();
                rtbName.Text = currentVehicle.getName();
                rtbBR.Text = currentVehicle.getBR();
            }
            // If vehicle list empty, then no vehicles fit current filter, display to user
            else
            {
                rtbNation.Text = "Vehicle Nation";
                rtbName.Text = "No vehicles fit current filter";
                rtbBR.Text = "Vehicle BR";
            }
            
        }
    }
}
