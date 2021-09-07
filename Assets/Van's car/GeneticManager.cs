using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
public class GeneticManager : MonoBehaviour
{
    [Header("References")]
    public CarControlled controller;

    [Header("Controls")]
    public int initialPopulation = 85;
    [Range(0.0f, 1.0f)]
    public float mutationRate = 0.055f;

    [Header("Crossover Controls")]
    public int bestAgentSelection = 10;
    public int worstAgentSelection = 4;
    public int numberToCrossover; //if we set 15, we will get 30 children

    private List<int> genePool = new List<int>(); //contain all the network that has been selected like bestAgentSelection and worstAgentSelection will be added 

    private int naturallySelected; //we will see how many need to be randomly generate

    private NeurolN[] population; //keeping track of population

    //Debug VIEW MODE
    [Header("Public View")]
    public int currentGeneration;
    public int currentGenome = 0;

    private void Start()
    {
        GeneratePopulation(); //randomize population
    }

    private void GeneratePopulation()
    {
        population = new NeurolN[initialPopulation];
        FillPopulationWithRandomValues(population, 0);
        ResetToCurrentGenome();
    }

    private void ResetToCurrentGenome()
    {
        controller.ResetNetwork(population[currentGenome]);
    }

    private void FillPopulationWithRandomValues(NeurolN[] newPopulation, int startingIndex)
    {
        while (startingIndex < initialPopulation)
        {
            newPopulation[startingIndex] = new NeurolN();
            newPopulation[startingIndex].Initialise(controller.Layers, controller.neurons);//initialize the network with this amount of LAYERS AND NEURONS
            startingIndex++;
        }
    }
}
