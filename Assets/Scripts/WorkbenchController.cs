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
    ResetCheckStates(weaponIndex);
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
    var damageElement = dataList.transform.Find("DamageElement");
    var damageElementBtn = damageElement.Find("UpgradeBtn").GetComponent<Button>();

    damageElementBtn.onClick.RemoveAllListeners();
    damageElementBtn.onClick.AddListener(() =>
    {
      UpgradeDamage(weaponIndex);
    });
    damageElement.Find("UpgradeBtn/Text").GetComponent<Text>().text = string.Format("Upgrade Lvl.{0} (λ{1})", (weapon.magazineUpgrade + 1), (weapon.damageUpgrade + 1) * 75);
    var damageUpgrades = damageElement.GetComponentsInChildren<Transform>().Where(t => t.name == "UpgradeIndicator").ToArray();
    for (int i = 0; i < weapon.damageUpgrade; i++) damageUpgrades[i].GetComponent<Image>().color = Color.yellow;
    //MagazineElement
    var magazineElement = dataList.transform.Find("MagazineElement");

    var magazineElementBtn = magazineElement.Find("UpgradeBtn").GetComponent<Button>();

    magazineElementBtn.onClick.RemoveAllListeners();
    magazineElementBtn.onClick.AddListener(() =>
    {
      UpgradeMagazine(weaponIndex);
    });
    magazineElement.Find("UpgradeBtn/Text").GetComponent<Text>().text = string.Format("Upgrade Lvl.{0} (λ{1})", (weapon.magazineUpgrade + 1), (weapon.magazineUpgrade + 1) * 75);
    var magazineUpgrades = magazineElement.GetComponentsInChildren<Transform>().Where(t => t.name == "UpgradeIndicator").ToArray();
    for (int i = 0; i < weapon.magazineUpgrade; i++) magazineUpgrades[i].GetComponent<Image>().color = Color.yellow;
  }

  public void SetUnlockPanel(int weaponIndex)
  {
    var player = FindObjectOfType<PlayerController>();
    var unlockPanel = transform.Find("WeaponSelect/UnlockPanel");
    unlockPanel.gameObject.SetActive(true);
    transform.Find("WeaponSelect/EquipBtn").gameObject.SetActive(false);
    unlockPanel.Find("UnlockBtn").GetComponent<Button>().onClick.AddListener(() =>
    {
      if (!player.Pay(2500)) return;
      FindObjectOfType<ArsenalController>().weapons[weaponIndex].GetComponent<WeaponController>().unlocked = true;
      SetDataList(weaponIndex);
    });
  }

  public void ResetCheckStates(int weaponIndex)
  {
    var scrollView = transform.Find("/WeaponSelect/WeaponScroll/Viewport/Content");
    int index = 0;
    foreach (Transform child in scrollView)
    {
      child.GetComponent<Toggle>().isOn = (index == weaponIndex);
      index++;
    }
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
    SetDataList(weaponIndex);
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
    SetDataList(weaponIndex);
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
