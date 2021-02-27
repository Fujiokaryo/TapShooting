using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MoveEventSO", menuName = "Create MoveEventSO")]

public class MoveEventSO : ScriptableObject
{
    private const float moveLimit = -3000f;
    public UnityAction<Transform, float> GetMoveEvent(MoveType moveType)
    {
        //moveTypeで分岐
        switch (moveType)
        {
            case MoveType.Straight:
                return MoveStraight;

            case MoveType.Meandering:
                return MoveMeandering;

            case MoveType.Boss_Horizontal:
                return MoveBossHorizontal;

            default:
                return Stop;
        }

    }

    private void MoveStraight(Transform tran, float duration)
    {
        tran.DOLocalMoveY(moveLimit, duration);
        Debug.Log("直進");
    }

    private void MoveMeandering(Transform tran, float duration)
    {
        tran.DOLocalMoveX(tran.position.x + Random.Range(200f, 400f), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        tran.DOLocalMoveY(moveLimit, duration);
        Debug.Log("蛇行");
    }

    public void MoveBossHorizontal(Transform tran, float duration)
    {
        tran.localPosition = new Vector3(0, tran.localPosition.y, tran.localPosition.z);

        tran.DOLocalMoveY(-500, 3.0f).OnComplete(()
            => {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(tran.DOLocalMoveX(tran.localPosition.x + 550, 2.5f).SetEase(Ease.Linear));
                sequence.Append(tran.DOLocalMoveX(tran.localPosition.x - 550, 5.0f).SetEase(Ease.Linear));
                sequence.Append(tran.DOLocalMoveX(tran.localPosition.x, 2.5f).SetEase(Ease.Linear));
                sequence.AppendInterval(1.0f).SetLoops(-1, LoopType.Restart);

            });
        Debug.Log("ボス水平");
    }

    public void  Stop(Transform tran, float duration)
    {
        Debug.Log("停止");
    }
}
