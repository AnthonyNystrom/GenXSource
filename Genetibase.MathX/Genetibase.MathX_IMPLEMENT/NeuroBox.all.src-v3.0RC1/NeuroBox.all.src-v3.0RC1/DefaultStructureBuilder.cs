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

namespace NeuroBox
{
	/// <summary>
	/// Default structure generator that builds simple (all to all) synapse structures.
	/// </summary>
	public class DefaultStructureBuilder: INetworkStructureBuilder
	{
		/// <summary>The neural network.</summary>
		private Network _network;
		/// <summary>The count of output neurons.</summary>
        private int _numberOfOutputs;
		/// <summary>The count of input neurons.</summary>
        private int _numberOfInputs;

		/// <summary>
		/// Instanciate a new network structure factory
		/// </summary>
        /// <param name="numberOfInputs">The count of input neurons.</param>
        /// <param name="numberOfClasses">The count of output neurons.</param>
		public DefaultStructureBuilder(int numberOfInputs, int numberOfOutputs)
		{
			_numberOfOutputs = numberOfOutputs;
			_numberOfInputs = numberOfInputs;
		}

        /// <summary>
        /// The neural network instance.
        /// </summary>
        public Network Network
        {
            get { return _network; }
            set { _network = value; }
        }

        public int NumberOfInputs
        {
            get { return _numberOfInputs; }
            set { _numberOfInputs = value; }
        }

        public int NumberOfOutputs
        {
            get { return _numberOfOutputs; }
            set { _numberOfOutputs = value; }
        }

		/// <summary>
		/// Adapt the factory's default configuration.
		/// </summary>
		public virtual void AdaptConfiguration()
		{
            /*BasicConfig config = _network.BasicConfiguration;*/
		}

		/// <summary>
		/// Connect the network.
		/// </summary>
		public virtual void ConnectNetwork()
		{
			Layer input = _network.InitUnboundInputLayer(_numberOfInputs);
			Layer hidden = _network.AddLayer((int)Math.Ceiling(_numberOfInputs/2d));
			_network.AddLayer(_numberOfOutputs);
			input.CrossLinkForward();
			hidden.CrossLinkForward();
		}
	}
}
