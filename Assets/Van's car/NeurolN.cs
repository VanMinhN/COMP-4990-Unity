using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using System;
public class NeurolN : MonoBehaviour
{
	public Matrix<float> inputLayer = Matrix<float>.Build.Dense(1, 3);

	public List<Matrix<float>> hiddenLayer = new List<Matrix<float>>();

	public Matrix<float> outputLayer = Matrix<float>.Build.Dense(1, 2);

	public List<Matrix<float>> weights = new List<Matrix<float>>();

	public List<float> biases = new List<float>();

	public float fitness;

	public void Initialise(int hiddenLayerCount, int hiddenNeurolCount)
	{
		inputLayer.Clear();
		hiddenLayer.Clear();
		outputLayer.Clear();
		weights.Clear();
		biases.Clear();
		for (int i = 0; i < hiddenLayerCount + 1; i++)
		{
			Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeurolCount);
			hiddenLayer.Add(f);
			biases.Add(UnityEngine.Random.Range(-1f, 1f));
			//Weights
			if (i == 0)
			{
				Matrix<float> inputToH1 = Matrix<float>.Build.Dense(3, hiddenNeurolCount);
				weights.Add(inputToH1);
			}
			Matrix<float> HiddenLToHiddenAL = Matrix<float>.Build.Dense(hiddenNeurolCount, hiddenNeurolCount); //from hidden layer to another hidden layer
			weights.Add(HiddenLToHiddenAL);

		}
		Matrix<float> OutputWeight = Matrix<float>.Build.Dense(hiddenNeurolCount, 2);
		weights.Add(OutputWeight);
		biases.Add(UnityEngine.Random.Range(-1f, 1f));

		RandomizeWeights();
	}

	public NeurolN InitialiseCopy(int hiddenLayerCount, int hiddenNeuronCount)
	{
		NeurolN n = new NeurolN();

		List<Matrix<float>> newWeights = new List<Matrix<float>>();

		for (int i = 0; i < this.weights.Count; i++)
		{
			Matrix<float> currentWeight = Matrix<float>.Build.Dense(weights[i].RowCount, weights[i].ColumnCount);

			for (int x = 0; x < currentWeight.RowCount; x++)
			{
				for (int y = 0; y < currentWeight.ColumnCount; y++)
				{
					currentWeight[x, y] = weights[i][x, y];
				}
			}

			newWeights.Add(currentWeight);
		}

		List<float> newBiases = new List<float>();

		newBiases.AddRange(biases);

		n.weights = newWeights;
		n.biases = newBiases;

		n.InitialiseHidden(hiddenLayerCount, hiddenNeuronCount);

		return n;
	}
	// initalize hidden layer
	public void InitialiseHidden(int hiddenLayerCount, int hiddenNeuronCount)
	{
		inputLayer.Clear();
		hiddenLayer.Clear();
		outputLayer.Clear();

		for (int i = 0; i < hiddenLayerCount + 1; i++)
		{
			Matrix<float> newHiddenLayer = Matrix<float>.Build.Dense(1, hiddenNeuronCount);
			hiddenLayer.Add(newHiddenLayer);
		}

	}
	//Make sure the weights are randomize
	public void RandomizeWeights()
	{
		for (int i = 0; i < weights.Count; i++)
		{
			for (int j = 0; j < weights[i].RowCount; j++)
			{
				for (int k = 0; k < weights[i].ColumnCount; k++)
				{
					weights[i][j, k] = UnityEngine.Random.Range(-1f, 1f);
				}
			}
		}
	}

	//return a copy of current neurol network
	public NeurolN InitalliseCopy(int hiddenLayerCount, int hiddenNeuronCount)
    {
		NeurolN n = new NeurolN();
		List<Matrix<float>> newWeights = new List<Matrix<float>>();

        for (int i=0;i < this.weights.Count;i++)
        {
			Matrix<float> currentWeights = Matrix<float>.Build.Dense(weights[i].RowCount,weights[i].ColumnCount);
            for (int x=0;x<currentWeights.RowCount;x++)
            {
                for (int y=0;y<currentWeights.ColumnCount;y++)
                {
					currentWeights[x, y] = weights[i][x, y];
                }
            }
			newWeights.Add(currentWeights);
        }
		List<float> newBiases = new List<float>();
		newBiases.AddRange(biases);

		n.weights = newWeights;
		n.biases = newBiases;

		n.InitialiseHidden(hiddenLayerCount, hiddenNeuronCount);

		return n;
	}

	//(float, float) notation is actually a struct
	/*instead of 
	struct a{
		float a,b;
	}
	public a RunNetwork(float a, float b, float c)
	we can just use 
	public (float, float) RunNetwork(float a, float b, float c)
	*/
	public (float, float) RunNetwork(float a, float b, float c)
	{
		inputLayer[0, 0] = a;
		inputLayer[0, 1] = b;
		inputLayer[0, 2] = c;

		inputLayer = inputLayer.PointwiseTanh();
		hiddenLayer[0] = ((inputLayer * weights[0]) + biases[0]).PointwiseTanh();

		for (int i = 1; i < hiddenLayer.Count; i++)
		{
			hiddenLayer[i] = ((hiddenLayer[i - 1] * weights[i]) + biases[i]).PointwiseTanh();
		}
		outputLayer = ((hiddenLayer[hiddenLayer.Count - 1] * weights[weights.Count - 1]) + biases[biases.Count - 1]).PointwiseTanh(); //adding last hiddenlayer

		//First output is speed and second is turing point
		return (Sigmoid(outputLayer[0, 0]), (float)Math.Tanh(outputLayer[0, 1]));
	}

	private float Sigmoid(float s)
	{
		return (1 / (1 + Mathf.Exp(-s)));
	}
}
