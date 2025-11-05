using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
  [SerializeField] private GameObject bulletPrefab;
  [SerializeField] private Slider powerSlider;
  [SerializeField] private GameObject predictionArrow;
  [SerializeField] private TextMeshProUGUI weightLabel;
  [SerializeField] private TextMeshProUGUI errorLabel;
  [SerializeField] private float powerMultiplier = 100f;

  public Neuron neuron { get; private set; }
  public Dictionary<float, float> shotResults = new();

  private void Awake() => neuron = new Neuron();

  public void Fire()
  {
    float power = powerSlider.value;
    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    Bullet bulletScript = bullet.GetComponent<Bullet>();

    bulletScript.Initialize(this, power, neuron);

    Vector2 direction = new(1f, 1f);
    rb.linearVelocity = direction.normalized * Mathf.Sqrt(power * powerMultiplier);
    bullet.transform.right = rb.linearVelocity;

    shotResults[power] = -1f;
    UpdatePredictionArrow();
  }

  public void RegisterHit(float power, float distance)
  {
    shotResults[power] = distance;

    float prediction = neuron.Compute(power);
    float error = prediction - distance;
    neuron.Adjust(error);
    errorLabel.text = $"Error = {error:F1}";
  }

  public void UpdatePredictionArrow()
  {
    float prediction = neuron.Compute(powerSlider.value);
    weightLabel.text = $"y = x * {neuron.Weight:F1}";
    predictionArrow.transform.position = new Vector3(transform.position.x + prediction, predictionArrow.transform.position.y);
  }
}