using UnityEngine;
using System.Collections;

namespace wararyo.EclairEventer
{
    /// <summary>
    /// EclairEventEditorのTimeline表示で使われる、一つのトラックを表すクラスです。
    /// 1つのトラックと1つのGameObjectが対応しています。
    /// </summary>
    public class Track
    {
        public GameObject gameObject;

        public Track(GameObject go)
        {
            gameObject = go;
        }
    }

}
