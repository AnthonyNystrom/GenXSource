
using System;
using System.Windows.Forms;
using System.Drawing;

	/// <summary>
	/// This interface should be implemented by any class whose instances are intended 
	/// to be executed by a thread.
	/// </summary>
	public interface IThreadRunnable
	{
		/// <summary>
		/// This method has to be implemented in order that starting of the thread causes the object's 
		/// run method to be called in that separately executing thread.
		/// </summary>
		void Run();
	}

/// <summary>
/// Contains conversion support elements such as classes, interfaces and static methods.
/// </summary>
public class SupportClass
{
	/// <summary>
	/// This class supports basic functionality of the JOptionPane class.
	/// </summary>
	public class OptionPaneSupport 
	{
		/// <summary>
		/// This method finds the form which contains an specific control.
		/// </summary>
		/// <param name="control">The control which we need to find its form.</param>
		/// <returns>The form which contains the control</returns>
		public static System.Windows.Forms.Form GetFrameForComponent(System.Windows.Forms.Control control)
		{
			System.Windows.Forms.Form result = null;
			if (control == null)return null;
			if (control is System.Windows.Forms.Form)
				result = (System.Windows.Forms.Form)control;
			else if (control.Parent != null)
				result = GetFrameForComponent(control.Parent);
			return result;
		}

		/// <summary>
		/// This method finds the MDI container form which contains an specific control.
		/// </summary>
		/// <param name="control">The control which we need to find its MDI container form.</param>
		/// <returns>The MDI container form which contains the control.</returns>
		public static System.Windows.Forms.Form GetDesktopPaneForComponent(System.Windows.Forms.Control control)
		{
			System.Windows.Forms.Form result = null;
			if (control == null)return null;
			if (control.GetType().IsSubclassOf(typeof(System.Windows.Forms.Form)))
				if (((System.Windows.Forms.Form)control).IsMdiContainer)
					result = (System.Windows.Forms.Form)control;
				else if (((System.Windows.Forms.Form)control).IsMdiChild)
					result = GetDesktopPaneForComponent(((System.Windows.Forms.Form)control).MdiParent);
				else if (control.Parent != null)
					result = GetDesktopPaneForComponent(control.Parent);
			return result;
		}
		
		/// <summary>
		/// This method retrieves the message that is contained into the object that is sended by the user.
		/// </summary>
		/// <param name="control">The control which we need to find its form.</param>
		/// <returns>The form which contains the control</returns>
		public static System.String GetMessageForObject(System.Object message)
		{
			System.String result = "";
			if (message == null)
			  return result;
			else 
		 	  result = message.ToString();
			return result;
		}


		/// <summary>
		/// This method displays a dialog with a Yes, No, Cancel buttons and a question icon.
		/// </summary>
		/// <param name="parent">The component which will be the owner of the dialog.</param>
		/// <param name="message">The message to be displayed; if it isn't an String it displays the return value of the ToString() method of the object.</param>
		/// <returns>The integer value which represents the button pressed.</returns>
		public static int ShowConfirmDialog(System.Windows.Forms.Control parent, System.Object message)
		{
			return ShowConfirmDialog(parent, message,"Select an option...", (int)System.Windows.Forms.MessageBoxButtons.YesNoCancel,
				(int)System.Windows.Forms.MessageBoxIcon.Question);
		}

		/// <summary>
		/// This method displays a dialog with specific buttons and a question icon.
		/// </summary>
		/// <param name="parent">The component which will be the owner of the dialog.</param>
		/// <param name="message">The message to be displayed; if it isn't an String it displays the result value of the ToString() method of the object.</param>
		/// <param name="title">The title for the message dialog.</param>
		/// <param name="optiontype">The set of buttons to be displayed in the message box; defined by the MessageBoxButtons enumeration.</param>
		/// <returns>The integer value which represents the button pressed.</returns>
		public static int ShowConfirmDialog(System.Windows.Forms.Control parent, System.Object message,
			System.String title,int optiontype)
		{
			return ShowConfirmDialog(parent, message, title, optiontype, (int)System.Windows.Forms.MessageBoxIcon.Question);
		}

		/// <summary>
		/// This method displays a dialog with specific buttons and specific icon.
		/// </summary>
		/// <param name="parent">The component which will be the owner of the dialog.</param>
		/// <param name="message">The message to be displayed; if it isn't an String it displays the return value of the ToString() method of the object.</param>
		/// <param name="title">The title for the message dialog.</param>
		/// <param name="optiontype">The set of buttons to be displayed in the message box; defined by the MessageBoxButtons enumeration.</param>
		/// <param name="messagetype">The messagetype defines the icon to be displayed in the message box.</param>
		/// <returns>The integer value which represents the button pressed.</returns>
		public static int ShowConfirmDialog(System.Windows.Forms.Control parent, System.Object message,
			System.String title, int optiontype, int messagetype)
		{
			return (int)System.Windows.Forms.MessageBox.Show(GetFrameForComponent(parent), GetMessageForObject(message), title,
				(System.Windows.Forms.MessageBoxButtons)optiontype, (System.Windows.Forms.MessageBoxIcon)messagetype);
		}

		/// <summary>
		/// This method displays a simple MessageBox.
		/// </summary>
		/// <param name="parent">The component which will be the owner of the dialog.</param>
		/// <param name="message">The message to be displayed; if it isn't an String it displays result value of the ToString() method of the object.</param>
		public static void ShowMessageDialog(System.Windows.Forms.Control parent, System.Object message)
		{
			ShowMessageDialog(parent, message, "Message", (int)System.Windows.Forms.MessageBoxIcon.Information);
		}

		/// <summary>
		/// This method displays a simple MessageBox with a specific icon.
		/// </summary>
		/// <param name="parent">The component which will be the owner of the dialog.</param>
		/// <param name="message">The message to be displayed; if it isn't an String it displays result value of the ToString() method of the object.</param>
		/// <param name="title">The title for the message dialog.</param>
		/// <param name="messagetype">The messagetype defines the icon to be displayed in the message box.</param>
		public static void ShowMessageDialog(System.Windows.Forms.Control parent, System.Object message,
			System.String title, int messagetype)
		{
			System.Windows.Forms.MessageBox.Show(GetFrameForComponent(parent), GetMessageForObject(message), title,
				System.Windows.Forms.MessageBoxButtons.OK, (System.Windows.Forms.MessageBoxIcon)messagetype);
		}
	}


	/*******************************/
	/// <summary>
	/// Recieves a form and an integer value representing the operation to perform when the closing 
	/// event is fired.
	/// </summary>
	/// <param name="form">The form that fire the event.</param>
	/// <param name="operation">The operation to do while the form is closing.</param>
	public static void CloseOperation(System.Windows.Forms.Form form, int operation)
	{
		switch (operation)
		{
			case 0:
				break;
			case 1:
				form.Hide();
				break;
			case 2:
				form.Dispose();
				break;
			case 3:
				form.Dispose();
				System.Windows.Forms.Application.Exit();
				break;
		}
	}


	/*******************************/
/// <summary>
/// Contains methods to construct customized Buttons
/// </summary>
public class ButtonSupport
{
	/// <summary>
	/// Creates a popup style Button with an specific text.	
	/// </summary>
	/// <param name="label">The text associated with the Button</param>
	/// <returns>The new Button</returns>
	public static Button CreateButton(System.String label)
	{			
		Button tempButton = new Button();
		tempButton.Text = label;
		tempButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		return tempButton;
    }

	/// <summary>
	/// Sets the an specific text for the Button
	/// </summary>
	/// <param name="Button">The button to be set</param>
	/// <param name="label">The text associated with the Button</param>
	public static void SetButton(Button Button, System.String label)
	{
		Button.Text = label;
		Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
	}

	/// <summary>
	/// Creates a Button with an specific text and style.
	/// </summary>
	/// <param name="label">The text associated with the Button</param>
	/// <param name="style">The style of the Button</param>
	/// <returns>The new Button</returns>
	public static Button CreateButton(System.String label, int style)
	{
		Button tempButton = new Button();
		tempButton.Text = label;
		setStyle(tempButton,style);
		return tempButton;
	}

	/// <summary>
	/// Sets the specific Text and Style for the Button
	/// </summary>
	/// <param name="Button">The button to be set</param>
	/// <param name="label">The text associated with the Button</param>
	/// <param name="style">The style of the Button</param>
	public static void SetButton(ButtonBase Button, System.String label, int style)
	{
		Button.Text = label;
		setStyle(Button,style);
	}

	/// <summary>
	/// Creates a standard style Button that contains an specific text and/or image
	/// </summary>
	/// <param name="control">The control to be contained analized to get the text and/or image for the Button</param>
	/// <returns>The new Button</returns>
	public static Button CreateButton(System.Windows.Forms.Control control)
	{
		Button tempButton = new Button();
		if(control.GetType().FullName == "System.Windows.Forms.Label")
		{
			tempButton.Image = ((System.Windows.Forms.Label)control).Image;
			tempButton.Text = ((System.Windows.Forms.Label)control).Text;
			tempButton.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
			tempButton.TextAlign = ((System.Windows.Forms.Label)control).TextAlign;
		}
		else
		{
			if(control.GetType().FullName == "System.Windows.Forms.PictureBox")//Tentative to see maps of UIGraphic
			{
				tempButton.Image = ((System.Windows.Forms.PictureBox)control).Image;
				tempButton.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
			}else
				tempButton.Text = control.Text;
		}
		tempButton.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
		return tempButton;
	}

	/// <summary>
	/// Sets an specific text and/or image to the Button
	/// </summary>
	/// <param name="Button">The button to be set</param>
	/// <param name="control">The control to be contained analized to get the text and/or image for the Button</param>
	public static void SetButton(ButtonBase Button,System.Windows.Forms.Control control)
	{
		if(control.GetType().FullName == "System.Windows.Forms.Label")
		{
			Button.Image = ((System.Windows.Forms.Label)control).Image;
			Button.Text = ((System.Windows.Forms.Label)control).Text;
			Button.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
			Button.TextAlign = ((System.Windows.Forms.Label)control).TextAlign;
		}
		else
		{
			if(control.GetType().FullName == "System.Windows.Forms.PictureBox")//Tentative to see maps of UIGraphic
			{
				Button.Image = ((System.Windows.Forms.PictureBox)control).Image;
				Button.ImageAlign = ((System.Windows.Forms.Label)control).ImageAlign;
			}
			else
				Button.Text = control.Text;
		}
		Button.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
	}

	/// <summary>
	/// Creates a Button with an specific control and style
	/// </summary>
	/// <param name="control">The control to be contained by the button</param>
	/// <param name="style">The style of the button</param>
	/// <returns>The new Button</returns>
	public static Button CreateButton(System.Windows.Forms.Control control, int style)
	{
		Button tempButton = CreateButton(control);
		setStyle(tempButton,style);
		return tempButton;
	}

	/// <summary>
	/// Sets an specific text and/or image to the Button
	/// </summary>
	/// <param name="Button">The button to be set</param>
	/// <param name="control">The control to be contained by the button</param>
	/// <param name="style">The style of the button</param>
	public static void SetButton(ButtonBase Button,System.Windows.Forms.Control control,int style)
	{
		SetButton(Button,control);
		setStyle(Button,style);
	}

	/// <summary>
	/// Sets the style of the Button
	/// </summary>
	/// <param name="Button">The Button that will change its style</param>
	/// <param name="style">The new style of the Button</param>
	/// <remarks> 
	/// If style is 0 then sets a popup style to the Button, otherwise sets a standard style to the Button.
	/// </remarks>
	public static void setStyle(ButtonBase Button, int style)
	{
		if (  (style == 0 ) || (style ==  67108864) || (style ==  33554432) ) 
			Button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		else if ( (style == 2097152) || (style == 1048576) ||  (style == 16777216 ) )
				Button.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
		else 
			throw new System.ArgumentException("illegal style: " + style);		
	}

