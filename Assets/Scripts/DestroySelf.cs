using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    // This method will be called by the animation event
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}