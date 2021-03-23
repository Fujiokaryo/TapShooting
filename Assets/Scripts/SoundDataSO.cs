using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO :ScriptableObject
{
    /// <summary>
    /// BGMの種類
    /// </summary>
    public enum BgmType
    {
        Main,
        Boss,
        Silence = 999
    }

    [Serializable]
    public class BgmData
    {
        public int no; //BGMの通し番号
        public BgmType bgmType; //BGMの種類
        public float volume = 0.05f; //BGMのボリューム
        public AudioClip bgmAudioClip; //BGMとして鳴らすオーディオファイル
    }

    public List<BgmData> bgmDataList = new List<BgmData>();

    /// <summary>
    /// SEの種類
    /// </summary>
    public enum SeType
    {
        //SE用の列挙子をゲームに合わせて登録
        BulletDamage_1,
        GameClear,
        GameOver
    }

    /// <summary>
    /// SE関連の情報を管理するクラス
    /// </summary>
    [Serializable]
    public class SeData
    {
        public int no;
        public SeType seType;
        public float volume = 0.5f;
        public AudioClip seAudioClip;
    }

    public List<SeData> seDataList = new List<SeData>();

    /// <summary>
    /// VOICEの種類
    /// </summary>
    public enum VoiceType
    {
        Start,
        Lose,
        Win,
        Attack,
        Hit,
        Warning
    }

    /// <summary>
    /// Voice関連の情報を管理するクラス
    /// </summary>
    [Serializable]
    public class VoiceData
    {
        public int no;
        public VoiceType voiceType;
        public float volume = 0.5f;
        public AudioClip voiceAudioClip;
    }

    public List<VoiceData> voiceDataList = new List<VoiceData>();
}