	/// <summary>
	/// Selects the Button
	/// </summary>
	/// <param name="Button">The Button that will change its style</param>
	/// <param name="select">It determines if the button woll be selected</param>
	/// <remarks> 
	/// If select is true thebutton will be selected, otherwise not.
	/// </remarks>
	public static void setSelected(ButtonBase Button, bool select)
	{
		if (select)
			Button.Select();
	}

	/// <summary>
	/// Receives a Button instance and sets the Text and Image properties.
	/// </summary>
	/// <param name="buttonInstance">Button instance to be set.</param>
	/// <param name="buttonText">Value to be set to Text.</param>
	/// <param name="icon">Value to be set to Image.</param>
	public static void SetStandardButton (ButtonBase buttonInstance, System.String buttonText , System.Drawing.Image icon )
	{
		buttonInstance.Text = buttonText;
		buttonInstance.Image = icon;
	}

	/// <summary>
	/// Creates a Button with a given text.
	/// </summary>
	/// <param name="buttonText">The text to be displayed in the button.</param>
	/// <returns>The new created button with text</returns>
    public static Button CreateStandardButton(System.String buttonText)
	{
        Button newButton = new Button();
		newButton.Text = buttonText;
		return newButton;
	}

	/// <summary>
	/// Creates a Button with a given image.
	/// </summary>
	/// <param name="buttonImage">The image to be displayed in the button.</param>
	/// <returns>The new created button with an image</returns>
	public static Button CreateStandardButton (System.Drawing.Image buttonImage)
	{
		Button newButton = new Button();
		newButton.Image = buttonImage;
		return newButton;
	}

	/// <summary>
	/// Creates a Button with a given image and a text.
	/// </summary>
	/// <param name="buttonText">The text to be displayed in the button.</param>
	/// <param name="buttonImage">The image to be displayed in the button.</param>
	/// <returns>The new created button with text and image</returns>
	public static Button CreateStandardButton (System.String buttonText, System.Drawing.Image buttonImage)
	{
		Button newButton = new Button();
		newButton.Text = buttonText;
		newButton.Image = buttonImage;
		return newButton;
	}
}
	/*******************************/
	/// <summary>
	/// Class used to store and retrieve an object command specified as a String.
	/// </summary>
	public class CommandManager
	{
		/// <summary>
		/// Private Hashtable used to store objects and their commands.
		/// </summary>
		private static System.Collections.Hashtable Commands = new System.Collections.Hashtable();

		/// <summary>
		/// Sets a command to the specified object.
		/// </summary>
		/// <param name="obj">The object that has the command.</param>
		/// <param name="cmd">The command for the object.</param>
		public static void SetCommand(System.Object obj, System.String cmd)
		{
			if (obj != null)
			{
				if (Commands.Contains(obj))
					Commands[obj] = cmd;
				else
					Commands.Add(obj, cmd);
			}
		}

		/// <summary>
		/// Gets a command associated with an object.
		/// </summary>
		/// <param name="obj">The object whose command is going to be retrieved.</param>
		/// <returns>The command of the specified object.</returns>
		public static System.String GetCommand(System.Object obj)
		{
			System.String result = "";
			if (obj != null)
                result = obj.ToString(); //  System.Convert.ToString(Commands[obj]);
			return result;
		}



		/// <summary>
		/// Checks if the Control contains a command, if it does not it sets the default
		/// </summary>
		/// <param name="button">The control whose command will be checked</param>
        public static void CheckCommand(Button button)
		{
			if (button != null)
			{
				if (GetCommand(button).Equals(""))
					SetCommand(button, button.Text);
			}
		}

		/// <summary>
		/// Checks if the Control contains a command, if it does not it sets the default
		/// </summary>
		/// <param name="button">The control whose command will be checked</param>
		public static void CheckCommand(System.Windows.Forms.MenuItem menuItem)
		{
			if (menuItem != null)
			{
				if (GetCommand(menuItem).Equals(""))
					SetCommand(menuItem, menuItem.Text);
			}
		}

		/// <summary>
		/// Checks if the Control contains a command, if it does not it sets the default
		/// </summary>
		/// <param name="button">The control whose command will be checked</param>
		public static void CheckCommand(System.Windows.Forms.ComboBox comboBox)
		{
			if (comboBox != null)
			{
				if (GetCommand(comboBox).Equals(""))
					SetCommand(comboBox,"comboBoxChanged");
			}
		}

	}
	/*******************************/
	/// <summary>
	/// Writes the exception stack trace to the received stream
	/// </summary>
	/// <param name="throwable">Exception to obtain information from</param>
	/// <param name="stream">Output sream used to write to</param>
	public static void WriteStackTrace(System.Exception throwable, System.IO.TextWriter stream)
	{
		stream.Write(throwable.StackTrace);
		stream.Flush();
	}

	/*******************************/
	/// <summary>
	/// Support class used to handle threads
	/// </summary>
	public class ThreadClass : IThreadRunnable
	{
		/// <summary>
		/// The instance of System.Threading.Thread
		/// </summary>
		private System.Threading.Thread threadField;
	      
		/// <summary>
		/// Initializes a new instance of the ThreadClass class
		/// </summary>
		public ThreadClass()
		{
			threadField = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
		}
	 
		/// <summary>
		/// Initializes a new instance of the Thread class.
		/// </summary>
		/// <param name="Name">The name of the thread</param>
		public ThreadClass(System.String Name)
		{
			threadField = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
			this.Name = Name;
		}
	      
		/// <summary>
		/// Initializes a new instance of the Thread class.
		/// </summary>
		/// <param name="Start">A ThreadStart delegate that references the methods to be invoked when this thread begins executing</param>
		public ThreadClass(System.Threading.ThreadStart Start)
		{
			threadField = new System.Threading.Thread(Start);
		}
	 
		/// <summary>
		/// Initializes a new instance of the Thread class.
		/// </summary>
		/// <param name="Start">A ThreadStart delegate that references the methods to be invoked when this thread begins executing</param>
		/// <param name="Name">The name of the thread</param>
		public ThreadClass(System.Threading.ThreadStart Start, System.String Name)
		{
			threadField = new System.Threading.Thread(Start);
			this.Name = Name;
		}
	      
		/// <summary>
		/// This method has no functionality unless the method is overridden
		/// </summary>
		public virtual void Run()
		{
		}
	      
		/// <summary>
		/// Causes the operating system to change the state of the current thread instance to ThreadState.Running
		/// </summary>
		public virtual void Start()
		{
			threadField.Start();
		}
	      
		/// <summary>
		/// Interrupts a thread that is in the WaitSleepJoin thread state
		/// </summary>
		public virtual void Interrupt()
		{
			threadField.Interrupt();
		}
	      
		/// <summary>
		/// Gets the current thread instance
		/// </summary>
		public System.Threading.Thread Instance
		{
			get
			{
				return threadField;
			}
			set
			{
				threadField = value;
			}
		}
	      
		/// <summary>
		/// Gets or sets the name of the thread
		/// </summary>
		public System.String Name
		{
			get
			{
				return threadField.Name;
			}
			set
			{
				if (threadField.Name == null)
					threadField.Name = value; 
			}
		}
	      
		/// <summary>
		/// Gets or sets a value indicating the scheduling priority of a thread
		/// </summary>
		public System.Threading.ThreadPriority Priority
		{
			get
			{
				return threadField.Priority;
			}
			set
			{
				threadField.Priority = value;
			}
		}
	      
		/// <summary>
		/// Gets a value indicating the execution status of the current thread
		/// </summary>
		public bool IsAlive
		{
			get
			{
				return threadField.IsAlive;
			}
		}
	      
		/// <summary>
		/// Gets or sets a value indicating whether or not a thread is a background thread.
		/// </summary>
		public bool IsBackground
		{
			get
			{
				return threadField.IsBackground;
			} 
			set
			{
				threadField.IsBackground = value;
			}
		}
	      
		/// <summary>
		/// Blocks the calling thread until a thread terminates
		/// </summary>
		public void Join()
		{
			threadField.Join();
		}
	      
		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses
		/// </summary>
		/// <param name="MiliSeconds">Time of wait in milliseconds</param>
		public void Join(long MiliSeconds)
		{
			lock(this)
			{
				threadField.Join(new System.TimeSpan(MiliSeconds * 10000));
			}
		}
	      
		/// <summary>
		/// Blocks the calling thread until a thread terminates or the specified time elapses
		/// </summary>
		/// <param name="MiliSeconds">Time of wait in milliseconds</param>
		/// <param name="NanoSeconds">Time of wait in nanoseconds</param>
		public void Join(long MiliSeconds, int NanoSeconds)
		{
			lock(this)
			{
				threadField.Join(new System.TimeSpan(MiliSeconds * 10000 + NanoSeconds * 100));
			}
		}
	      
		/// <summary>
		/// Resumes a thread that has been suspended
		/// </summary>
		public void Resume()
		{
			threadField.Resume();
		}
	      
		/// <summary>
		/// Raises a ThreadAbortException in the thread on which it is invoked, 
		/// to begin the process of terminating the thread. Calling this method 
		/// usually terminates the thread
		/// </summary>
		public void Abort()
		{
			threadField.Abort();
		}
	      
		/// <summary>
		/// Raises a ThreadAbortException in the thread on which it is invoked, 
		/// to begin the process of terminating the thread while also providing
		/// exception information about the thread termination. 
		/// Calling this method usually terminates the thread.
		/// </summary>
		/// <param name="stateInfo">An object that contains application-specific information, such as state, which can be used by the thread being aborted</param>
		public void Abort(System.Object stateInfo)
		{
			lock(this)
			{
				threadField.Abort(stateInfo);
			}
		}
	      
		/// <summary>
		/// Suspends the thread, if the thread is already suspended it has no effect
		/// </summary>
		public void Suspend()
		{
			threadField.Suspend();
		}
	      
		/// <summary>
		/// Obtain a String that represents the current Object
		/// </summary>
		/// <returns>A String that represents the current Object</returns>
		public override System.String ToString()
		{
			return "Thread[" + Name + "," + Priority.ToString() + "," + "" + "]";
		}
	     
		/// <summary>
		/// Gets the currently running thread
		/// </summary>
		/// <returns>The currently running thread</returns>
		public static ThreadClass Current()
		{
			ThreadClass CurrentThread = new ThreadClass();
			CurrentThread.Instance = System.Threading.Thread.CurrentThread;
			return CurrentThread;
		}
	}


	/*******************************/
	/// <summary>
	/// This class contains static methods to manage tab controls.
	/// </summary>
	public class TabControlSupport
	{
		/// <summary>
		/// Create a new instance of TabControl and set the alignment property.
		/// </summary>
		/// <param name="alignment">The alignment property value.</param>
		/// <returns>New TabControl instance.</returns>
		public static System.Windows.Forms.TabControl CreateTabControl( System.Windows.Forms.TabAlignment alignment)
		{
			System.Windows.Forms.TabControl tabcontrol = new System.Windows.Forms.TabControl();
			tabcontrol.Alignment = alignment;
			return tabcontrol;
		}

		/// <summary>
		/// Set the alignment property to an instance of TabControl .
		/// </summary>
		/// <param name="tabcontrol">An instance of TabControl.</param>
		/// <param name="alignment">The alignment property value.</param>
		public static void SetTabControl( System.Windows.Forms.TabControl tabcontrol, System.Windows.Forms.TabAlignment alignment)
		{
			tabcontrol.Alignment = alignment;
		}

		/// <summary>
		/// Method to add TabPages into the TabControl object.
		/// </summary>
		/// <param name="tabControl">The TabControl to be modified.</param>
		/// <param name="component">A component to be added into the new TabControl.</param>
		public static System.Windows.Forms.Control AddTab(System.Windows.Forms.TabControl tabControl, System.Windows.Forms.Control component)
		{
			System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage();
			tabPage.Controls.Add(component);
			tabControl.TabPages.Add(tabPage);
			return component;
		}
	
