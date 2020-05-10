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

  public void SetDataList(int weaponIndex)
  {
    var weapon = FindObjectOfType<ArsenalController>().weapons[weaponIndex].GetComponent<WeaponController>();
    if (!weapon.unlocked)
    {
      SetUnlockPanel(weaponIndex);
      return;
    }
    transform.Find("WeaponSelect/UnlockPanel").gameObject.SetActive(false);
    var equipBtn = transform.Find("WeaponSelect/EquipBtn");
    var equipBtnButton = equipBtn.GetComponent<Button>();
    equipBtn.gameObject.SetActive(true);
    equipBtnButton.onClick.RemoveAllListeners();
    equipBtnButton.onClick.AddListener(() =>
    {
      FindObjectOfType<PlayerController>().Equip(FindObjectOfType<ArsenalController>().weapons[weaponIndex]);
    });
    var dataList = transform.Find("WeaponSelect/DataList");
    //DamageElement
    SetDataElement("Damage", weapon, weaponIndex, false);
    //MagazineElement
    SetDataElement("Damage", weapon, weaponIndex, false);
  }
  
  public void SetDataElement(string name, WeaponController weapon, int weaponIndex, bool refreshOnly = false)
  {
    var element = transform.Find("WeaponSelect/DataList/"+name+"Element");
    var elementBtn = element.Find("UpgradeBtn").GetComponent<Button>();

    if (!refreshOnly) elementBtn.onClick.RemoveAllListeners();
    var elemUpgradeTicks = element.GetComponentsInChildren<Transform>().Where(t => t.name == "UpgradeIndicator").ToArray();
    if (name == "Damage")
    {
      if (!refreshOnly) elementBtn.onClick.AddListener(() => { UpgradeDamage(weaponIndex); });
      elementBtn.transform.Find("Text").GetComponent<Text>().text = string.Format("Upgrade Lvl.{0} (λ{1})", (weapon.damageUpgrade + 1), (weapon.damageUpgrade + 1) * 75);
      for (int i = 0; i < weapon.damageUpgrade; i++) elemUpgradeTicks[i].GetComponent<Image>().color = Color.yellow;
    }
    else if (name == "Magazine")
    {
      if (!refreshOnly) elementBtn.onClick.AddListener(() => { UpgradeDamage(weaponIndex); });
      elementBtn.transform.Find("Text").GetComponent<Text>().text = string.Format("Upgrade Lvl.{0} (λ{1})", (weapon.magazineUpgrade + 1), (weapon.magazineUpgrade + 1) * 75);
      for (int i = 0; i < weapon.magazineUpgrade; i++) elemUpgradeTicks[i].GetComponent<Image>().color = Color.yellow;
    }
  }

  public void SetUnlockPanel(int weaponIndex)
  {
    var player = FindObjectOfType<PlayerController>();
    var unlockPanel = transform.Find("WeaponSelect/UnlockPanel");
    unlockPanel.gameObject.SetActive(true);
    transform.Find("WeaponSelect/EquipBtn").gameObject.SetActive(false);
    unlockPanel.Find("UnlockBtn").GetComponent<Button>().onClick.AddListener(() =>
    {
      if (!player.Pay(1000)) return;
      FindObjectOfType<ArsenalController>().weapons[weaponIndex].GetComponent<WeaponController>().unlocked = true;
      SetDataList(weaponIndex);
    });
  }

  public void ClosePanel()
  {
    FindObjectOfType<PlayerController>().SetState(PlayerState.Active);
  }


  public void UpgradeMagazine(int weaponIndex)
  {
    var weapon = FindObjectOfType<ArsenalController>().weapons[weaponIndex];
    var weaponController = weapon.GetComponent<WeaponController>();
    var player = FindObjectOfType<PlayerController>();

    if (weaponController.magazineUpgrade >= 5) return; //5 levels
    if (!player.Pay((weaponController.magazineUpgrade + 1) * 75)) return;
    weaponController.magazineUpgrade++;
    weaponController.UpdateUI();
    player.Equip(weapon);
    SetDataElement("Damage", weaponController, weaponIndex, true);
  }

  public void UpgradeDamage(int weaponIndex)
  {
    var weapon = FindObjectOfType<ArsenalController>().weapons[weaponIndex];
    var weaponController = weapon.GetComponent<WeaponController>();
    var player = FindObjectOfType<PlayerController>();

    if (weaponController.damageUpgrade >= 5) return; //5 levels
    if (!player.Pay((weaponController.damageUpgrade + 1) * 75)) return;
    weaponController.damageUpgrade++;
    weaponController.UpdateUI();
    player.Equip(weapon);
    SetDataElement("Damage", weaponController, weaponIndex, true);
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
