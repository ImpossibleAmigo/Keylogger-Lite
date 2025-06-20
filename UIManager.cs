using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KeyloggerLite
{
    public class UIManager
    {
        private readonly MainForm form;
        private readonly KeyLogger keyLogger;
        private readonly DataGridViewConfigurator gridConfigurator;
        private readonly FileSaver fileSaver;

        private Label? lblWorkingTimeLabel, lblWorkingTime, lblTotal;
        private RoundedButton? btnStartFinish, btnInfo, btnClear, btnSaveAs, btnBack;
        private Panel? mainPanel, infoPanel;

        public UIManager(MainForm form, KeyLogger keyLogger)
        {
            this.form = form ?? throw new ArgumentNullException(nameof(form));
            this.keyLogger = keyLogger ?? throw new ArgumentNullException(nameof(keyLogger));
            this.gridConfigurator = new DataGridViewConfigurator();
            this.fileSaver = new FileSaver(keyLogger);

            keyLogger.TimeUpdated += time => lblWorkingTime!.Text = time;
            keyLogger.TotalKeyPressesUpdated += count => lblTotal!.Text = $"Total: {count}";
            keyLogger.KeysUpdated += keyCounts => gridConfigurator.UpdateGrid(keyCounts);
        }

        public void InitializeUI()
        {
            form.Size = new Size(480, 550);
            mainPanel = new Panel { Size = form.Size, Location = Point.Empty, BackColor = ColorTranslator.FromHtml("#CCCCCC") };
            infoPanel = new Panel { Size = form.Size, Location = Point.Empty, BackColor = ColorTranslator.FromHtml("#CCCCCC"), Visible = false };
            form.Controls.Add(mainPanel);
            form.Controls.Add(infoPanel);

            lblWorkingTimeLabel = new Label { Text = "Working time:", Size = new Size(130, 25), Location = new Point(140, 47), Font = new Font("Segoe UI", 12F) };
            lblWorkingTime = new Label { Text = "0D; 0H; 0M; 0S", Size = new Size(200, 40), Location = new Point(140, 72), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White, TextAlign = ContentAlignment.MiddleCenter };
            lblTotal = new Label { Text = "Total: 0", Size = new Size(70, 20), Location = new Point(206, 118), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White, Font = new Font("Segoe UI", 9F) };

            btnStartFinish = CreateButton("Start/Finish", new Point(80, 150), BtnStartFinish_Click);
            btnInfo = CreateButton("Info...", new Point(263, 150), BtnInfo_Click);
            btnClear = CreateButton("Clear", new Point(80, 260), BtnClear_Click);
            btnSaveAs = CreateButton("Save as...", new Point(263, 260), BtnSaveAs_Click);
            btnBack = CreateButton("Back", new Point(400, 70), BtnBack_Click, new Size(60, 30), 1);

            mainPanel.Controls.AddRange(new Control[] { lblWorkingTimeLabel, lblWorkingTime, btnStartFinish, btnInfo, btnClear, btnSaveAs });
            infoPanel.Controls.Add(btnBack);

            gridConfigurator.ConfigureDataGridView(infoPanel);
        }

        private RoundedButton CreateButton(string text, Point loc, EventHandler click, Size? size = null, int borderSize = 3)
        {
            var btn = new RoundedButton
            {
                Text = text,
                Size = size ?? new Size(125, 70),
                Location = loc,
                BackColor = Color.White,
                BorderRadius = 10,
                BorderColor = Color.Black,
                BorderSize = borderSize,
                Font = new Font("Segoe UI", 13F)
            };
            btn.Click += click;
            btn.MouseDown += (s, e) => btn.BackColor = ColorTranslator.FromHtml("#BCBCBC");
            btn.MouseUp += (s, e) => btn.BackColor = Color.White;
            return btn;
        }

        private void BtnStartFinish_Click(object? s, EventArgs e)
        {
            if (!keyLogger.IsRunning) keyLogger.Start(); else keyLogger.Stop();
        }

        private void BtnInfo_Click(object? s, EventArgs e)
        {
            if (mainPanel != null)
                mainPanel.Visible = false;
            if (infoPanel != null)
                infoPanel.Visible = true;

            infoPanel?.Controls.Add(lblWorkingTimeLabel!);
            infoPanel?.Controls.Add(lblWorkingTime!);
            infoPanel?.Controls.Add(lblTotal!);

        }

        private void BtnClear_Click(object? s, EventArgs e) => keyLogger.Clear();
        private void BtnSaveAs_Click(object? s, EventArgs e)
        {
            if (!keyLogger.IsRunning && keyLogger.LoggedKeys.Count > 0)
                fileSaver.SaveToFile();
        }
        private void BtnBack_Click(object? s, EventArgs e)
        {
            if (infoPanel != null)
                infoPanel.Visible = false;
            if (mainPanel != null)
            {
                mainPanel.Visible = true;
                mainPanel.Controls.Add(lblWorkingTimeLabel!);
                mainPanel.Controls.Add(lblWorkingTime!);
            }
        }
    }
}