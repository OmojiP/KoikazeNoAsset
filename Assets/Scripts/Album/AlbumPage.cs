using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockBishoujo.Album
{

    [CreateAssetMenu(fileName = "New AlbumPage", menuName = "BlockBishoujo Asset/Album/AlbumPage")]
    public class AlbumPage : ScriptableObject
    {
        public Sprite[] stillSprites;
        public Sprite buttonSprite;
    }
}