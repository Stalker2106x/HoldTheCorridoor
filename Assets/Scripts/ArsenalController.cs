using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsenalController : MonoBehaviour
{
  [SerializeField]
  public List<GameObject> weapons;
  // Start is called before the first frame update
  void Start()
  {
    foreach (var weapon in weapons)
    {
      var controller = weapon.GetComponent<WeaponController>();
      controller.damageUpgrade = 0;
      controller.magazineUpgrade = 0;
    }
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
