using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private Vector3 initialPosition;

    [SerializeField] private Vector2[] listOfPoints_Automatic;
    [SerializeField] private Transform[] listOfPoints_Manual;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<PlatformOutliner>() != null)
        {
            initialPosition = GetComponent<PlatformOutliner>().pathPoints[0];
            GameObject go = Instantiate(enemyPrefab, initialPosition, Quaternion.identity, transform.GetChild(0));
            listOfPoints_Automatic = GetComponent<PlatformOutliner>().pathPoints;
            go.GetComponent<SnailBehaviour>().SetListPointOfPath(listOfPoints_Automatic);
        }
        else
        {
            initialPosition = transform.GetChild(0).position;
            GameObject go = Instantiate(enemyPrefab, initialPosition, Quaternion.identity, transform.GetChild(0));
            go.GetComponent<KoboldBehaviour>().SetListPointOfPath(listOfPoints_Manual);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