		/// <summary>
		/// Method to add TabPages into the TabControl object.
		/// </summary>
		/// <param name="tabControl">The TabControl to be modified.</param>
		/// <param name="TabLabel">The label for the new TabPage.</param>
		/// <param name="component">A component to be added into the new TabControl.</param>
		public static System.Windows.Forms.Control AddTab(System.Windows.Forms.TabControl tabControl, System.String tabLabel, System.Windows.Forms.Control component)
		{
			System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage(tabLabel);
			tabPage.Controls.Add(component);
			tabControl.TabPages.Add(tabPage);
			return component;
		}

		/// <summary>
		/// Method to add TabPages into the TabControl object.
		/// </summary>
		/// <param name="tabControl">The TabControl to be modified.</param>
		/// <param name="component">A component to be added into the new TabControl.</param>
		/// <param name="constraints">The object that should be displayed in the tab but won't because of limitations</param>		
		public static void AddTab(System.Windows.Forms.TabControl tabControl, System.Windows.Forms.Control component, System.Object constraints)
		{
			System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage();
			if (constraints is System.String) 
			{
				tabPage.Text = (String)constraints;
			}
			tabPage.Controls.Add(component);
			tabControl.TabPages.Add(tabPage);
		}

		/// <summary>
		/// Method to add TabPages into the TabControl object.
		/// </summary>
		/// <param name="tabControl">The TabControl to be modified.</param>
		/// <param name="TabLabel">The label for the new TabPage.</param>
		/// <param name="constraints">The object that should be displayed in the tab but won't because of limitations</param>
		/// <param name="component">A component to be added into the new TabControl.</param>
		public static void AddTab(System.Windows.Forms.TabControl tabControl, System.String tabLabel, System.Object constraints, System.Windows.Forms.Control component)
		{
			System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage(tabLabel);
			tabPage.Controls.Add(component);
			tabControl.TabPages.Add(tabPage);
		}

		/// <summary>
		/// Method to add TabPages into the TabControl object.
		/// </summary>
		/// <param name="tabControl">The TabControl to be modified.</param>
		/// <param name="tabLabel">The label for the new TabPage.</param>
		/// <param name="image">Background image for the TabPage.</param>
		/// <param name="component">A component to be added into the new TabControl.</param>
		public static void AddTab(System.Windows.Forms.TabControl tabControl, System.String tabLabel, System.Drawing.Image image, System.Windows.Forms.Control component)
		{
			System.Windows.Forms.TabPage tabPage = new System.Windows.Forms.TabPage(tabLabel);			
			tabPage.BackgroundImage = image;
			tabPage.Controls.Add(component);
			tabControl.TabPages.Add(tabPage);			
		}
	}


	/*******************************/
	/// <summary>
	/// This method works as a handler for the Control.Layout event, it arranges the controls into the container 
	/// control in a left-to-right orientation.
	/// The location of each control will be calculated according the number of them in the container. 
	/// The corresponding alignment, the horizontal and vertical spacing between the inner controls are specified
	/// as an array of object values in the Tag property of the container.
	/// </summary>
	/// <param name="event_sender">The container control in which the controls will be relocated.</param>
	/// <param name="eventArgs">Data of the event.</param>
	public static void FlowLayoutResize(System.Object event_sender, System.Windows.Forms.LayoutEventArgs eventArgs)
	{
		System.Windows.Forms.Control container = (System.Windows.Forms.Control) event_sender;
		if (container.Tag is System.Array)
		{
			System.Object[] items = (System.Object[]) container.Tag;
			if (items.Length == 3)
			{
				container.SuspendLayout();

				int width = container.Width;
				int height = container.Height;
				if (!(container is System.Windows.Forms.ScrollableControl))
				{
					width = container.DisplayRectangle.Width;
					height = container.DisplayRectangle.Height;
				}
				else
					if (container is System.Windows.Forms.Form)
					{
						width = ((System.Windows.Forms.Form) container).ClientSize.Width;
						height = ((System.Windows.Forms.Form) container).ClientSize.Height;
					}
				System.Drawing.ContentAlignment alignment = (System.Drawing.ContentAlignment) items[0];
				int horizontal = (int) items[1];
				int vertical = (int) items[2];

				// Split controls in several rows
                System.Collections.IList rows = new System.Collections.ArrayList();
                System.Collections.IList list = new System.Collections.ArrayList();
				int tempWidth = 0;
				int tempHeight = 0;
				int totalHeight = 0;
				for (int index = 0; index < container.Controls.Count; index++)
				{
					if (tempHeight < container.Controls[index].Height)
						tempHeight = container.Controls[index].Height;

					list.Add(container.Controls[index]);

					if (index == 0) tempWidth = container.Controls[0].Width;

					if (index == container.Controls.Count - 1)
					{
						rows.Add(list);
						totalHeight += tempHeight + vertical;
					}
					else
					{
						tempWidth += horizontal + container.Controls[index + 1].Width;
						if (tempWidth >= width - horizontal * 2)
						{
							rows.Add(list);
							totalHeight += tempHeight + vertical;
							tempHeight = 0;
							list = new System.Collections.ArrayList();
							tempWidth = container.Controls[index + 1].Width;
						}
					}
				}
				totalHeight -= vertical;

				// Break out alignment coordinates
				int h = 0;
				int cx = 0;
				int cy = 0;
				if (((int) alignment & 0x00F) > 0)
				{
					h = (int) alignment;
					cy = 1;
				}
				if (((int) alignment & 0x0F0) > 0)
				{
					h = (int) alignment >> 4;
					cy = 2;
				}
				if (((int) alignment & 0xF00) > 0)
				{
					h = (int) alignment >> 8;
					cy = 3;
				}
				if (h == 1) cx = 1;
				if (h == 2) cx = 2;
				if (h == 4) cx = 3;

				int ypos = vertical;
				if (cy == 2) ypos = height / 2 - totalHeight / 2;
				if (cy == 3) ypos = height - totalHeight - vertical;
				foreach (System.Collections.IList row in rows)
				{
					int maxHeight = PlaceControls(row, width, cx, ypos, horizontal);
					ypos += vertical + maxHeight;
				}
				container.ResumeLayout();
			}
		}
	}

	private static int PlaceControls(System.Collections.IList controls, int width, int cx, int ypos, int horizontal)
	{
		int count = controls.Count;
		int controlsWidth = 0;
		int maxHeight = 0;
		foreach (System.Windows.Forms.Control control in controls)
		{
			controlsWidth += control.Width;
			if (maxHeight < control.Height) maxHeight = control.Height;
		}
		controlsWidth += horizontal * (count - 1);

		// Start x point
		int xpos = 0;
		if (cx == 1) xpos = horizontal; // Left
		if (cx == 2) xpos = width / 2 - controlsWidth / 2; // Center
		if (cx == 3) xpos = width - horizontal - controlsWidth; // Right

		// Place controls
		int x = xpos;
		foreach (System.Windows.Forms.Control control in controls)
		{
			int y = ypos + (maxHeight / 2) - control.Height / 2;
			control.Location = new System.Drawing.Point(x, y);
			x += control.Width + horizontal;
		}
		return maxHeight;
	}


	/*******************************/
	/// <summary>
	/// Implements number format functions
	/// </summary>
	[Serializable]
	public class TextNumberFormat
	{

		//Current localization number format infomation
		private System.Globalization.NumberFormatInfo numberFormat;
		//Enumeration of format types that can be used
		private enum formatTypes { General, Number, Currency, Percent };
		//Current format type used in the instance
		private int numberFormatType;
		//Indicates if grouping is being used
		private bool groupingActivated;
		//Current separator used
		private System.String separator;
		//Number of maximun digits in the integer portion of the number to represent the number
		private int maxIntDigits;
		//Number of minimum digits in the integer portion of the number to represent the number
		private int minIntDigits;
		//Number of maximun digits in the fraction portion of the number to represent the number
		private int maxFractionDigits;
		//Number of minimum digits in the integer portion of the number to represent the number
		private int minFractionDigits;

		/// <summary>
		/// Initializes a new instance of the object class with the default values
		/// </summary>
		public TextNumberFormat()
		{
			this.numberFormat      = new System.Globalization.NumberFormatInfo();
			this.numberFormatType  = (int)TextNumberFormat.formatTypes.General;
			this.groupingActivated = true;
			this.separator = this.GetSeparator( (int)TextNumberFormat.formatTypes.General );
			this.maxIntDigits = 127;
			this.minIntDigits = 1;
			this.maxFractionDigits = 3;
			this.minFractionDigits = 0;
		}

		/// <summary>
		/// Sets the Maximum integer digits value. 
		/// </summary>
		/// <param name="newValue">the new value for the maxIntDigits field</param>
		public void setMaximumIntegerDigits(int newValue)
		{
			maxIntDigits = newValue;
			if (newValue <= 0)
			{
				maxIntDigits = 0;
				minIntDigits = 0;
			}
			else if (maxIntDigits < minIntDigits)
			{
				minIntDigits = maxIntDigits;
			}
		}

		/// <summary>
		/// Sets the minimum integer digits value. 
		/// </summary>
		/// <param name="newValue">the new value for the minIntDigits field</param>
		public void setMinimumIntegerDigits(int newValue)
		{
			minIntDigits = newValue;
			if (newValue <= 0)
			{
				minIntDigits = 0;
			}
			else if (maxIntDigits < minIntDigits)
			{
				maxIntDigits = minIntDigits;
			}
		}

		/// <summary>
		/// Sets the maximum fraction digits value. 
		/// </summary>
		/// <param name="newValue">the new value for the maxFractionDigits field</param>
		public void setMaximumFractionDigits(int newValue)
		{
			maxFractionDigits = newValue;
			if (newValue <= 0)
			{
				maxFractionDigits = 0;
				minFractionDigits = 0;
			}
			else if (maxFractionDigits < minFractionDigits)
			{
				minFractionDigits = maxFractionDigits;
			}
		}
		
		/// <summary>
		/// Sets the minimum fraction digits value. 
		/// </summary>
		/// <param name="newValue">the new value for the minFractionDigits field</param>
		public void setMinimumFractionDigits(int newValue)
		{
			minFractionDigits = newValue;
			if (newValue <= 0)
			{
				minFractionDigits = 0;
			}
			else if (maxFractionDigits < minFractionDigits)
			{
				maxFractionDigits = minFractionDigits;
			}
		}

		/// <summary>
		/// Initializes a new instance of the class with the specified number format
		/// and the amount of fractional digits to use
		/// </summary>
		/// <param name="theType">Number format</param>
		/// <param name="digits">Number of fractional digits to use</param>
		private TextNumberFormat(TextNumberFormat.formatTypes theType, int digits)
		{
			this.numberFormat      = System.Globalization.NumberFormatInfo.CurrentInfo;
			this.numberFormatType  = (int)theType;
			this.groupingActivated = true;
			this.separator = this.GetSeparator( (int)theType );
			this.maxIntDigits = 127;
			this.minIntDigits = 1;
			this.maxFractionDigits = 3;
			this.minFractionDigits = 0;
		}

		/// <summary>
		/// Initializes a new instance of the class with the specified number format,
		/// uses the system's culture information,
		/// and assigns the amount of fractional digits to use
		/// </summary>
		/// <param name="theType">Number format</param>
		/// <param name="cultureNumberFormat">Represents information about a specific culture including the number formatting</param>
		/// <param name="digits">Number of fractional digits to use</param>
		private TextNumberFormat(TextNumberFormat.formatTypes theType, System.Globalization.CultureInfo cultureNumberFormat, int digits)
		{
			this.numberFormat      = cultureNumberFormat.NumberFormat;
			this.numberFormatType  = (int)theType;
			this.groupingActivated = true;
			this.separator = this.GetSeparator( (int)theType );
			this.maxIntDigits = 127;
			this.minIntDigits = 1;
			this.maxFractionDigits = 3;
			this.minFractionDigits = 0;
		}

