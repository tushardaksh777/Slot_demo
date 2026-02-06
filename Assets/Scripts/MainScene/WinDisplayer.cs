using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDisplayer : MonoBehaviour
{
    public List<float> symbolsHighlightDuration = new List<float>();
    public float symbolHighlightDuration = 0.5f; // Duration for which the symbols will be highlighted
    public float delayBetweenPaylines = 0.5f; // Delay between highlighting each payline
    protected List<PaylineWin> currentHighlightedSymbols = new List<PaylineWin>();
    protected bool startHeightlightingSymbols = false;

    List<SymbolHolderHandler> symbolHolders = new List<SymbolHolderHandler>();

    private Coroutine displayPaylinesRoutine;
    private void Awake()
    {
        GameManager.Instance.OnStartWinDisplayer += StartWinDisplay;
        GameManager.Instance.OnResultDisplayerDone += OnResultDisplayerFinished;
    }

    void Start()
    {
        symbolHolders = ReelManager.instance.symbolHolderList;
    }

    protected void StartWinDisplay()
    {
        currentHighlightedSymbols = new List<PaylineWin>();
        SpinResponseHandler spinResponseHandler = GameManager.Instance.spinResponse;
        currentHighlightedSymbols = spinResponseHandler.paylines;
        if (currentHighlightedSymbols.Count == 0)
        {
            Debug.Log("No paylines to display.");
            OnResultDisplayerFinished();
            return;
        }

        Debug.Log("Starting win display...");
        startHeightlightingSymbols = true;
        if (displayPaylinesRoutine != null)
            StopCoroutine(displayPaylinesRoutine);

        displayPaylinesRoutine = StartCoroutine(DisplayPaylines());
    }
    IEnumerator DisplayPaylines()
    {
        //Here we will start highlighting the symbols,
        //we also highlight the paylines and show the win amount for each payline
        while (startHeightlightingSymbols)
        {
            for (int i = 0; i < currentHighlightedSymbols.Count; i++)
            {
                int hightlightDurationIndex = 0;
                for (int j = 0; j < currentHighlightedSymbols[i].positions.Count; j++)
                {
                    int index = currentHighlightedSymbols[i].positions[j];
                    symbolHolders[index].symbolHandler.SetHighlightSymbol();
                    hightlightDurationIndex = symbolHolders[index].symbolHandler.symbolID;
                }
                PlayWinSounds(hightlightDurationIndex);
                yield return new WaitForSeconds(symbolsHighlightDuration[hightlightDurationIndex]);

                for (int j = 0; j < currentHighlightedSymbols[i].positions.Count; j++)
                {
                    int index = currentHighlightedSymbols[i].positions[j];
                    symbolHolders[index].symbolHandler.SetNormalSymbol();
                }

                yield return new WaitForSeconds(delayBetweenPaylines);
            }
        }
    }

    protected void OnResultDisplayerFinished()
    {
        startHeightlightingSymbols = false;
        if (displayPaylinesRoutine != null)
            StopCoroutine(displayPaylinesRoutine);

        for (int i = 0; i < symbolHolders.Count; i++)
        {
            symbolHolders[i].symbolHandler.SetNormalSymbol();
        }
        Debug.Log("Win display finished.");
    }

    public void PlayWinSounds(int symbolID)
    {
        if (currentHighlightedSymbols == null || currentHighlightedSymbols.Count == 0)
            return;

        int symbolId = symbolID;

        var audioType = SymbolAudioMapper.GetAudioType(symbolId);
        AudioManager.Instance?.PlayWin(audioType);
    }
}
