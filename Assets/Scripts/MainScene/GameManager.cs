using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public ResponseHandler responseHandler;
    public UIManager uiManager;

    [HideInInspector]
    public SpinResponseHandler initResponse;
    [HideInInspector]
    public SpinResponseHandler spinResponse;

    public float spinDelay = 0.5f; // Delay in seconds before invoking the spin result event;

    public bool canSpin = true;

    public Action OnGameInit;
    public Action OnSpinRequested;
    public Action OnSpinStarted;
    public Action OnSpinResultReceived;
    public Action OnSpinEnded;
    public Action OnStartWinDisplayer;
    public Action OnResultDisplayerDone;

    //UIValues
    protected double balanceAmount;
    protected double betAmount;
    protected double WinAmount;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        OnSpinRequested += RequestSpin;
        OnStartWinDisplayer += OnResultDisplayerStart;
        OnSpinEnded += OnSpinEnd;
    }
    void Start()
    {
        initResponse = responseHandler.GetInitResponse();
        OnGameInit?.Invoke();
        canSpin = true;
        balanceAmount = 1000;
        betAmount = 5;
        WinAmount = 0;
        uiManager.OnUpdateUI?.Invoke(balanceAmount, initResponse.totalWin, betAmount);
    }

    protected void RequestSpin()
    {
        canSpin = false;
        balanceAmount -= betAmount;
        uiManager.OnUpdateUI?.Invoke(balanceAmount , 0, betAmount);
        OnResultDisplayerDone?.Invoke();
        OnSpinStarted?.Invoke();
        spinResponse = responseHandler.GetNextResponse();
        
        StartCoroutine(SimulateSpinDelay());
    }

    IEnumerator SimulateSpinDelay()
    {
        yield return new WaitForSeconds(spinDelay); // Simulate a delay of 1 second
        OnSpinResultReceived?.Invoke();
    }

    protected void OnResultDisplayerStart()
    {
        canSpin = true;
        balanceAmount += spinResponse.totalWin;
        uiManager.OnUpdateUI?.Invoke(balanceAmount, spinResponse.totalWin, betAmount);
    }

    protected void OnSpinEnd()
    {
        OnStartWinDisplayer?.Invoke();
    }
}
