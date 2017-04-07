using UnityEngine;

/// <summary>
/// 循环随机生成跑道
/// </summary>
public class EnvGenerator : MonoBehaviour
{
    public GameObject[] forests;
    public Forest forest1;
    public Forest forest2;

    private int forestCount = 2;

    Forest GenerateForest()
    {
        forestCount++;
        float z = 3000 * forestCount;
        int index = Random.Range(0, 3);//0 1 2 
        GameObject go = GameObject.Instantiate(forests[index], new Vector3(0, 0, z), Quaternion.identity) as GameObject;
        return go.GetComponent<Forest>();
    }

    public void UpdateForest()
    {
        forest1 = forest2;
        forest2 = GenerateForest();
    }
}
