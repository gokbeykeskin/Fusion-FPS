using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 GetRandomSpawnPoint()
    {
        return new Vector3(Random.Range(-20, 20), 4, Random.Range(-20, 20));
    }

    public static Vector3 GetLobbySpawnPoint()
    {
        return new Vector3(100, 2, 100);
    }

    public static void SetRenderLayerInChildren(Transform transform, int layerNumber)
    {
        foreach(Transform trans in transform.gameObject.GetComponentsInChildren<Transform>(true))
        {
            if (trans.CompareTag("IgnoreLayerChange"))
            {

                continue;
            }
            trans.gameObject.layer = layerNumber;
        }
    }

}
