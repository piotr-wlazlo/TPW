//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TP.ConcurrentProgramming.Data {
    internal class DataImplementation : DataAbstractAPI {
        #region Fields

        private bool Disposed = false;
        private readonly List<Thread> BallThreads = new();
        private readonly List<BallWorker> BallWorkers = new();
        private readonly object _lock = new object();
        private List<Ball> BallsList = new();

        private const double BallRadius = 10.0;
        double[] MassValues = [1.0, 2.5, 5.0];

        #endregion Fields

        #region ctor

        public DataImplementation() {
        }

        #endregion ctor

        #region DataAbstractAPI

        public override void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler) {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));
            if (upperLayerHandler == null)
                throw new ArgumentNullException(nameof(upperLayerHandler));

            Random random = new Random();

            for (int i = 0; i < numberOfBalls; i++) {
                Vector startingPosition = new Vector(
                    random.Next(10, 390),
                    random.Next(10, 390)
                );

                Vector velocity = new Vector(
                    random.NextDouble() * 3 - 1,
                    random.NextDouble() * 3 - 1
                );
                double Mass = MassValues[random.Next(MassValues.Length)];

                Ball newBall = new Ball(startingPosition, velocity, Mass, BallRadius * 2.0);
                upperLayerHandler(startingPosition, newBall);

                lock (_lock) {
                    BallsList.Add(newBall);
                }

                var Worker = new BallWorker(newBall, _lock, BallsList, () => Disposed);
                Thread thread = new Thread(Worker.Run);
                BallWorkers.Add(Worker);
                BallThreads.Add(thread);
                thread.Start();
            }
        }

        public override void AddBall(Action<IVector, IBall> upperLayerHandler) {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));
            if (upperLayerHandler == null)
                throw new ArgumentNullException(nameof(upperLayerHandler));

            Random random = new Random();

            Vector startingPosition = new Vector(
                random.Next(10, 390),
                random.Next(10, 390)
            );

            Vector velocity = new Vector(
                random.NextDouble() * 3 - 1,
                random.NextDouble() * 3 - 1
            );

            double Mass = MassValues[random.Next(MassValues.Length)];

            Ball newBall = new Ball(startingPosition, velocity, Mass, BallRadius * 2.0);
            upperLayerHandler(startingPosition, newBall);

            lock (_lock) {
                BallsList.Add(newBall);
            }

            var Worker = new BallWorker(newBall, _lock, BallsList, () => Disposed);
            Thread thread = new Thread(Worker.Run);
            BallWorkers.Add(Worker);
            BallThreads.Add(thread);
            thread.Start();
        }

        public override void RemoveBall() {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));

            BallWorker Worker = null;
            Thread BallThread = null;

            lock (_lock) {
                if (BallsList.Count > 0) {
                    Ball BallToRemove = BallsList[BallsList.Count - 1];
                    Worker = BallWorkers[BallWorkers.Count - 1];
                    BallThread = BallThreads[BallThreads.Count - 1];

                    BallsList.RemoveAt(BallsList.Count - 1);
                    BallWorkers.RemoveAt(BallWorkers.Count - 1);
                    BallThreads.RemoveAt(BallThreads.Count - 1);
                }
            }


            if (Worker != null && BallThread != null) {
                Worker.Stop();
                BallThread.Join();
            }
        }

        #endregion DataAbstractAPI

        #region IDisposable

        protected void Dispose(bool disposing) {
            if (!Disposed) {
                if (disposing) {
                    Disposed = true;

                    foreach (var Worker in BallWorkers) {
                        Worker.Stop();
                    }

                    foreach (var thread in BallThreads) {
                        thread.Join();
                    }

                    lock (_lock) {
                        BallsList.Clear();
                        BallWorkers.Clear();
                        BallThreads.Clear();
                    }
                }
            }
        }

        public override void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

        #region BallWorker

        private class BallWorker {
            private readonly Ball _ball;
            private readonly object _lock;
            private readonly List<Ball> _ballsList;
            private readonly Func<bool> _isDisposed;
            private bool _localDisposed = false;
            private const double BallRadius = 10.0;
            private const double TableWidth = 400.0;
            private const double TableHeight = 400.0;

            public BallWorker(Ball ball, object lockObject, List<Ball> ballsList, Func<bool> isDisposed) {
                _ball = ball;
                _lock = lockObject;
                _ballsList = ballsList;
                _isDisposed = isDisposed;
            }

            public void Run() {
                while (!_isDisposed() && !_localDisposed) {
                    lock (_lock) {
                        UpdatePosition();
                        HandleCollisions();
                    }
                    Thread.Sleep(16);
                }
            }

            public void Stop() {
                _localDisposed = true;
            }

            private void UpdatePosition() {
                Vector position = (Vector)_ball.Position;
                Vector velocity = (Vector)_ball.Velocity;

                double x = position.x;
                double y = position.y;
                double vx = velocity.x;
                double vy = velocity.y;

                if ((x - BallRadius <= 0 && vx < 0) || (x + BallRadius >= TableWidth && vx > 0)) {
                    vx = -vx;
                }

                if ((y - BallRadius <= 0 && vy < 0) || (y + BallRadius >= TableHeight && vy > 0)) {
                    vy = -vy;
                }

                _ball.Velocity = new Vector(vx, vy);
                _ball.Move(new Vector(vx, vy));
            }

            private void HandleCollisions() {
                foreach (var other in _ballsList) {
                    if (other == _ball)
                        continue;

                    Vector pos1 = _ball.Position;
                    Vector pos2 = other.Position;
                    Vector delta = pos1 - pos2;
                    double distance = delta.Length;

                    if (distance < (_ball.Diameter + other.Diameter) / 2.0 && distance > 0) {
                        Vector vel1 = (Vector)_ball.Velocity;
                        Vector vel2 = (Vector)other.Velocity;

                        double mass1 = _ball.Mass;
                        double mass2 = other.Mass;

                        Vector normal = delta.Normalize();
                        Vector tangent = new Vector(-normal.y, normal.x);

                        double vel1Normal = normal.Dot(vel1);
                        double vel2Normal = normal.Dot(vel2);
                        double vel1Tangent = tangent.Dot(vel1);
                        double vel2Tangent = tangent.Dot(vel2);

                        double newVel1Normal = (vel1Normal * (mass1 - mass2) + 2 * mass2 * vel2Normal) / (mass1 + mass2);
                        double newVel2Normal = (vel2Normal * (mass2 - mass1) + 2 * mass1 * vel1Normal) / (mass1 + mass2);

                        Vector newVel1NormalVec = normal * newVel1Normal;
                        Vector newVel2NormalVec = normal * newVel2Normal;

                        Vector newVel1 = newVel1NormalVec + tangent * vel1Tangent;
                        Vector newVel2 = newVel2NormalVec + tangent * vel2Tangent;

                        _ball.Velocity = newVel1;
                        other.Velocity = newVel2;

                        double overlap = (_ball.Diameter + other.Diameter) / 2.0 - distance;
                        Vector correction = normal * (overlap / 2.0);

                        _ball.Move(correction);
                        other.Move(new Vector(-correction.x, -correction.y));
                    }
                }
            }
        }

        #endregion BallWorker
        
        #region TestingInfrastructure

        [Conditional("DEBUG")]
        internal void CheckBallsList(Action<IEnumerable<IBall>> returnBallsList) {
            lock (BallsList) {
                returnBallsList(BallsList);
            }
        }

        [Conditional("DEBUG")]
        internal void CheckNumberOfBalls(Action<int> returnNumberOfBalls) {
            returnNumberOfBalls(BallsList.Count);
        }

        [Conditional("DEBUG")]
        internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed) {
            returnInstanceDisposed(Disposed);
        }

        #endregion TestingInfrastructure
    }
}
