using UnityEngine;

public class PoliceCar : MonoBehaviour
{
    public float followSpeed = 1;
    public AudioSource squealing;
    public AudioSource siren;

    private Transform player;
    private Vector3 offset;
    private bool havePlaySquealing = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        offset = transform.position - player.position;
    }

    void Update()
    {
        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);

        if (GameController.gameState == GameState.Playing)
        {
            if (!siren.isPlaying)
            {
                siren.Play();
            }
        }
        else
        {
            siren.Stop();
        }
        if (GameController.gameState == GameState.End)
        {
            if (havePlaySquealing == false)
            {
                squealing.Play();
                havePlaySquealing = true;
            }
        }
    }

}
