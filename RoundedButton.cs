using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace KeyloggerLite
{
    public class RoundedButton : Button
    {
        public int BorderRadius { get; set; } = 5;
        public Color BorderColor { get; set; } = Color.Black;
        public int BorderSize { get; set; } = 1;

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Resize += (s, e) => Invalidate();
        }

        private GraphicsPath GetRoundPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle surface = ClientRectangle;
            Rectangle border = Rectangle.Inflate(surface, -BorderSize, -BorderSize);

            using (GraphicsPath pathSurface = GetRoundPath(surface, BorderRadius))
            using (GraphicsPath pathBorder = GetRoundPath(border, BorderRadius - BorderSize))
            using (Pen penBorder = new Pen(BorderColor, BorderSize))
            {
                pe.Graphics.FillPath(new SolidBrush(BackColor), pathSurface);
                if (BorderSize > 0)
                    pe.Graphics.DrawPath(penBorder, pathBorder);

                Region = new Region(pathSurface);
                TextRenderer.DrawText(pe.Graphics, Text, Font, surface, ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }
    }
}