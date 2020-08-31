using ChaiFoxes.FMODAudio.Studio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MonoGame_Experiments.Components
{
    public class Player : Actor
    {
        private Vector2 _velocity = Vector2.Zero;
        private Vector2 _positionCheck = Vector2.Zero;
        private Vector2 _positionLast = Vector2.Zero;
        private Queue<float> _previousSpeeds = new Queue<float>(10);

        private Sprite _sprite;

        private float _coyoteTime = 0f;
        private float _coyoteTimeMax = .1f;
        private float _jumpBufferTime = 0f;
        private float _jumpBufferTimeMax = .1f;
        private bool _isGrounded = false;
        private bool _camJump
        {
            get
            {
                if (_coyoteTime > 0)
                {
                    return true;
                }

                return false;
            }
        }

        private EventDescription _jumpEventDescription;
        private EventDescription _landEventDescription;
        private Dictionary<string, EventInstance> _eventInstances = new Dictionary<string, EventInstance>();

        public Player(Entity entity) : base(entity)
        {
            Collider.LocalPosition = new Vector2(1, 3);
            Collider.Width = 14;
            Collider.Height = 13;

            _jumpEventDescription = AudioManager.LoadEvent("event:/SOUNDS/PLAYER/Jump");
            _landEventDescription = AudioManager.LoadEvent("event:/SOUNDS/PLAYER/Land");
            _jumpEventDescription.LoadSampleData();
            _landEventDescription.LoadSampleData();
            _eventInstances.Add("Jump", _jumpEventDescription.CreateInstance());
            _eventInstances.Add("Land", _landEventDescription.CreateInstance());

            Texture2D texture = ContentHandler.Instance.Load<Texture2D>("Sprites/SlimeCube");
            entity.AddComponent(new Sprite(entity, texture, texture.Width, texture.Height, Vector2.Zero, .5f));
        }

        public override void Awake()
        {
            base.Awake();

            Transform.OnMoveEvent += UpdateFollowCameraPosition;

            UpdateFollowCameraPosition();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_sprite == null) _sprite = _entity.GetComponent<Sprite>();

            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            _positionLast = _positionCheck;
            _positionCheck = Transform.Position;
            _previousSpeeds.Enqueue(Vector2.Distance(_positionCheck, _positionLast));
            if (_previousSpeeds.Count > 10) _previousSpeeds.Dequeue();

            float moveAmountX = 0;
            if (Input.IsInputDown(new List<Keys> { Keys.Left, Keys.A }, new List<Buttons> { Buttons.DPadLeft, Buttons.LeftThumbstickLeft }))
            {
                moveAmountX -= 2;
                _sprite.FlipX = true;
            }
            if (Input.IsInputDown(new List<Keys> { Keys.Right, Keys.D }, new List<Buttons> { Buttons.DPadRight, Buttons.LeftThumbstickRight }))
            {
                moveAmountX += 2;
                _sprite.FlipX = false;
            }

            _velocity.X = MathHelper.Lerp(_velocity.X, moveAmountX, 0.2f);

            float grav = 0.2f;
            if (_velocity.Y < 1 && _velocity.Y > -0.5f)
                grav /= 2;
            _velocity.Y += grav;
            if (_velocity.Y > 0)
            {
                _velocity.Y += grav / 2;
                if (_eventInstances["Jump"].PlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING) _eventInstances["Jump"].Stop();
            }

            // TODO: Take the _jumpBufferTime out and into some sort of PlayerInput class, which would buffer this input action.
            _jumpBufferTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Input.IsInputPressed(new List<Keys> { Keys.Space }, new List<Buttons> { Buttons.A }))
            {
                if (_camJump)
                {
                    Jump();
                }
                else
                {
                    _jumpBufferTime = _jumpBufferTimeMax;
                }
            }
            if (_jumpBufferTime > 0 && _isGrounded && _positionCheck.Y == _positionLast.Y) Jump();

            if (Input.IsInputReleased(new List<Keys> { Keys.Space }, new List<Buttons> { Buttons.A }))
            {
                if (_velocity.Y < -1) _velocity.Y /= 3;
            }

            bool zoomCam = Input.IsInputDown(new List<Keys>() { Keys.E }, new List<Buttons>() { Buttons.LeftTrigger });
            //Game.Camera.Zoom = MathHelper.Lerp(Game.Camera.Zoom, zoomCam ? 2 : 1, 0.1f);


            Transform.Rotation += MathHelper.ToRadians(1);

            bool isGrounded = _isGrounded;

            MoveY(_velocity.Y * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds, OnCollideY, 4 + (int)_velocity.X);
            Collider.Update(gameTime);
            MoveX(_velocity.X * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds, OnCollideX);
            Collider.Update(gameTime);

            _coyoteTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            _isGrounded = false;
            if (Collider.CollideAt(Game._currentScene.Solids, Collider.Position + Vector2.UnitY) && _velocity.Y >= 0)
            {
                _coyoteTime = _coyoteTimeMax;
                _isGrounded = true;
            }

            if (_isGrounded && isGrounded == false) _eventInstances["Land"].Start();

        }

        private void Jump()
        {
            _velocity.Y = -4 + ((MathF.Min(MathF.Sign(_positionCheck.Y - _positionLast.Y), 0) * _previousSpeeds.Max()) / 1.5f);
            _coyoteTime = 0f;
            _jumpBufferTime = 0f;
            _isGrounded = false;

            _eventInstances["Jump"].Start();
        }

        public void UpdateFollowCameraPosition()
        {
            Game.Camera.SetCenter(_entity.transform.Position);
        }

        public override void Squish()
        {
            base.Squish();

            Entity.Destroy(_entity);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void OnCollideX()
        {
            _velocity.X = 0;
        }
        private void OnCollideY()
        {
            _velocity.Y = 0;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _jumpEventDescription.UnloadSampleData();
            _landEventDescription.UnloadSampleData();

            foreach (var pair in _eventInstances)
                pair.Value.Dispose();
        }

    }
}
