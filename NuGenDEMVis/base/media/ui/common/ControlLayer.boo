import Genetibase.VisUI.Scripting from "NuGenVisUI"
import System.Windows.Forms from "System.Windows.Forms"

class ControlLayer(ScriptLayer):
	def icon_onclick(obj as object, args as MouseEventArgs):
		MessageBox.Show("sample boo code")