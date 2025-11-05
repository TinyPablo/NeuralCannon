using System;

public class Neuron
{
  private float weight;
  public float Weight => weight;

  public Neuron()
  {
    Random rand = new Random();
    weight = (float)(rand.NextDouble() * 2 - 1);
  }

  public float Compute(float input) => input * weight;

  public void Adjust(float error, float learningRate = 0.1f)
  {
    if (error > 0)
      weight -= learningRate;
    else if (error < 0)
      weight += learningRate;
  }
}