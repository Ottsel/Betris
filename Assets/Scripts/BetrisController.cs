using UnityEngine;

public class BetrisController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (CompareTag("Done")) return;
        if (!other.gameObject.CompareTag("Done")) return;
        gameObject.tag = "Done";
    }
}