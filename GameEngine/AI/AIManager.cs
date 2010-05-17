using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Gdd.Game.Engine.AI
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AIManager : Microsoft.Xna.Framework.GameComponent
    {
        public static MessageQueue messageQueue =  new MessageQueue();

        //NOTE: Needs to be changed to our gameObject
        //NOTE: Needs to be set to the Scene's gameObject list
        public ICollection<IAIEntity> objectList;

        public AIManager(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            double t = gameTime.TotalGameTime.TotalSeconds;
            messageQueue.updateCurrentTime(t);
            messageQueue.deliverMessages();

            foreach (IAIEntity ai in objectList) 
            {
                ai.UpdateAI(gameTime);
            }

            base.Update(gameTime);
        }
    }
}