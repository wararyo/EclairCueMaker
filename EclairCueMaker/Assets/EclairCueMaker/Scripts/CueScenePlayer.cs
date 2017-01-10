using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace wararyo.EclairCueMaker
{
    public class CueScenePlayer : MonoBehaviour
    {
		public CueScene cueScene;

        private float time = 0;

        private int cursor = 0;
        public int Cursor
        {
            get
            {
                return cursor;
            }
            set
            {
                OnCursorChanged(cursor, value);
                cursor = value;
            }
        }

        protected virtual void OnCursorChanged(int before, int after) {; }

        protected virtual void Start()
        {
            //StartCoroutine(coroutine());
        }

        protected virtual void Update()
        {
            time += Time.deltaTime;
			if (cueScene.Count == cursor)
                return;
			if (0 <= cueScene.cueList[cursor].time)
            {
				if (cueScene.cueList[cursor].time < time)
                {
                    Invoke();
                }
            }
        }

        public void Invoke()
        {
			if (cueScene.cueList[cursor].gameObjectName != ""){
                GameObject go = GameObject.Find(cueScene.cueList[cursor].gameObjectName);

                var cueEvent = go.GetComponents<CueEventBase> () [0];
				cueEvent.Cue(cueScene.cueList[cursor].parameter);//抽象クラス最高！
				Debug.Log(go.name);
			}
            time = 0;
            Cursor++;
        }
    }

}