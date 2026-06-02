using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public int startMoney = 100;

    public static int Lives;
    public int startLives = 20; // The satellite can take 20 hits

    void Start()
    {
        Money = startMoney;
        Lives = startLives;
    }
}