using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsenalController : MonoBehaviour
{
  public List<GameObject> weapons;
  // Start is called before the first frame update
  void Start()
  {
  }

  public void Init()
  {
    AddWeapon(Resources.Load<GameObject>("Weapons/glock"), true);
    AddWeapon(Resources.Load<GameObject>("Weapons/mac10"), false);
    AddWeapon(Resources.Load<GameObject>("Weapons/nova"), false);
  }

  public void AddWeapon(GameObject weapon, bool unlocked)
  {
    GameObject weaponObj = Instantiate<GameObject>(weapon, transform, false);
    var weaponController = weaponObj.GetComponent<WeaponController>();
    weaponObj.gameObject.name = "Weapon";
    weaponController.unlocked = unlocked;
    weaponObj.SetActive(false);
    weapons.Add(weaponObj);
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
