using UnityEngine;

public class PlayerSmallCollider : MonoBehaviour
{
    private PlayerMove playerMove;

    void Awake()
    {
        playerMove = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerMove>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.obstacles && playerMove.isSliding)
        {
            GameController.Instance.GameOver();
        }
    }

}
