using System;

public class Neuron
{
  float _weight;
  public float Weight => _weight;

  public Neuron()
  {
    Random rand = new Random();
    _weight = (float)(rand.NextDouble() * 2 - 1);
  }

  public float Compute(float input) => input * _weight;

  public void Adjust(float error, float learningRate = 0.1f)
  {
    if (error > 0)
      _weight -= learningRate;
    else if (error < 0)
      _weight += learningRate;
      
  }
}