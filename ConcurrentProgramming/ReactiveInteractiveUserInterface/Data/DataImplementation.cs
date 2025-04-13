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
      MoveTimer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(16));
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
        Vector startingPosition = new(random.Next(10, 390), random.Next(10, 390));
        Vector startingVelocity = new(random.NextDouble() * 3 - 1, random.NextDouble() * 3 - 1);
        
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
        foreach (Ball item in BallsList) {
            double xVelocity = item.Velocity.x;
            double yVelocity = item.Velocity.y;

            double xPosition = item.Position.x;
            double yPosition = item.Position.y;

            double newX = xVelocity + xPosition;
            double newY = yVelocity + yPosition;

            if (newX <= 10) {
                xVelocity = Math.Abs(xVelocity);
            }
            else if (newX >= 380) {
                xVelocity = -Math.Abs(xVelocity);
            }

            if (newY <= 0) {
                yVelocity = Math.Abs(yVelocity);
            }
            else if (newY >= 390) {
                yVelocity = -Math.Abs(yVelocity);
            }

            item.Velocity = new Vector(xVelocity, yVelocity);
            item.Move(new Vector(newX - xPosition, newY - yPosition));
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