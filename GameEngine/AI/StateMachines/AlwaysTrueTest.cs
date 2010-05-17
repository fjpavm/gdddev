using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gdd.Game.Engine.AI.StateMachines
{
    public class AlwaysTrueTest
    {
        /// <summary>
        /// The always true.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The always true.
        /// </returns>
        public static bool alwaysTrue(Message msg, object obj)
        {
            return true;
        }
    }
}
