using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class AdaptivePerformanceInitializer : MonoBehaviour
{
    void Start()
    {
        var ap = Holder.Instance;
        if (ap == null)
        {
            Debug.LogError("Adaptive Performance is not available.");
            return;
        }

        if (!ap.Active)
        {
            Debug.Log("Adaptive Performance is available but not active.");
        }
        else
        {
            Debug.Log("Adaptive Performance is active.");
        }
    }
}
