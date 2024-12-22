using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmojiP.StorySystem
{
    [Serializable, CreateAssetMenu(fileName = "New StoryEpisodeCell", menuName = "OmojiP/Story System/Story Episode/Story Episode Cell")]
    public class StoryEpisodeCell : ScriptableObject
    {
        [SerializeField] private ScriptableObject firstEvent;

        [SerializeField] public int episodeNumber;
        [SerializeField] public string episodeName;
        [SerializeField] public Sprite firstBackGroundSprite;
        
        public IStoryEvent FirstEvent
        {
            get
            {
                if(firstEvent == null) return null;
                
                if (firstEvent is IStoryEvent storyEvent)
                {
                    return storyEvent;
                }
                else
                {
                    Debug.LogError("nextStoryEvent is not an IStoryEvent.");
                    return null;
                }
            }
        }
        
    }
}