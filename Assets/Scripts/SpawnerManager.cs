using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using UnityEngine;

public class SpawnerManager : NetworkBehaviour
{
    [SerializeField]
    private GameObject _SpawnPrefab;
    [SerializeField]
    private KeyCode _SpawnKey;
    [SerializeField]
    private TextMeshProUGUI _SpawnCountText;

    private readonly SyncVar<int> _spawnedCount = new(0);

    public override void OnStartNetwork()
    {
        _spawnedCount.OnChange += (prev, next, asServer) =>
        {
            _SpawnCountText.text = $"Spawned: {next}";
        };
    }

    private void Update()
    {
        if (!IsServerStarted)
        {
            return;
        }

        if (Input.GetKeyDown(_SpawnKey))
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        Vector3 position = new(Random.Range(-20f, 20f), Random.Range(-15f, 15f));
        Spawn(Instantiate(_SpawnPrefab, position, Quaternion.identity));

        ++_spawnedCount.Value;
    }
}