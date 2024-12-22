using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BlockBishoujo.InGame
{
    public class GridBlockGenerator : MonoBehaviour
    {
        // [SerializeField] private Vector2Int[] exceptSpriteIndex;
        // [SerializeField] Sprite[] slicedSprites; // スライスされたスプライトの配列
        // [SerializeField] Sprite[] slicedWhiteSprites; // スライスされたスプライトの配列
        // [SerializeField] Vector2Int gridSize = new Vector2Int(10, 3); // グリッドサイズ (列 x 行)
        // [SerializeField] Vector2 spaceSize = new Vector2(0.8f, 0.7f); // スプライトの間隔サイズ (列 x 行)
        // [SerializeField] float spriteSize = 1f; // 大きさ倍率

        [SerializeField] Block blockPrefab; // Block Prefab
        
        void ResetGrid()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        public Block[] GenerateGrid(GridBlockData blockData)
        {
            if (blockData.slicedSprites.Length == 0 || blockPrefab == null || blockData.slicedSprites.Length != blockData.slicedWhiteSprites.Length)
            {
                Debug.LogError("スライスされたスプライトまたはプレハブが設定されていません。");
                return null;
            }
            
            this.transform.position = blockData.generatePosition;
            
            int index = 0;
            int exceptIndex = 0;

            List<Block> blockList = new ();

            Vector3 offset = new Vector3(-blockData.gridSize.x * 0.5f * blockData.spaceSize.x, blockData.gridSize.y * 0.5f * blockData.spaceSize.y, 0) + new Vector3(blockData.spaceSize.x *0.5f, -blockData.spaceSize.y *0.5f, 0);
            
            for (int y = 0; y < blockData.gridSize.y; y++)
            {
                for (int x = 0; x < blockData.gridSize.x; x++)
                {
                    if (index >= blockData.slicedSprites.Length)
                        return null;

                    if (blockData.exceptSpriteIndex.Length > exceptIndex && x == blockData.exceptSpriteIndex[exceptIndex].x && y == blockData.exceptSpriteIndex[exceptIndex].y)
                    {
                        index++;
                        exceptIndex++;
                        continue;
                    }
                    
                    // ブロック生成・画像セット
                    var newBlock = Instantiate(blockPrefab, transform);
                    newBlock.InitBlock(blockData.slicedSprites[index], blockData.slicedWhiteSprites[index]);
                    
                    // 位置/大きさの設定
                    newBlock.transform.localPosition = (offset + new Vector3(x * blockData.spaceSize.x, -y * blockData.spaceSize.y, 0)) * blockData.spriteSize;
                    newBlock.transform.localScale = Vector3.one * blockData.spriteSize;

                    blockList.Add(newBlock);
                    index++;
                }
            }
            
            return blockList.ToArray();
        }
    }
}