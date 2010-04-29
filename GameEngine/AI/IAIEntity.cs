using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Gdd.Game.Engine.AI
{
    public interface IAIEntity : IMessageProcessor
    {
        void UpdateAI(GameTime time);
    }
}
