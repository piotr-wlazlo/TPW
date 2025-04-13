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

    internal Ball(Vector initialPosition, Vector initialVelocity) {
      position = initialPosition;
      Velocity = initialVelocity;
    }

    #endregion ctor

    #region IBall

    public event EventHandler<IVector>? NewPositionNotification;

    public IVector Velocity { get; set; }
    public IVector Position { get => position; }

    #endregion IBall

    #region private

    private Vector position;

    private void RaiseNewPositionChangeNotification() {
      NewPositionNotification?.Invoke(this, position);
    }

    internal void Move(Vector delta) {
      position = new Vector(position.x + delta.x, position.y + delta.y);
      RaiseNewPositionChangeNotification();
    }

    #endregion private
  }
}