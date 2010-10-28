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
using System.Collections.Generic;

namespace NeuroBox
{
    /// <summary>
    /// Array based structure builder that generates synapse structures
    /// based on an array description.
    /// </summary>
    public class ArrayStructureBuilder : INetworkStructureBuilder
    {
        /// <summary>The neural network.</summary>
        private Network _network;
        /// <summary>The count of output neurons.</summary>
        private int _numberOfOutputs;
        /// <summary>The count of input neurons.</summary>
        private int _numberOfInputs;

        /// <summary>The layer structure array.</summary>
        private int[] _layers;
        /// <summary>The synapse structure array.</summary>
        private bool[,] _synapses;

        /// <summary>
        /// Instanciate a new network structure factory
        /// </summary>
        /// <param name="layers">The layer structure array (count of neurons per layer).</param>
        /// <param name="synapses">The synapse structure array (needs to be square!).</param>
        public ArrayStructureBuilder(int[] layers, bool[,] synapses)
        {
            _layers = layers;
            _synapses = synapses;
            _numberOfOutputs = layers[layers.Length - 1];
            _numberOfInputs = layers[0];

            if(_synapses.GetLength(0) != _synapses.GetLength(1))
                throw new ArgumentException("Synapse Connection Array needs to be in square form", "synapses");
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
        }

        public int NumberOfOutputs
        {
            get { return _numberOfOutputs; }
        }

        public int[] Layers
        {
            get { return _layers; }
        }

        public bool[,] Synapses
        {
            get { return _synapses; }
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
            int len = _synapses.GetLength(0);

            List<Neuron> neurons = new List<Neuron>(len);
            Layer layer = _network.InitUnboundInputLayer(_layers[0]);
            neurons.AddRange(layer);
            for(int l = 1; l < _layers.Length; l++)
            {
                layer = _network.AddLayer(_layers[l]);
                neurons.AddRange(layer);
            }

            for(int from = 0; from < len; from++)
                for(int to = 0; to < len; to++)
                    if(_synapses[from, to])
                        neurons[from].ConnectToNeuron(neurons[to]);
        }
    }
}
