using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_Wash
{
    public partial class Form1 : Form
    {
        private BackgroundWorker worker;
        private delegate void DELEGATE();

        private Timer timer;
        private Stopwatch sw;

        private int currentWashProcess = 0; // Ranges from [0-8]
        private int lastWashProcess = 0;
        private int lastProcess = 0;

        private int sliderValue = 0; // Ranges from [0-4]

        private List<bool> controlList;

        private bool processFinished = false;
        private bool processRunning = false;
        private bool washingInterruptedLED = false;
        private bool appStopped = false;


        public Form1()
        {
            InitializeComponent();
            createList();
            createTimer();
            loadImages();

            worker = new BackgroundWorker();
        }

        #region Util
        // Creates Controller List
        public void createList()
        {
            controlList = new List<bool>();
            for (int i = 0; i < 8; i++)
            {
                controlList.Add(false);
            }
        }

        public void createTimer()
        {
            timer = new Timer();
            timer.Interval = (50); // Milsec
            timer.Tick += new EventHandler(timer_tick);

            sw = new Stopwatch();
        }

        public void loadImages()
        {
            pictureBox1.Image = Properties.Resources.OFF;
            pictureBox1.Refresh();
            pictureBox2.Image = Properties.Resources.OFF;
            pictureBox2.Refresh();
            pictureBox3.Image = Properties.Resources.OFF;
            pictureBox3.Refresh();
            pictureBox4.Image = Properties.Resources.OFF;
            pictureBox4.Refresh();
            pictureBox5.Image = Properties.Resources.OFF;
            pictureBox5.Refresh();
            pictureBox6.Image = Properties.Resources.OFF;
            pictureBox6.Refresh();
            pictureBox7.Image = Properties.Resources.OFF;
            pictureBox7.Refresh();
            pictureBox8.Image = Properties.Resources.OFF;
            pictureBox8.Refresh();
            pictureBox9.Image = Properties.Resources.OFF;
            pictureBox9.Refresh();
        }

        // Sets Vacancy Status
        public void setVacantStatus(bool vacancy)
        {
            if(vacancy)
            {
                btn_vacant.BackColor = Color.Lime;
                btn_vacant.Text = "Wash Vacant";
            }
            else
            {
                btn_vacant.BackColor = Color.Red;
                btn_vacant.Text = "Wash In Progress";
            }
        }

        // Checks if slider is in the correct position
        public bool isCarPositionCorrect()
        {
            if (sliderValue == 1)
            {
                if (currentWashProcess == 0 || currentWashProcess == 1)
                    return true;
                else
                    return false;
            }
            else if (sliderValue == 2)
            {
                if (currentWashProcess == 2 || currentWashProcess == 3 || currentWashProcess == 4 || currentWashProcess == 5)
                    return true;
                else
                    return false;
            }
            else if (sliderValue == 3)
            {
                if (currentWashProcess == 6 || currentWashProcess == 7)
                    return true;
                else
                    return false;
            }
            else // Defensive else, all options covered above
                return false;
        }

        public void disableWashOptions()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
        }

        public void enableWashOptions()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
        }

        public void resetButtonsIndicators()
        {
            // Reset button click status
            button1.BackColor = SystemColors.ActiveBorder;
            button2.BackColor = SystemColors.ActiveBorder;
            button3.BackColor = SystemColors.ActiveBorder;
            button4.BackColor = SystemColors.ActiveBorder;
            button5.BackColor = SystemColors.ActiveBorder;
            button6.BackColor = SystemColors.ActiveBorder;
            button7.BackColor = SystemColors.ActiveBorder;
            button8.BackColor = SystemColors.ActiveBorder;

            for (int i = 0; i < 8; i++)
            {
                controlList[i] = false;
            }

            // Turn of all indicators
            changeIndicatorStatus(1, false);
            changeIndicatorStatus(2, false);
            changeIndicatorStatus(3, false);
            changeIndicatorStatus(4, false);
            changeIndicatorStatus(5, false);
            changeIndicatorStatus(6, false);
            changeIndicatorStatus(7, false);
            changeIndicatorStatus(8, false);
            changeIndicatorStatus(9, false);
        }

        // ON OFF Indicator LEDs
        public void changeIndicatorStatus(int indicator,bool power)
        {
            switch (indicator)
            {
                case 1:
                    if(power)
                    {
                        pictureBox1.Image = Properties.Resources.ON;
                        pictureBox1.Refresh();
                    }
                    else
                    {
                        pictureBox1.Image = Properties.Resources.OFF;
                        pictureBox1.Refresh();
                    }
                    break;
                case 2:
                    if (power)
                    {
                        pictureBox2.Image = Properties.Resources.ON;
                        pictureBox2.Refresh();
                    }
                    else
                    {
                        pictureBox2.Image = Properties.Resources.OFF;
                        pictureBox2.Refresh();
                    }
                    break;
                case 3:
                    if (power)
                    {
                        pictureBox3.Image = Properties.Resources.ON;
                        pictureBox3.Refresh();
                    }
                    else
                    {
                        pictureBox3.Image = Properties.Resources.OFF;
                        pictureBox3.Refresh();
                    }
                    break;
                case 4:
                    if (power)
                    {
                        pictureBox4.Image = Properties.Resources.ON;
                        pictureBox4.Refresh();
                    }
                    else
                    {
                        pictureBox4.Image = Properties.Resources.OFF;
                        pictureBox4.Refresh();
                    }
                    break;
                case 5:
                    if (power)
                    {
                        pictureBox5.Image = Properties.Resources.ON;
                        pictureBox5.Refresh();
                    }
                    else
                    {
                        pictureBox5.Image = Properties.Resources.OFF;
                        pictureBox5.Refresh();
                    }
                    break;
                case 6:
                    if (power)
                    {
                        pictureBox6.Image = Properties.Resources.ON;
                        pictureBox6.Refresh();
                    }
                    else
                    {
                        pictureBox6.Image = Properties.Resources.OFF;
                        pictureBox6.Refresh();
                    }
                    break;
                case 7:
                    if (power)
                    {
                        pictureBox7.Image = Properties.Resources.ON;
                        pictureBox7.Refresh();
                    }
                    else
                    {
                        pictureBox7.Image = Properties.Resources.OFF;
                        pictureBox7.Refresh();
                    }
                    break;
                case 8:
                    if (power)
                    {
                        pictureBox8.Image = Properties.Resources.ON;
                        pictureBox8.Refresh();
                    }
                    else
                    {
                        pictureBox8.Image = Properties.Resources.OFF;
                        pictureBox8.Refresh();
                    }
                    break;
                case 9:
                    if (power)
                    {
                        pictureBox9.Image = Properties.Resources.ON;
                        pictureBox9.Refresh();
                    }
                    else
                    {
                        pictureBox9.Image = Properties.Resources.OFF;
                        pictureBox9.Refresh();
                    }
                    break;
                
                default:
                    // do nothing
                    break;
            }
        }

        // Resets default
        private void terminateApp()
        {
            // Set vacancy to true
            setVacantStatus(true);

            // Enable all wash options
            enableWashOptions();

            // Reset button click status & indicators
            resetButtonsIndicators();

            // Reset car slider
            trackBar_slider.Value = 0;
            sliderValue = 0;

            lbl_elapsed.Text = "0.000";

            timer.Stop();
            sw.Stop();
            sw.Reset();

            appStopped = true;

            currentWashProcess = 0;

            lastWashProcess = 0;
            lastProcess = 0;

            processFinished = false;
            processRunning = false;
            washingInterruptedLED = false;

            btn_start.Enabled = true;

        }
        #endregion


        #region Invokers
        private void runProcess()
        {
            changeIndicatorStatus(currentWashProcess + 1, true);
            timer.Start();
            sw.Start();
        }

        private void stopProcess()
        {
            changeIndicatorStatus(currentWashProcess + 1, false);
            timer.Stop();
            sw.Stop();

        }

        

        private void completeWashing()
        {
            // Vehicle out of position LED
            changeIndicatorStatus(9, true);

            timer.Stop();
            sw.Stop(); sw.Reset();

            lbl_elapsed.Text = "0.000";

            appStopped = true;
        }

        private void washingInterrupted()
        {
            if (!appStopped)
            {
                if (washingInterruptedLED)
                {
                    changeIndicatorStatus(9, washingInterruptedLED);
                    changeIndicatorStatus(lastProcess + 1, false);
                }
                else
                {
                    changeIndicatorStatus(9, washingInterruptedLED);
                    changeIndicatorStatus(lastProcess + 1, true);
                }
            }
        }
        #endregion


        public void Controller(object sender, DoWorkEventArgs e)
        {
            Delegate run = new DELEGATE(runProcess);
            Delegate stop = new DELEGATE(stopProcess);
            Delegate lastWash = new DELEGATE(completeWashing);
            Delegate interrupt = new DELEGATE(washingInterrupted);

            while (!appStopped)
            {
                if(!processRunning)
                {
                    this.Invoke(run);
                    processRunning = true;
                    processFinished = false;
                    lastProcess = currentWashProcess;
                }
                    
                // Process Completed, move to next
                if(processFinished)
                {
                    this.Invoke(stop);

                    // Last Wash Process
                    if (currentWashProcess == lastWashProcess)
                    {
                        this.Invoke(lastWash);
                    }

                    controlList[currentWashProcess] = false;

                    for (int i = currentWashProcess; i < 8; i++)
                    {
                        if (controlList[i])
                        {
                            currentWashProcess = i;
                            break;
                        }
                            
                    }

                    processRunning = false;
                }


                bool flag = true;
                // Washing interrupted by slider
                while (!isCarPositionCorrect() && flag)
                {
                    sw.Stop();

                    if (flag)
                    {
                        washingInterruptedLED = true;
                        this.Invoke(interrupt);
                        sw.Stop();
                        flag = false;
                    }

                    if (isCarPositionCorrect())
                    {
                        washingInterruptedLED = false;
                        this.Invoke(interrupt);
                        sw.Start();
                        flag = true;
                        break;
                    }
                }
            }
            
        }

        private void timer_tick(object sender, EventArgs e)
        {
            lbl_elapsed.Text = string.Format("{0:D1}:{1:D1}",sw.Elapsed.Seconds,sw.Elapsed.Milliseconds);

            if(sw.Elapsed.Seconds == 5)
            {
                lbl_elapsed.Text = "0.000";
                processFinished = true;
                sw.Reset();
                timer.Stop();
            }
        }

        private void trackBar_slider_ValueChanged(object sender, EventArgs e)
        {
            sliderValue = trackBar_slider.Value;

            if (sliderValue == 4)
            {
                terminateApp();
                btn_start.Enabled = true;
            }
        }

        // Start & Stop Buttons
        #region Start / Stop
        private void btn_start_Click(object sender, EventArgs e)
        {
            // Set Default
            appStopped = false;
            changeIndicatorStatus(9, false);

            // HPW always included
            controlList[3] = true;

            // Find first wash process
            for (int i = 0; i < 8; i++)
            {
                if (controlList[i])
                {
                    currentWashProcess = i;
                    break;
                }
            }

            // Find last wash process
            for (int i = 0; i < 8; i++)
            {
                if (controlList[i])
                {
                    lastWashProcess = i;
                }
            }

            btn_start.Enabled = false;

            disableWashOptions();
            setVacantStatus(false);

            worker.DoWork += Controller;
            worker.RunWorkerAsync();
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            terminateApp();
        }
        #endregion
        
        // Wash Option Buttons
        #region Wash Options
        private void button1_Click(object sender, EventArgs e)
        {
            if (controlList[0])
            {
                controlList[0] = false;
                button1.BackColor = SystemColors.ActiveBorder;
            }
            else
            {
                controlList[0] = true;
                button1.BackColor = SystemColors.ControlDarkDark;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (controlList[1])
            {
                controlList[1] = false;
                button2.BackColor = SystemColors.ActiveBorder;
            }
            else
            {
                controlList[1] = true;
                button2.BackColor = SystemColors.ControlDarkDark;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (controlList[2])
            {
                controlList[2] = false;
                button3.BackColor = SystemColors.ActiveBorder;
            }
            else
            {
                controlList[2] = true;
                button3.BackColor = SystemColors.ControlDarkDark;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (controlList[3])
            {
                controlList[3] = false;
                button4.BackColor = SystemColors.ActiveBorder;
            }
            else
            {
                controlList[3] = true;
                button4.BackColor = SystemColors.ControlDarkDark;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (controlList[4])
            {
                controlList[4] = false;
                button5.BackColor = SystemColors.ActiveBorder;
            }
            else
            {
                controlList[4] = true;
                button5.BackColor = SystemColors.ControlDarkDark;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (controlList[5])
            {
                controlList[5] = false;
                button6.BackColor = SystemColors.ActiveBorder;
            }
            else
            {
                controlList[5] = true;
                button6.BackColor = SystemColors.ControlDarkDark;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (controlList[6])
            {
                controlList[6] = false;
                button7.BackColor = SystemColors.ActiveBorder;
            }
            else
            {
                controlList[6] = true;
                button7.BackColor = SystemColors.ControlDarkDark;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (controlList[7])
            {
                controlList[7] = false;
                button8.BackColor = SystemColors.ActiveBorder;
            }
            else
            {
                controlList[7] = true;
                button8.BackColor = SystemColors.ControlDarkDark;
            }
        }
        #endregion

        // Button Painters
        #region Painter
        private void btn_Stop_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, btn_Stop.ClientRectangle,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset);
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, button1.ClientRectangle,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset);
        }

        private void button2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, button2.ClientRectangle,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset);
        }

        private void button3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, button3.ClientRectangle,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset);
        }

        private void button4_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, button4.ClientRectangle,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset);
        }

        private void button5_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, button5.ClientRectangle,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset);
        }

        private void button6_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, button6.ClientRectangle,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset);
        }

        private void button7_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, button7.ClientRectangle,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset);
        }

        private void button8_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, button8.ClientRectangle,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset);
        }

        private void btn_start_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, btn_start.ClientRectangle,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
            SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset);
        }
        #endregion Painter
    }
}
