using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMode : MonoBehaviour
{
    public CharacterMovement CM1;
    public CharacterMovement CM2;
    
    public void chooseRandom()
    {
        CM1.AgentMode = 0;
    }
    public void choosePlayer()
    {
        CM1.AgentMode = 1;
    }
    public void chooseMCTS()
    {
        CM1.AgentMode = 2;
    }
}
