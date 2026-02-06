using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SymbolsManager : MonoBehaviour
{
    public static SymbolsManager instance;
    public Transform pool;

    public List<SymbolHandler> Symbols = new List<SymbolHandler>();
    public int bufferSize = 15;

    protected List<List<SymbolHandler>> pooledSymbols = new List<List<SymbolHandler>>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        GameManager.Instance.OnGameInit += InitializeSymbols;
    }

    public void InitializeSymbols()
    {
        for (int i = 0; i < Symbols.Count; i++)
        {
            pooledSymbols.Add(new List<SymbolHandler>());
            
            for (int j = 0; j < bufferSize; j++)
            {
                SymbolHandler symbol = Instantiate(Symbols[i]);
                symbol.gameObject.SetActive(false);
                symbol.gameObject.transform.localPosition = Vector3.zero;
                symbol.gameObject.transform.SetParent(pool);
                pooledSymbols[i].Add(symbol);
            }
        }
    }

    public SymbolHandler GetSymbolByID(int id)
    {
        if (pooledSymbols[id].Count > 0)
        {
            SymbolHandler symbol = pooledSymbols[id][0];
            pooledSymbols[id].RemoveAt(0);
            symbol.gameObject.SetActive(true);
            symbol.gameObject.transform.SetParent(null);
            return symbol;
        }
        else
        {
            SymbolHandler symbol = Instantiate(Symbols[id]);
            symbol.gameObject.SetActive(true);
            symbol.gameObject.transform.SetParent(null);
            return symbol;
        }
    }

    public void ReturnToPool(SymbolHandler symbol)
    {
        symbol.gameObject.SetActive(false);
        symbol.gameObject.transform.SetParent(pool);
        pooledSymbols[symbol.symbolID].Add(symbol);
    }
}
