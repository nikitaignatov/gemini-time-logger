using System;
using System.Net.Mime;
using System.Windows.Forms;

namespace Gemini.Commander.Nfc.Dialogs
{
    public class TimeLogMessageForm : Form
    {
        public Action<string,string, ContactType> Message { get; set; }
        public TimeLogMessageForm()
        {
            var panel = new FlowLayoutPanel();
            Controls.Add(panel);

            var width = 490;
            Width = width + 20;
            var text = new RichTextBox();
            var ticket = new TextBox();
            var button = new Button();
            var cbox = new ComboBox();
            cbox.DataSource = Enum.GetValues(typeof(ContactType));
            button.Text = "save";
            panel.Width = width;
            text.Width = width;

            button.Click += (sender, e) =>
            {
                Message(text.Text,ticket.Text, (ContactType)cbox.SelectedItem);
                Close();
            }; ;

            panel.Controls.Add(button);
            panel.Controls.Add(cbox);
            panel.Controls.Add(ticket);
            panel.Controls.Add(text);
        }
    }
}