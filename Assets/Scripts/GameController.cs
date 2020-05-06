using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  List<System.Tuple<float, int>> wavesDefinition = new List < System.Tuple<float, int> >
  {
    new System.Tuple<float, int>(0f, 0),
    new System.Tuple<float, int>(2f, 10),
    new System.Tuple<float, int>(1.8f, 15),
    new System.Tuple<float, int>(1.6f, 15),
    new System.Tuple<float, int>(1.4f, 20),
    new System.Tuple<float, int>(1.2f, 25),
    new System.Tuple<float, int>(1f, 25),
    new System.Tuple<float, int>(0.8f, 30),
    new System.Tuple<float, int>(0.5f, 50)
  };

  int _wave;
  int _creaturesToSpawn;
  bool _interwave;
  float _waveTimer;
  // Start is called before the first frame update
  void Start()
  {
    ResetGame();
  }

  public void ResetGame()
  {
    var creatures = FindObjectsOfType<CreatureController>();
    for (var i = creatures.Length - 1; i >= 0; i--)
    {
      Destroy(creatures[i].gameObject);
    }
    FindObjectOfType<UIController>().SetGameOverUI(false);
    FindObjectOfType<PlayerController>().Init();
    _wave = 0;
    EndWave();
  }

  public void SetLights()
  {
    if (_interwave)
    {
      GameObject.Find("Level/NeonLeft/Light").GetComponent<Light>().color = Color.white;
      GameObject.Find("Level/NeonRight/Light").GetComponent<Light>().color = Color.white;
    }
    else
    {
      GameObject.Find("Level/NeonLeft/Light").GetComponent<Light>().color = Color.red;
      GameObject.Find("Level/NeonRight/Light").GetComponent<Light>().color = Color.red;
    }
  }

  public void SetWave()
  {
    if (!_interwave) return;
    _interwave = false;
    _creaturesToSpawn = wavesDefinition[_wave].Item2;
    _waveTimer = wavesDefinition[_wave].Item1;
    FindObjectOfType<UIController>().SetStatusText(string.Format("Wave {0} inbound", _wave));
    SetLights();
  }

  public void EndWave()
  {
    _wave++;
    _interwave = true;
    FindObjectOfType<UIController>().SetStatusText("Chilling in the corridor");
    SetLights();
  }

  // Update is called once per frame
  void Update()
  {
    if (_interwave) return;
    _waveTimer -= Time.deltaTime;
    if (_waveTimer <= 0)
    {
      if (_creaturesToSpawn > 0)
      {
        var spawners = FindObjectsOfType<SpawnerController>();
        _creaturesToSpawn--;
        spawners[Random.Range(0, spawners.Length)].Spawn();
        _waveTimer = 2;
      }
    }
    if (_creaturesToSpawn <= 0 && FindObjectsOfType<CreatureController>().Length <= 0) EndWave();
  }
}
