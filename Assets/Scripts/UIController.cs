﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
  Text _statusText;
  Text _healthIndicator;
  Text _resourcesIndicator;
  Text _ammoIndicator;
  GameObject _gameMenu;
  GameObject _gameOverUI;
  GameObject _workbenchUI;

  // Start is called before the first frame update
  void Start()
  {
    _statusText = transform.Find("StatusIndicator").GetComponent<Text>();
    _healthIndicator = transform.Find("HealthIndicator").GetComponent<Text>();
    _resourcesIndicator = transform.Find("ResourcesIndicator").GetComponent<Text>();
    _ammoIndicator = transform.Find("AmmoIndicator").GetComponent<Text>();
    _gameMenu = transform.Find("GameMenu").gameObject;
    _gameOverUI = transform.Find("GameOverUI").gameObject;
    _workbenchUI = transform.Find("WorkbenchUI").gameObject;
  }
  public void SetMusicVolume(float volume)
  {
    Camera.main.GetComponent<AudioSource>().volume = volume;
  }

  public void SetStatusText(string text)
  {
    _statusText.text = text;
  }

  public void SetAmmoIndicator(int ammo, int magSize)
  {
    _ammoIndicator.text = ammo+"/"+magSize;
  }
  public void SetHealthIndicator(int value)
  {
    _healthIndicator.text = "+"+value;
  }
  public void SetResourcesIndicator(int value)
  {
    _resourcesIndicator.text = "λ " + value;
  }
  public void SetGameOverUI(bool active)
  {
    _gameOverUI.SetActive(active);
  }
  public void SetWorkbenchUI(bool active)
  {
    _workbenchUI.SetActive(active);
    if (active)
    {
      _workbenchUI.GetComponent<WorkbenchController>().SetDataList(0);

    }
    else _workbenchUI.GetComponent<WorkbenchController>().ClosePanel();
  }

  public void ExitGame()
  {
    Application.Quit();
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (_gameMenu.activeSelf)
      {
        _gameMenu.SetActive(false);
        FindObjectOfType<PlayerController>().SetState(PlayerState.Active);
      }
      else
      {
        _gameMenu.SetActive(true);
        FindObjectOfType<PlayerController>().SetState(PlayerState.Inactive);
      }
    }
  }
}
