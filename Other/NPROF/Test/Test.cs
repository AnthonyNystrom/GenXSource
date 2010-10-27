/***************************************************************************
                          profiler.cpp  -  description
                             -------------------
    begin                : Sat Jan 18 2003
    copyright            : (C) 2003,2004,2005,2006 by Matthew Mastracci, Christian Staudenmeyer
    email                : mmastrac@canada.com
 ***************************************************************************/

/***************************************************************************
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

namespace NProf.Test
{
	public class Tests:TestRunner
	{
		public static void Main()
		{
			new Tests().Run();
		}
		private static string NProfDirectory
		{
			get
			{
				return new DirectoryInfo(System.Windows.Forms.Application.StartupPath).Parent.Parent.Parent.FullName;
			}
		}
		public class TestProfilee : Test
		{
			Run run;
			public override object GetResult(out int level)
			{
				level = 1;
				ProjectInfo project = new ProjectInfo();
				NProf.form.Project = project;
				Profiler profiler = new Profiler();
				//project.ApplicationName = @"D:\Meta\0.2\bin\Debug\Meta.exe";
				//project.Arguments = "-test";
				project.ApplicationName = Path.Combine(NProfDirectory, @"TestProfilee\bin\Debug\TestProfilee.exe");

				run = project.CreateRun(profiler);
				run.profiler.Completed += new EventHandler(profiler_Completed);
				run.Start();

				while (result == null)
				{
					Thread.Sleep(100);
				}
				return result;
			}

			void profiler_Completed(object sender, EventArgs e)
			{
				result = run;
			}
			private object result = null;
			//void run_StateChanged(Run run, RunState rsOld, RunState rsNew)
			////void run_StateChanged(Run run, RunState rsOld, RunState rsNew)
			//{
			//    if (rsNew == RunState.Finished)
			//    //if (rsNew == NProf.Glue.Profiler.Project.RunState.Finished)
			//    {
			//        result = run;
			//    }
			//}
		}
	}
	public abstract class TestRunner
	{
		public static string TestDirectory
		{
			get
			{
				return @"D:\nprof\trunk\nprof\Test\Tests";
			}
		}
		public abstract class Test
		{
			public bool RunTest()
			{
				int level;
				Console.Write(this.GetType().Name + "...");


				DateTime startTime = DateTime.Now;
				object result = GetResult(out level);
				TimeSpan duration = DateTime.Now - startTime;


				string testDirectory = Path.Combine(TestDirectory, this.GetType().Name);

				string resultPath = Path.Combine(testDirectory, "result.txt");
				string checkPath = Path.Combine(testDirectory, "check.txt");

				Directory.CreateDirectory(testDirectory);
				if (!File.Exists(checkPath))
				{
					File.Create(checkPath).Close();
				}

				StringBuilder stringBuilder = new StringBuilder();
				Serialize(result, "", stringBuilder, level);

				File.WriteAllText(resultPath, stringBuilder.ToString(), Encoding.UTF8);
				string successText;
				bool success=File.ReadAllText(resultPath).Equals(File.ReadAllText(checkPath));
				if (!success)
				{
					successText = "failed";
				}
				else
				{
					successText = "succeeded";
				}
				Console.WriteLine(" " + successText + "  " + duration.TotalSeconds.ToString() + " s");
				return success;
			}
			public abstract object GetResult(out int level);
		}
		public void Run()
		{
			bool allTestsSucessful = true;
			foreach (Type testType in this.GetType().GetNestedTypes())
			{
				if (testType.IsSubclassOf(typeof(Test)))
				{
					Test test = (Test)testType.GetConstructor(new Type[] { }).Invoke(null);
					int level;
					if (!test.RunTest())
					{
						allTestsSucessful = false;
					}
				}
			}
			if (!allTestsSucessful)
			{
				Console.ReadLine();
			}
		}
		public const char indentationChar = '\t';

		private static bool UseToStringMethod(Type type)
		{
			return (type.IsValueType || type.IsPrimitive || type==typeof(string));
		}
		private static bool UseProperty(PropertyInfo property, int level)
		{
			object[] attributes = property.GetCustomAttributes(typeof(SerializeAttribute), false);
			object[] ignore = property.GetCustomAttributes(typeof(XmlIgnoreAttribute), false);


			return property.Name!="SyncRoot" && !property.PropertyType.IsSubclassOf(typeof(Delegate)) &&
				ignore.Length==0 
				&& property.GetGetMethod().GetParameters().Length==0;
		}
		public static void Serialize(object obj, string indent, StringBuilder builder, int level)
		{
			
			if (obj!=null && obj is ProcessInfo)
			{
			}
			if (obj == null)
			{
				builder.Append(indent + "null\n");
			}
			else if (UseToStringMethod(obj.GetType()))
			{
				builder.Append(indent + "\"" + obj.ToString() + "\"" + "\n");
			}
			else
			{
				foreach (PropertyInfo property in obj.GetType().GetProperties())
				{
					if (UseProperty((PropertyInfo)property, level))
					{
						object val = property.GetValue(obj, null);
						builder.Append(indent + property.Name);
						if (val != null)
						{
							builder.Append(" (" + val.GetType().Name + ")");
						}
						builder.Append(":\n");
						Serialize(val, indent + indentationChar, builder, level);
					}
				}
				string specialEnumerableSerializationText;
				if (obj is ISerializeEnumerableSpecial && (specialEnumerableSerializationText = ((ISerializeEnumerableSpecial)obj).Serialize()) != null)
				{
					builder.Append(indent + specialEnumerableSerializationText + "\n");
				}
				else if (obj is System.Collections.IEnumerable)
				{
					foreach (object entry in (System.Collections.IEnumerable)obj)
					{
						builder.Append(indent + "Entry (" + entry.GetType().Name + ")\n");
						Serialize(entry, indent + indentationChar, builder, level);
					}
				}
			}
		}
	}
	public interface ISerializeEnumerableSpecial
	{
		string Serialize();
	}
	public class SerializeAttribute : Attribute
	{
		public SerializeAttribute()
			: this(1)
		{
		}
		public SerializeAttribute(int level)
		{
			this.level = level;
		}
		private int level;
		public int Level
		{
			get
			{
				return level;
			}
		}
	}
}
