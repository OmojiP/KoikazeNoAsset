using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OmojiP.StorySystem;
using UnityEngine;

public class TestStoryStart : MonoBehaviour
{
    [SerializeField] private StoryEpisodeCell episodeCell;
    
    // Start is called before the first frame update
    void Start()
    {
        var storyManager = FindObjectOfType<StoryManager>();
        storyManager.StorySystemSetUp(episodeCell);
        storyManager.StorySystemStart(episodeCell, () => { }).Forget();
    }
}
