using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MetaType
{
  Spawn,
  Clip
}

public class MetaPropController : MonoBehaviour
{
  [SerializeField]
  public MetaType type;
  // Start is called before the first frame update
  void Start()
  {
    GetComponent<MeshRenderer>().enabled = false;
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
