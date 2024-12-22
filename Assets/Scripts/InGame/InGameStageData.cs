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
    [CreateAssetMenu(fileName = "New InGameStageData", menuName = "BlockBishoujo Asset/InGame/StageData")]
    public class InGameStageData : ScriptableObject
    {
        [Header("Mission Data")]
        [SerializeField] public float timeLimit = 30f;
        [SerializeField] public Sprite missionSprite;
        // [SerializeField] public string missionName;
        [SerializeField] public Sprite missionInGameSprite;
        
        [Header("BackGround Data")]
        [SerializeField] public Sprite backGroundSprite;
        
        [Header("Character Data")]
        [SerializeField] public InGameCharacter character;
        
        [Header("Block Data")]
        [SerializeField] public GridBlockData gridBlockData;
    }
}