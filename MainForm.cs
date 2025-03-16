using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Net.Http;

namespace VehicleRandomizer
{
    public partial class Main : Form
    {

        public string CURR_VERSION = "File > Update";

        // DatabaseManager dbm;
        DatabaseManager2 dbm;
        WTVehicle currentVehicle;

        private StreamReader reader;
        public Main()
        {
            InitializeComponent();
            ReadFromFile();
            lblVersion.Text = CURR_VERSION;
            dbm = new DatabaseManager2();
            Debug.WriteLine("MAIN");
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
                rtbNation.Text = currentVehicle.nationString();
                rtbName.Text = currentVehicle.Name;
                // determine which BR to display
                if (radioArcade.Checked) { rtbBR.Text = currentVehicle.BRArcade; }
                if (radioRealistic.Checked) { rtbBR.Text = currentVehicle.BRRealistic; }
                if (radioSimulator.Checked) { rtbBR.Text = currentVehicle.BRSimulator; }

            }
            // If vehicle list empty, then no vehicles fit current filter, display to user
            else
            {
                rtbNation.Text = "Vehicle Nation";
                rtbName.Text = "No vehicles fit current filter";
                rtbBR.Text = "Vehicle BR";
            }

        }

        public void ReadFromFile()
        {
            using (reader = new StreamReader("currentVersion.wtdb"))
            {
                try
                {

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        CURR_VERSION = line;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    updateVersion();
                }
            }
        }

        public void updateVersion()
        {
            //var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            //HttpClient client = new HttpClient();
            //var response = client.GetAsync("https://wiki.warthunder.com/Category:Updates").Result;
            //string html = "NULL";
            //if (response.IsSuccessStatusCode) { html = response.Content.ReadAsStringAsync().Result; }
            //htmlDoc.LoadHtml(html);
            //var version = htmlDoc.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", "").Contains("buzz")).ToList();
            //var currentUpdateInnerText = version[0].InnerText;
            //var s = currentUpdateInnerText.Trim().Trim('\n');
            //var updateName = s.Split('\n')[0];
            CURR_VERSION = "War Thunder";
            lblVersion.Text = CURR_VERSION;
            saveToFile();
        }

        public void saveToFile()
        {
            using (StreamWriter file = File.CreateText("currentVersion.wtdb"))
            {
                file.WriteLine(CURR_VERSION);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            updateData();
        }

        private async Task updateData()
        {
            if (MessageBox.Show("Are you sure you want to update? This may take several minutes. A dialog will pop up when the update is complete.", "Update", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                // dbm.updateVehicleList();
                await Task.Run(() => dbm.updateVehicleList());
                updateVersion();

                MessageBox.Show("Update complete!");
            }
        }

        private void radioArcade_CheckedChanged(object sender, EventArgs e)
        {
            if (radioArcade.Checked) { rtbBR.Text = currentVehicle.BRArcade; }
        }

        private void radioRealistic_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRealistic.Checked) { rtbBR.Text = currentVehicle.BRRealistic; }
        }

        private void radioSimulator_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSimulator.Checked) { rtbBR.Text = currentVehicle.BRSimulator; }
        }
    }
}
