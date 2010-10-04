using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gdd.Game.Engine.AI.StateMachines
{
    class AnimationCommandDictionary
    {
        static List<string> commandList = new List<string>();

        static public int register(string commandName)
        {
            for (int i = 0; i < commandList.Count; i++) 
            {
                //NOTE: we may be regestering the same command for a different AIMonster
                //so this loop and test avoid repetitions
                if (commandList[i] == commandName) {
                    return i;
                }
            }
            int r = commandList.Count;
            commandList.Add(commandName);
            return r;
        }

        static public int lookUp(string commandName) 
        {
            for (int i = 0; i < commandList.Count; i++)
            {
                if (commandList[i] == commandName)
                {
                    return i;
                }
            }
            return -1; 
        } 
    }
}
