using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnTriangleEnds : StateMachineBehaviour
{
    //����� ��������� ����� �� ���������� ��������
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SceneManager.LoadScene("MainPage");
    }
}
