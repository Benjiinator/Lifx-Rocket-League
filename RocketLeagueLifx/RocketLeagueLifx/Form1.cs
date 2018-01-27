using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Memory;

namespace RocketLeagueLifx
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Runs Background worker so that ui is responsive.
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }
        // Creates a new Memory reader/writer object using a library called memory.dll which can be found here: https://github.com/erfg12/memory.dll/wiki
        public Mem m = new Mem();
        // Creates a new object which stores a your lifx token and a memory pointer to each goal.
        public LifxSettings lifxSettings = new LifxSettings();
        // Sets each goals value to -2(having it at 0 or - 1 causes problems if there is already 1 or 0 goals on either side).
        public int BlueGoals = -2;
        public int OrangeGoals = -2;
        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                int pID = m.getProcIDFromName("RocketLeague"); // Get proc ID of game

                bool openProc = false; //Is process open?

                if (pID > 0) //Try opening process
                {
                    openProc = m.OpenProcess("RocketLeague");
                    label2.Invoke((MethodInvoker)delegate
                    {
                        label2.Text = pID.ToString();
                    });
                } else
                {
                    label2.Invoke((MethodInvoker)delegate
                    {
                        label2.Text = "#";
                    });
                }

                if (openProc) //Run if Rocket League is Open
                {
                    // Display in the ui of the program that Rocket League is Open.
                    label4.Invoke((MethodInvoker)delegate
                    {
                        label4.Text = "Open";
                        label4.ForeColor = Color.Green;
                    });

                    // Displays in the programs ui the amount of goals that orange currently have.
                    label6.Invoke((MethodInvoker)delegate
                    {
                        label6.Text = m.readInt(lifxSettings.orangeGoalPointer).ToString();
                    });
                    // Displays in the programs ui the amount of goals that blue currently have.
                    label8.Invoke((MethodInvoker)delegate
                    {
                        label8.Text = m.readInt(lifxSettings.blueGoalPointer).ToString();
                    });

                    //If amount of goals on blue team + 1 = memory reading of blue goals run this code (Runs if blue scores).
                    if (BlueGoals + 1 == m.readInt(lifxSettings.blueGoalPointer))
                    {
                        // Sets blue color over 0.3 seconds
                        Lifx("{\"brightness\": 0.80, \"color\": { \"hue\": 209, \"saturation\": 1, \"kelvin\": 2500, \"duration\": \"0.3\" } }");
                        // Sets Brightsness over 0.5 Seconds
                        Lifx("{\"brightness\": \"0.25\"}");
                        // Goes back to a normal color over 10 seconds
                        Lifx("{\"brightness\": 0.60, \"duration\": \"10.0\", \"color\": { \"hue\": 49, \"saturation\": 1, \"kelvin\": 2500 } }");
                    }
                    //If amount of goals on orange team + 1 = memory reading of blue goals run this code (Runs if orange scores).
                    if (OrangeGoals + 1 == m.readInt(lifxSettings.orangeGoalPointer))
                    {
                        // Sets orange color over 0.3 seconds
                        Lifx("{\"brightness\": 0.80, \"color\": { \"hue\": 35, \"saturation\": 1, \"kelvin\": 2500, \"duration\": \"0.3\" } }");
                        // Sets Brightsness over 0.5 Seconds
                        Lifx("{\"brightness\": \"0.25\"}");
                        // Goes back to a normal color over 10 seconds
                        Lifx("{\"brightness\": 0.60, \"duration\": \"10.0\", \"color\": { \"hue\": 49, \"saturation\": 1, \"kelvin\": 2500 } }");
                    }

                    //Set goal variables = memory reading of each goal.
                    BlueGoals = m.readInt(lifxSettings.blueGoalPointer);
                    OrangeGoals = m.readInt(lifxSettings.orangeGoalPointer);

                } else // Runs if Rocket League is Closed.
                {
                    // Display in the ui of the program that Rocket League is Closed.
                    label4.Invoke((MethodInvoker)delegate
                    {
                        label4.Text = "Closed";
                        label4.ForeColor = Color.DarkRed;
                    });
                }
            }
        }
        public void Lifx(string data)
        {
            if(lifxSettings.token != "")
            {
                WebClient client = new WebClient();
                client.Headers.Add("authorization", "Bearer " + lifxSettings.token);
                string url = "https://api.lifx.com/v1/lights/all/state";
                client.UploadData(url, "PUT", Encoding.UTF8.GetBytes(data));
            }
        }
        public class LifxSettings
        {
            // Your Lifx token which can be genereted here: https://cloud.lifx.com/sign_in
            public string token = "";
            // Pointer to each goal in the memory.
            public string blueGoalPointer = "base+0x019CBB24,0xAC,0x294,0x4DC,0x0,0x20C";
            public string orangeGoalPointer = "base+0x019CBB24,0xAC,0x294,0x4DC,0x4,0x20C";

        }

    }
}
