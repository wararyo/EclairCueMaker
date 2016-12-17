using UnityEngine;
using System.Collections;

namespace wararyo.EclairEventer
{

    public class EventManager : MonoBehaviour
    {

        public QueueTable timeline;

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
            if (timeline.GetList().Count == cursor)
                return;
            if (0 <= timeline.GetList()[cursor].Key)
            {
                if (timeline.GetList()[cursor].Key < time)
                {
                    Invoke();
                }
            }
        }

        IEnumerator coroutine()
        {
            foreach (QueuePair q in timeline.GetList())
            {
                yield return new WaitForSeconds(q.Key);
                Debug.Log("Queue:" + q.Value.ToString());
                if (q.Value != null) q.Value.GetComponent<AnimationQueueBase>().Queue();
                Cursor++;
            }
        }

        public void Invoke()
        {
            if (timeline.GetList()[cursor].Value != null) timeline.GetList()[cursor].Value.GetComponents<AnimationQueueBase>()[0].Queue();//抽象クラス最高！
            time = 0;
            Cursor++;
        }
    }

    /// <summary>
    /// ジェネリックを隠すために継承してしまう
    /// [System.Serializable]を書くのを忘れない
    /// </summary>
    [System.Serializable]
    public class QueueTable : Serialize.TableBase<float, GameObject, QueuePair>
    {


    }

    /// <summary>
    /// ジェネリックを隠すために継承してしまう
    /// [System.Serializable]を書くのを忘れない
    /// </summary>
    [System.Serializable]
    public class QueuePair : Serialize.KeyAndValue<float, GameObject>
    {

        public QueuePair(float key, GameObject value) : base(key, value)
        {

        }
    }
}