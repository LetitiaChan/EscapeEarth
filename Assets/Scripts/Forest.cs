using UnityEngine;

public class Forest : MonoBehaviour
{
    public float startLength = 50;
    public float minLength = 100;
    public float maxLength = 200;

    public GameObject[] obstacles;

    private Transform player;
    private WayPoints waypoints;
    private int targetWayPointIndex = 0;

    void Awake()
    {
        GameObject playerGo = GameObject.FindGameObjectWithTag(Tags.player);
        if (playerGo != null)
        {
            player = playerGo.transform;
        }
        waypoints = transform.FindChild("waypoints").GetComponent<WayPoints>();
        targetWayPointIndex = waypoints.waypoints.Length - 2;
    }

    void Start()
    {
        GenerateObstacles();
    }

    void GenerateObstacles()
    {
        float z = startLength;
        while (true)
        {
            float length = Random.Range(minLength, maxLength);
            z += length;
            if (z > 3000) break;
            Vector3 waypoint = GetWayPoint(z);
            GenerateObstacles(waypoint);
        }
    }

    public Vector3 GetNextWayPoint()
    {
        while (true)
        {
            print(waypoints.waypoints[targetWayPointIndex].position.z - player.position.z);
            if (waypoints.waypoints[targetWayPointIndex].position.z - player.position.z < 0.5f)
            {
                targetWayPointIndex--;
                if (targetWayPointIndex < 0)
                {
                    targetWayPointIndex = 0;

                    Destroy(this.gameObject,0.5f);
                    Camera.main.SendMessage("UpdateForest", SendMessageOptions.DontRequireReceiver);
                    return waypoints.waypoints[0].position;
                }
            }
            else
            {
                return waypoints.waypoints[targetWayPointIndex].position;
            }
        }
    }

    void GenerateObstacles(Vector3 position)
    {
        int index = Random.Range(0, obstacles.Length);
        GameObject go = Instantiate(obstacles[index], position, Quaternion.identity) as GameObject;
        go.transform.parent = transform;
    }

    Vector3 GetWayPoint(float z)
    {
        Transform[] wps = waypoints.waypoints;
        int index = GetIndex(z);

        return Vector3.Lerp(wps[index + 1].position, wps[index].position,
            (z + wps[wps.Length - 1].position.z - wps[index + 1].position.z) / (wps[index].position.z - wps[index + 1].position.z));
    }

    int GetIndex(float z)
    {
        Transform[] wps = waypoints.waypoints;
        float startZ = wps[wps.Length - 1].position.z;
        int index = 0;
        for (int i = 0; i < wps.Length; i++)
        {
            if (wps[i].position.z - startZ >= z)
            {
                index = i;
            }
            else
            {
                break;
            }
        }
        return index;
    }

}
