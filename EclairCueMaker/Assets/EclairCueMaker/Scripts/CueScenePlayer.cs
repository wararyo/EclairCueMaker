using UnityEngine;
using System.Collections;

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
			if (cueScene.cueList[cursor].gameObject != null){
				var cueEvent = cueScene.cueList [cursor].gameObject.GetComponents<CueEventBase> () [0];
				cueEvent.Cue<string>(cueScene.cueList[cursor].parameter);//抽象クラス最高！
				Debug.Log(cueScene.cueList[cursor].gameObject.name);
			}
			Debug.Log ("nullだなぁ");
            time = 0;
            Cursor++;
        }
    }

}