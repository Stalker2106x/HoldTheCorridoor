using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCreatures
{
  public Dictionary<CreatureType, int> creatures;

  public CreatureType GetRandom()
  {
    CreatureType type;
    do
    {
      type = (CreatureType)Random.Range(0, System.Enum.GetNames(typeof(CreatureType)).Length);

    } while (creatures[type] <= 0);
    creatures[type]--;
    return (type);
  }

  public bool IsEmpty()
  {
    foreach (var entry in creatures)
    {
      if (entry.Value > 0) return (false);
    }
    return (true);
  }

  public WaveCreatures()
  {
    creatures = new Dictionary<CreatureType, int>();
  }
}

public class WaveDefinition
{
  public WaveCreatures creatures;
  public float spawnDelay;

  public WaveDefinition(float spawnDelay_, int normalAmount, int rusherAmount, int juggernautAmount)
  {
    spawnDelay = spawnDelay_;
    creatures = new WaveCreatures();
    creatures.creatures.Add(CreatureType.Normal, normalAmount);
    creatures.creatures.Add(CreatureType.Rusher, rusherAmount);
    creatures.creatures.Add(CreatureType.Juggernaut, juggernautAmount);
  }
}

public enum CreatureType
{
  Normal,
  Rusher,
  Juggernaut
}

public class SpawnerController : MonoBehaviour
{
  [SerializeField]
  GameObject creaturePrefab;

  // Start is called before the first frame update
  void Start()
  {
  }

  public void Spawn(CreatureType type)
  {
    var creature = Instantiate(creaturePrefab, transform.position, Quaternion.identity);
    var creatureController = creature.GetComponent<CreatureController>();
    switch (type)
    {
      default:
      case CreatureType.Normal:
        creatureController.Init(60, 10, 2.5f);
        break;
      case CreatureType.Rusher:
        creatureController.Init(50, 10, 5f);
        break;
      case CreatureType.Juggernaut:
        creatureController.Init(180, 40, 1.5f);
        break;
    }
  }

  // Update is called once per frame
  void Update()
  {
  }
}
