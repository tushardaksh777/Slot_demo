using UnityEngine;

public class ResponseHandler : MonoBehaviour
{
    public TextAsset config;
    protected SpinResponseJsonWrapper spinResponse = null;
    protected int spinResponseCounter = -1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spinResponse = JsonUtility.FromJson<SpinResponseJsonWrapper>(config.text);
        Debug.Log("Spin response loaded successfully!");
    }

    public SpinResponseHandler GetNextResponse()
    {
        if (spinResponse == null)
        {
            Debug.LogError("Spin response is null!");
            return null;
        }

        spinResponseCounter++;

        if (spinResponseCounter >= spinResponse.spins.Count)
        {
            Debug.LogWarning("No more spin responses available, resetting counter.");
            spinResponseCounter = 0; // Reset counter to loop through responses again
        }
        Debug.Log("Spin response counter " + spinResponseCounter);
        return spinResponse.spins[spinResponseCounter];
    }

    public SpinResponseHandler GetInitResponse()
    {
        if (spinResponse == null)
        {
            Debug.LogError("Spin response is null!");
            return null;
        }
        return spinResponse.initResponse;
    }
}
