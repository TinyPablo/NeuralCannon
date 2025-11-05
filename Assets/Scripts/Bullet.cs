using UnityEngine;

public class Bullet : MonoBehaviour
{
  private Cannon cannon;
  private float power;
  private Neuron neuron;

  public void Initialize(Cannon cannonRef, float powerValue, Neuron neuronRef)
  {
    cannon = cannonRef;
    power = powerValue;
    neuron = neuronRef;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (!collision.collider.CompareTag("Ground")) return;

    float distance = transform.position.x - cannon.transform.position.x;
    cannon.RegisterHit(power, distance);

    // Debug.Log($"Predicted: {neuron.Compute(power)} | Power: {power} | Distance: {distance}");
    Destroy(gameObject);
  }
}