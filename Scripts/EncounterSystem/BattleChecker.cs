using UnityEngine;

namespace Sands{
    public class BattleChecker : MonoBehaviour
    {

        //Script adds and checks the different Statuses of Hero or Enemy Party Members

        public bool UsedTurn {get; set;}        //tracks if hero took turn
        public bool IsDead {get; set;}          //track if hero is defeated
        public int DOTCounter { get; set;}      //track if hero is under effect of Damage Over Time (DOT)

        void Start()
        {
            UsedTurn = false;
            IsDead = false;
            DOTCounter = 0;
        }
    }
}
