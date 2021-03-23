using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public SoundDataSO soundDataSO;

    public const float CROSS_FADE_TIME = 1.0f;

    //ボリューム関連
    public float bgmVolume = 0.1f;
    public float seVolume = 0.2f;
    public float voiceVolume = 0.2f;
    public bool isMute = false;

    //VoiceDataを種類毎に仕分け
    public List<SoundDataSO.VoiceData> startVoiceList = new List<SoundDataSO.VoiceData>();
    public List<SoundDataSO.VoiceData> loseVoiceList = new List<SoundDataSO.VoiceData>();
    public List<SoundDataSO.VoiceData> winVoiceList = new List<SoundDataSO.VoiceData>();
    public List<SoundDataSO.VoiceData> attackVoiceList = new List<SoundDataSO.VoiceData>();
    public List<SoundDataSO.VoiceData> hitVoiceList = new List<SoundDataSO.VoiceData>();
    public List<SoundDataSO.VoiceData> warningVoiceList = new List<SoundDataSO.VoiceData>();

    //各オーディオファイル再生用のAudioSource
    private AudioSource[] bgmSource = new AudioSource[2];

    //SE用のAudioSourceコンポーネントを代入するための変数の宣言。複数用意しているのは重複してなることを想定
    private AudioSource[] seSources = new AudioSource[10];

    private AudioSource voiceSource = new AudioSource();

    private bool isCrossFading; //クロスフェード処理中かどうか判定用

    private void Awake()
    {
        //シングルトンかつシーン遷移しても破棄されないようにする
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //BGM時AudioSource追加
        bgmSource[0] = gameObject.AddComponent<AudioSource>();
        bgmSource[1] = gameObject.AddComponent<AudioSource>();
        bgmSource[1].volume = 0;

        //SE用のAudioSource追加
        for(int i = 0; i < seSources.Length; i++)
        {
            seSources[i] = gameObject.AddComponent<AudioSource>();
        }

        //Voise 用のAudioSource追加
        voiceSource = gameObject.AddComponent<AudioSource>();

        //ボイスの種類毎に仕分けして各Listに代入
        AssortmentListToVoices();
    }

    private void AssortmentListToVoices()
    {
        foreach(SoundDataSO.VoiceData voiceData in soundDataSO.voiceDataList)
        {
            switch(voiceData.voiceType)
            {
                case SoundDataSO.VoiceType.Start:
                    startVoiceList.Add(voiceData);
                    break;

                case SoundDataSO.VoiceType.Lose:
                    loseVoiceList.Add(voiceData);
                    break;

                case SoundDataSO.VoiceType.Win:
                    winVoiceList.Add(voiceData);
                    break;

                case SoundDataSO.VoiceType.Attack:
                    attackVoiceList.Add(voiceData);
                    break;

                case SoundDataSO.VoiceType.Hit:
                    hitVoiceList.Add(voiceData);
                    break;

                case SoundDataSO.VoiceType.Warning:
                    warningVoiceList.Add(voiceData);
                    break;
            }
        }
    }


    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="newBgmType"></param>
    /// <param name="loopFlg"></param>
    public void PlayBGM(SoundDataSO.BgmType newBgmType, bool loopFlg = true)
    {
        //BGMをSilenceの状態にする場合
        if((int)newBgmType == 999)
        {
            StopBGM();
            return;
        }

        //再生するBGM用のBgmDataを取得
        SoundDataSO.BgmData newBgmData = null;
        foreach(SoundDataSO.BgmData bgmData in soundDataSO.bgmDataList.Where(x => x.bgmType == newBgmType))
        {
            newBgmData = bgmData;
            break;
        }

        //対象となるデータがなければ処理しない
        if(newBgmData == null)
        {
            return;
        }

        //同じBGMの場合は処理しない
        if(bgmSource[0].clip != null && bgmSource[0].clip == newBgmData.bgmAudioClip)
        {
            return;
        }
        else if(bgmSource[1].clip != null && bgmSource[1].clip == newBgmData.bgmAudioClip)
        {
            return;
        }

        //フェードでBGM開始
        if(bgmSource[0].clip == null && bgmSource[1].clip == null)
        {
            bgmSource[0].loop = loopFlg;
            bgmSource[0].clip = newBgmData.bgmAudioClip;
            bgmSource[0].volume = newBgmData.volume;
            bgmSource[0].Play();
        }
        else
        {
            //クロスフェード処理を利用してBGMを切り替え
            StartCoroutine(CrossFadeChangeBGM(newBgmData, loopFlg));
        }
     
    }

    /// <summary>
    /// BGMクロスフェード処理
    /// </summary>
    /// <param name="bgmData"></param>
    /// <param name="loopFlg">ループ指定。ループしない場合だけfalse指定</param>
    /// <returns></returns>
    private IEnumerator CrossFadeChangeBGM(SoundDataSO.BgmData bgmData, bool loopFlg)
    {
        isCrossFading = true;

        if(bgmSource[0].clip != null)
        {
            //[0]が再生されている場合、[0]の音量を徐々に下げて、[1]を新しい曲として再生
            bgmSource[1].DOFade(bgmData.volume, CROSS_FADE_TIME).SetEase(Ease.Linear);
            bgmSource[1].clip = bgmData.bgmAudioClip;
            bgmSource[1].loop = loopFlg;
            bgmSource[1].Play();
            bgmSource[0].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CROSS_FADE_TIME);
            bgmSource[0].Stop();
            bgmSource[0].clip = null;
        }
        else
        {
            //[1]が再生されている場合、[1]の音量を徐々にさげて、[0]を新しい曲として再生
            bgmSource[0].DOFade(bgmData.volume, CROSS_FADE_TIME).SetEase(Ease.Linear);
            bgmSource[0].clip = bgmData.bgmAudioClip;
            bgmSource[0].loop = loopFlg;
            bgmSource[0].Play();
            bgmSource[1].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

            yield return new WaitForSeconds(CROSS_FADE_TIME);
            bgmSource[1].Stop();
            bgmSource[1].clip = null;
        }
        isCrossFading = false;
    }
    /// <summary>
    /// BGM完全停止
    /// </summary>
    public void StopBGM()
    {
        bgmSource[0].Stop();
        bgmSource[1].Stop();
        bgmSource[0].clip = null;
        bgmSource[1].clip = null;
    }

    /// <summary>
    /// BGM一時停止
    /// </summary>
    public void MuteBGM()
    {
        bgmSource[0].Stop();
        bgmSource[1].Stop();
    }

    /// <summary>
    /// 一時停止したBGMを再開
    /// </summary>
    public void ResumeBGM()
    {
        bgmSource[0].Play();
        bgmSource[1].Play();
    }
    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="newSeType"></param>
    public void PlaySE(SoundDataSO.SeType newSeType)
    {
        //再生するSE用のSeDataを取得
        SoundDataSO.SeData newSeData = null;
        foreach(SoundDataSO.SeData seData in soundDataSO.seDataList.Where(x => x.seType == newSeType))
        {
            newSeData = seData;
            break;
        }

        //再生中ではないVを使ってSEを鳴らす
        foreach(AudioSource source in seSources)
        {
            if(source.isPlaying == false)
            {
                source.clip = newSeData.seAudioClip;
                source.volume = newSeData.volume;
                source.Play();
                return;
            }
        }
    }
    /// <summary>
    /// SE停止
    /// </summary>
    public void StopSE()
    {
        //すべてのSE用のAudioSourceを停止する
        foreach(AudioSource source in seSources)
        {
            source.Stop();
            source.clip = null;
        }
    }

    /// <summary>
    /// 指定された種類のボイス再生
    /// 種類内に複数のボイスの登録がある場合には、その中からランダムな1つのボイスを再生
    /// </summary>
    /// <param name="voiceType"></param>
    public void PlayVoice(SoundDataSO.VoiceType voiceType)
    {
        //ボイス再生中は重複して再生しない
        if(voiceSource.isPlaying)
        {
            return;
        }

        //ボイスのデータを取得
        SoundDataSO.VoiceData voiceData = GetVoice(voiceType);

        //再生するボイスを指定
        voiceSource.clip = voiceData.voiceAudioClip;

        //ボリュームを設定
        voiceSource.volume = voiceData.volume;

        //ボイス再生
        voiceSource.Play();
    }

    /// <summary>
    /// 指定されたVoiceTypeよりListを設定し、List内にあるVoiceDataをランダムで１つ取得
    /// </summary>
    /// <param name="voiceType"></param>
    /// <returns></returns>
    private SoundDataSO.VoiceData GetVoice(SoundDataSO.VoiceType voiceType)
    {
        //再生するボイス用のVoiceDataを取得
        List<SoundDataSO.VoiceData> list = new List<SoundDataSO.VoiceData>();

        switch(voiceType)
        {
            case SoundDataSO.VoiceType.Start:
                list = startVoiceList;
                break;

            case SoundDataSO.VoiceType.Lose:
                list = loseVoiceList;
                break;

            case SoundDataSO.VoiceType.Win:
                list = winVoiceList;
                break;

            case SoundDataSO.VoiceType.Attack:
                list = attackVoiceList;
                break;

            case SoundDataSO.VoiceType.Hit:
                list = hitVoiceList;
                break;

            case SoundDataSO.VoiceType.Warning:
                list = warningVoiceList;
                break;
                
        }
        //場面に応じたボイスをランダムで１つ取得して戻す
        return list[Random.Range(0, list.Count)];
    }

    /// <summary>
    /// ボイス停止
    /// </summary>
    public void StopVoice()
    {
        voiceSource.Stop();
        voiceSource.clip = null;
    }
}
