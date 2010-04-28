// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Camera.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The camera.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine
{
    using Scenes;

    using Microsoft.Xna.Framework;
    using Gdd.Game.Engine.Levels.Characters;

    /// <summary>
    /// The camera.
    /// </summary>
    public class Camera : SceneComponent
    {
        #region Constants and Fields

        /// <summary>
        /// The aspect ratio.
        /// </summary>
        private float aspectRatio;

        /// <summary>
        /// The far plane distance.
        /// </summary>
        private float farPlaneDistance;

        /// <summary>
        /// The field of view.
        /// </summary>
        private float fieldOfView = 45.0f;

        /// <summary>
        /// The has changed.
        /// </summary>
        private bool hasChanged;

        /// <summary>
        /// The look vector.
        /// </summary>
        protected Vector3 lookVector;

        /// <summary>
        /// The near plane distance.
        /// </summary>
        private float nearPlaneDistance;

        /// <summary>
        /// The position.
        /// </summary>
        protected Vector3 position;

        /// <summary>
        /// The right vector.
        /// </summary>
        private Vector3 rightVector;

        /// <summary>
        /// The rotation radian.
        /// </summary>
        private Vector3 rotationRadian;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="game">
        /// The game instance.
        /// </param>
        /// <param name="pos">
        /// The position.
        /// </param>
        public Camera(Game game, Vector3 pos)
            : base(game)
        {
            this.aspectRatio = game.GraphicsDevice.Viewport.AspectRatio;
            this.fieldOfView = 45.0f;
            this.nearPlaneDistance = 1.0f;
            this.farPlaneDistance = 10000.0f;

            this.rotationRadian = Vector3.Zero; // MathHelper.PiOver4;

            this.position = pos;

            this.hasChanged = true;


            this.UpdatePerspective();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets AspectRatio.
        /// </summary>
        public float AspectRatio
        {
            get
            {
                return this.aspectRatio;
            }

            set
            {
                this.aspectRatio = value;
                this.UpdatePerspective();
            }
        }

        /// <summary>
        /// Gets or sets FarPlaneDistance.
        /// </summary>
        public float FarPlaneDistance
        {
            get
            {
                return this.farPlaneDistance;
            }

            set
            {
                this.farPlaneDistance = value;
                this.UpdatePerspective();
            }
        }

        /// <summary>
        /// Gets or sets FieldOfView.
        /// </summary>
        public float FieldOfView
        {
            get
            {
                return this.fieldOfView;
            }

            set
            {
                this.fieldOfView = value;
                this.UpdatePerspective();
            }
        }

        /// <summary>
        /// Gets LookAt.
        /// </summary>
        public Vector3 LookAt
        {
            get
            {
                return this.lookVector;
            }
        }

        /// <summary>
        /// Gets or sets NearPlaneDistance.
        /// </summary>
        public float NearPlaneDistance
        {
            get
            {
                return this.nearPlaneDistance;
            }

            set
            {
                this.nearPlaneDistance = value;
                this.UpdatePerspective();
            }
        }

        /// <summary>
        /// Gets Perspective.
        /// </summary>
        public Matrix Perspective { get; private set; }

        /// <summary>
        /// Gets Pos.
        /// </summary>
        public Vector3 Pos
        {
            get
            {
                return this.position;
            }
        }

        /// <summary>
        /// Gets or sets RotationRadian.
        /// </summary>
        public Vector3 RotationRadian
        {
            get
            {
                return this.rotationRadian;
            }

            set
            {
                this.rotationRadian = value;
                this.hasChanged = true;
            }
        }

        /// <summary>
        /// Gets View.
        /// </summary>
        public Matrix View { get; protected set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Moves the Camera.
        /// </summary>
        /// <param name="delta">
        /// The delta.
        /// </param>
        public void Move(Vector3 delta)
        {
            this.position += delta;
            this.hasChanged = true;
        }

        /// <summary>
        /// The move z.
        /// </summary>
        /// <param name="delta">
        /// The delta.
        /// </param>
        public void MoveForwardBackward(float delta)
        {
            this.position += this.lookVector * delta;
            this.hasChanged = true;
        }

        /// <summary>
        /// The move y.
        /// </summary>
        /// <param name="delta">
        /// The delta.
        /// </param>
        public void MoveUpDown(float delta)
        {
            this.position += Vector3.Up * delta;
            this.hasChanged = true;
        }

        /// <summary>
        /// The pitch.
        /// </summary>
        /// <param name="degrees">
        /// The degrees.
        /// </param>
        public void Pitch(float degrees)
        {
            this.rotationRadian.X = MathHelper.WrapAngle(this.rotationRadian.X + degrees);
            this.hasChanged = true;
        }

        /// <summary>
        /// The project.
        /// </summary>
        /// <param name="vector">
        /// The vector.
        /// </param>
        /// <returns>
        /// </returns>
        public Vector2 Project(Vector3 vector)
        {
            Vector3 projection = this.Game.GraphicsDevice.Viewport.Project(
                vector, this.Perspective, this.View, Matrix.Identity);
            return new Vector2(projection.X, projection.Y);
        }

        /// <summary>
        /// Rolls the Camera.
        /// </summary>
        /// <param name="degrees">
        /// The degrees.
        /// </param>
        public void Roll(float degrees)
        {
            this.rotationRadian.Z = MathHelper.WrapAngle(this.rotationRadian.Z + degrees);
            this.hasChanged = true;
        }

        /// <summary>
        /// The move x.
        /// </summary>
        /// <param name="delta">
        /// The delta.
        /// </param>
        public void StrafeRightLeft(float delta)
        {
            this.position += this.rightVector * delta;
            this.hasChanged = true;
        }

        /// <summary>
        /// The unproject.
        /// </summary>
        /// <param name="x">
        /// The x coordinate.
        /// </param>
        /// <param name="y">
        /// The y coordinate.
        /// </param>
        /// <returns>
        /// Returns the Ray from the near plane to the far plane.
        /// </returns>
        public Ray Unproject(int x, int y)
        {
            var nearSource = new Vector3(x, y, this.nearPlaneDistance);
            var farSource = new Vector3(x, y, this.farPlaneDistance);

            // find the two screen space positions in world space
            Vector3 nearPoint = this.Game.GraphicsDevice.Viewport.Unproject(
                nearSource, this.Perspective, this.View, Matrix.Identity);
            Vector3 farPoint = this.Game.GraphicsDevice.Viewport.Unproject(
                farSource, this.Perspective, this.View, Matrix.Identity);

            // normalized direction vector from nearPoint to farPoint
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // create a ray using nearPoint as the source
            return new Ray(nearPoint, direction);
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Update(GameTime gameTime)
        {
            if (this.hasChanged)
            {
                this.lookVector = Vector3.Transform(
                    -Vector3.UnitZ, 
                    Matrix.CreateFromYawPitchRoll(this.rotationRadian.Y, this.rotationRadian.X, this.rotationRadian.Z));
                this.rightVector = Vector3.Transform(
                    Vector3.UnitX, 
                    Matrix.CreateFromYawPitchRoll(this.rotationRadian.Y, this.rotationRadian.X, this.rotationRadian.Z));

                this.View = Matrix.CreateLookAt(this.position, this.position + this.lookVector, Vector3.Up);
                this.hasChanged = false;
            }
        }

        /// <summary>
        /// Yaws the Camera.
        /// </summary>
        /// <param name="degrees">
        /// The degrees.
        /// </param>
        public void Yaw(float degrees)
        {
            this.rotationRadian.Y = MathHelper.WrapAngle(this.rotationRadian.Y + degrees);
            this.hasChanged = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the Perspective.
        /// </summary>
        private void UpdatePerspective()
        {
            this.Perspective = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(this.fieldOfView), this.aspectRatio, this.nearPlaneDistance, this.farPlaneDistance);
        }

        #endregion
    }
}