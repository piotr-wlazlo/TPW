//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System;
using System.Diagnostics;

namespace TP.ConcurrentProgramming.Data {
  internal class DataImplementation : DataAbstractAPI {
    #region ctor
        
    public DataImplementation() {
      MoveTimer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000/60));
    }

        #endregion ctor

    private const double TableWidth = 400.0;
    private const double TableHeight = 400.0;
    private const double BallRadius = 10.0;
    private const double TableBorder = 5.0;
        
    #region DataAbstractAPI

    public override void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler) {
      if (Disposed)
        throw new ObjectDisposedException(nameof(DataImplementation));
      if (upperLayerHandler == null)
        throw new ArgumentNullException(nameof(upperLayerHandler));
      Random random = new Random();

      for (int i = 0; i < numberOfBalls; i++) {
        Vector startingPosition = new(random.Next(10, (int)TableWidth - 10), random.Next(10, (int)TableHeight - 10));
        
        double velocity = random.NextDouble() * 3 - 1;
        double angle = 2 * Math.PI * random.NextDouble();

        Vector startingVelocity = new(velocity * Math.Cos(angle), velocity * Math.Sin(angle));

        Ball newBall = new(startingPosition, startingVelocity);
        
        upperLayerHandler(startingPosition, newBall);
        
        BallsList.Add(newBall);
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

        Ball newBall = new(startingPosition, startingVelocity);
        upperLayerHandler(startingPosition, newBall);
        BallsList.Add(newBall);
    }

        public override void RemoveBall() {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));
            if (BallsList.Count == 0)
                throw new InvalidOperationException("no balls to remove");
            if (BallsList.Count > 0) {
                BallsList.RemoveAt(BallsList.Count - 1);
            }
        }
        
    #endregion DataAbstractAPI

    #region IDisposable

    protected virtual void Dispose(bool disposing) {
      if (!Disposed) {
        if (disposing) {
          MoveTimer.Dispose();
          BallsList.Clear();
        }
        Disposed = true;
      }
      else
        throw new ObjectDisposedException(nameof(DataImplementation));
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

    private readonly Timer MoveTimer;
    private Random RandomGenerator = new();
    private List<Ball> BallsList = [];

        private void Move(object? x) {
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

                    double xPosition1 = Ball1.Position.x;
                    double yPosition1 = Ball1.Position.y;
                    Vector Position1 = new Vector(xPosition1, yPosition1);

                    double xPosition2 = Ball2.Position.x;
                    double yPosition2 = Ball2.Position.y;
                    Vector Position2 = new Vector(xPosition2, yPosition2);

                    double xCoordinate = xPosition1 - xPosition2;
                    double yCoordinate = yPosition1 - yPosition2;

                    double Distance = Math.Sqrt(xCoordinate * xCoordinate + yCoordinate * yCoordinate);

                    if (Distance < BallRadius * 2 && Distance > 0) {
                        Vector temp = (Vector)Ball1.Velocity;
                        Ball1.Velocity = Ball2.Velocity;
                        Ball2.Velocity = temp;

                        double Overlap = BallRadius * 2 - Distance;

                        double moveX = (xCoordinate / Distance) * (Overlap / 2);
                        double moveY = (yCoordinate / Distance) * (Overlap / 2);

                        Ball1.Move(new Vector(moveX, moveY));
                        Ball2.Move(new Vector(-moveX, -moveY));
                    }

                }
            }
        }


        #endregion private

        #region TestingInfrastructure

        [Conditional("DEBUG")]
    internal void CheckBallsList(Action<IEnumerable<IBall>> returnBallsList)
    {
      returnBallsList(BallsList);
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