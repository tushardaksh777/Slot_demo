using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ReelManager : MonoBehaviour
{
    public static ReelManager instance;
    public SymbolsManager symbolsManager;

    enum ReelState
    {
        Idle,
        Starting,
        Spinning,
        Stopping
    }

    ReelState state = ReelState.Idle;
    public List<Reels> reels = new List<Reels>();

    //[HideInInspector]
    public List<SymbolHolderHandler> symbolHolderList = new List<SymbolHolderHandler>();

    public float startDelay = 0.25f;
    public float stopDelay = 0.25f;
    public float startDelayBetweenReels = 0.25f;
    public float stopDelayBetweenReels = 0.25f;
    //public float autoStopDelay = 1f;

    protected bool spinResultReceived = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        GameManager.Instance.OnGameInit += ReelManagerInit;
        GameManager.Instance.OnSpinStarted += ReelManagerSpinStart;
        GameManager.Instance.OnSpinResultReceived += ResultReceived;
    }
    void Start()
    {
        state = ReelState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == ReelState.Idle)
        {
            
        }
        else if (state == ReelState.Starting)
        {

        }
        else if (state == ReelState.Spinning)
        {
            if (spinResultReceived)
                state = ReelState.Stopping;

        }
        else if (state == ReelState.Stopping)
        {
            StoppingSpin();
            state = ReelState.Idle;
        }
    }

    protected void ReelManagerInit()
    {
        for (int i = 0; i < reels.Count; i++)
        {
            reels[i].reelManager = this;
        }
        List<int> initialSymbols = GameManager.Instance.initResponse.symbols;
        SetInitalScreenSymbols(GetSymbolsForReels(initialSymbols));
        SetSymbolHoldersList();
    }

    protected void SetSymbolHoldersList()
    {
        int reelCount = reels.Count;
        int rowCount = reels[0].symbolsHolders.Count;

        symbolHolderList = new List<SymbolHolderHandler>(reelCount * rowCount);

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < reelCount; j++)
            {
                symbolHolderList.Add(reels[j].symbolsHolders[i]);
            }
        }
    }

    protected void ReelManagerSpinStart()
    {
        spinResultReceived = false;
        state = ReelState.Starting;
        AudioManager.Instance?.PlaySpinStart();
        StartCoroutine(StartingSpin());
    }

    IEnumerator StartingSpin()
    {
        yield return new WaitForSeconds(startDelay);

        foreach (var reel in reels)
        {
            reel.StartSpin();
            yield return new WaitForSeconds(startDelayBetweenReels);
        }

        state = ReelState.Spinning;
    }

    protected void ResultReceived()
    {
        spinResultReceived = true;
    }

    protected void StoppingSpin()
    {
        List<int> stoppedSymbols = GameManager.Instance.spinResponse.symbols;
        for (int i = 0; i < reels.Count; i++)
        {
            reels[i].ClearSymbols();
        }
        StartCoroutine(StoppingSpin(GetSymbolsForReels(stoppedSymbols)));
    }

    IEnumerator StoppingSpin(List<List<int>> symbols)
    {
        yield return new WaitForSeconds(stopDelay);

        for (int i = 0; i < reels.Count; i++)
        {
            reels[i].StopSpinAndSetResult(symbols[i] , (reels.Count - 1 == i)?true:false);
            yield return new WaitForSeconds(stopDelayBetweenReels);
        }
    }

    protected void SetInitalScreenSymbols(List<List<int>> symbols)
    {
        for (int i = 0; i < reels.Count; i++)
        {
            reels[i].SetScreenSymbols(symbols[i]);
        }
    }

    protected List<List<int>> GetSymbolsForReels(List<int> symbols)
    {
        List<List<int>> reelSymbols = new List<List<int>>();

        for (int i = 0; i < reels.Count; i++)
        {
            reelSymbols.Add(new List<int>());
            for (int j = 0; j < reels[i].symbolsHolders.Count; j++)
            {
                reelSymbols[i].Add(symbols[j * reels.Count + i]);
            }
        }

        return reelSymbols;
    }
}
