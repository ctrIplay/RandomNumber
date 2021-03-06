﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace RandomNumber
{
    public partial class Form1 : Form
    {
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void ApplySettings()
        {
            BackColor = Settings.BackgroundColor;
            labelNumber.ForeColor = Settings.TextColor;

            if (Settings.Interval > -1)
            {
                timer1.Interval = Settings.Interval;
                timer1.Enabled = true;
            }
            else
                timer1.Enabled = false;
        }

        private void UpdateNumber()
        {
            string nextNumber = random.Next(0, 1001).ToString();
            int len = nextNumber.Length;

            nextNumber = nextNumber.Insert(len - 1, ".");
            if (len == 1) nextNumber = '0' + nextNumber;

            labelNumber.Text = nextNumber;
        }

        private void SetSelectedIntervalItem()
        {
            updateNumberOnClickToolStripMenuItem.Checked = Settings.UpdateNumberOnClick;

            foreach (ToolStripMenuItem menuItem in setUpdateIntervalToolStripMenuItem.DropDownItems)
                menuItem.Checked = Convert.ToInt32(menuItem.Tag) == Settings.Interval;
        }

        private Color SelectColor(Color currentColor)
        {
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.AllowFullOpen = true;
                colorDialog.Color = currentColor;

                if (colorDialog.ShowDialog() == DialogResult.OK)
                    currentColor = colorDialog.Color;

                return currentColor;
            }
        }

        #region ContextMenu events

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SetSelectedIntervalItem();
        }

        private void secondToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            int interval = Convert.ToInt32(menuItem.Tag);
            Settings.Interval = interval;

            ApplySettings();
        }

        private void selectBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.BackgroundColor = SelectColor(Settings.BackgroundColor);

            ApplySettings();
        }

        private void selectTextColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.TextColor = SelectColor(Settings.TextColor);

            ApplySettings();
        }

        private void updateNumberOnClickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.UpdateNumberOnClick = !Settings.UpdateNumberOnClick;
        }

        #endregion

        #region Other events

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateNumber();
            ApplySettings();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateNumber();
        }

        private void labelNumber_MouseDown(object sender, EventArgs e)
        {
            if (Settings.UpdateNumberOnClick)
            {
                UpdateNumber();

                if (timer1.Enabled)
                {
                    // Restart timer to reset interval
                    timer1.Stop();
                    timer1.Start();
                }
            }
        }

        #endregion
    }
}
