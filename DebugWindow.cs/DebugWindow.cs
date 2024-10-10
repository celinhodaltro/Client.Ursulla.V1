using System;
using System.Windows.Forms;

namespace Client.Common;

public partial class DebugWindow : Form
{
    public DebugWindow()
    {
        InitializeComponent();

        // Carregar o conteúdo HTML do recurso incorporado (caso necessário)
        // Usar "using" para garantir que o StreamReader seja descartado adequadamente
        // var assembly = Assembly.GetExecutingAssembly();
        // using (var html = new System.IO.StreamReader(assembly.GetManifestResourceStream("RC.DebugPage.htm")))
        // {
        //     DebugBrowser.DocumentText = html.ReadToEnd();
        // }

        Log.Instance.OnLogMessage += OnLogMessage;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Log.Instance.OnLogMessage -= OnLogMessage;
            components?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void OnLogMessage(object sender, Log.Message message)
    {
        string logMessage = $"{message.time.ToLongTimeString()} {message.level} - {(sender != null ? sender.ToString() : "Unknown")}: {message.text}\r\n";
        DebugText.AppendText(logMessage);
        DebugText.SelectionStart = DebugText.TextLength;
        DebugText.ScrollToCaret();
    }
}
