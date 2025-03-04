using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シングルトン
/// 
/// ジェネリックで汎用性強化
/// where T : new()はインスタンス化の保証
/// </summary>
public abstract class SingletonSample<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    /// <summary>
    /// インスタンスを取得する
    /// </summary>
    public static T GetInstance { get { return instance; } }

    public virtual void Awake()
    {
        if (instance == null)
        {
            // 初回のみインスタンス化
            instance = (T)FindAnyObjectByType(typeof(T));
        }
        else
        {
            // 複製禁止
            Destroy(gameObject);
        }
    }
}