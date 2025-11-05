using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Canon : MonoBehaviour
{
  [SerializeField] GameObject bulletPrefab;
  [SerializeField] Slider slider;
  [SerializeField] GameObject bulletPointer;
  [SerializeField] private TextMeshProUGUI functionText;

  [SerializeField] float bulletSpeed;
  public float magicNumber = 100f;

  public Neuron neuron;
  public Dictionary<float, float> shootingData = new();
  public float neuronWeight => neuron.Weight;

  private void Awake()
  {
    neuron = new();
  }

  public void Fire()
  {
    GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    var rb = b.GetComponent<Rigidbody2D>();
    var bullet = b.GetComponent<Bullet>();

    bullet.Init(this, slider.value, neuron);

    Vector2 dir = new Vector2(1f, 1f).normalized;
    rb.linearVelocity = dir * Mathf.Sqrt(slider.value * magicNumber);
    b.transform.right = rb.linearVelocity;

    shootingData[slider.value] = -1f;

    MoveBulletPointerToPredictedPos();
  }

  public void RegisterImpact(float sliderValue, float dist)
  {
    shootingData[sliderValue] = dist;
    float pred = neuron.Compute(sliderValue);
    float error = pred - dist;
    neuron.Adjust(error);
  }

  public void MoveBulletPointerToPredictedPos()
  {
    float pred = neuron.Compute(slider.value);
    functionText.text = $"Y = X * {neuronWeight:F2}";
    bulletPointer.transform.position = new Vector3(transform.position.x + pred, bulletPointer.transform.position.y);
  }
}