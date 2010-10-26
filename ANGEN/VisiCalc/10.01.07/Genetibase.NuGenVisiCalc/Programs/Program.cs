/* -----------------------------------------------
 * Program.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Resources;
using Genetibase.NuGenVisiCalc.Types;
using System.Collections;

namespace Genetibase.NuGenVisiCalc.Programs
{
	[Serializable]
	internal sealed class Program
	{
		#region Properties.Public

		/*
		 * Nodes
		 */

		private IList<NodeBase> _nodes;

		public IList<NodeBase> Nodes
		{
			get
			{
				return _nodes;
			}
		}

		/*
		 * ParentNode
		 */

		private NodeBase _parentNode;

		public NodeBase ParentNode
		{
			get
			{
				return _parentNode;
			}
			set
			{
				_parentNode = value;
			}
		}

		/*
		 * ParentProgram
		 */

		private Program _parentProgram;

		public Program ParentProgram
		{
			get
			{
				return _parentProgram;
			}
			set
			{
				_parentProgram = value;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * AddNode
		 */

		public void AddNode(NodeBase nodeToAdd)
		{
			if (nodeToAdd == null)
			{
				throw new ArgumentNullException("nodeToAdd");
			}

			nodeToAdd.ContainingProgram = this;

			/* If we are adding an input node, we need to set its index starting at 1 because of the on/off node. */

			Int32 inputCount = 1;
			ProgramInput programInput = nodeToAdd as ProgramInput;

			if (programInput != null)
			{
				for (Int32 i = 0; i < _nodes.Count; i++)
				{
					if (_nodes[i] is ProgramInput)
					{
						inputCount++;
					}
				}

				programInput.InputIndex = inputCount;
			}

			/* If we are adding an output node, we need to set its index. */

			Int32 outputCount = 0;
			ProgramOutput programOutput = nodeToAdd as ProgramOutput;

			if (programOutput != null)
			{
				for (Int32 i = 0; i < _nodes.Count; i++)
				{
					if (_nodes[i] is ProgramOutput)
					{
						outputCount++;
					}
				}

				programOutput.OutputIndex = outputCount;
			}

			_nodes.Add(nodeToAdd);
			UpdateParentNode();
		}

		/*
		 * ClearNodes
		 */

		public void ClearNodes()
		{
			_nodes.Clear();
			UpdateParentNode();
		}

		/*
		 * RemoveNode
		 */

		private Int32 _lastRemovedInputIndex;
		private Int32 _lastRemovedOutputIndex;

		/// <summary>
		/// </summary>
		/// <param name="nodeToRemove"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="nodeToRemove"/> is <see langword="null"/>.</para>
		/// </exception>
		public void RemoveNode(NodeBase nodeToRemove)
		{
			if (nodeToRemove == null)
			{
				throw new ArgumentNullException("nodeToRemove");
			}

			/* Disconnect from other nodes. */

			for (Int32 i = 0; i < _nodes.Count; i++)
			{
				NodeBase currentNode = _nodes[i];

				for (Int32 j = 0; j < currentNode.InputsLength; j++)
				{
					if (currentNode.GetInput(j).Connection != null)
					{
						if (Object.ReferenceEquals(nodeToRemove, currentNode.GetInput(j).Connection.Node))
						{
							currentNode.GetInput(j).Connection = null;
						}
					}
				}
			}

			/* If it is a program output, disconnect the parent connection. */

			ProgramOutput programOutput = nodeToRemove as ProgramOutput;

			if (programOutput != null)
			{
				_lastRemovedOutputIndex = programOutput.OutputIndex;
			}

			_nodes.Remove(nodeToRemove);

			ProgramInput programInput = nodeToRemove as ProgramInput;

			if (programInput != null)
			{
				Int32 inputCount = 1;
				_lastRemovedInputIndex = programInput.InputIndex;

				for (Int32 i = 0; i < _nodes.Count; i++)
				{
					ProgramInput currentProgramInput = _nodes[i] as ProgramInput;

					if (currentProgramInput != null)
					{
						if (currentProgramInput.InputIndex > _lastRemovedInputIndex)
						{
							programInput.InputIndex = programInput.InputIndex - 1;
						}

						inputCount++;
					}
				}
			}

			programOutput = nodeToRemove as ProgramOutput;

			if (programOutput != null)
			{
				Int32 outputCount = 0;

				for (Int32 i = 0; i < _nodes.Count; i++)
				{
					ProgramOutput currentProgramOutput = _nodes[i] as ProgramOutput;

					if (currentProgramOutput != null)
					{
						if (currentProgramOutput.OutputIndex > _lastRemovedOutputIndex)
						{
							currentProgramOutput.OutputIndex = currentProgramOutput.OutputIndex - 1;
						}

						outputCount++;
					}
				}

				if (nodeToRemove.ContainingProgram.ParentNode != null)
				{
					NodePort removedPort = nodeToRemove.ContainingProgram.ParentNode.GetOutput(outputCount);

					foreach (NodeBase node in nodeToRemove.ContainingProgram.ParentNode.ContainingProgram.Nodes)
					{
						for (Int32 i = 0; i < node.InputsLength; i++)
						{
							if (node.GetInput(i).Connection != null)
							{
								if (Object.ReferenceEquals(node.GetInput(i).Connection, removedPort))
								{
									node.GetInput(i).Connection = null;
								}
							}
						}
					}
				}
			}

			UpdateParentNode();
		}

		/// <summary>
		/// </summary>
		/// <param name="nodeToRemove"></param>
		/// <exception cref="OutOfRangeException">
		/// <paramref name="nodeToRemoveIndex"/> should be within the bounds of the <see cref="Nodes"/> collection.
		/// </exception>
		public void RemoveNode(Int32 nodeToRemoveIndex)
		{
			RemoveNode(_nodes[nodeToRemoveIndex]);
		}

		/*
		 * UpdateParentNode
		 */

		public void UpdateParentNode()
		{
			if (_parentNode != null)
			{
				NodePort[] oldInputs = new NodePort[_parentNode.InputsLength];
				_parentNode.Inputs.CopyTo(oldInputs, 0);

				NodePort[] oldOutputs = null;

				if (_parentNode.Outputs != null)
				{
					oldOutputs = new NodePort[_parentNode.OutputsLength];
					_parentNode.Outputs.CopyTo(oldOutputs, 0);
				}

				Int32 inputCount = 0;
				Int32 outputCount = 0;

				for (Int32 i = 0; i < _nodes.Count; i++)
				{
					NodeBase currentNode = _nodes[i];

					if (currentNode is ProgramInput)
					{
						inputCount++;
					}
					else if (currentNode is ProgramOutput)
					{
						outputCount++;
					}
				}

				_parentNode.CreateInputs(inputCount + 1);

				if ((inputCount + 1) >= oldInputs.Length)
				{
					/* Set new inputs equal to old inputs. */
					for (Int32 i = 0; i < oldInputs.Length; i++)
					{
						_parentNode.Inputs[i] = oldInputs[i];
					}

					/* Add new inputs. */
					for (Int32 i = oldInputs.Length; i < inputCount + 1; i++)
					{
						_parentNode.SetInput(i, "", 0.0);
					}
				}
				else
				{
					/* An input was removed. */
					for (Int32 i = 0; i < inputCount + 1; i++)
					{
						_parentNode.Inputs[i] = oldInputs[i];
					}
				}

				_parentNode.CreateOutputs(outputCount);

				if (oldOutputs != null)
				{
					if (outputCount >= oldOutputs.Length)
					{
						for (Int32 i = 0; i < oldOutputs.Length; i++)
						{
							_parentNode.Outputs[i] = oldOutputs[i];
						}

						for (Int32 i = oldOutputs.Length; i < outputCount; i++)
						{
							_parentNode.SetOutput(i, "", 0.0);
						}
					}
					else
					{
						/* An output was removed. */
						for (Int32 i = 0; i < outputCount; i++)
						{
							_parentNode.Outputs[i] = oldOutputs[i];
						}
					}
				}
				else
				{
					/* All new outputs. */
					for (Int32 i = 0; i < outputCount; i++)
					{
						_parentNode.SetOutput(i, "", 0.0);
					}
				}
			}
		}

		#endregion

		#region Methods.Public.Static

		/*
		 * Insert
		 */

		public static ProgramNode Insert(String fileName)
		{
			ProgramNode programNode = null;

			using (ResourceReader resourceReader = new ResourceReader(fileName))
			{
				IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();

				while (enumerator.MoveNext())
				{
					switch (Convert.ToString(enumerator.Key))
					{
						case "ProgramNode":
						{
							programNode = (ProgramNode)enumerator.Value;
							break;
						}
					}
				}
			}

			return programNode;
		}

		/*
		 * Load
		 */

		public static Program Load(String fileName)
		{
			Program program = null;

			using (ResourceReader resourceReader = new ResourceReader(fileName))
			{
				IDictionaryEnumerator enumerator = resourceReader.GetEnumerator();

				while (enumerator.MoveNext())
				{
					switch (Convert.ToString(enumerator.Key))
					{
						case "Program":
						{
							program = (Program)enumerator.Key;
							break;
						}
						case "ProgramNode":
						{
							program = ((ProgramNode)enumerator.Value).SubProgram;
							break;
						}
					}
				}
			}

			return program;
		}

		/*
		 * Save
		 */

		/// <summary>
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="programToSave"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="programToSave"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void Save(String fileName, Program programToSave)
		{
			if (programToSave == null)
			{
				throw new ArgumentNullException("programToSave");
			}

			using (ResourceWriter resourceWriter = new ResourceWriter(fileName))
			{
				ProgramNode programNode = new ProgramNode(null);
				programNode.SubProgram = programToSave;
				programNode.SubProgram.ParentNode = programNode;

				resourceWriter.AddResource("ProgramNode", programNode);
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="SchematicProgram"/> class.
		/// </summary>
		public Program()
		{
			_nodes = new List<NodeBase>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SchematicProgram"/> class.
		/// </summary>
		/// <param name="parentProgram">Can be <see langword="null"/>.</param>
		public Program(Program parentProgram)
			: this()
		{
			_parentProgram = parentProgram;
		}
	}
}
