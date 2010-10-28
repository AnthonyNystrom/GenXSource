#region Copyright 2001-2006 Christoph Daniel Rüegg [GNU Public License]
/*
NeuroBox, a library for neural network generation, propagation and training
Copyright (c) 2001-2006, Christoph Daniel Rueegg, http://cdrnet.net/. All rights reserved.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
*/
#endregion

using System;
using NeuroBox;

namespace NeuroBox.PatternMatching.Grid2D
{
	/// <summary>
	/// Default structure generator that builds intermediate synapse structures.
	/// </summary>
	public class DefaultGrid2DStructureBuilder: INetworkStructureBuilder
	{
		/// <summary>The configuration instance.</summary>
		protected Grid2DConfig _config;
		/// <summary>The neural network.</summary>
		protected Network _network;
		/// <summary>The input grid's width.</summary>
		protected int _width;
		/// <summary>The input grid's height.</summary>
		protected int _height;
		/// <summary>The count of output neurons.</summary>
		protected int _numberOfOutputs;
		/// <summary>The count of input neurons (=width*height).</summary>
		protected int _numberOfInputs;

		/// <summary>
		/// Instanciate a new network structure factory
		/// </summary>
		/// <param name="network">The network.</param>
		/// <param name="gridX">The width of the grid.</param>
		/// <param name="gridY">The height of the grid.</param>
		/// <param name="outputSize">The count of output neurons.</param>
		public DefaultGrid2DStructureBuilder(int gridX, int gridY, int numberOfOutputs)
		{
			_width = gridX;
			_height = gridY;
			_numberOfOutputs = numberOfOutputs;
			_numberOfInputs = _width * _height;
		}

		/// <summary>
		/// The neural network instance.
		/// </summary>
		public Network Network
		{
            get { return _network; }
            set
            {
                _network = value;
                if(_config == null)
                    _config = new Grid2DConfig(_network.Node);
                else
                    _config.Node = _network.Node;
            }
		}

		/// <summary>
		/// Adapt the factory's default configuration.
		/// </summary>
		public virtual void AdaptConfiguration()
		{
            /*
            BasicConfig config = _network.BasicConfiguration;
            config.FlatspotEliminationEnable.Value = true;
            //config.ManhattanTrainingEnable.Value = true;
            config.MomentumTermEnable.Value = false;
            config.WeightDecayEnable.Value = true;
            //config.SymmetryPreventionEnable.Value = true;
            config.BiasNeuronEnable.Value = true;
            config.BiasNeuronOutput.Value = 0.90d;
            config.WeightDecay.Value = 0.02; //0.005;
            config.LearningRate.Value = 0.3; //0.1;
            */
		}

		/// <summary>
		/// Connect the network.
		/// </summary>
		public virtual void ConnectNetwork()
		{
			Layer input = _network.InitUnboundInputLayer(_numberOfInputs);
			Layer hidden;
			if(_config.All2AllEnable.Value)
			{
				if(_config.HorizontalLinesEnable.Value && _config.VerticalLinesEnable.Value)
					hidden = _network.AddLayer((int)Math.Ceiling(_numberOfInputs/4d));
				else
					hidden = _network.AddLayer((int)Math.Ceiling(_numberOfInputs/2d));
				input.CrossLinkForward(); //link input to existing hidden neurons
			}
			else
				hidden = _network.AddLayer(0);
			_network.AddLayer(_numberOfOutputs);

			if(_config.VerticalLinesEnable.Value)
				hidden.AddRange(AddVerticalLines(input,hidden));
			if(_config.HorizontalLinesEnable.Value)
				hidden.AddRange(AddHorizontalLines(input,hidden));
			if(_config.RingsEnable.Value)
				hidden.AddRange(AddRings(input,hidden));
			if(_config.LittleSquaresEnable.Value)
				hidden.AddRange(AddSquares(input,hidden));

			hidden.CrossLinkForward(); //link hidden to output layer
		}

		#region Advanced Connections
		/// <summary>
		/// Connect input neurons in vertical lines to new hidden neurons.
		/// </summary>
		/// <param name="input">The input layer.</param>
		/// <param name="hidden">The neuron to connect to.</param>
		/// <returns>The set of new hidden neurons.</returns>
		protected Neuron[] AddVerticalLines(Layer input, Layer hidden)
		{
			Neuron[] vertical = new Neuron[_width];
			for(int i=0;i<_width;i++)
			{
				vertical[i] = new Neuron(_network.Node, hidden);
				ConnectVerticalLine(input,vertical[i],_width,i,0,_height-1);
			}
			return vertical;
		}

		/// <summary>
		/// Connect input neurons in horizontal lines to new hidden neurons.
		/// </summary>
		/// <param name="input">The input layer.</param>
		/// <param name="hidden">The neuron to connect to.</param>
		/// <returns>The set of new hidden neurons.</returns>
		protected Neuron[] AddHorizontalLines(Layer input, Layer hidden)
		{
			Neuron[] horizontal = new Neuron[_height];
			for(int i=0;i<_height;i++)
			{
                horizontal[i] = new Neuron(_network.Node, hidden);
				ConnectHorizontalLine(input,horizontal[i],_width,i,0,_width-1);
			}
			return horizontal;
		}

