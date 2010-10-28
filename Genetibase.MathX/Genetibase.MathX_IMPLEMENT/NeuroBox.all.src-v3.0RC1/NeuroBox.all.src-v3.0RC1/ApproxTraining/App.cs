using System;
using System.Collections.Generic;
using System.Text;

using NeuroBox;
using NeuroBox.FunctionFitting;

namespace NeuroBox.Demo.ApproxTraining
{
    class App
    {
        static double my_func(double[] x)
        {
            return 3 * x[0] + 5; 
        }

        static void Main(string[] args)
        {
            Controler ffc = new Controler(1);

            Network net = new Network(ffc.Node);
            net.InitUnboundInputLayer(1).BasicConfiguration.ActivationType.Value = EActivationType.Linear;
            net.AddLayer(1, EActivationType.Linear);
            net.AutoLinkFeedforward();

            Neuron neuron = net.LastLayer[0];
            Synapse synapse = neuron.SourceSynapses[0];

            ffc.ImportNetwork(net, false);

            BasicConfig config = ffc.NeuralNetwork.BasicConfiguration;
            config.BiasNeuronEnable.Value = true;
            config.BiasNeuronOutput.Value = 1.0;
            config.FlatspotEliminationEnable.Value = false;
            config.WeightDecayEnable.Value = false;
            config.SymmetryPreventionEnable.Value = false;
            config.ManhattanTrainingEnable.Value = false;
            config.LearningRate.Value = 0.005;

            StochasticCoordinateGenerator scg = new StochasticCoordinateGenerator(0,10,100);
            //RegularCoordinateGenerator rcg = new RegularCoordinateGenerator(-25, 25, 50);
            DynamicSampleProvider dsp = new DynamicSampleProvider(my_func, scg); //rcg);

            ffc.Provider = dsp; // new CachedSampleProvider(dsp);

            Console.WriteLine("TARGET FUNCTION:             3*x+5");
            Console.WriteLine("TARGET Synapse Weight      = 3.0");
            Console.WriteLine("TARGET Bias Weight         = 5.0");
            Console.WriteLine("TARGET Mean Squared Error <= 0.000000001");
            Console.WriteLine();

            Console.WriteLine("Synapse Weight: " + synapse.Weight + " - Bias Weight: " + neuron.BiasNeuronWeight);
            Console.WriteLine("Initial MSE: " + ffc.EstimateMeanSquaredError());
            Console.WriteLine();

            ffc.TrainAllSamplesOnce();
            Console.WriteLine("Synapse Weight: " + synapse.Weight + " - Bias Weight: " + neuron.BiasNeuronWeight);
            Console.WriteLine("Trained MSE: " + ffc.EstimateMeanSquaredError());
            Console.WriteLine();

            ffc.TrainAllSamplesOnce();
            Console.WriteLine("Synapse Weight: " + synapse.Weight + " - Bias Weight: " + neuron.BiasNeuronWeight);
            Console.WriteLine("Trained MSE: " + ffc.EstimateMeanSquaredError());
            Console.WriteLine();

            ffc.TrainAllSamplesOnce();
            Console.WriteLine("Synapse Weight: " + synapse.Weight + " - Bias Weight: " + neuron.BiasNeuronWeight);
            Console.WriteLine("Trained MSE: " + ffc.EstimateMeanSquaredError());
            Console.WriteLine();

            Console.WriteLine("Auto Training, maximum 1000 Epochs");
            Console.WriteLine();
            if(ffc.TrainAllSamplesUntil(0.000000001, 1000))
            {
                Console.WriteLine("Synapse Weight: " + synapse.Weight + " - Bias Weight: " + neuron.BiasNeuronWeight);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("SUCCEEDS auto training with MSE: " + ffc.EstimateMeanSquaredError());
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Synapse Weight: " + synapse.Weight + " - Bias Weight: " + neuron.BiasNeuronWeight);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("FAILS auto training with MSE: " + ffc.EstimateMeanSquaredError());
                Console.ResetColor();
            }

            Console.ReadKey();
        }
    }
}
