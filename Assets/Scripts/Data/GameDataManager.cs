using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace BlockBishoujo.Manager
{
    public static class GameDataManager
    {
        private static bool isDataLoaded = false;
        private static GameData gameData;
        
        /// <summary>
        /// データをリセットして保存
        /// </summary>
        public static async UniTask ResetAndSaveData()
        {
            bool[] originOpenStageIds = new bool[Constant.Scenes.STAGE_COUNT];
            originOpenStageIds[0] = true;
            
            gameData = new GameData(false, originOpenStageIds, new bool[Constant.Scenes.STAGE_COUNT], new bool[Constant.Scenes.STAGE_COUNT]);
            
            await SaveGameData();
        }

        private static string DataPath => Application.persistentDataPath + "/GameData/data.json";
        private static string DataDirectoryPath => Application.persistentDataPath + "/GameData";
        
        /// <summary>
        /// データをロード、無ければリセットして保存
        /// </summary>
        private static async UniTask LoadGameData()
        {
            isDataLoaded = true;

            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                if (gameData.isOpenStages[0] == false)
                {
                    await ResetAndSaveData();
                }
                return;
            }
            
            if (!Directory.Exists(DataDirectoryPath))
            {
                Directory.CreateDirectory(DataDirectoryPath);
            }
            
            if (File.Exists(DataPath))
            {
                Debug.Log("Loading Game Data");
                var text = await File.ReadAllTextAsync(DataPath);
                
                Debug.Log($"Loading Game Data Complete {text}");
                
                if (!string.IsNullOrEmpty(text))
                {
                    gameData = JsonUtility.FromJson<GameData>(text);
                    
                    Debug.Log($"GameData Loaded : {text}");
                    return;
                }
            }
            
            Debug.Log("Game Data NotFound Reset And Save");
            await ResetAndSaveData();
        }
        
        /// <summary>
        /// データを保存
        /// </summary>
        private static async UniTask SaveGameData()
        {
            if(!isDataLoaded)
            {
                Debug.Log("SaveGameData() on NotLoaded GameData");
                await LoadGameData();
            }
            
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return;
            }
            
            if (!Directory.Exists(DataDirectoryPath))
            {
                Directory.CreateDirectory(DataDirectoryPath);
            }
            
            Debug.Log("Game Data Save");
            var text = JsonUtility.ToJson(gameData);
            Debug.Log($"Game Data Save this text : {text}");
            await File.WriteAllTextAsync(DataPath, text);
            Debug.Log("Game Data Saved");
        }

        /// <summary>
        /// Openingを見たら呼ぶ
        /// </summary>
        public static async UniTask PlayedOpening()
        {
            if(!isDataLoaded)
            {
                await LoadGameData();
            }
            
            gameData.isPlayedOpening = true;
            
            await SaveGameData();
        }
        
        /// <summary>
        /// ステージクリア情報を保存
        /// </summary>
        /// <param name="stageId"></param>
        public static async UniTask ClearStage(int stageId)
        {
            Debug.Log("Game Data Stage Clear");
            
            if(!isDataLoaded)
            {
                await LoadGameData();
            }
            
            gameData.isStageClears[stageId] = true;
            var openIds = GetOpenStageIdsThisClear(stageId);

            Debug.Log(openIds);

            if (openIds != null)
            {
                foreach (var openId in openIds)
                {
                    gameData.isOpenStages[openId] = true;
                }
            }

            await SaveGameData();
        }
        
        /// <summary>
        /// ステージクリアに対して開放されるステージのid
        /// </summary>
        /// <param name="clearStageId"></param>
        private static int[] GetOpenStageIdsThisClear(int clearStageId)
        {
            if (clearStageId is >= 0 and < Constant.Scenes.STAGE_COUNT - 1)
            {
                return new[] { clearStageId +1 };
            }

            return null;
        }

        /// <summary>
        /// ステージ選択時に新たにアンロックされたステージがあるか確認し、返す
        /// </summary>
        /// <returns></returns>
        public static async UniTask<int[]> GetNewUnlockedStageIds()
        {
            if(!isDataLoaded)
            {
                await LoadGameData();
            }
            
            List<int> newUnlockedStageIds = new List<int>();

            for (int stageId = 0; stageId < Constant.Scenes.STAGE_COUNT; stageId++)
            {
                if (gameData.isOpenStages[stageId] && !gameData.isStageUnlockAnimationPlayed[stageId])
                {
                    newUnlockedStageIds.Add(stageId);
                }
            }

            return newUnlockedStageIds.ToArray();
        }

        public static async UniTask<bool[]> GetIsStageUnlockAnimationPlayed()
        {
            if(!isDataLoaded)
            {
                await LoadGameData();
            }
            return gameData.isStageUnlockAnimationPlayed;
        }
        public static async UniTask<bool[]> GetIsStageClears()
        {
            if(!isDataLoaded)
            {
                await LoadGameData();
            }
            return gameData.isStageClears;
        }
        public static async UniTask<bool[]> GetIsOpenStages()
        {
            if(!isDataLoaded)
            {
                await LoadGameData();
            }
            return gameData.isOpenStages;
        }
        public static async UniTask<bool> GetIsPlayedOpening()
        {
            if(!isDataLoaded)
            {
                await LoadGameData();
            }
            return gameData.isPlayedOpening;
        }


        /// <summary>
        /// ステージアンロックanimation状況をステージクリアと同等に更新し、保存
        /// </summary>
        public static async UniTask UpdateUnlockAnimationPlayedStages()
        {
            Debug.Log("ステージアンロックanimation状況をステージクリアと同等に更新し、保存");
            
            if(!isDataLoaded)
            {
                await LoadGameData();
            }
            
            for (int stageId = 0; stageId < Constant.Scenes.STAGE_COUNT; stageId++)
            {
                gameData.isStageUnlockAnimationPlayed[stageId] = gameData.isOpenStages[stageId];
            }
            
            await SaveGameData();
        }
    }
}