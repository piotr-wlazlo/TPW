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
    #region ctor
        
    public DataImplementation() {
        MoveTaskTokenSource = new CancellationTokenSource();
        MoveTask = Task.Run(() => MoveAsync(MoveTaskTokenSource.Token));
    }

    #endregion ctor

    private const double TableWidth = 400.0;
    private const double TableHeight = 400.0;
    private const double BallRadius = 10.0;
    double[] MassValues = [1.0, 2.5, 5.0];


    #region DataAbstractAPI

    public override void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler) {
      if (Disposed)
        throw new ObjectDisposedException(nameof(DataImplementation));
      if (upperLayerHandler == null)
        throw new ArgumentNullException(nameof(upperLayerHandler));
      Random random = new Random();

      lock (BallsList) {
        for (int i = 0; i < numberOfBalls; i++) {
            Vector startingPosition = new(random.Next(10, (int)TableWidth - 10), random.Next(10, (int)TableHeight - 10));

            double velocity = random.NextDouble() * 3 - 1;
            double angle = 2 * Math.PI * random.NextDouble();

            Vector startingVelocity = new(velocity * Math.Cos(angle), velocity * Math.Sin(angle));

            double Mass = MassValues[random.Next(MassValues.Length)];

            Ball newBall = new(startingPosition, startingVelocity, Mass, BallRadius * 2.0);
            upperLayerHandler(startingPosition, newBall);
            BallsList.Add(newBall);
        }
        
      }
    }
    
    public override void AddBall(Action<IVector, IBall> upperLayerHandler) {
        if (Disposed)
            throw new ObjectDisposedException(nameof(DataImplementation));
        if (upperLayerHandler == null)
            throw new ArgumentNullException(nameof(upperLayerHandler));

        Random random = new Random();

        Vector startingPosition = new(random.Next(10, 390), random.Next(10, 390));
        Vector startingVelocity = new(random.NextDouble() * 3 - 1, random.NextDouble() * 3 - 1);

        double Mass = MassValues[random.Next(MassValues.Length)];

        Ball newBall = new(startingPosition, startingVelocity, Mass, BallRadius * 2.0);
        upperLayerHandler(startingPosition, newBall);

        lock (BallsList) {
            BallsList.Add(newBall);
        }
    }

        public override void RemoveBall() {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));
            if (BallsList.Count == 0)
                throw new InvalidOperationException("no balls to remove");
            lock (BallsList) {
                if (BallsList.Count > 0) {
                    BallsList.RemoveAt(BallsList.Count - 1);
                }
            }
        }

        private async Task MoveAsync(CancellationToken token) {
            try {
                while (!token.IsCancellationRequested) {
                    lock (BallsList) {
                        Move();
                    }

                    await Task.Delay(16, token);
                }
            }
            catch (TaskCanceledException) { }
        }

        #endregion DataAbstractAPI

        #region IDisposable

        protected void Dispose(bool disposing) {
            if (!Disposed) {
                if (disposing) {
                    MoveTaskTokenSource?.Cancel();
                    MoveTask?.Wait();
                    MoveTaskTokenSource?.Dispose();

                    lock (BallsList) {
                        BallsList.Clear();
                    }
                }
                Disposed = true;
            }
        }


        public override void Dispose() {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }

    #endregion IDisposable

    #region private

    //private bool disposedValue;
    private bool Disposed = false;

    private Random RandomGenerator = new();
    private List<Ball> BallsList = [];
        private Task? MoveTask;
        private CancellationTokenSource MoveTaskTokenSource;


        private void Move() {
            foreach (Ball ball in BallsList) {
                ball.Move(new Vector(ball.Velocity.x, ball.Velocity.y));

                double xPosition = ball.Position.x;
                double yPosition = ball.Position.y;

                double xVelocity = ball.Velocity.x;
                double yVelocity = ball.Velocity.y;

                if ((xPosition - BallRadius <= 0 && xVelocity < 0) || xPosition + BallRadius >= TableWidth && xVelocity > 0) {
                    xVelocity = -xVelocity;
                }

                if ((yPosition - BallRadius <= 0 && yVelocity < 0) || yPosition + BallRadius >= TableHeight && yVelocity > 0) {
                    yVelocity = -yVelocity;
                }

                double newX = xPosition + xVelocity;
                double newY = yPosition + yVelocity;

                ball.Velocity = new Vector(xVelocity, yVelocity);
                ball.Move(new Vector(newX - xPosition, newY - yPosition));
            }

            for (int i = 0; i < BallsList.Count; i++) {
                for (int j = i + 1; j < BallsList.Count; j++) {
                    Ball Ball1 = BallsList[i];
                    Ball Ball2 = BallsList[j];

                    Vector Position1 = Ball1.Position;
                    Vector Position2 = Ball2.Position;
                    Vector Delta = Position1 - Position2;
                    double Distance = Delta.Length;

                    if (Distance < (Ball1.Diameter + Ball2.Diameter) / 2.0 && Distance > 0) {
                        Vector Velocity1 = (Vector)Ball1.Velocity;
                        Vector Velocity2 = (Vector)Ball2.Velocity;

                        double Mass1 = Ball1.Mass;
                        double Mass2 = Ball2.Mass;

                        Vector Normal = Delta.Normalize();
                        Vector Tangent = new Vector(-Normal.y, Normal.x);

                        double Velocity1Normal = Normal.Dot(Velocity1);
                        double Velocity2Normal = Normal.Dot(Velocity2);

                        double Velocity1Tangent = Tangent.Dot(Velocity1);
                        double Velocity2Tangent = Tangent.Dot(Velocity2);

                        double newVelocity1Normal = (Velocity1Normal * (Mass1 - Mass2) + 2 * Mass2 * Velocity2Normal) / (Mass1 + Mass2);
                        double newVelocity2Normal = (Velocity2Normal * (Mass2 - Mass1) + 2 * Mass1 * Velocity1Normal) / (Mass1 + Mass2);

                        Vector newVelocity1NormalVector = Normal * newVelocity1Normal;
                        Vector newVelocity2NormalVector = Normal * newVelocity2Normal;

                        Vector newVelocity1TangentVector = Tangent * Velocity1Tangent;
                        Vector newVelocity2TangentVector = Tangent * Velocity2Tangent;

                        Ball1.Velocity = newVelocity1NormalVector + newVelocity1TangentVector;
                        Ball2.Velocity = newVelocity2NormalVector + newVelocity2TangentVector;

                        double Overlap = (Ball1.Diameter + Ball2.Diameter) / 2.0 - Distance;
                        Vector correction = Normal * (Overlap / 2.0);

                        Ball1.Move(correction);
                        Ball2.Move(new Vector(-correction.x, -correction.y));
                    }
                }
            }
        }


        #endregion private

        #region TestingInfrastructure

        [Conditional("DEBUG")]
    internal void CheckBallsList(Action<IEnumerable<IBall>> returnBallsList)
    {
            lock (BallsList) {
                returnBallsList(BallsList);
            }
        }

    [Conditional("DEBUG")]
    internal void CheckNumberOfBalls(Action<int> returnNumberOfBalls)
    {
      returnNumberOfBalls(BallsList.Count);
    }

    [Conditional("DEBUG")]
    internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
    {
      returnInstanceDisposed(Disposed);
    }

    #endregion TestingInfrastructure
  }
}