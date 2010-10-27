using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Netron.Neon.WinFormsUI;
using System.Reflection;
namespace Netron.Cobalt
{
    public partial class ShellForm : DockContent
    {

        private string helpText;
        private bool cobaltMode;

        public bool CobaltMode
        {
            get { return cobaltMode; }
            set {
                cobaltMode = value;
                if (value)
                {
                    Shell.BackColor = Color.White;
                    Shell.ForeColor = Color.Black;                  
                    Shell.Prompt = "IDE>";
                    Shell.Clear();
                    this.Text = "Cobalt.IDE";
                }
                else {
                    Shell.BackColor = Color.Black;
                    Shell.ForeColor = Color.White;
                    Shell.Prompt = ">>";
                    Shell.Clear();
                    this.Text = "Command prompt";
                }
            }
        }
        public ShellForm()
        {
            InitializeComponent();


            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<Cobalt.IDE Shell>");
            stringBuilder.Append(System.Environment.NewLine);
            //stringBuilder.Append(System.Environment.NewLine);            
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append("Commands Available:");
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append("(1) All DOS commands that operate on a single line");
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append("(2) prompt - Changes prompt. Usage (prompt=<desired_prompt>");
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append("(3) history - prints history of entered commands");
            stringBuilder.Append(System.Environment.NewLine);
            stringBuilder.Append("(4) cls - Clears the screen");
            
            stringBuilder.Append(System.Environment.NewLine);

            helpText = stringBuilder.ToString();

            CobaltMode = true;}

        private void shellControl1_CommandEntered(object sender, CommandEnteredEventArgs e)
        {
            string command = e.Command;

            if (command.Equals("switch", StringComparison.CurrentCultureIgnoreCase))
            {
                CobaltMode = !CobaltMode;
                return;
            }

            if (cobaltMode)
            {
                ProcessInternalCommand(command);
            }
            else 
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
                startInfo.Arguments = "/C " + e.Command;
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                Process p = Process.Start(startInfo);
                string output = p.StandardOutput.ReadToEnd();
                string error = p.StandardError.ReadToEnd();

                p.WaitForExit();
                if (output.Length != 0)
                    Shell.WriteText(output);
                else if (error.Length != 0)
                    Shell.WriteText(error);
            }
        }

        private bool ProcessInternalCommand(string command)
        {
            if (command == "cls")
                Shell.Clear();
            else if (command == "history")
            {
                string[] commands = Shell.GetCommandHistory();
                StringBuilder stringBuilder = new StringBuilder(commands.Length);
                foreach (string s in commands)
                {
                    stringBuilder.Append(s);
                    stringBuilder.Append(System.Environment.NewLine);
                }
                Shell.WriteText(stringBuilder.ToString());
            }
            else if (command == "help")
            {
                Shell.WriteText(GetHelpText());

            }
            //else if (command.StartsWith("prompt"))
            //{
            //    string[] parts = command.Split(new char[] { '=' });
            //    if (parts.Length == 2 && parts[0].Trim() == "prompt")
            //        Shell.Prompt = parts[1].Trim();
            //}
            else if (command.Equals("switch", StringComparison.CurrentCultureIgnoreCase))
            {

            }
            else
            {
                //command += "Application.";
                MethodInfo minfo = typeof(Netron.Cobalt.Application).GetMethod(command);
                if (minfo != null)
                {
                    minfo.Invoke(null, new object[] { "There you go" });
                }
                else {
                    PropertyInfo pinfo = typeof(Netron.Cobalt.Application).GetProperty(command);
                    if (pinfo != null)
                    {
                        Shell.WriteText( pinfo.GetValue(null, new object[] { }).ToString());
                    }
                }

                return false;
            }

            return true;
        }

       

        

        private string GetHelpText()
        {
            return helpText;
        }
    }
}