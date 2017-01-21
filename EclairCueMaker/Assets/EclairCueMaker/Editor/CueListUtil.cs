using UnityEngine;
using System.Collections.Generic;

namespace wararyo.EclairCueMaker
{

    public static class CueListUtil
    {
        public static List<KeyValuePair<float, Cue>> GenerateAbsoluteCueList(List<Cue> cueList)
        {
            List<KeyValuePair<float, Cue>> absoluteCueList = new List<KeyValuePair<float, Cue>>();
            float sum = 0;
            foreach (Cue c in cueList)
            {
                sum += c.time;
                absoluteCueList.Add(new KeyValuePair<float,Cue>(sum, c));
            }
            return absoluteCueList;
        }
    }
}
