using System;
using System.Drawing;
using System.Windows.Forms;

namespace KeyloggerLite
{
    public class MainForm : Form
    {
        private readonly UIManager uiManager;
        private readonly KeyLogger keyLogger;
        private GlobalKeyboardHook? globalHook;
        public MainForm()
        {
            keyLogger = new KeyLogger();
            uiManager = new UIManager(this, keyLogger);
            InitializeComponent();
            uiManager.InitializeUI();
            globalHook = new GlobalKeyboardHook();
            globalHook.KeyPressed += OnGlobalKeyPressed;
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
            KeyDown += MainForm_KeyDown;
            KeyPress += MainForm_KeyPress;
        }

        private void MainForm_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Optionally handle key press events here, or leave empty if not needed
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            // Optionally handle key down events here, or leave empty if not needed
        }
        

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            globalHook?.Dispose();
            base.OnFormClosed(e);
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

            if (key < Keys.Space || key > Keys.Z || Control.ModifierKeys != Keys.None)
                keyLogger.HandleKeyPress($"[{key}]");
            else
                keyLogger.HandleKeyPress(key.ToString());
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            globalHook?.Dispose();
            base.OnFormClosing(e);
        }
    }
}