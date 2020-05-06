using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WorkbenchController : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
  }

  public void RefreshPanel()
  {
    SetDataList();
  }

  public void SetDataList()
  {
    var weapon = FindObjectOfType<ArsenalController>().weapons[0].GetComponent<WeaponController>();
    var dataList = transform.Find("WeaponSelect/DataList");
    //DamageElement
    var damageElement = dataList.transform.Find("DamageElement");
    damageElement.Find("UpgradeBtn/Text").GetComponent<Text>().text = string.Format("Upgrade (λ{0})", (weapon.magazineUpgrade + 1) * 75);
    var damageUpgrades = damageElement.GetComponentsInChildren<Transform>().Where(t => t.name == "UpgradeIndicator").ToArray();
    for (int i = 0; i < weapon.damageUpgrade; i++) damageUpgrades[i].GetComponent<Image>().color = Color.green;
    //MagazineElement
    var magazineElement = dataList.transform.Find("MagazineElement");
    magazineElement.Find("UpgradeBtn/Text").GetComponent<Text>().text = string.Format("Upgrade (λ{0})", (weapon.magazineUpgrade + 1) * 75);
    var magazineUpgrades = magazineElement.GetComponentsInChildren<Transform>().Where(t => t.name == "UpgradeIndicator").ToArray();
    for (int i = 0; i < weapon.magazineUpgrade; i++) magazineUpgrades[i].GetComponent<Image>().color = Color.green;
  }

  public void ClosePanel()
  {
    FindObjectOfType<PlayerController>().SetState(PlayerState.Active);
  }


  public void UpgradeMagazine()
  {
    var weapon = FindObjectOfType<ArsenalController>().weapons[0].GetComponent<WeaponController>();
    var player = FindObjectOfType<PlayerController>();

    if (!player.Pay((weapon.magazineUpgrade + 1) * 75)) return;
    weapon.magazineUpgrade++;
    weapon.UpdateUI();
    RefreshPanel();
  }
  public void UpgradeDamage()
  {
    var weapon = FindObjectOfType<ArsenalController>().weapons[0].GetComponent<WeaponController>();
    var player = FindObjectOfType<PlayerController>();

    if (!player.Pay((weapon.damageUpgrade + 1) * 75)) return;
    weapon.damageUpgrade++;
    weapon.UpdateUI();
    RefreshPanel();
  }
  public void UpgradeSpeed()
  {
    var player = FindObjectOfType<PlayerController>();

    if (!player.Pay((player.speedUpgrade + 1) * 75)) return;
    FindObjectOfType<PlayerController>().speedUpgrade++;
    RefreshPanel();
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
