using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPanelCloseEvent : StateMachineBehaviour
{
    //Уничтожение префаба после завершения анимации закрытия панели
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject);
    }
}
