using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockBishoujo.Constant
{
    public static class Tags
    {
        public const string BALL = "Ball";
        public const string WALL = "Wall";
        public const string BLOCK = "Block";
        public const string BALL_COLLECT = "Ball Collect";
        
    }
    
    public static class Scenes
    {
        public const int STAGE_COUNT = 3;
        
        public const string TITLE = "Title";
        public const string STAGE_SELECT = "StageSelect";
        public const string OPENING = "Opening";
        public const string ENDING = "Ending";
        public const string INGAME = "InGame";
        public const string STAGE_PROLOGUE = "StagePrologue";
        public const string STAGE_EPILOGUE = "StageEpilogue";
        public const string STAGE_ALBUM = "Album";
        private const string STAGE_GAME = "StageGame";

        public static string GetStageSceneName(int stageId, SceneStateType sceneStateType)
        {
            if (stageId >= STAGE_COUNT || stageId < 0)
            {
                throw new System.Exception($"Invalid stage number {stageId}");
            }
            
            switch (sceneStateType)
            {
                case SceneStateType.STAGE_GAME:
                    return STAGE_GAME + stageId;
                case SceneStateType.STAGE_PROLOGUE:
                    return STAGE_PROLOGUE + stageId;
                case SceneStateType.STAGE_EPILOGUE:
                    return STAGE_EPILOGUE + stageId;
                default:
                    throw new System.ArgumentException("Invalid scene state");
            }
        }


        public enum SceneStateType
        {
            STAGE_PROLOGUE,
            STAGE_EPILOGUE,
            STAGE_GAME,
        }
    }
}
