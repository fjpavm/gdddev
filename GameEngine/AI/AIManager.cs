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

        protected ICollection<IAIEntity> objectList;

        protected MessageProcessorGroup heroPositionListners = new MessageProcessorGroup();

        protected Levels.Characters.Hero hero;

        public void addSceneComponent(Scenes.SceneComponent sc)
        {
            AI.IAIEntity ai = sc as AI.IAIEntity;
            if (ai != null)
            {
                objectList.Add(ai);
                heroPositionListners.addMessageProcessor(ai);
            }
            Levels.Characters.Hero h = sc as Levels.Characters.Hero;
            if (h != null) 
            {
                hero = h;
            }
        }

        public void removeSceneComponent(Scenes.SceneComponent sc)
        {
            AI.IAIEntity ai = sc as AI.IAIEntity;
            if (ai != null)
            {
                objectList.Remove(ai);
                heroPositionListners.removeMessageProcessor(ai);
            }
            Levels.Characters.Hero h = sc as Levels.Characters.Hero;
            if (h != null)
            {
                hero = null;
            }
        }

        public AIManager(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            objectList = new List<IAIEntity>();
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
            if (hero != null) 
            {
                Message m = new Message();
                m.from = hero;
                m.to = heroPositionListners;
                m.timeDelivery = 0;
                m.MessageType = MessageTypes.characterPosition;
                messageQueue.sendMessage(m);
            }
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