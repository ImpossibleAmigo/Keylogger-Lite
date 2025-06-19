using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KeyloggerLite
{
    public class DataGridViewConfigurator
    {
        private DataGridView? dgvKeys;

        public void ConfigureDataGridView(Panel infoPanel)
        {
            dgvKeys = new DataGridView
            {
                Location = new Point(10, 150),
                Size = new Size(445, 352),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToResizeColumns = false,
                RowHeadersVisible = false,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                BackgroundColor = ColorTranslator.FromHtml("#B4B4B4"),
                GridColor = Color.Black,
                Font = new Font("Segoe UI", 11F),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = ColorTranslator.FromHtml("#D0D0D0"),
                    ForeColor = Color.Black,
                    SelectionBackColor = ColorTranslator.FromHtml("#CCCCCC"),
                    SelectionForeColor = Color.Black,
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = ColorTranslator.FromHtml("#B4B4B4"),
                    ForeColor = Color.Black,
                    Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                    Alignment = DataGridViewContentAlignment.TopLeft
                },
                EnableHeadersVisualStyles = false
            };

            dgvKeys.Columns.Add("Keys", "Keys");
            dgvKeys.Columns.Add("Quantity", "Quantity");
            dgvKeys.Columns[0].Width = 210;
            dgvKeys.Columns[1].Width = 210;
            dgvKeys.SelectionChanged += (s, e) => dgvKeys.ClearSelection();
            infoPanel.Controls.Add(dgvKeys);
        }
        public void UpdateGrid(Dictionary<string, int> keyCounts)
        {
            if (dgvKeys == null) return;
            dgvKeys.Rows.Clear();
            foreach (var kv in keyCounts)
                dgvKeys.Rows.Add(kv.Key, kv.Value);
        }
    }
}