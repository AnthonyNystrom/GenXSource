/* -----------------------------------------------
 * ExpressionSchemaBuilder.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using VisiTypes = Genetibase.NuGenVisiCalc.Types;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Genetibase.NuGenVisiCalc.Operators;
using Genetibase.NuGenVisiCalc.Params;
using Genetibase.Shared.ComponentModel;
using Genetibase.NuGenVisiCalc.Programs;

namespace Genetibase.NuGenVisiCalc.Expression
{
	internal static class ExpressionSchemaBuilder
	{
		public static void BuildSchema(INuGenServiceProvider serviceProvider, Canvas canvas, String expression)
		{
			BuildSchema(canvas, new ExpressionTree(serviceProvider, expression));
		}

		public static void BuildSchema(Canvas canvas, ExpressionTree tree)
		{
			Int32 levelStep = canvas.SchemaRectangle.Width / (tree.RootNode.GetDeepLevel() + 1);

			canvas.ClearNodes();
			AddNode(canvas, null, tree.RootNode, new List<NumberParam>(), new Point(canvas.SchemaRectangle.Width - 50 - levelStep / 2, canvas.SchemaRectangle.Height / 2), levelStep, 0, canvas.SchemaRectangle.Height);
			canvas.Schema.Invalidate();
		}

		private static void AddNode(Canvas canvas, NodePort parentInput, ExpressionTreeNode expressionNode, List<NumberParam> variables, Point location, Int32 levelStep, Int32 minY, Int32 maxY)
		{
			Int32 leafs = (expressionNode.LeftNode != null ? 1 : 0) + (expressionNode.RightNode != null ? 1 : 0);
			NodeBase node = null;

			if (expressionNode.Token.IsNumber)
			{
				node = new VisiTypes.Number(Double.Parse(expressionNode.Token.TokenBody));
				canvas.AddNode(node, location);
			}

			if (expressionNode.Token.IsVariable)
			{
				Boolean exist = false;
				foreach (NumberParam param in variables)
				{
					if (param.Name == expressionNode.Token.TokenBody)
					{
						exist = true;
						node = param;
						break;
					}
				}

				if (!exist)
				{
					node = new NumberParam();
					((NumberParam)node).Name = expressionNode.Token.TokenBody;
					((NumberParam)node).Header = expressionNode.Token.TokenBody;
					variables.Add((NumberParam)node);
					canvas.AddNode(node, location);
				}
			}

			if (expressionNode.Token.IsOperator)
			{
				node = new Operator(expressionNode.Token.OperatorDescriptor);
				canvas.AddNode(node, location);

				switch (leafs)
				{
					case 1:
					{
						AddNode(canvas, node.GetInput(0), expressionNode.LeftNode, variables, new Point(location.X - levelStep, location.Y), levelStep, minY, maxY);
						break;
					}
					case 2:
					{
						Int32 y2 = (maxY - minY) / 2;
						AddNode(canvas, node.GetInput(0), expressionNode.LeftNode, variables, new Point(location.X - levelStep, minY + y2 / 2), levelStep, minY, minY + y2);
						AddNode(canvas, node.GetInput(1), expressionNode.RightNode, variables, new Point(location.X - levelStep, maxY - y2 / 2), levelStep, maxY - y2, maxY);
						break;
					}
				}
			}

			if (parentInput != null && node != null)
			{
				parentInput.Connection = node.GetOutput(0);
			}
		}

		private static Boolean NeedToSkip(NodeBase node)
		{
			return node is VisiTypes.ProgramInput
				|| node is VisiTypes.ProgramOutput
				|| node is VisiTypes.NumericResult
				|| node is ProgramNode
				;
		}

		public static String BuildExpressionFromSchema(INuGenServiceProvider serviceProvider, Canvas canvas)
		{
			// Check all nodes for connected inputs
			foreach (NodeBase node in canvas.SelectedProgram.Nodes)
			{
				if (node.InputsLength > 0 && !NeedToSkip(node))
				{
					foreach (NodePort port in node.Inputs)
					{
						if (port.Connection == null)
						{
							throw new ArgumentException(String.Format("Node \"{0}\" on schema has unplugged input. Expression cannot be builded", node.Header));
						}
					}
				}
			}

			List<NodeBase> rootNodes = new List<NodeBase>();
			List<NodeBase> nodes = new List<NodeBase>();

			// Find a root node. 

			foreach (NodeBase node in canvas.SelectedProgram.Nodes)
			{
				if (NeedToSkip(node))
				{
					continue;
				}

				rootNodes.Add(node);

				if (node.InputsLength > 0)
				{
					foreach (NodePort port in node.Inputs)
					{
						if (!nodes.Contains(port.Connection.Node))
						{
							nodes.Add(port.Connection.Node);
						}
					}
				}
			}

			foreach (NodeBase node in nodes)
			{
				if (rootNodes.Contains(node))
				{
					rootNodes.Remove(node);
				}
			}

			if (rootNodes.Count == 0)
			{
				throw new ArgumentException("Schema doesn't have root node");
			}

			if (rootNodes.Count > 1)
			{
				throw new ArgumentException("Schema has more than one root node");
			}

			nodes.Clear();

			if (HasCircularLinks(rootNodes[0], nodes))
			{
				throw new ArgumentException("Schema have circular links.");
			}

			return BuildExpressionNode(serviceProvider, rootNodes[0]).ToString();
		}

		/*
		 * HasCircularLinks
		 */

		/// <summary>
		/// </summary>
		/// <param name="node"></param>
		/// <param name="nodesWay"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="node"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="nodesWay"/> is <see langword="null"/>.</para>
		/// </exception>
		public static Boolean HasCircularLinks(NodeBase node, IList<NodeBase> nodesWay)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}

			if (nodesWay == null)
			{
				throw new ArgumentNullException("nodesWay");
			}

			if (nodesWay.Contains(node))
			{
				return true;
			}

			nodesWay.Add(node);

			if (node.InputsLength > 0)
			{
				foreach (NodePort port in node.Inputs)
				{
					if (
						port.Connection != null
						&& !NeedToSkip(port.Connection.Node)
						&& HasCircularLinks(port.Connection.Node, nodesWay)
						)
					{
						return true;
					}
				}
			}

			nodesWay.Remove(node);
			return false;
		}

		public static ExpressionTreeNode BuildExpressionNode(INuGenServiceProvider serviceProvider, NodeBase schemaNode)
		{
			if (schemaNode == null)
			{
				return null;
			}

			if (NeedToSkip(schemaNode))
			{
				throw new InvalidOperationException("Expression node connected to unsupported node. Expression node cannot be recognized");
			}

			ExpressionTreeNode node = null;

			if (schemaNode is Operator)
			{
				ExpressionToken token = new ExpressionToken(serviceProvider, ((Operator)schemaNode).OperatorDescriptor.StringRepresentation);
				node = new ExpressionTreeNode(token);

				switch (token.OperatorDescriptor.PrimitiveOperator)
				{
					case PrimitiveOperator.Add:
					case PrimitiveOperator.Sub:
					case PrimitiveOperator.Mul:
					case PrimitiveOperator.Div:
					{
						node.LeftNode = BuildExpressionNode(serviceProvider, schemaNode.Inputs[0].Connection.Node);
						node.RightNode = BuildExpressionNode(serviceProvider, schemaNode.Inputs[1].Connection.Node);
						break;
					}
				}

				if (token.IsOneVariableFunction)
				{
					node.LeftNode = BuildExpressionNode(serviceProvider, schemaNode.Inputs[0].Connection.Node);
				}

				if (token.IsTwoVariableFunction)
				{
					node.LeftNode = BuildExpressionNode(serviceProvider, schemaNode.Inputs[0].Connection.Node);
					node.RightNode = BuildExpressionNode(serviceProvider, schemaNode.Inputs[1].Connection.Node);
				}

				return node;
			}

			if (schemaNode is NumberParam)
			{
				return new ExpressionTreeNode(new ExpressionToken(serviceProvider, ((NumberParam)schemaNode).Name));
			}

			if (schemaNode is VisiTypes.Number)
			{
				return new ExpressionTreeNode(new ExpressionToken(serviceProvider, ((VisiTypes.Number)schemaNode).Value.ToString()));
			}

			return null;
		}
	}
}