		/// <summary>
		/// Returns an initialized instance of the TextNumberFormat object
		/// using number representation.
		/// </summary>
		/// <returns>The object instance</returns>
		public static TextNumberFormat getTextNumberInstance()
		{
			TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Number, 3);
			return instance;
		}

		/// <summary>
		/// Returns an initialized instance of the TextNumberFormat object
		/// using currency representation.
		/// </summary>
		/// <returns>The object instance</returns>
		public static TextNumberFormat getTextNumberCurrencyInstance()
		{
			TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Currency, 3);
			return instance.setToCurrencyNumberFormatDefaults(instance);
		}

		/// <summary>
		/// Returns an initialized instance of the TextNumberFormat object
		/// using percent representation.
		/// </summary>
		/// <returns>The object instance</returns>
		public static TextNumberFormat getTextNumberPercentInstance()
		{
			TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Percent, 3);
			return instance.setToPercentNumberFormatDefaults(instance);
		}

		/// <summary>
		/// Returns an initialized instance of the TextNumberFormat object
		/// using number representation, it uses the culture format information provided.
		/// </summary>
		/// <param name="culture">Represents information about a specific culture</param>
		/// <returns>The object instance</returns>
		public static TextNumberFormat getTextNumberInstance(System.Globalization.CultureInfo culture)
		{
			TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Number, culture, 3);
			return instance;
		}

		/// <summary>
		/// Returns an initialized instance of the TextNumberFormat object
		/// using currency representation, it uses the culture format information provided.
		/// </summary>
		/// <param name="culture">Represents information about a specific culture</param>
		/// <returns>The object instance</returns>
		public static TextNumberFormat getTextNumberCurrencyInstance(System.Globalization.CultureInfo culture)
		{
			TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Currency, culture, 3);
			return instance.setToCurrencyNumberFormatDefaults(instance);
		}

		/// <summary>
		/// Returns an initialized instance of the TextNumberFormat object
		/// using percent representation, it uses the culture format information provided.
		/// </summary>
		/// <param name="culture">Represents information about a specific culture</param>
		/// <returns>The object instance</returns>
		public static TextNumberFormat getTextNumberPercentInstance(System.Globalization.CultureInfo culture)
		{
			TextNumberFormat instance = new TextNumberFormat(TextNumberFormat.formatTypes.Percent, culture, 3);
            return instance.setToPercentNumberFormatDefaults(instance);
		}

		/// <summary>
		/// Clones the object instance
		/// </summary>
		/// <returns>The cloned object instance</returns>
		public System.Object Clone()
		{
			return (System.Object)this;
		}

		/// <summary>
		/// Determines if the received object is equal to the
		/// current object instance
		/// </summary>
		/// <param name="textNumberObject">TextNumber instance to compare</param>
		/// <returns>True or false depending if the two instances are equal</returns>
		public override bool Equals(Object obj) 
		{
			// Check for null values and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) 
				return false;
			SupportClass.TextNumberFormat param = (SupportClass.TextNumberFormat)obj;
			return (numberFormat == param.numberFormat) && (numberFormatType == param.numberFormatType) 
				&& (groupingActivated == param.groupingActivated) && (separator == param.separator) 
				&& (maxIntDigits == param.maxIntDigits)	&& (minIntDigits == param.minIntDigits) 
				&& (maxFractionDigits == param.maxFractionDigits) && (minFractionDigits == param.minFractionDigits);
		}

		
		/// <summary>
		/// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>A hash code for the current Object</returns>
		public override int GetHashCode()
		{
			return numberFormat.GetHashCode() ^ numberFormatType ^ groupingActivated.GetHashCode() 
				 ^ separator.GetHashCode() ^ maxIntDigits ^ minIntDigits ^ maxFractionDigits ^ minFractionDigits;
		}

		/// <summary>
		/// Formats a number with the current formatting parameters
		/// </summary>
		/// <param name="number">Source number to format</param>
		/// <returns>The formatted number string</returns>
		public System.String FormatDouble(double number)
		{
			if (this.groupingActivated)
			{
				return SetIntDigits(number.ToString(this.GetCurrentFormatString() + this.GetNumberOfDigits( number ), this.numberFormat));
			}
			else
			{
				return SetIntDigits((number.ToString(this.GetCurrentFormatString() + this.GetNumberOfDigits( number ) , this.numberFormat)).Replace(this.separator,""));
			}
		}
		
		/// <summary>
		/// Formats a number with the current formatting parameters
		/// </summary>
		/// <param name="number">Source number to format</param>
		/// <returns>The formatted number string</returns>
		public System.String FormatLong(long number)
		{			
			if (this.groupingActivated)
			{
				return SetIntDigits(number.ToString(this.GetCurrentFormatString() + this.minFractionDigits , this.numberFormat));
			}
			else
			{
				return SetIntDigits((number.ToString(this.GetCurrentFormatString() + this.minFractionDigits , this.numberFormat)).Replace(this.separator,""));
			}
		}
		
		
		/// <summary>
		/// Formats the number according to the specified number of integer digits 
		/// </summary>
		/// <param name="number">The number to format</param>
		/// <returns></returns>
		private System.String SetIntDigits(String number)
		{			
			String decimals = "";
			String fraction = "";
			int i = number.IndexOf(this.numberFormat.NumberDecimalSeparator);
			if (i > 0)
			{
				fraction = number.Substring(i);
				decimals = number.Substring(0,i).Replace(this.numberFormat.NumberGroupSeparator,"");
			}
			else decimals = number.Replace(this.numberFormat.NumberGroupSeparator,"");
			decimals = decimals.PadLeft(this.MinIntDigits,'0');
			if ((i = decimals.Length - this.MaxIntDigits) > 0) decimals = decimals.Remove(0,i);
			if (this.groupingActivated) 
			{
				for (i = decimals.Length;i > 3;i -= 3)
				{
					decimals = decimals.Insert(i - 3,this.numberFormat.NumberGroupSeparator);
				}
			}
			decimals = decimals + fraction;
			if (decimals.Length == 0) return "0";
			else return decimals;
		}

		/// <summary>
		/// Gets the list of all supported cultures
		/// </summary>
		/// <returns>An array of type CultureInfo that represents the supported cultures</returns>
		public static System.Globalization.CultureInfo[] GetAvailableCultures()
		{
			return System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.AllCultures);
		}

		/// <summary>
		/// Obtains the current format representation used
		/// </summary>
		/// <returns>A character representing the string format used</returns>
		private System.String GetCurrentFormatString()
		{
			System.String currentFormatString = "n";  //Default value
			switch (this.numberFormatType)
			{
				case (int)TextNumberFormat.formatTypes.Currency:
					currentFormatString = "c";
					break;

				case (int)TextNumberFormat.formatTypes.General:
					currentFormatString = "n";
					break;

				case (int)TextNumberFormat.formatTypes.Number:
					currentFormatString = "n";
					break;

				case (int)TextNumberFormat.formatTypes.Percent:
					currentFormatString = "p";
					break;
			}
			return currentFormatString;
		}

		/// <summary>
		/// Retrieves the separator used, depending on the format type specified
		/// </summary>
		/// <param name="numberFormatType">formatType enumarator value to inquire</param>
		/// <returns>The values of character separator used </returns>
		private System.String GetSeparator(int numberFormatType)
		{
			System.String separatorItem = " ";  //Default Separator

			switch (numberFormatType)
			{
				case (int)TextNumberFormat.formatTypes.Currency:
					separatorItem = this.numberFormat.CurrencyGroupSeparator;
					break;

				case (int)TextNumberFormat.formatTypes.General:
					separatorItem = this.numberFormat.NumberGroupSeparator;
					break;

				case (int)TextNumberFormat.formatTypes.Number:
					separatorItem = this.numberFormat.NumberGroupSeparator;
					break;

				case (int)TextNumberFormat.formatTypes.Percent:
					separatorItem = this.numberFormat.PercentGroupSeparator;
					break;
			}
			return separatorItem;
		}

		/// <summary>
		/// Boolean value stating if grouping is used or not
		/// </summary>
		public bool GroupingUsed
		{
			get
			{
				return (this.groupingActivated);
			}
			set
			{
				this.groupingActivated = value;
			}
		}

		/// <summary>
		/// Minimum number of integer digits to use in the number format
		/// </summary>
		public int MinIntDigits
		{
			get
			{
				return this.minIntDigits;
			}
			set
			{
				this.minIntDigits = value;
			}
		}

		/// <summary>
		/// Maximum number of integer digits to use in the number format
		/// </summary>
		public int MaxIntDigits
		{
			get
			{
				return this.maxIntDigits;
			}
			set
			{
				this.maxIntDigits = value;
			}
		}

		/// <summary>
		/// Minimum number of fraction digits to use in the number format
		/// </summary>
		public int MinFractionDigits
		{
			get
			{
				return this.minFractionDigits;
			}
			set
			{
				this.minFractionDigits = value;
			}
		}

		/// <summary>
		/// Maximum number of fraction digits to use in the number format
		/// </summary>
		public int MaxFractionDigits
		{
			get
			{
				return this.maxFractionDigits;
			}
			set
			{
				this.maxFractionDigits = value;
			}
		}

		/// <summary>
		/// Sets the values of minFractionDigits and maxFractionDigits to the currency standard
		/// </summary>
		/// <param name="format">The TextNumberFormat instance to set</param>
		/// <returns>The TextNumberFormat with corresponding the default values</returns>
		private TextNumberFormat setToCurrencyNumberFormatDefaults( TextNumberFormat format )
		{
			format.maxFractionDigits = 2;
			format.minFractionDigits = 2;
			return format;
		}

		/// <summary>
		/// Sets the values of minFractionDigits and maxFractionDigits to the percent standard
		/// </summary>
		/// <param name="format">The TextNumberFormat instance to set</param>
		/// <returns>The TextNumberFormat with corresponding the default values</returns>
		private TextNumberFormat setToPercentNumberFormatDefaults( TextNumberFormat format )
		{
			format.maxFractionDigits = 0;
			format.minFractionDigits = 0;
			return format;
		}

		/// <summary>
		/// Gets the number of fraction digits thats must be used by the format methods
		/// </summary>
		/// <param name="number">The double number</param>
		/// <returns>The number of fraction digits to use</returns>
		private int GetNumberOfDigits( Double number )
		{
			int counter = 0;
			double temp = System.Math.Abs(number);
			while ( (temp % 1) > 0 )
			{
				temp *= 10;
				counter++;
			}
			return (counter < this.minFractionDigits) ? this.minFractionDigits : (( counter < this.maxFractionDigits ) ? counter : this.maxFractionDigits); 
		}
	}
	/*******************************/
	/// <summary>
	/// Give functions to obtain information of graphic elements
	/// </summary>
	public class GraphicsManager
	{
		//Instance of GDI+ drawing surfaces graphics hashtable
		static public GraphicsHashTable manager = new GraphicsHashTable();

		/// <summary>
		/// Creates a new Graphics object from the device context handle associated with the Graphics
		/// parameter
		/// </summary>
		/// <param name="oldGraphics">Graphics instance to obtain the parameter from</param>
		/// <returns>A new GDI+ drawing surface</returns>
		public static System.Drawing.Graphics CreateGraphics(System.Drawing.Graphics oldGraphics)
		{
			System.Drawing.Graphics createdGraphics;
			System.IntPtr hdc = oldGraphics.GetHdc();
			createdGraphics = System.Drawing.Graphics.FromHdc(hdc);
			oldGraphics.ReleaseHdc(hdc);
			return createdGraphics;
		}

		/// <summary>
		/// This method draws a Bezier curve.
		/// </summary>
		/// <param name="graphics">It receives the Graphics instance</param>
		/// <param name="array">An array of (x,y) pairs of coordinates used to draw the curve.</param>
		public static void Bezier(System.Drawing.Graphics graphics, int[] array)
		{
			System.Drawing.Pen pen;
			pen = GraphicsManager.manager.GetPen(graphics);
			try
			{
				graphics.DrawBezier(pen, array[0], array[1], array[2], array[3], array[4], array[5], array[6], array[7]);
			}
			catch(System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException(e.ToString());
			}
		}

		/// <summary>
		/// Gets the text size width and height from a given GDI+ drawing surface and a given font
		/// </summary>
		/// <param name="graphics">Drawing surface to use</param>
		/// <param name="graphicsFont">Font type to measure</param>
		/// <param name="text">String of text to measure</param>
		/// <returns>A point structure with both size dimentions; x for width and y for height</returns>
		public static System.Drawing.Point GetTextSize(System.Drawing.Graphics graphics, System.Drawing.Font graphicsFont, System.String text)
		{
			System.Drawing.Point textSize;
			System.Drawing.SizeF tempSizeF;
			tempSizeF = graphics.MeasureString(text, graphicsFont);
			textSize = new System.Drawing.Point();
			textSize.X = (int) tempSizeF.Width;
			textSize.Y = (int) tempSizeF.Height;
			return textSize;
		}

		/// <summary>
		/// Gets the text size width and height from a given GDI+ drawing surface and a given font
		/// </summary>
		/// <param name="graphics">Drawing surface to use</param>
		/// <param name="graphicsFont">Font type to measure</param>
		/// <param name="text">String of text to measure</param>
		/// <param name="width">Maximum width of the string</param>
		/// <param name="format">StringFormat object that represents formatting information, such as line spacing, for the string</param>
		/// <returns>A point structure with both size dimentions; x for width and y for height</returns>
		public static System.Drawing.Point GetTextSize(System.Drawing.Graphics graphics, System.Drawing.Font graphicsFont, System.String text, System.Int32 width, System.Drawing.StringFormat format)
		{
			System.Drawing.Point textSize;
			System.Drawing.SizeF tempSizeF;
			tempSizeF = graphics.MeasureString(text, graphicsFont, width, format);
			textSize = new System.Drawing.Point();
			textSize.X = (int) tempSizeF.Width;
			textSize.Y = (int) tempSizeF.Height;
			return textSize;
		}

		/// <summary>
		/// Gives functionality over a hashtable of GDI+ drawing surfaces
		/// </summary>
		public class GraphicsHashTable:System.Collections.Hashtable 
		{
			/// <summary>
			/// Gets the graphics object from the given control
			/// </summary>
			/// <param name="control">Control to obtain the graphics from</param>
			/// <returns>A graphics object with the control's characteristics</returns>
			public System.Drawing.Graphics GetGraphics(System.Windows.Forms.Control control)
			{
				System.Drawing.Graphics graphic;
				if (control.Visible == true)
				{
					graphic = control.CreateGraphics();
					SetColor(graphic, control.ForeColor);
					SetFont(graphic, control.Font);
				}
				else
				{
					graphic = null;
				}
				return graphic;
			}

			/// <summary>
			/// Sets the background color property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given background color.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="color">Background color to set</param>
			public void SetBackColor(System.Drawing.Graphics graphic, System.Drawing.Color color)
			{
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).BackColor = color;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.BackColor = color;
					Add(graphic, tempProps);
				}
			}

			/// <summary>
			/// Gets the background color property to the given graphics object in the hashtable. If the element doesn't exist, then it returns White.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The background color of the graphic</returns>
			public System.Drawing.Color GetBackColor(System.Drawing.Graphics graphic)
			{
				if (this[graphic] == null)
					return System.Drawing.Color.White;
				else
					return ((GraphicsProperties) this[graphic]).BackColor;
			}

			/// <summary>
			/// Sets the text color property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given text color.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="color">Text color to set</param>
			public void SetTextColor(System.Drawing.Graphics graphic, System.Drawing.Color color)
			{
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).TextColor = color;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.TextColor = color;
					Add(graphic, tempProps);
				}
			}

			/// <summary>
			/// Gets the text color property to the given graphics object in the hashtable. If the element doesn't exist, then it returns White.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The text color of the graphic</returns>
			public System.Drawing.Color GetTextColor(System.Drawing.Graphics graphic) 
			{
				if (this[graphic] == null)
					return System.Drawing.Color.White;
				else
					return ((GraphicsProperties) this[graphic]).TextColor;
			}

			/// <summary>
			/// Sets the GraphicBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given GraphicBrush.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="brush">GraphicBrush to set</param>
			public void SetBrush(System.Drawing.Graphics graphic, System.Drawing.SolidBrush brush) 
			{
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).GraphicBrush = brush;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.GraphicBrush = brush;
					Add(graphic, tempProps);
				}
			}
			
			/// <summary>
			/// Sets the GraphicBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given GraphicBrush.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="brush">GraphicBrush to set</param>
			public void SetPaint(System.Drawing.Graphics graphic, System.Drawing.Brush brush) 
			{
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).PaintBrush = brush;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.PaintBrush = brush;
					Add(graphic, tempProps);
				}
			}
			
			/// <summary>
			/// Sets the GraphicBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given GraphicBrush.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="color">Color to set</param>
			public void SetPaint(System.Drawing.Graphics graphic, System.Drawing.Color color) 
			{
				System.Drawing.Brush brush = new System.Drawing.SolidBrush(color);
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).PaintBrush = brush;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.PaintBrush = brush;
					Add(graphic, tempProps);
				}
			}


			/// <summary>
			/// Gets the HatchBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Blank.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The HatchBrush setting of the graphic</returns>
			public System.Drawing.Drawing2D.HatchBrush GetBrush(System.Drawing.Graphics graphic)
			{
				if (this[graphic] == null)
					return new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Plaid,System.Drawing.Color.Black,System.Drawing.Color.Black);
				else
					return new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Plaid,((GraphicsProperties) this[graphic]).GraphicBrush.Color,((GraphicsProperties) this[graphic]).GraphicBrush.Color);
			}
			
			/// <summary>
			/// Gets the HatchBrush property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Blank.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The Brush setting of the graphic</returns>
			public System.Drawing.Brush GetPaint(System.Drawing.Graphics graphic)
			{
                if (this[graphic] == null)
                    return new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Plaid, System.Drawing.Color.Black, System.Drawing.Color.Black);
                else
                {
                    return ((GraphicsProperties)this[graphic]).PaintBrush;
                }
			}

			/// <summary>
			/// Sets the GraphicPen property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given Pen.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="pen">Pen to set</param>
			public void SetPen(System.Drawing.Graphics graphic, System.Drawing.Pen pen) 
			{
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).GraphicPen = pen;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.GraphicPen = pen;
					Add(graphic, tempProps);
				}
			}

			/// <summary>
			/// Gets the GraphicPen property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Black.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The GraphicPen setting of the graphic</returns>
			public System.Drawing.Pen GetPen(System.Drawing.Graphics graphic)
			{
				if (this[graphic] == null)
					return System.Drawing.Pens.Black;
				else
					return ((GraphicsProperties) this[graphic]).GraphicPen;
			}

			/// <summary>
			/// Sets the GraphicFont property to the given graphics object in the hashtable. If the element doesn't exist, then it adds the graphic element to the hashtable with the given Font.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="Font">Font to set</param>
			public void SetFont(System.Drawing.Graphics graphic, System.Drawing.Font font) 
			{
				if (this[graphic] != null)
					((GraphicsProperties) this[graphic]).GraphicFont = font;
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.GraphicFont = font;
					Add(graphic,tempProps);
				}
			}

			/// <summary>
			/// Gets the GraphicFont property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Microsoft Sans Serif with size 8.25.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The GraphicFont setting of the graphic</returns>
			public System.Drawing.Font GetFont(System.Drawing.Graphics graphic)
			{
				if (this[graphic] == null)
					return new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				else
					return ((GraphicsProperties) this[graphic]).GraphicFont;
			}

			/// <summary>
			/// Sets the color properties for a given Graphics object. If the element doesn't exist, then it adds the graphic element to the hashtable with the color properties set with the given value.
			/// </summary>
			/// <param name="graphic">Graphic element to search or add</param>
			/// <param name="color">Color value to set</param>
			public void SetColor(System.Drawing.Graphics graphic, System.Drawing.Color color) 
			{
				if (this[graphic] != null)
				{
					((GraphicsProperties) this[graphic]).GraphicPen.Color = color;
					((GraphicsProperties) this[graphic]).GraphicBrush.Color = color;
					((GraphicsProperties) this[graphic]).color = color;
				}
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.GraphicPen.Color = color;
					tempProps.GraphicBrush.Color = color;
					tempProps.color = color;
					Add(graphic,tempProps);
				}
			}

			/// <summary>
			/// Gets the color property to the given graphics object in the hashtable. If the element doesn't exist, then it returns Black.
			/// </summary>
			/// <param name="graphic">Graphic element to search</param>
			/// <returns>The color setting of the graphic</returns>
			public System.Drawing.Color GetColor(System.Drawing.Graphics graphic) 
			{
				if (this[graphic] == null)
					return System.Drawing.Color.Black;
				else
					return ((GraphicsProperties) this[graphic]).color;
			}

			/// <summary>
			/// This method gets the TextBackgroundColor of a Graphics instance
			/// </summary>
			/// <param name="graphic">The graphics instance</param>
			/// <returns>The color value in ARGB encoding</returns>
			public System.Drawing.Color GetTextBackgroundColor(System.Drawing.Graphics graphic)
			{
				if (this[graphic] == null)
					return System.Drawing.Color.Black;
				else 
				{ 
					return ((GraphicsProperties) this[graphic]).TextBackgroundColor;
				}
			}

			/// <summary>
			/// This method set the TextBackgroundColor of a Graphics instace
			/// </summary>
			/// <param name="graphic">The graphics instace</param>
			/// <param name="color">The System.Color to set the TextBackgroundColor</param>
			public void SetTextBackgroundColor(System.Drawing.Graphics graphic, System.Drawing.Color color) 
			{
				if (this[graphic] != null)
				{
					((GraphicsProperties) this[graphic]).TextBackgroundColor = color;								
				}
				else
				{
					GraphicsProperties tempProps = new GraphicsProperties();
					tempProps.TextBackgroundColor = color;				
					Add(graphic,tempProps);
				}
			}

			/// <summary>
			/// Structure to store properties from System.Drawing.Graphics objects
			/// </summary>
			class GraphicsProperties
			{
				public System.Drawing.Color TextBackgroundColor = System.Drawing.Color.Black;
				public System.Drawing.Color color = System.Drawing.Color.Black;
				public System.Drawing.Color BackColor = System.Drawing.Color.White;
				public System.Drawing.Color TextColor = System.Drawing.Color.Black;
				public System.Drawing.SolidBrush GraphicBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
				// public System.Drawing.Brush PaintBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
				public System.Drawing.Pen   GraphicPen = new System.Drawing.Pen(System.Drawing.Color.Black);
				public System.Drawing.Font  GraphicFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));

                private Brush _brush;
                public Brush PaintBrush
                {
                    get {
                        if (_brush == null)
                        {
                            _brush = new SolidBrush(color); 
                        }

                        return _brush; 
                    }
                    set
                    {
                        _brush = value; 
                    }
                }
	
			}
		}
	}

	/*******************************/
	/// <summary>
	/// Calculates the ascent of the font, using the GetCellAscent and GetEmHeight methods
	/// </summary>
	/// <param name="font">The Font instance used to obtain the Ascent</param>
	/// <returns>The ascent of the font</returns>
	public static int GetAscent(System.Drawing.Font font)
	{		
		System.Drawing.FontFamily fontFamily = font.FontFamily;
		int ascent = fontFamily.GetCellAscent(font.Style);
		int ascentPixel = (int)font.Size * ascent / fontFamily.GetEmHeight(font.Style);
		return ascentPixel;
	}

	/*******************************/
	/// <summary>
	/// Calculates the descent of the font, using the GetCellDescent and GetEmHeight
	/// </summary>
	/// <param name="font">The Font instance used to obtain the Descent</param>
	/// <returns>The Descent of the font </returns>
	public static int GetDescent(System.Drawing.Font font)
	{		
		System.Drawing.FontFamily fontFamily = font.FontFamily;
		int descent = fontFamily.GetCellDescent(font.Style);
		int descentPixel = (int) font.Size * descent / fontFamily.GetEmHeight(font.Style);
		return descentPixel;
	}

	/*******************************/
	/// <summary>
	/// This class contains support methods to work with GraphicsPath and Ellipses.
	/// </summary>
	public class Ellipse2DSupport
	{
		/// <summary>
		/// Creates a object and adds an ellipse to it.
		/// </summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
		/// <param name="width">The width of the bounding rectangle that defines the ellipse.</param>
		/// <param name="height">The height of the bounding rectangle that defines the ellipse.</param>
		/// <returns>Returns a GraphicsPath object containing an ellipse.</returns>
		public static System.Drawing.Drawing2D.GraphicsPath CreateEllipsePath(float x, float y, float width, float height)
		{
			System.Drawing.Drawing2D.GraphicsPath ellipsePath = new System.Drawing.Drawing2D.GraphicsPath();
			ellipsePath.AddEllipse(x, y, width, height);
			return ellipsePath;
		}

		/// <summary>
		/// Resets the x-coordinate of the ellipse path contained in the specified GraphicsPath object.
		/// </summary>
		/// <param name="ellipsePath">The GraphicsPath object that will be set.</param>
		/// <param name="x">The new x-coordinate.</param>
		public static void SetX(System.Drawing.Drawing2D.GraphicsPath ellipsePath, float x)
		{
			System.Drawing.RectangleF rectangle = ellipsePath.GetBounds();
			rectangle.X = x;
			ellipsePath.Reset();
			ellipsePath.AddEllipse(rectangle);
		}

		/// <summary>
		/// Resets the y-coordinate of the ellipse path contained in the specified GraphicsPath object.
		/// </summary>
		/// <param name="ellipsePath">The GraphicsPath object that will be set.</param>
		/// <param name="y">The new y-coordinate.</param>
		public static void SetY(System.Drawing.Drawing2D.GraphicsPath ellipsePath, float y)
		{
			System.Drawing.RectangleF rectangle = ellipsePath.GetBounds();
			rectangle.Y = y;
			ellipsePath.Reset();
			ellipsePath.AddEllipse(rectangle);
		}

		/// <summary>
		/// Resets the width of the ellipse path contained in the specified GraphicsPath object.
		/// </summary>
		/// <param name="ellipsePath">The GraphicsPath object that will be set.</param>
		/// <param name="width">The new width.</param>
		public static void SetWidth(System.Drawing.Drawing2D.GraphicsPath ellipsePath, float width)
		{
			System.Drawing.RectangleF rectangle = ellipsePath.GetBounds();
			rectangle.Width = width;
			ellipsePath.Reset();
			ellipsePath.AddEllipse(rectangle);
		}

		/// <summary>
		/// Resets the height of the ellipse path contained in the specified GraphicsPath object.
		/// </summary>
		/// <param name="ellipsePath">The GraphicsPath object that will be set.</param>
		/// <param name="height">The new height.</param>
		public static void SetHeight(System.Drawing.Drawing2D.GraphicsPath ellipsePath, float height)
		{
			System.Drawing.RectangleF rectangle = ellipsePath.GetBounds();
			rectangle.Height = height;
			ellipsePath.Reset();
			ellipsePath.AddEllipse(rectangle);
		}
	}


	/*******************************/
	/// <summary>
	/// Adds the X and Y coordinates to the current graphics path.
	/// </summary>
	/// <param name="graphPath"> The current Graphics path</param>
	/// <param name="xCoordinate">The x coordinate to be added</param>
	/// <param name="yCoordinate">The y coordinate to be added</param>
	public static void AddPointToGraphicsPath(System.Drawing.Drawing2D.GraphicsPath graphPath, int x, int y)
	{
		System.Drawing.PointF[] tempPointArray = new System.Drawing.PointF[graphPath.PointCount + 1];
		byte[] tempPointTypeArray = new byte[graphPath.PointCount + 1];

		if (graphPath.PointCount == 0)
		{
			tempPointArray[0] = new System.Drawing.PointF(x, y);		
			System.Drawing.Drawing2D.GraphicsPath tempGraphicsPath = new System.Drawing.Drawing2D.GraphicsPath(tempPointArray, new byte[]{(byte)System.Drawing.Drawing2D.PathPointType.Start});
			graphPath.AddPath(tempGraphicsPath, false);
		}
		else
		{
			graphPath.PathPoints.CopyTo(tempPointArray, 0);
			tempPointArray[graphPath.PointCount] = new System.Drawing.Point(x, y);
			
			graphPath.PathTypes.CopyTo(tempPointTypeArray, 0);
			tempPointTypeArray[graphPath.PointCount] = (byte) System.Drawing.Drawing2D.PathPointType.Line;

			System.Drawing.Drawing2D.GraphicsPath tempGraphics = new System.Drawing.Drawing2D.GraphicsPath(tempPointArray, tempPointTypeArray);
			graphPath.Reset();
			graphPath.AddPath(tempGraphics, false);
			graphPath.CloseFigure();
		}
	}
	/*******************************/
	/// <summary>
	/// This class contains support methods to work with GraphicsPath and Lines.
	/// </summary>
	public class Line2DSupport
	{
		/// <summary>
		/// Creates a GraphicsPath object and adds a line to it.
		/// </summary>
		/// <param name="x1">The x-coordinate of the starting point of the line.</param>
		/// <param name="y1">The y-coordinate of the starting point of the line.</param>
		/// <param name="x2">The x-coordinate of the endpoint of the line.</param>
		/// <param name="y2">The y-coordinate of the endpoint of the line.</param>
		/// <returns>Returns a GraphicsPath object containing the line.</returns>
		public static System.Drawing.Drawing2D.GraphicsPath CreateLine2DPath(float x1, float y1, float x2, float y2)
		{
			System.Drawing.Drawing2D.GraphicsPath linePath = new System.Drawing.Drawing2D.GraphicsPath();
			linePath.AddLine(x1, y1, x2, y2);
			return linePath;
		}

		/// <summary>
		/// Creates a GraphicsPath object and adds a line to it.
		/// </summary>
		/// <param name="p1">The starting point of the line.</param>
		/// <param name="p2">The endpoint of the line.</param>
		/// <returns>Returns a GraphicsPath object containing the line</returns>
		public static System.Drawing.Drawing2D.GraphicsPath CreateLine2DPath(System.Drawing.PointF p1, System.Drawing.PointF p2)
		{
			System.Drawing.Drawing2D.GraphicsPath linePath = new System.Drawing.Drawing2D.GraphicsPath();
			linePath.AddLine(p1, p2);
			return linePath;
		}

		/// <summary>
		/// Resets the specified GraphicsPath object an adds a line to it with the specified values.
		/// </summary>
		/// <param name="linePath">The GraphicsPath object to reset.</param>
		/// <param name="x1">The x-coordinate of the starting point of the line.</param>
		/// <param name="y1">The y-coordinate of the starting point of the line.</param>
		/// <param name="x2">The x-coordinate of the endpoint of the line.</param>
		/// <param name="y2">The y-coordinate of the endpoint of the line.</param>
		public static void SetLine(System.Drawing.Drawing2D.GraphicsPath linePath, float x1, float y1, float x2, float y2)
		{
			linePath.Reset();
			linePath.AddLine(x1, y1, x2, y2);
		}

		/// <summary>
		/// Resets the specified GraphicsPath object an adds a line to it with the specified values.
		/// </summary>
		/// <param name="linePath">The GraphicsPath object to reset.</param>
		/// <param name="p1">The starting point of the line.</param>
		/// <param name="p2">The endpoint of the line.</param>
		public static void SetLine(System.Drawing.Drawing2D.GraphicsPath linePath, System.Drawing.PointF p1, System.Drawing.PointF p2)
		{
			linePath.Reset();
			linePath.AddLine(p1, p2);
		}

		/// <summary>
		/// Resets the specified GraphicsPath object an adds a line to it.
		/// </summary>
		/// <param name="linePath">The GraphicsPath object to reset.</param>
		/// <param name="newLinePath">The line to add.</param>
		public static void SetLine(System.Drawing.Drawing2D.GraphicsPath linePath, System.Drawing.Drawing2D.GraphicsPath newLinePath)
		{
			linePath.Reset();
			linePath.AddPath(newLinePath, false);
		}
	}


	/*******************************/
	/// <summary>
	/// Provides overloaded methods to create and set values to an instance of System.Drawing.Pen.
	/// </summary>
	public class StrokeConsSupport
	{
		/// <summary>
		/// Creates an instance of System.Drawing.Pen with the default SolidBrush black.
		/// And then set the parameters into their corresponding properties.
		/// </summary>
		/// <param name="width">The width of the stroked line.</param>
		/// <param name="cap">The DashCap end of line style.</param>
		/// <param name="join">The LineJoin style.</param>
		/// <returns>A new instance with the values set.</returns>
		public static System.Drawing.Pen CreatePenInstance(float width, int cap, int join)
		{
			System.Drawing.Pen tempPen = new System.Drawing.Pen(System.Drawing.Brushes.Black,width);
			tempPen.StartCap = (System.Drawing.Drawing2D.LineCap)  cap;
			tempPen.EndCap = (System.Drawing.Drawing2D.LineCap) cap;
			tempPen.LineJoin = (System.Drawing.Drawing2D.LineJoin)join;
			return tempPen;
		}

		/// <summary>
		/// Creates an instance of System.Drawing.Pen with the default SolidBrush black.
		/// And then set the parameters into their corresponding properties.
		/// </summary>
		/// <param name="width">The width of the stroked line.</param>
		/// <param name="cap">The DashCap end of line style.</param>
		/// <param name="join">The LineJoin style.</param>
		/// <param name="miterlimit">The limit of the line.</param>
		/// <returns>A new instance with the values set.</returns>
		public static System.Drawing.Pen CreatePenInstance(float width, int cap, int join, float miterlimit)
		{
			System.Drawing.Pen tempPen = new System.Drawing.Pen(System.Drawing.Brushes.Black,width);
			tempPen.StartCap = (System.Drawing.Drawing2D.LineCap)  cap;
			tempPen.EndCap = (System.Drawing.Drawing2D.LineCap) cap;
			tempPen.LineJoin = (System.Drawing.Drawing2D.LineJoin)join;
			tempPen.MiterLimit = miterlimit;
			return tempPen;
		}

		/// <summary>
		/// Creates an instance of System.Drawing.Pen with the default SolidBrush black.
		/// And then set the parameters into their corresponding properties.
		/// </summary>
		/// <param name="width">The width of the stroked line.</param>
		/// <param name="cap">The DashCap end of line style.</param>
		/// <param name="join">The LineJoin style.</param>
		/// <param name="miterlimit">The limit of the line.</param>
		/// <param name="dashPattern">The array to use to make the dash.</param>
		/// <param name="dashOffset">The space between each dash.</param>
		/// <returns>A new instance with the values set.</returns>
		public static System.Drawing.Pen CreatePenInstance(float width, int cap, int join, float miterlimit,float[] dashPattern, float dashOffset)
		{
			System.Drawing.Pen tempPen = new System.Drawing.Pen(System.Drawing.Brushes.Black,width);
			tempPen.StartCap = (System.Drawing.Drawing2D.LineCap)  cap;
			tempPen.EndCap = (System.Drawing.Drawing2D.LineCap) cap;
			tempPen.LineJoin = (System.Drawing.Drawing2D.LineJoin)join;
			tempPen.MiterLimit = miterlimit;
			tempPen.DashPattern = dashPattern;
			tempPen.DashOffset = dashOffset;
			return tempPen;
		}

		/// <summary>
		/// Sets a Pen instance with the corresponding DashCap and LineJoin values.
		/// </summary>
		/// <param name="cap">The DashCap end of line style.</param>
		/// <param name="join">The LineJoin style.</param>
		/// <returns>A new instance with the values set.</returns>
		public static void SetPen(System.Drawing.Pen tempPen, int cap, int join)
		{
			tempPen.StartCap = (System.Drawing.Drawing2D.LineCap)  cap;
			tempPen.EndCap = (System.Drawing.Drawing2D.LineCap) cap;
			tempPen.LineJoin = (System.Drawing.Drawing2D.LineJoin)join;
		}

		/// <summary>
		/// Sets a Pen instance with the corresponding DashCap, LineJoin and MiterLimit values.
		/// </summary>
		/// <param name="cap">The DashCap end of line style.</param>
		/// <param name="join">The LineJoin style.</param>
		/// <param name="miterlimit">The limit of the line.</param>
		public static void SetPen(System.Drawing.Pen tempPen, int cap, int join, float miterlimit)
		{
			tempPen.StartCap = (System.Drawing.Drawing2D.LineCap)  cap;
			tempPen.EndCap = (System.Drawing.Drawing2D.LineCap) cap;
			tempPen.LineJoin = (System.Drawing.Drawing2D.LineJoin)join;
			tempPen.MiterLimit = miterlimit;
		}

		/// <summary>
		/// Sets a Pen instance with the corresponding DashCap, LineJoin, MiterLimit, DashPattern and 
		/// DashOffset values.
		/// </summary>
		/// <param name="cap">The DashCap end of line style.</param>
		/// <param name="join">The LineJoin style.</param>
		/// <param name="miterlimit">The limit of the line.</param>
		/// <param name="dashPattern">The array to use to make the dash.</param>
		/// <param name="dashOffset">The space between each dash.</param>
		public static void SetPen(System.Drawing.Pen tempPen, float width, int cap, int join, float miterlimit, float[] dashPattern, float dashOffset)
		{
			tempPen.StartCap = (System.Drawing.Drawing2D.LineCap)  cap;
			tempPen.EndCap = (System.Drawing.Drawing2D.LineCap) cap;
			tempPen.LineJoin = (System.Drawing.Drawing2D.LineJoin)join;
			tempPen.MiterLimit = miterlimit;
			tempPen.DashPattern = dashPattern;
			tempPen.DashOffset = dashOffset;
		}
	}

	/*******************************/
	/// <summary>
	/// This class contains support methods to work with GraphicsPath and Arcs.
	/// </summary>
	public class Arc2DSupport
	{
		/// <summary>
		/// Specifies an OPEN arc type.
		/// </summary>
		public const int OPEN = 0;
		/// <summary>
		/// Specifies an CLOSED arc type.
		/// </summary>
		public const int CLOSED = 1;
		/// <summary>
		/// Specifies an PIE arc type.
		/// </summary>
		public const int PIE = 2;
		/// <summary>
		/// Creates a GraphicsPath object and adds an arc to it with the specified arc values and closure type.
		/// </summary>
		/// <param name="x">The x coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn.</param>
		/// <param name="y">The y coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn.</param>
		/// <param name="height">The height of the rectangular region that defines the ellipse from which the arc is drawn.</param>
		/// <param name="width">The width of the rectangular region that defines the ellipse from which the arc is drawn.</param>
		/// <param name="start">The starting angle of the arc measured in degrees.</param>
		/// <param name="extent">The angular extent of the arc measured in degrees.</param>
		/// <param name="arcType">The closure type for the arc.</param>
		/// <returns>Returns a new GraphicsPath object that contains the arc path.</returns>
		public static System.Drawing.Drawing2D.GraphicsPath CreateArc2D(float x, float y, float height, float width, float start, float extent, int arcType)
		{
			System.Drawing.Drawing2D.GraphicsPath arc2DPath = new System.Drawing.Drawing2D.GraphicsPath();
			switch (arcType)
			{
				case OPEN:
					arc2DPath.AddArc(x, y, height, width, start * -1, extent * -1);
					break;
				case CLOSED:
					arc2DPath.AddArc(x, y, height, width, start * -1, extent * -1);
					arc2DPath.CloseFigure();
					break;
				case PIE:
					arc2DPath.AddPie(x, y, height, width, start * -1, extent * -1);
					break;
				default:
					break;
			}
			return arc2DPath;
		}
		/// <summary>
		/// Creates a GraphicsPath object and adds an arc to it with the specified arc values and closure type.
		/// </summary>
		/// <param name="ellipseBounds">A RectangleF structure that represents the rectangular bounds of the ellipse from which the arc is taken.</param>
		/// <param name="start">The starting angle of the arc measured in degrees.</param>
		/// <param name="extent">The angular extent of the arc measured in degrees.</param>
		/// <param name="arcType">The closure type for the arc.</param>
		/// <returns>Returns a new GraphicsPath object that contains the arc path.</returns>
		public static System.Drawing.Drawing2D.GraphicsPath CreateArc2D(System.Drawing.RectangleF ellipseBounds, float start, float extent, int arcType)
		{
			System.Drawing.Drawing2D.GraphicsPath arc2DPath = new System.Drawing.Drawing2D.GraphicsPath();
			switch (arcType)
			{
				case OPEN:
					arc2DPath.AddArc(ellipseBounds, start * -1, extent * -1);
					break;
				case CLOSED:
					arc2DPath.AddArc(ellipseBounds, start * -1, extent * -1);
					arc2DPath.CloseFigure();
					break;
				case PIE:
					arc2DPath.AddPie(ellipseBounds.X, ellipseBounds.Y, ellipseBounds.Width, ellipseBounds.Height, start * -1, extent * -1);
					break;
				default:
					break;
			}

			return arc2DPath;
		}

		/// <summary>
		/// Resets the specified GraphicsPath object and adds an arc to it with the speficied values.
		/// </summary>
		/// <param name="arc2DPath">The GraphicsPath object to reset.</param>
		/// <param name="x">The x coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn.</param>
		/// <param name="y">The y coordinate of the upper-left corner of the rectangular region that defines the ellipse from which the arc is drawn.</param>
		/// <param name="height">The height of the rectangular region that defines the ellipse from which the arc is drawn.</param>
		/// <param name="width">The width of the rectangular region that defines the ellipse from which the arc is drawn.</param>
		/// <param name="start">The starting angle of the arc measured in degrees.</param>
		/// <param name="extent">The angular extent of the arc measured in degrees.</param>
		/// <param name="arcType">The closure type for the arc.</param>
		public static void SetArc(System.Drawing.Drawing2D.GraphicsPath arc2DPath, float x, float y, float height, float width, float start, float extent, int arcType)
		{
			arc2DPath.Reset();
			switch (arcType)
			{
				case OPEN:
					arc2DPath.AddArc(x, y, height, width, start * -1, extent * -1);
					break;
				case CLOSED:
					arc2DPath.AddArc(x, y, height, width, start * -1, extent * -1);
					arc2DPath.CloseFigure();
					break;
				case PIE:
					arc2DPath.AddPie(x, y, height, width, start * -1, extent * -1);
					break;
				default:
					break;
			}
		}
	}
	/*******************************/
	/// <summary>
	/// The class performs token processing in strings
	/// </summary>
	public class Tokenizer: System.Collections.IEnumerator
	{
		/// Position over the string
		private long currentPos = 0;

		/// Include demiliters in the results.
		private bool includeDelims = false;

		/// Char representation of the String to tokenize.
		private char[] chars = null;
			
		//The tokenizer uses the default delimiter set: the space character, the tab character, the newline character, and the carriage-return character and the form-feed character
		private string delimiters = " \t\n\r\f";		

		/// <summary>
		/// Initializes a new class instance with a specified string to process
		/// </summary>
		/// <param name="source">String to tokenize</param>
		public Tokenizer(System.String source)
		{			
			this.chars = source.ToCharArray();
		}

		/// <summary>
		/// Initializes a new class instance with a specified string to process
		/// and the specified token delimiters to use
		/// </summary>
		/// <param name="source">String to tokenize</param>
		/// <param name="delimiters">String containing the delimiters</param>
		public Tokenizer(System.String source, System.String delimiters):this(source)
		{			
			this.delimiters = delimiters;
		}


		/// <summary>
		/// Initializes a new class instance with a specified string to process, the specified token 
		/// delimiters to use, and whether the delimiters must be included in the results.
		/// </summary>
		/// <param name="source">String to tokenize</param>
		/// <param name="delimiters">String containing the delimiters</param>
		/// <param name="includeDelims">Determines if delimiters are included in the results.</param>
		public Tokenizer(System.String source, System.String delimiters, bool includeDelims):this(source,delimiters)
		{
			this.includeDelims = includeDelims;
		}	


		/// <summary>
		/// Returns the next token from the token list
		/// </summary>
		/// <returns>The string value of the token</returns>
		public System.String NextToken()
		{				
			return NextToken(this.delimiters);
		}

		/// <summary>
		/// Returns the next token from the source string, using the provided
		/// token delimiters
		/// </summary>
		/// <param name="delimiters">String containing the delimiters to use</param>
		/// <returns>The string value of the token</returns>
		public System.String NextToken(System.String delimiters)
		{
			//According to documentation, the usage of the received delimiters should be temporary (only for this call).
			//However, it seems it is not true, so the following line is necessary.
			this.delimiters = delimiters;

			//at the end 
			if (this.currentPos == this.chars.Length)
				throw new System.ArgumentOutOfRangeException();
			//if over a delimiter and delimiters must be returned
			else if (   (System.Array.IndexOf(delimiters.ToCharArray(),chars[this.currentPos]) != -1)
				     && this.includeDelims )                	
				return "" + this.chars[this.currentPos++];
			//need to get the token wo delimiters.
			else
				return nextToken(delimiters.ToCharArray());
		}

		//Returns the nextToken wo delimiters
		private System.String nextToken(char[] delimiters)
		{
			string token="";
			long pos = this.currentPos;

			//skip possible delimiters
			while (System.Array.IndexOf(delimiters,this.chars[currentPos]) != -1)
				//The last one is a delimiter (i.e there is no more tokens)
				if (++this.currentPos == this.chars.Length)
				{
					this.currentPos = pos;
					throw new System.ArgumentOutOfRangeException();
				}
			
			//getting the token
			while (System.Array.IndexOf(delimiters,this.chars[this.currentPos]) == -1)
			{
				token+=this.chars[this.currentPos];
				//the last one is not a delimiter
				if (++this.currentPos == this.chars.Length)
					break;
			}
			return token;
		}

				
		/// <summary>
		/// Determines if there are more tokens to return from the source string
		/// </summary>
		/// <returns>True or false, depending if there are more tokens</returns>
		public bool HasMoreTokens()
		{
			//keeping the current pos
			long pos = this.currentPos;
			
			try
			{
				this.NextToken();
			}
			catch (System.ArgumentOutOfRangeException)
			{				
				return false;
			}
			finally
			{
				this.currentPos = pos;
			}
			return true;
		}

		/// <summary>
		/// Remaining tokens count
		/// </summary>
		public int Count
		{
			get
			{
				//keeping the current pos
				long pos = this.currentPos;
				int i = 0;
			
				try
				{
					while (true)
					{
						this.NextToken();
						i++;
					}
				}
				catch (System.ArgumentOutOfRangeException)
				{				
					this.currentPos = pos;
					return i;
				}
			}
		}

		/// <summary>
		///  Performs the same action as NextToken.
		/// </summary>
		public System.Object Current
		{
			get
			{
				return (Object) this.NextToken();
			}		
		}		
		
		/// <summary>
		//  Performs the same action as HasMoreTokens.
		/// </summary>
		/// <returns>True or false, depending if there are more tokens</returns>
		public bool MoveNext()
		{
			return this.HasMoreTokens();
		}
		
		/// <summary>
		/// Does nothing.
		/// </summary>
		public void  Reset()
		{
			;
		}			
	}
	/*******************************/
	/// <summary>
	/// Support Methods for FileDialog class. Note that several methods receive a DirectoryInfo object, but it won't be used in all cases.
	/// </summary>
	public class FileDialogSupport
	{
		/// <summary>
		/// Creates an OpenFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the OpenFileDialog.</param>
		/// <returns>A new instance of OpenFileDialog.</returns>
		public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.IO.FileInfo path)
		{
			System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
			temp_fileDialog.InitialDirectory = path.Directory.FullName;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an OpenFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the OpenFileDialog.</param>
		/// <param name="directory">Directory to get the path from.</param>
		/// <returns>A new instance of OpenFileDialog.</returns>
		public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.IO.FileInfo path, System.IO.DirectoryInfo directory)
		{
			System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
			temp_fileDialog.InitialDirectory = path.Directory.FullName;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates a OpenFileDialog open in a given path.
		/// </summary>		
		/// <returns>A new instance of OpenFileDialog.</returns>
		public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog()
		{
			System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
			temp_fileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);			
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an OpenFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the OpenFileDialog</param>
		/// <returns>A new instance of OpenFileDialog.</returns>
		public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog (System.String path)
		{
			System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
			temp_fileDialog.InitialDirectory = path;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an OpenFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the OpenFileDialog.</param>
		/// <param name="directory">Directory to get the path from.</param>
		/// <returns>A new instance of OpenFileDialog.</returns>
		public static System.Windows.Forms.OpenFileDialog CreateOpenFileDialog(System.String path, System.IO.DirectoryInfo directory)
		{
			System.Windows.Forms.OpenFileDialog temp_fileDialog = new System.Windows.Forms.OpenFileDialog();
			temp_fileDialog.InitialDirectory = path;
			return temp_fileDialog;
		}

		/// <summary>
		/// Modifies an instance of OpenFileDialog, to open a given directory.
		/// </summary>
		/// <param name="fileDialog">OpenFileDialog instance to be modified.</param>
		/// <param name="path">Path to be opened by the OpenFileDialog.</param>
		/// <param name="directory">Directory to get the path from.</param>
		public static void SetOpenFileDialog(System.Windows.Forms.FileDialog fileDialog, System.String path, System.IO.DirectoryInfo directory)
		{
			fileDialog.InitialDirectory = path;
		}

		/// <summary>
		/// Modifies an instance of OpenFileDialog, to open a given directory.
		/// </summary>
		/// <param name="fileDialog">OpenFileDialog instance to be modified.</param>
		/// <param name="path">Path to be opened by the OpenFileDialog</param>
		public static void SetOpenFileDialog(System.Windows.Forms.FileDialog fileDialog, System.IO.FileInfo path)
		{
			fileDialog.InitialDirectory = path.Directory.FullName;
		}

		/// <summary>
		/// Modifies an instance of OpenFileDialog, to open a given directory.
		/// </summary>
		/// <param name="fileDialog">OpenFileDialog instance to be modified.</param>
		/// <param name="path">Path to be opened by the OpenFileDialog.</param>
		public static void SetOpenFileDialog(System.Windows.Forms.FileDialog fileDialog, System.String path)
		{
			fileDialog.InitialDirectory = path;
		}

		///
		///  Use the following static methods to create instances of SaveFileDialog.
		///  By default, JFileChooser is converted as an OpenFileDialog, the following methods
		///  are provided to create file dialogs to save files.
		///	
		
		
		/// <summary>
		/// Creates a SaveFileDialog.
		/// </summary>		
		/// <returns>A new instance of SaveFileDialog.</returns>
		public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog()
		{
			System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
			temp_fileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);			
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an SaveFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the SaveFileDialog.</param>
		/// <returns>A new instance of SaveFileDialog.</returns>
		public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.IO.FileInfo path)
		{
			System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
			temp_fileDialog.InitialDirectory = path.Directory.FullName;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an SaveFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the SaveFileDialog.</param>
		/// <param name="directory">Directory to get the path from.</param>
		/// <returns>A new instance of SaveFileDialog.</returns>
		public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.IO.FileInfo path, System.IO.DirectoryInfo directory)
		{
			System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
			temp_fileDialog.InitialDirectory = path.Directory.FullName;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates a SaveFileDialog open in a given path.
		/// </summary>
		/// <param name="directory">Directory to get the path from.</param>
		/// <returns>A new instance of SaveFileDialog.</returns>
		public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.IO.DirectoryInfo directory)
		{
			System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
			temp_fileDialog.InitialDirectory = directory.FullName;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an SaveFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the SaveFileDialog</param>
		/// <returns>A new instance of SaveFileDialog.</returns>
		public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog (System.String path)
		{
			System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
			temp_fileDialog.InitialDirectory = path;
			return temp_fileDialog;
		}

		/// <summary>
		/// Creates an SaveFileDialog open in a given path.
		/// </summary>
		/// <param name="path">Path to be opened by the SaveFileDialog.</param>
		/// <param name="directory">Directory to get the path from.</param>
		/// <returns>A new instance of SaveFileDialog.</returns>
		public static System.Windows.Forms.SaveFileDialog CreateSaveFileDialog(System.String path, System.IO.DirectoryInfo directory)
		{
			System.Windows.Forms.SaveFileDialog temp_fileDialog = new System.Windows.Forms.SaveFileDialog();
			temp_fileDialog.InitialDirectory = path;
			return temp_fileDialog;
		}
	}
	/*******************************/
	public delegate void PropertyChangeEventHandler(System.Object sender, PropertyChangingEventArgs e);

	/// <summary>
	/// EventArgs for support to the contrained properties.
	/// </summary>
	public class PropertyChangingEventArgs : System.ComponentModel.PropertyChangedEventArgs
	{   
		private System.Object oldValue;
		private System.Object newValue;

		/// <summary>
		/// Initializes a new PropertyChangingEventArgs instance.
		/// </summary>
		/// <param name="propertyName">Property name that fire the event.</param>
		public PropertyChangingEventArgs(System.String propertyName) : base(propertyName)
		{
		}

		/// <summary>
		/// Initializes a new PropertyChangingEventArgs instance.
		/// </summary>
		/// <param name="propertyName">Property name that fire the event.</param>
		/// <param name="oldVal">Property value to be replaced.</param>
		/// <param name="newVal">Property value to be set.</param>
		public PropertyChangingEventArgs(System.String propertyName, System.Object oldVal, System.Object newVal) : base(propertyName)
		{
			this.oldValue = oldVal;
			this.newValue = newVal;
		}

		/// <summary>
		/// Gets or sets the old value of the event.
		/// </summary>
		public System.Object OldValue
		{
			get
			{
				return this.oldValue;
			}
			set
			{
				this.oldValue = value;
			}
		}
	        
		/// <summary>
		/// Gets or sets the new value of the event.
		/// </summary>
		public System.Object NewValue
		{
			get
			{
				return this.newValue;
			}
			set
			{
				this.newValue = value;
			}
		}
	}


	/*******************************/
	/// <summary>
	/// Contains methods to get an set a ToolTip
	/// </summary>
	public class ToolTipSupport
	{
		static System.Windows.Forms.ToolTip supportToolTip = new System.Windows.Forms.ToolTip();

		/// <summary>
		/// Get the ToolTip text for the specific control parameter.
		/// </summary>
		/// <param name="control">The control with the ToolTip</param>
		/// <returns>The ToolTip Text</returns>
		public static System.String getToolTipText(System.Windows.Forms.Control control)
		{
			return(supportToolTip.GetToolTip(control));
		}
		 
		/// <summary>
		/// Set the ToolTip text for the specific control parameter.
		/// </summary>
		/// <param name="control">The control to set the ToolTip</param>
		/// <param name="text">The text to show on the ToolTip</param>
		public static void setToolTipText(System.Windows.Forms.Control control, System.String text)
		{
			supportToolTip.SetToolTip(control,text);
		}
	}

	/*******************************/
	/// <summary>
	/// Action that will be executed when a toolbar button is clicked.
	/// </summary>
	/// <param name="event_sender">The object that fires the event.</param>
	/// <param name="event_args">An EventArgs that contains the event data.</param>
	public static void ToolBarButtonClicked(System.Object event_sender, System.Windows.Forms.ToolStripItemClickedEventArgs event_args)
	{
        //Button button = (Button) event_args.ClickedItem;
        //button.PerformClick();
	}


	/*******************************/
	/// <summary>
	/// This class contains support methods to work with ButtonGroup.
	/// </summary>
	public class ButtonGroupSupport
	{
		/// <summary>
		/// Indicates whether an specific button is selected or not within a panel
		/// </summary>
		/// <param name="theArrayList">A reference to the penel where we will look in.</param>
		/// <param name="theButton">The button we want to know if it is selected.</param>
		/// <returns>A boolean value indicating if the button is selected within the panel.</returns>
		public static bool IsSelected(System.Collections.IList theArrayList, ButtonBase theButton)
		{
			if(theArrayList.Contains(theButton))
			{
				return (theButton.Focused)? true : false;
			}
			return false;
		}

		/// <summary>
		/// Sets or remove the focus of an specific button within a panel.
		/// </summary>
		/// <param name="theArrayList">A reference to the penel whitch contains the button.</param>
		/// <param name="theButton">The button we want to pass the focus to.</param>
		/// <param name="theValue">A boolean value indicating whether the button should get the focus or lose it.</param>
		public static void SetSelected(System.Collections.IList theArrayList, ButtonBase theButton, bool theValue)
		{																											 
			if(theArrayList.Contains(theButton))
			{
				// while(theArrayList.GetEnumerator().MoveNext())
                foreach (object item in theArrayList)
				{
                    Control ctrl = item as Control;

                    if (ctrl != null)
                    {
                        if (ctrl.Equals(theButton))
                        {
                            if (theValue == true)
                                ctrl.Select();
                            else
                                ctrl.FindForm().Select();
                        }
                    }
				}
			}
		}
	
		/// <summary>
		/// Gets a reference to the button witch contains the focus within a panel.
		/// </summary>
		/// <param name="theArrayList">A reference to the penel whitch contains the button.</param>
		/// <returns> A reference to the button whitch contains the focus within the panel. Null if none</returns>
		
		public static ButtonBase GetSelection(System.Collections.IList theArrayList)
		{																											 
			while(theArrayList.GetEnumerator().MoveNext()){
					if(((System.Windows.Forms.Control)theArrayList.GetEnumerator().Current).Focused)
						return (theArrayList.GetEnumerator().Current is ButtonBase)?
							((ButtonBase)theArrayList.GetEnumerator().Current) :
							null;
				}
			return null;
		}
	}

	/*******************************/
	public static bool IsDataFormatSupported(System.Windows.Forms.IDataObject data, System.Windows.Forms.DataFormats.Format format) 
	{
		bool result = false;
		if ((data != null) && (format != null))
		{
			System.String[] formats = data.GetFormats(true);
			int count = formats.GetLength(0);
			for (int index = 0; index < count; index++) 
			{
				if (formats[index].Equals(format.Name))
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}


}
