using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockBishoujo.InGame
{
    /// <summary>
    /// Blockの破壊を監視し、破壊されるたびに残りブロック数を引数としてイベントonBlockUpdateを呼び出す
    /// </summary>
    public class BlocksManager
    {
        // Blockがすべて破壊されることを監視する

        public int blockAllCount = 0;
        private int ballEnterBlockCount = 0;

        public event Action<int> onBlockUpdate;

        private Block[] blocks;

        public BlocksManager(Block [] blocks)
        {
            // Blockをすべて取得し、全ブロック数を把握し、イベントをBlockにセットする
            // Block[] blocks = FindObjectsOfType<Block>();

            this.blocks = blocks;
            
            foreach (var block in blocks)
            {
                block.onBallEntered += OnBallEnteredBlock;
            }

            blockAllCount = blocks.Length;
        }

        public void GameEnd()
        {
            foreach (var block in blocks)
            {
                block.GameEnd();
            }
        }


        // ブロックにボールが当たった際に実行されるイベント
        private void OnBallEnteredBlock()
        {
            ballEnterBlockCount++;

            onBlockUpdate?.Invoke(blockAllCount - ballEnterBlockCount);

            if (ballEnterBlockCount >= blockAllCount)
            {
                Debug.Log("Stage Clear");
            }
        }
    }
}