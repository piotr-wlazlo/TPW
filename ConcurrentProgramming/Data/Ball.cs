//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data {
    internal class Ball : IBall {
        #region ctor

        internal Ball(Vector initialPosition, Vector initialVelocity, double mass, double diameter = 10.0) {
            position = initialPosition;
            Velocity = initialVelocity;
            Mass = mass;
            Diameter = diameter;
            BallID = Interlocked.Increment(ref NextBallID);
        }

        #endregion ctor

        #region IBall

        public event EventHandler<IVector>? NewPositionNotification;

        public IVector Velocity { get; set; }
        public int BallID { get; }


        #endregion IBall

        #region private

        private Vector position;
        private static int NextBallID = 0;

        private void RaiseNewPositionChangeNotification() {
            NewPositionNotification?.Invoke(this, position);
        }

        internal void Move(Vector delta) {
            position = new Vector(position.x + delta.x, position.y + delta.y);
            RaiseNewPositionChangeNotification();
        }

        #endregion private

        public Vector Position => position;

        IVector IBall.Position => Position;
        public double Mass { get; }
        public double Diameter { get; }
    }
}