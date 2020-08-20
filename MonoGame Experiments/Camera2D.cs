﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Experiments
{
    public class Camera2D
    {
        #region Variables
        private Matrix _transformationMatrix = Matrix.Identity;
        private Matrix _inverseMatrix = Matrix.Identity;
        private Vector2 _position = Vector2.Zero;
        private float _rotation = 0;
        private Vector2 _zoom = Vector2.One;
        private Vector2 _origin = Vector2.Zero;
        private bool _hasChanged;

        public Viewport Viewport;
        #endregion

        #region Constructors
        public Camera2D(Viewport viewport)
        {
            Viewport = viewport;
        }

        public Camera2D(int width, int height)
        {
            Viewport = new Viewport();
            Viewport.Width = width;
            Viewport.Height = height;
        }
        #endregion

        #region Methods
        private void UpdateMatrices()
        {

            var positionTranslationMatrix = Matrix.CreateTranslation(new Vector3()
            {
                X = -(int)Math.Floor(_position.X),
                Y = -(int)Math.Floor(_position.Y),
                Z = 0
            });

            var rotationMatrix = Matrix.CreateRotationZ(_rotation);

            var scaleMatrix = Matrix.CreateScale(new Vector3()
            {
                X = _zoom.X,
                Y = _zoom.Y,
                Z = 1
            });

            var originTranslationMatrix = Matrix.CreateTranslation(new Vector3()
            {
                X = (int)Math.Floor(_origin.X),
                Y = (int)Math.Floor(_origin.Y),
                Z = 0
            });

            _transformationMatrix = positionTranslationMatrix * rotationMatrix * scaleMatrix * originTranslationMatrix;

            _inverseMatrix = Matrix.Invert(_transformationMatrix);

            _hasChanged = false;
        }

        private void SetCenter(Vector2 position)
        {
            //Position = position - new Vector2(_transformationMatrix.)
        }
        #endregion

        #region Properties
        public Matrix TransformationMatrix
        {
            get
            {
                // If a change is detected, update matrices before returning value
                if (_hasChanged)
                    UpdateMatrices();

                return _transformationMatrix;
            }
        }

        public Matrix InverseMatrix
        {
            get
            {
                // If a change is detected, update matrices before returning value
                if (_hasChanged)
                    UpdateMatrices();

                return _inverseMatrix;
            }
        }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                // If the value hasn't actually changed, just return back
                if (_position == value) { return; }

                // Set the position value
                _position = value;

                _position = Vector2.Clamp(_position, Vector2.Zero, new Vector2(512 - Viewport.Width, 512 - Viewport.Height));

                // Flag that a change has been made
                _hasChanged = true;
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                // If the value hasn't actually changed, just return back
                if (_rotation == value) { return; }

                // Set the rotation value
                _rotation = value;

                // Flag that a change has been made
                _hasChanged = true;
            }
        }

        public Vector2 Zoom
        {
            get { return _zoom; }
            set
            {
                // If the value hasn't actually changed, just return back
                if (_zoom == value) { return; }

                // Set the zoom value
                _zoom = value;

                // Flag that a change has been made
                _hasChanged = true;
            }
        }

        public Vector2 Origin
        {
            get { return _origin; }
            set
            {
                // If the value hasn't actually changed, just return back
                if (_origin == value) { return; }

                // Set the origin value
                _origin = value;

                // Flag that a change has been made
                _hasChanged = true;
            }
        }

        public float X
        {
            get { return _position.X; }
            set
            {
                // If the value hasn't actually changed, just return back
                if (_position.X == value) { return; }

                // Set the position x value
                _position.X = value;

                // Flag that a change has been made
                _hasChanged = true;
            }
        }

        public float Y
        {
            get { return _position.Y; }
            set
            {
                // If the value hasn't actually changed, just return back
                if (_position.Y == value) { return; }

                // Set the position y value
                _position.Y = value;

                // Flag that a change has been made
                _hasChanged = true;
            }
        }
        #endregion

        #region Conversion Methods
        public Vector2 ScreenToWorld(Vector2 position)
        {
            return Vector2.Transform(position, InverseMatrix);
        }

        public Vector2 WorldToScreen(Vector2 position)
        {
            return Vector2.Transform(position, TransformationMatrix);
        }
        #endregion
    }
}
