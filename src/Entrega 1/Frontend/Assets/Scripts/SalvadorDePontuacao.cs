using UnityEngine;
using System.Collections.Generic;

public class SalvadorDePontuacao : MonoBehaviour
{
    public static SalvadorDePontuacao Instancia { get; private set; }

    private const int MAX_RANKING = 5;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SalvarPontuacao(string nome, int pontuacao)
    {
        List<(string, int)> ranking = CarregarRanking();
        ranking.Add((nome, pontuacao));
        ranking.Sort((a, b) => b.Item2.CompareTo(a.Item2));

        if (ranking.Count > MAX_RANKING)
            ranking.RemoveRange(MAX_RANKING, ranking.Count - MAX_RANKING);

        PlayerPrefs.SetInt("Ranking_Total", ranking.Count);
        for (int i = 0; i < ranking.Count; i++)
        {
            PlayerPrefs.SetString($"Ranking_Nome_{i}", ranking[i].Item1);
            PlayerPrefs.SetInt($"Ranking_Pontos_{i}", ranking[i].Item2);
        }
        PlayerPrefs.Save();
    }

    public List<(string, int)> CarregarRanking()
    {
        List<(string, int)> ranking = new();
        int total = PlayerPrefs.GetInt("Ranking_Total", 0);

        for (int i = 0; i < total; i++)
        {
            string nome = PlayerPrefs.GetString($"Ranking_Nome_{i}", "---");
            int pontos = PlayerPrefs.GetInt($"Ranking_Pontos_{i}", 0);
            ranking.Add((nome, pontos));
        }
        return ranking;
    }

    public void LimparRanking()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}