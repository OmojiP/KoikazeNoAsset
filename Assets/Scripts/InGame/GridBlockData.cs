using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BlockBishoujo.InGame
{
    /// <summary>
    /// ミッション情報、キャラクターの情報、ブロックの情報
    /// </summary>
    [CreateAssetMenu(fileName = "New GridBlockData", menuName = "BlockBishoujo Asset/InGame/GridBlockData")]
    public class GridBlockData : ScriptableObject
    {
        [SerializeField] public Vector3 generatePosition;
        [SerializeField] public Vector2Int[] exceptSpriteIndex;
        [SerializeField] public Sprite[] slicedSprites; // スライスされたスプライトの配列
        [SerializeField] public Sprite[] slicedWhiteSprites; // スライスされたスプライトの配列
        [SerializeField] public Vector2Int gridSize = new Vector2Int(10, 3); // グリッドサイズ (列 x 行)
        [SerializeField] public Vector2 spaceSize = new Vector2(0.8f, 0.7f); // スプライトの間隔サイズ (列 x 行)
        [SerializeField] public float spriteSize = 1f; // 大きさ倍率
    }
}