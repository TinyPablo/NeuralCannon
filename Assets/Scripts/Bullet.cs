using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
  private Canon canon;
  private float sliderValue;
  private Neuron neuron;

  public void Init(Canon canonRef, float sliderVal, Neuron neuronRef)
  {
    neuron = neuronRef;
    canon = canonRef;
    sliderValue = sliderVal;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.CompareTag("Ground"))
    {
      float dist = transform.position.x - canon.transform.position.x;
      canon.RegisterImpact(sliderValue, dist);
      Debug.Log($"pred: {neuron.Compute(sliderValue)} for: {sliderValue} dist: {dist}");
      
      Destroy(gameObject);
    }
  }
}