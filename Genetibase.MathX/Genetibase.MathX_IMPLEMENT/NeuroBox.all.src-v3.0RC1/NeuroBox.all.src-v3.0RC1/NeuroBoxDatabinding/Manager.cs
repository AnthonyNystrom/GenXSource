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
using System.IO;
using System.Collections;
using NeuroBox;

namespace NeuroBox.Repository
{
	/// <summary>
	/// Neural Network Repsoitory. This class manages the transformation
	/// between Neural Network instances and DataSet descriptions.
	/// </summary>
	public class Manager
	{
		NeuralDataSet database;
		Network currentNetwork = null;
		NeuralDataSet.NetworksRow currentNetworkRow = null;
		Hashtable currentNeuronMap = null;
		Hashtable currentSynapseMap = null;

		/// <summary>
		/// Create a manager with an empty repository.
		/// </summary>
		public Manager()
		{
			database = new NeuralDataSet();
		}
		/// <summary>
		/// Create a manager with the passed repository.
		/// </summary>
		/// <param name="repository">The repository to work with.</param>
		public Manager(NeuralDataSet repository)
		{
			database = repository;
		}

		/// <summary>
		/// Save the current repository to a file or textbased stream (e.g. StreamWriter or StringWriter) in clean XML.
		/// </summary>
		/// <param name="writer">A TextWriter that maps e.g. to a writable file stream.</param>
		public void WriteRepository(TextWriter writer)
		{
			database.WriteXml(writer,System.Data.XmlWriteMode.WriteSchema);
		}

		/// <summary>
		/// Save the current repository to a file.
		/// </summary>
		/// <param name="fileName">The full path for the new XML file. Recommended filename extension: ".xml.NB2"</param>
		public void WriteRepository(string fileName)
		{
			StreamWriter writer = new StreamWriter(fileName,false);
			WriteRepository(writer);
			writer.Close();
		}

		/// <summary>
		/// Create a new manager based on a repository available as an XML stream (e.g. StreamReader or StringReader).
		/// </summary>
		/// <param name="reader">A TextReader that maps e.g. to a readable file stream.</param>
		/// <returns>The manager instance bound to the loaded repository.</returns>
		public static Manager ReadRepository(TextReader reader)
		{
			NeuralDataSet database = new NeuralDataSet();
			database.ReadXml(reader,System.Data.XmlReadMode.IgnoreSchema);
			return new Manager(database);
		}

		/// <summary>
		/// Create a new manager based on a repository avaibale as an XML file.
		/// </summary>
		/// <param name="fileName">The full path to the XML file.</param>
		/// <returns>The manager instance bound to the loaded repository.</returns>
		public static Manager ReadRepository(string fileName)
		{
			StreamReader reader = new StreamReader(fileName);
			Manager m = ReadRepository(reader);
			reader.Close();
			return m;
		}

		/// <summary>
		/// Direct access to the repository (typed DataSet).
		/// </summary>
		public NeuralDataSet Repository
		{
			get {return database;}
		}

		/// <summary>
		/// The currently loaded neural network instance.
		/// </summary>
		public Network CurrentNetwork
		{
			get {return currentNetwork;}
		}

		/// <summary>
		/// The repository ID of the currently loaded neural network instance.
		/// </summary>
		public int CurrentNetworkID
		{
			get {return currentNetworkRow.nwID;}
		}

		/// <summary>
		/// The repository title of the currently loaded neural network instance.
		/// </summary>
		public string CurrentNetworkTitle
		{
			get {return currentNetworkRow.nw_name;}
			set {currentNetworkRow.nw_name = value;}
		}

		/// <summary>
		/// Load a network from the repository by its repository ID.
		/// </summary>
		/// <param name="networkID">The network's repository ID</param>
		public void LoadNetworkStructure(int networkID)
		{
			NeuralDataSet.NetworksRow[] rows = (NeuralDataSet.NetworksRow[])database.Networks.Select("nwID = " + networkID);
			if(rows.Length != 1)
				throw new ArgumentOutOfRangeException("networkId",networkID,"Network not found");
			currentNetworkRow = rows[0];
			RebuildCurrentNetworkStructure();
		}

		/// <summary>
		/// Load a network from the repository by its repository title.
		/// </summary>
		/// <param name="title">The network's repository title</param>
		public void LoadNetworkStructure(string title)
		{
			NeuralDataSet.NetworksRow[] rows = (NeuralDataSet.NetworksRow[])database.Networks.Select("nw_name LIKE '" + title.Trim() + "'");
			if(rows.Length == 0)
				throw new ArgumentOutOfRangeException("title",title,"Network not found");
			currentNetworkRow = rows[0];
			RebuildCurrentNetworkStructure();
		}

