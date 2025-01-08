using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<PlatformOutliner>() != null)
        {
            initialPosition = GetComponent<PlatformOutliner>().pathPoints[0];
            GameObject go = Instantiate(enemyPrefab, initialPosition, Quaternion.identity, transform.GetChild(0));
            go.GetComponent<SnailBehaviour>().SetListPointOfPath(GetComponent<PlatformOutliner>().pathPoints);
        }
        else
        {
            initialPosition = transform.GetChild(1).position;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
