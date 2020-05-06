using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
  [SerializeField]
  GameObject creaturePrefab;

  // Start is called before the first frame update
  void Start()
  {
  }

  public void Spawn()
  {
    Instantiate(creaturePrefab, transform.position, Quaternion.identity);
  }

  // Update is called once per frame
  void Update()
  {
  }
}