		/// <summary>
		/// Save the current network to the repository, name it with a new
		/// repository title and mark it as the current network. You SHOULD
		/// save a network after you alter its structure if you want to save
		/// any training sets trained with the new structre.
		/// </summary>
		/// <param name="title">The new network's repository title (choosable).</param>
		/// <returns>The new network's repository ID</returns>
		public int SaveNetworkStructure(string title)
		{
			return ImportNetworkStructure(title,currentNetwork,true);
		}

		/// <summary>
		/// Import a network to the repository and name it with a new repository title.
		/// </summary>
		/// <param name="title">The new network's repository title (choosable).</param>
		/// <param name="network">The neural network instance to import.</param>
		/// <param name="setAsCurrent">Whether the network should be marked as the current network.</param>
		/// <returns>The new network's repository ID</returns>
		public int ImportNetworkStructure(string title, Network network, bool setAsCurrent)
		{
			NeuralDataSet.NetworksRow row = database.Networks.AddNetworksRow(title);
			if(setAsCurrent)
			{
				SaveNetworkStructureAsCurrent(row, network);
				currentNetwork = network;
				currentNetworkRow = row;
			}
			else
				SaveNetworkStructure(row, network);
			return row.nwID;
		}

		/// <summary>
		/// Load a training set from the repository by its repository ID.
		/// </summary>
		/// <param name="trainingsetID">The training set's repository ID</param>
		public void LoadNetworkState(int trainingsetID)
		{
			NeuralDataSet.TrainingSetsRow[] rows = (NeuralDataSet.TrainingSetsRow[])database.TrainingSets.Select("trFK_network = " + currentNetworkRow.nwID + " AND trID = " + trainingsetID);
			if(rows.Length == 0)
				throw new ArgumentOutOfRangeException("trainingsetID",trainingsetID,"TrainingSet not found");
			LoadTrainingSet(rows[0]);
		}

		/// <summary>
		/// Load a training set from the repository by its repository title.
		/// </summary>
		/// <param name="title">The training set's repository title</param>
		public void LoadNetworkState(string title)
		{
			NeuralDataSet.TrainingSetsRow[] rows = (NeuralDataSet.TrainingSetsRow[])database.TrainingSets.Select("trFK_network = " + currentNetworkRow.nwID + " AND tr_name LIKE '" + title.Trim() + "'");
			if(rows.Length == 0)
				throw new ArgumentOutOfRangeException("title",title,"TrainingSet not found");
			LoadTrainingSet(rows[0]);
		}

		/// <summary>
		/// Save the current training to the repository and name it with
		/// a new repository title
		/// </summary>
		/// <param name="title">The new training set's repository title (choosable).</param>
		/// <returns>The new training set's repository ID</returns>
		public int SaveNetworkState(string title)
		{
			NeuralDataSet.TrainingSetsRow row = database.TrainingSets.AddTrainingSetsRow(currentNetworkRow,title);
			SaveTrainingSet(row);
			return row.trID;
		}

		#region Private Helper Functions
		private void RebuildCurrentNetworkStructure()
		{
			currentNetwork = new Network();
			currentNeuronMap = new Hashtable();
			currentSynapseMap = new Hashtable();

			//Build Layers and Neurons
			NeuralDataSet.LayersRow[] layerRows = SelectLayersFromNetwork(currentNetworkRow);
			if(layerRows.Length == 0)
				return;
			NeuralDataSet.NeuronsRow[] neuronRows = SelectNeuronsFromLayer(layerRows[0]);
			currentNetwork.InitUnboundInputLayer(neuronRows.Length);
			Layer[] layers = new Layer[layerRows.Length];
			layers[0] = currentNetwork.FirstLayer;
			AppendNeuronsToNeuronMap(currentNeuronMap,neuronRows,layers[0]);
			for(int i=1;i<layerRows.Length;i++)
			{
				neuronRows = SelectNeuronsFromLayer(layerRows[i]);
				layers[i] = currentNetwork.AddLayer(neuronRows.Length);
				AppendNeuronsToNeuronMap(currentNeuronMap,neuronRows,layers[i]);
			}

			//Build Synapses
			NeuralDataSet.SynapsesRow[] synapseRows = SelectSynapsesFromNetwork(currentNetworkRow);
			for(int i=0;i<synapseRows.Length;i++)
				currentSynapseMap.Add(synapseRows[i].syID,
					((Neuron)currentNeuronMap[synapseRows[i].syFK_neuronSource]).ConnectToNeuron((Neuron)currentNeuronMap[synapseRows[i].syFK_neuronTarget]));
		}

		private void AppendNeuronsToNeuronMap(Hashtable map, NeuralDataSet.NeuronsRow[] rows, Layer layer)
		{
			for(int i=0;i<rows.Length;i++)
				map.Add(rows[i].neID,layer[i]);
		}

