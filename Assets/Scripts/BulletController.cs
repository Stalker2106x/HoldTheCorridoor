using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
  public int damage;
  int _speed;
  // Start is called before the first frame update
  void Start()
  {
    _speed = 30;
  }

  private void OnCollisionEnter(Collision collision)
  {
    Destroy(gameObject);
  }

  // Update is called once per frame
  void Update()
  {
    transform.Translate(Vector3.forward * _speed * Time.deltaTime);
  }
}
