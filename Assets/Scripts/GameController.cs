using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  List<WaveDefinition> wavesDefinition = new List <WaveDefinition>
  {
    new WaveDefinition(0f, 0, 0, 0),
    new WaveDefinition(2f, 10, 0, 0),
    new WaveDefinition(1.8f, 15, 5, 0),
    new WaveDefinition(1.7f, 20, 5, 0),
    new WaveDefinition(1.6f, 25, 5, 1),
    new WaveDefinition(1.5f, 0, 15, 2),
    new WaveDefinition(1.4f, 35, 0, 5),
    new WaveDefinition(1.3f, 50, 15, 0),
    new WaveDefinition(1.2f, 50, 25, 5),
    new WaveDefinition(1.1f, 75, 25, 0),
    new WaveDefinition(1f, 100, 50, 10)
  };

  int _wave;
  bool _interwave;
  WaveCreatures _waveCreatures;
  float _waveTimer;
  // Start is called before the first frame update
  void Start()
  {
    ResetGame();
  }

  public void ResetGame()
  {
    FindObjectOfType<ArsenalController>().Init();
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
    _waveCreatures = wavesDefinition[_wave].creatures;
    _waveTimer = wavesDefinition[_wave].spawnDelay;
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
      if (!_waveCreatures.IsEmpty())
      {
        var spawners = FindObjectsOfType<SpawnerController>();
        spawners[Random.Range(0, spawners.Length)].Spawn(_waveCreatures.GetRandom());
        _waveTimer = 2;
      }
    }
    if (_waveCreatures.IsEmpty() && FindObjectsOfType<CreatureController>().Length <= 0) EndWave();
  }
}
