using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    public static TeleportationManager Instance;

    private HashSet<Transform> ignoreList = new HashSet<Transform>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool CanTeleport(Transform player)
    {
        return !ignoreList.Contains(player);
    }

    public void RegisterTeleport(Transform player, float ignoreDuration = 1f)
    {
        if (!ignoreList.Contains(player))
        {
            ignoreList.Add(player);
            StartCoroutine(RemoveFromIgnoreListAfterDelay(player, ignoreDuration));
        }
    }

    private IEnumerator RemoveFromIgnoreListAfterDelay(Transform player, float delay)
    {
        yield return new WaitForSeconds(delay);
        ignoreList.Remove(player);
    }
}
