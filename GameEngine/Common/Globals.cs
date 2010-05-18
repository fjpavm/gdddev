using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gdd.Game.Engine.Common
{
    public enum DISPLAY_STATE
    {
        THREE_DIM = 0,
        TWO_DIM = 1,
        BOTH = 2
    } ;

    public class Globals
    {
        public static DISPLAY_STATE displayState = DISPLAY_STATE.THREE_DIM;
    }
}
