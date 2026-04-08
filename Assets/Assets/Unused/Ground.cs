using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private GameObject[] groundList;
    
    public void LandInit(int level)
    {
        foreach (GameObject ground in groundList)
        {
            ground.SetActive(false);
        }
        groundList[level - 1].SetActive(true);
    }
}
