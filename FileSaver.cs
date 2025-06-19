using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace KeyloggerLite
{
    public class FileSaver
    {
        private readonly KeyLogger keyLogger;

        public FileSaver(KeyLogger keyLogger)
        {
            this.keyLogger = keyLogger;
        }

        public void SaveToFile()
        {
            using (var sfd = new SaveFileDialog { Filter = "Text Files (*.txt)|*.txt" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var sb = new StringBuilder();
                    var keyCounts = keyLogger.GetKeyCounts();

                    sb.AppendLine($"Total: {keyLogger.LoggedKeys.Count}");
                    sb.AppendLine($"{"Keys",-20} {"Quantity",10}");
                    sb.AppendLine(new string('-', 34));

                    foreach (var kv in keyCounts)
                        sb.AppendLine($"{kv.Key,-20} {kv.Value,10}");

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                }
            }
        }
    }
}
