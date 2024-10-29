
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

        public bool Flash { get; private set; }

        public bool MoveUp { get; private set; }
        public bool MoveLeft { get; private set; }
        public bool MoveDown { get; private set; }
        public bool MoveRight { get; private set; }


        public bool Switch0 { get; private set; }
        public bool Switch1 { get; private set; }
        public bool Switch2 { get; private set; }
        public bool Switch3 { get; private set; }
        public bool Switch4 { get; private set; }

        public bool Refill { get; private set; }

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

                #region Flash Input
                if ((currentKBState.IsKeyDown(Keys.Space) && !previousKBState.IsKeyDown(Keys.Space) || currentGPState.IsButtonDown(Buttons.A) && !previousGPState.IsButtonDown(Buttons.A)))
                {
                    Flash = true;
                }
                else
                {
                    Flash = false;
                }
                #endregion

                #region Bool Direction Input

                if ((currentKBState.IsKeyDown(Keys.Up) || currentKBState.IsKeyDown(Keys.W)) && !(previousKBState.IsKeyDown(Keys.Up) || previousKBState.IsKeyDown(Keys.W)))
                {
                    MoveUp = true;
                }
                else
                {
                    MoveUp = false;
                }
                if ((currentKBState.IsKeyDown(Keys.Down) || currentKBState.IsKeyDown(Keys.S)) && !(previousKBState.IsKeyDown(Keys.Down) || previousKBState.IsKeyDown(Keys.S)))

                {
                    MoveDown = true;
                }
                else
                {
                    MoveDown = false;
                }
                if ((currentKBState.IsKeyDown(Keys.Right) || currentKBState.IsKeyDown(Keys.D)) && !(previousKBState.IsKeyDown(Keys.Right) || previousKBState.IsKeyDown(Keys.D)))
                {
                    MoveRight = true;
                }
                else
                {
                    MoveRight = false;
                }
                if ((currentKBState.IsKeyDown(Keys.Left) || currentKBState.IsKeyDown(Keys.A)) && !(previousKBState.IsKeyDown(Keys.Left) || previousKBState.IsKeyDown(Keys.A)))

                {
                    MoveLeft = true;
                }
                else
                {
                    MoveLeft = false;
                }

                #endregion

                #region Item Switching

                if ((currentKBState.IsKeyDown(Keys.D1)) && !(previousKBState.IsKeyDown(Keys.D1)))
                {
                    Switch0 = true;

                }
                else
                {
                    Switch0 = false;
                }
                if ((currentKBState.IsKeyDown(Keys.D2)) && !(previousKBState.IsKeyDown(Keys.D2)))
                {
                    Switch1 = true;

                }
                else
                {
                    Switch1 = false;
                }
                if ((currentKBState.IsKeyDown(Keys.D3)) && !(previousKBState.IsKeyDown(Keys.D3)))
                {
                    Switch2 = true;

                }
                else
                {
                    Switch2 = false;
                }
                if ((currentKBState.IsKeyDown(Keys.D4)) && !(previousKBState.IsKeyDown(Keys.D4)))
                {
                    Switch3 = true;

                }
                else
                {
                    Switch3 = false;
                }
                if ((currentKBState.IsKeyDown(Keys.D5)) && !(previousKBState.IsKeyDown(Keys.D5)))
                {
                    Switch4 = true;

                }
                else
                {
                    Switch4 = false;
                }

                #endregion

                #region Refill

                if ((currentKBState.IsKeyDown(Keys.R)) && !(previousKBState.IsKeyDown(Keys.R)))
                {
                    Refill = true;

                }
                else
                {
                    Refill = false;
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
