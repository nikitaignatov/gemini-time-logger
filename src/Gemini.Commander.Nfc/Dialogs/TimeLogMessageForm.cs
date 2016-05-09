using System;
using System.Windows.Forms;

namespace Gemini.Commander.Nfc.Dialogs
{
    public class TimeLogMessageForm : Form
    {
        public Action<string> Message { get; set; }
        public TimeLogMessageForm()
        {
            var panel = new FlowLayoutPanel();
            Controls.Add(panel);

            var width = 490;
            Width = width + 20;
            var text = new RichTextBox();
            var button = new Button();
            button.Text = "save";
            panel.Width = width;
            text.Width = width;

            button.Click += (sender, e) =>
            {
                Message(text.Text);
                Close();
            }; ;

            panel.Controls.Add(button);
            panel.Controls.Add(text);
        }
    }
}