using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace OmojiP.Utils
{
    public class DebugCanvas : MonoBehaviour
    {
        [SerializeField] public bool isDebug;

        [SerializeField] Text fpsText;

        // Start is called before the first frame update
        void Start()
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            isDebug = true;
            DontDestroyOnLoad(this.gameObject);

            ShowFPS(1f, destroyCancellationToken).Forget();
#else
            Destroy(this.gameObject);
#endif
        }

        // Update is called once per frame
        void Update()
        {
            if (isDebug)
            {
                
            }
        }

        async UniTask ShowFPS(float updateSpan, CancellationToken ct)
        {
            while (true)
            {
                fpsText.text = $"FPS: {(int)(1f / Time.unscaledDeltaTime)}";
                await UniTask.Delay((int)(1000* updateSpan), cancellationToken: ct);
            }
        }
    }
}