		/// <summary>
		/// Connect input neurons in rings to new hidden neurons.
		/// </summary>
		/// <param name="input">The input layer.</param>
		/// <param name="hidden">The neuron to connect to.</param>
		/// <returns>The set of new hidden neurons.</returns>
		protected Neuron[] AddRings(Layer input, Layer hidden)
		{
			Neuron[] ring = new Neuron[Math.Min(_width,_height)/2];
			for(int i=0;i<ring.Length;i++)
			{
                ring[i] = new Neuron(_network.Node, hidden);
				ConnectRing(input,ring[i],_width,_height,i);
			}
			return ring;
		}

		/// <summary>
		/// Connect input neurons in squares to new hidden neurons.
		/// </summary>
		/// <param name="input">The input layer.</param>
		/// <param name="hidden">The neuron to connect to.</param>
		/// <returns>The set of new hidden neurons.</returns>
		protected Neuron[] AddSquares(Layer input, Layer hidden)
		{
			if(_width < 2 || _height < 2)
				return new Neuron[] {};
			int widthhalf = (int)Math.Ceiling(_width/2d);
			int heighthalf = (int)Math.Ceiling(_height/2d);
			Neuron[] area = new Neuron[widthhalf*heighthalf];
			widthhalf -= widthhalf % 2;
			heighthalf -= heighthalf % 2;
			int item = 0;
			for(int i=0;i<=widthhalf;i+=2)
			{
				for(int j=0;j<=heighthalf;j+=2)
				{
                    area[item] = new Neuron(_network.Node, hidden);
					ConnectArea(input,area[item++],_width,i,j,2,2);
				}
				for(int j=_height-2;j>heighthalf;j-=2)
				{
                    area[item] = new Neuron(_network.Node, hidden);
					ConnectArea(input,area[item++],_width,i,j,2,2);
				}
			}
			for(int i=_width-2;i>widthhalf;i-=2)
			{
				for(int j=0;j<=heighthalf;j+=2)
				{
                    area[item] = new Neuron(_network.Node, hidden);
					ConnectArea(input,area[item++],_width,i,j,2,2);
				}
				for(int j=_height-2;j>heighthalf;j-=2)
				{
                    area[item] = new Neuron(_network.Node, hidden);
					ConnectArea(input,area[item++],_width,i,j,2,2);
				}
			}
			return area;
		}
		#endregion

		#region Basic Connections
		/// <summary>
		/// Connect input neurons of a specified vertical line to a given neuron.
		/// </summary>
		/// <param name="input">The input layer.</param>
		/// <param name="neuron">The neuron to connect to.</param>
		/// <param name="width">The width of the input grid (needed for transforming (x,y) to linear position).</param>
		/// <param name="x">The constant X-Coordinate of the line.</param>
		/// <param name="ymin">The top end Y-Coordinate.</param>
		/// <param name="ymax">The bottom end Y-Coordinate.</param>
		protected void ConnectVerticalLine(Layer input, Neuron neuron, int width, int x, int ymin, int ymax)
		{
			for(int i=ymin;i<ymax+1;i++)
				input[x+i*width].ConnectToNeuron(neuron);
		}

		/// <summary>
		/// Connect input neurons of a specified horizontal line to a given neuron.
		/// </summary>
		/// <param name="input">The input layer.</param>
		/// <param name="neuron">The neuron to connect to.</param>
		/// <param name="width">The width of the input grid (needed for transforming (x,y) to linear position).</param>
		/// <param name="y">The constant Y-Coordinate of the line.</param>
		/// <param name="xmin">The left end X-Coordinate.</param>
		/// <param name="xmax">The right end X-Coordinate.</param>
		protected void ConnectHorizontalLine(Layer input, Neuron neuron, int width, int y, int xmin, int xmax)
		{
			for(int i=xmin;i<xmax+1;i++)
				input[i+y*width].ConnectToNeuron(neuron);
		}

		/// <summary>
		/// Connect input neurons of a specified 'ring' to a given neuron.
		/// </summary>
		/// <param name="input">The input layer.</param>
		/// <param name="neuron">The neuron to connect to.</param>
		/// <param name="width">The width of the input grid (needed for transforming (x,y) to linear position).</param>
		/// <param name="height">The height of the input grid (needed for transforming (x,y) to linear position).</param>
		/// <param name="shift">The radial shift. 0 means the biggest radius, 1 the second biggest etc.</param>
		protected void ConnectRing(Layer input, Neuron neuron, int width, int height, int shift)
		{
			ConnectVerticalLine(input,neuron,width,shift,shift,height-1-shift);
			ConnectVerticalLine(input,neuron,width,width-1-shift,shift,height-1-shift);
			ConnectHorizontalLine(input,neuron,width,shift,shift+1,width-2-shift);
			ConnectHorizontalLine(input,neuron,width,height-1-shift,shift+1,width-2-shift);
		}

		/// <summary>
		/// Connect input neurons of a specified oblong to a given neuron.
		/// </summary>
		/// <param name="input">The input layer.</param>
		/// <param name="neuron">The neuron to connect to.</param>
		/// <param name="width">The width of the input grid (needed for transforming (x,y) to linear position).</param>
		/// <param name="x">X-Coordinate of the top left cornet.</param>
		/// <param name="y">Y-Coordinate of the top left cornet.</param>
		/// <param name="dx">Width of the oblong.</param>
		/// <param name="dy">Height of the oblong.</param>
		protected void ConnectArea(Layer input, Neuron neuron, int width, int x, int y, int dx, int dy)
		{
			for(int i=x;i<x+dx;i++)
				for(int j=y;j<y+dy;j++)
					input[i+j*width].ConnectToNeuron(neuron);
		}
		#endregion
	}
}
