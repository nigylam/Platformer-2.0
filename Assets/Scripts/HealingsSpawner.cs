using UnityEngine;

public class HealingsSpawner : MonoBehaviour
{
    [SerializeField] Healing[] _healings;

    private void OnEnable()
    {
        int randomEnabledHealing = Random.Range(0, _healings.Length);
        _healings[randomEnabledHealing].gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        foreach(var healing in _healings)
            healing.gameObject.SetActive(false);
    }
}
