﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Experiments.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MonoGame_Experiments.Components
{
    public class Solid : Component, IRigidbody
    {

        public Collider Collider { get { return _entity.GetComponent<Collider>(); } }
        public static List<IRigidbody> Solids = new List<IRigidbody>();

        public Solid()
        {
            Solids.Add(this);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public bool MoveX(float amount, Action onCollide)
        {
            return false;
        }

        public bool MoveY(float amount, Action onCollide)
        {
            return false;
        }

        private List<Actor> GetAllRidingActors()
        {
            List<Actor> returnValue = new List<Actor>();

            foreach (Actor actor in Actor.Actors)
            {
                if (actor.IsRiding(this))
                {
                    returnValue.Add(actor);
                }
            }

            return returnValue;
        }

        public void Move(float x, float y)
        {
            float xRemainder = x;
            float yRemainder = y;
            int moveX = (int)Math.Round(xRemainder);
            int moveY = (int)Math.Round(yRemainder);

            Vector2 pos = Transform.Position;

            if (moveX != 0 || moveY != 0)
            {
                // Loop through every Actor in the Level
                // Add it to a list if actor.IsRiding(this) is true
                List<Actor> riding = GetAllRidingActors();

                // Make this Solid non-collidable for Actors,
                // so that Actors moved by it do not get stuck on it
                Collider.Collidable = false;


                if (moveX != 0)
                {
                    xRemainder -= moveX;
                    pos.X += moveX;
                    Transform.Position = pos;
                    //Collider.RefreshPositionToTransform();

                    if (moveX > 0)
                    {
                        foreach (Actor actor in Actor.Actors)
                        {
                            if (Collider.CollisionCheck(actor.Collider, Vector2.Zero))
                            {
                                // Push right
                                actor.MoveX(Collider.Right - actor.Collider.Left, new Action(actor.Squish));
                            }
                            else if (riding.Contains(actor))
                            {
                                // Carry right
                                actor.MoveX(moveX, null);
                            }
                        }
                    }
                    else
                    {
                        foreach (Actor actor in Actor.Actors)
                        {
                            if (Collider.CollisionCheck(actor.Collider, Vector2.Zero))
                            {
                                // Push left
                                actor.MoveX(Collider.Left - actor.Collider.Right, new Action(actor.Squish));
                            }
                            else if (riding.Contains(actor))
                            {
                                // Carry left
                                actor.MoveX(moveX, null);
                            }
                        }
                    }
                }

                if (moveY != 0)
                {
                    // Do y-axis movement
                    yRemainder -= moveY;
                    pos.Y += moveY;
                    Transform.Position = pos;
                    //Collider.RefreshPositionToTransform();

                    if (moveY > 0)
                    {
                        foreach (Actor actor in Actor.Actors)
                        {
                            if (Collider.CollisionCheck(actor.Collider, Vector2.Zero))
                            {
                                // Push down
                                actor.MoveY(Collider.Bottom - actor.Collider.Top, new Action(actor.Squish));
                                //actor.MoveY(moveY, null);
                            }
                            else if (riding.Contains(actor))
                            {
                                // Carry down
                                actor.MoveY(moveY, null);
                            }
                        }
                    }
                    else
                    {
                        foreach (Actor actor in Actor.Actors)
                        {
                            if (Collider.CollisionCheck(actor.Collider, Vector2.Zero))
                            {
                                // Push up
                                actor.MoveY(Collider.Top - actor.Collider.Bottom, new Action(actor.Squish));
                                //actor.MoveY(moveY, null);

                            }
                            else if (riding.Contains(actor))
                            {
                                // Carry up
                                actor.MoveY(moveY, null);
                            }
                        }
                    }
                }

                // Re-enable collisions for this Solid
                Collider.Collidable = true;

                Transform.Position = pos;
                Collider.RefreshPositionToTransform();
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Solids.Remove(this);
        }

    }
}
