using System;
using System.Drawing;
using System.Windows.Forms;

namespace KeyloggerLite
{
    public class MainForm : Form
    {
        private readonly UIManager uiManager;
        private readonly KeyLogger keyLogger;

        public MainForm()
        {
            keyLogger = new KeyLogger();
            uiManager = new UIManager(this, keyLogger);
            InitializeComponent();
            uiManager.InitializeUI();
        }

        public KeyLogger KeyLogger => keyLogger;
        public UIManager UIManager => uiManager;

        private void InitializeComponent()
        {
            Text = "Keylogger-Lite";
            Size = new Size(480, 550);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = ColorTranslator.FromHtml("#CCCCCC");
            KeyPreview = true;

            MouseDown += Form_MouseDown;
            KeyDown += MainForm_KeyDown;
            KeyPress += MainForm_KeyPress;
        }

        private void Form_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Capture = false;
                Message msg = Message.Create(Handle, 0xA1, new IntPtr(2), IntPtr.Zero);
                WndProc(ref msg);
            }
        }

        private void MainForm_KeyPress(object? sender, KeyPressEventArgs e)
        {
            keyLogger.HandleKeyPress(e.KeyChar.ToString());
        }
        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode < Keys.Space || e.KeyCode > Keys.Z || e.Modifiers != Keys.None)
                keyLogger.HandleKeyPress($"[{e.KeyCode}]");
        }
    }
}