		private void SaveNetworkStructure(NeuralDataSet.NetworksRow networkRow, Network network)
		{
			Hashtable neuronInvMap = new Hashtable();
			Layer layer = network.FirstLayer;
			SaveLayer(neuronInvMap,layer,networkRow);
			while(layer.TargetLayer != null)
			{
				layer = layer.TargetLayer;
				SaveLayer(neuronInvMap,layer,networkRow);
			}
			foreach(Neuron neuron in neuronInvMap.Keys)
				foreach(Synapse synapse in neuron.SourceSynapses)
					database.Synapses.AddSynapsesRow((NeuralDataSet.NeuronsRow)neuronInvMap[synapse.SourceNeuron],(NeuralDataSet.NeuronsRow)neuronInvMap[neuron],networkRow);
		}

		private void SaveNetworkStructureAsCurrent(NeuralDataSet.NetworksRow networkRow, Network network)
		{
			Hashtable neuronInvMap = new Hashtable();
			currentNeuronMap = new Hashtable();
			currentSynapseMap = new Hashtable();
			Layer layer = network.FirstLayer;
			SaveLayerAsCurrent(neuronInvMap,layer,networkRow);
			while(layer.TargetLayer != null)
			{
				layer = layer.TargetLayer;
				SaveLayerAsCurrent(neuronInvMap,layer,networkRow);
			}
			foreach(Neuron neuron in neuronInvMap.Keys)
				foreach(Synapse synapse in neuron.SourceSynapses)
				{
					NeuralDataSet.SynapsesRow row = database.Synapses.AddSynapsesRow((NeuralDataSet.NeuronsRow)neuronInvMap[synapse.SourceNeuron],(NeuralDataSet.NeuronsRow)neuronInvMap[neuron],networkRow);
					currentSynapseMap.Add(row.syID,synapse);
				}
		}

		private void SaveLayer(Hashtable neuronInvMap, Layer layer, NeuralDataSet.NetworksRow networkRow)
		{
			NeuralDataSet.LayersRow layerRow = database.Layers.AddLayersRow(networkRow,layer.Title);
			for(int i=0;i<layer.Count;i++)
				neuronInvMap.Add(layer[i],database.Neurons.AddNeuronsRow(layerRow));
		}
		private void SaveLayerAsCurrent(Hashtable neuronInvMap, Layer layer, NeuralDataSet.NetworksRow networkRow)
		{
			NeuralDataSet.LayersRow layerRow = database.Layers.AddLayersRow(networkRow,layer.Title);
			for(int i=0;i<layer.Count;i++)
			{
				NeuralDataSet.NeuronsRow row = database.Neurons.AddNeuronsRow(layerRow);
				neuronInvMap.Add(layer[i],row);
				currentNeuronMap.Add(row.neID,layer[i]);
			}
		}

		private void LoadTrainingSet(NeuralDataSet.TrainingSetsRow trainingSetRow)
		{
			NeuralDataSet.TrainingItemRow[] itemRows = SelectTrainingItemsFromTrainingSet(trainingSetRow);
			for(int i=0;i<itemRows.Length;i++)
				((Synapse)currentSynapseMap[itemRows[i].tiFK_synapse]).Weight = itemRows[i].ti_weight;
		}

		private void SaveTrainingSet(NeuralDataSet.TrainingSetsRow trainingSetRow)
		{
			NeuralDataSet.SynapsesRow[] synapseRows = SelectSynapsesFromNetwork(currentNetworkRow);
			for(int i=0;i<synapseRows.Length;i++)
				database.TrainingItem.AddTrainingItemRow(trainingSetRow,synapseRows[i],((Synapse)currentSynapseMap[synapseRows[i].syID]).Weight);
		}

		private NeuralDataSet.LayersRow[] SelectLayersFromNetwork(NeuralDataSet.NetworksRow network)
		{
			return (NeuralDataSet.LayersRow[])database.Layers.Select("laFK_network = " + network.nwID);
		}
		private NeuralDataSet.NeuronsRow[] SelectNeuronsFromLayer(NeuralDataSet.LayersRow layer)
		{
            return (NeuralDataSet.NeuronsRow[])database.Neurons.Select("neFK_layer = " + layer.laID);
		}
		private NeuralDataSet.SynapsesRow[] SelectSynapsesFromNetwork(NeuralDataSet.NetworksRow network)
		{
			return (NeuralDataSet.SynapsesRow[])database.Synapses.Select("syFK_network = " + network.nwID);
		}
		private NeuralDataSet.TrainingItemRow[] SelectTrainingItemsFromTrainingSet(NeuralDataSet.TrainingSetsRow trainingSet)
		{
			return (NeuralDataSet.TrainingItemRow[])database.TrainingItem.Select("tiFK_trainingset = " + trainingSet.trID);
		}
		#endregion
		
	}
}
