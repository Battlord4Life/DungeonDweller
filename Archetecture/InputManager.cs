
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DungeonDweller.Archetecture
{
    public enum direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public class InputManager
    {

        KeyboardState currentKBState;
        KeyboardState previousKBState;
        MouseState currentMState;
        MouseState previousMState;
        GamePadState currentGPState;
        GamePadState previousGPState;



        /// <summary>
        /// Requested Direciton
        /// </summary>
        public Vector2 Direction { get; private set; }

        /// <summary>
        /// Input for exiting
        /// </summary>
        public bool Exit { get; private set; }

        public bool Jump { get; private set; }

        /// <summary>
        /// Input if the player is Moving
        /// </summary>
        public bool Moving { get; private set; }

        public direction EnumDirection { get; private set; }

        public bool Active { get; set; }

        public InputManager()
        {
            Active = true;
        }

        /// <summary>
        /// Updates each frame
        /// </summary>
        /// <param name="gameTime">information</param>
        public void Update(GameTime gameTime)
        {

            #region Input State Updating

            previousKBState = currentKBState;
            previousMState = currentMState;
            previousGPState = currentGPState;
            currentKBState = Keyboard.GetState();
            currentMState = Mouse.GetState();
            currentGPState = GamePad.GetState(0);

            #endregion

            if (Active)
            {

                #region Direction Input

                Vector2 PrevDir = Direction;
                //Get Mouse Pos
                Direction = currentGPState.ThumbSticks.Right * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Moving = !(Direction == PrevDir);



                //Get Postion from KB

                if (currentKBState.IsKeyDown(Keys.Up) || currentKBState.IsKeyDown(Keys.W))
                {
                    Moving = true;
                    Direction += new Vector2(0, -100 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    EnumDirection = direction.North;
                }
                if (currentKBState.IsKeyDown(Keys.Down) || currentKBState.IsKeyDown(Keys.S))
                {
                    Moving = true;
                    Direction += new Vector2(0, 100 * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    EnumDirection = direction.South;
                }
                if (currentKBState.IsKeyDown(Keys.Left) || currentKBState.IsKeyDown(Keys.A))
                {
                    Moving = true;
                    Direction += new Vector2(-100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                    EnumDirection = direction.West;

                }
                if (currentKBState.IsKeyDown(Keys.Right) || currentKBState.IsKeyDown(Keys.D))
                {
                    Moving = true;
                    Direction += new Vector2(100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                    EnumDirection = direction.East;
                }

                #endregion

                #region Jump Input
                if ((currentKBState.IsKeyDown(Keys.Space) && !previousKBState.IsKeyDown(Keys.Space) || currentGPState.IsButtonDown(Buttons.A) && !previousGPState.IsButtonDown(Buttons.A)))
                {
                    Jump = true;
                }
                else
                {
                    Jump = false;
                }


                #endregion
            }
            else
            {
                Direction = new Vector2(0, 0);

            }
            #region Exit Input

            if (currentGPState.Buttons.Back == ButtonState.Pressed || currentKBState.IsKeyDown(Keys.Escape))
                Exit = true;

            #endregion



        }

    }
}
