using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpinResponseJsonWrapper
{
    public SpinResponseHandler initResponse;
    public List<SpinResponseHandler> spins;
}

[System.Serializable]
public class SpinResponseHandler
{
    public List<int> symbols;
    public List<PaylineWin> paylines;
    public int totalWin;
    public bool isBigWin;
}

[System.Serializable]
public class PaylineWin
{
    public int paylineId;
    public int winAmount;
    public List<int> positions;
}