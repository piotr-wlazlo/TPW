﻿//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.BusinessLogic {
  internal class Ball : IBall {
    public Ball(Data.IBall ball) {
      ball.NewPositionNotification += RaisePositionChangeEvent;
      Mass = ball.Mass;
      Diameter = ball.Diameter;
      BallID = ball.BallID;
    }

    #region IBall

    public event EventHandler<IPosition>? NewPositionNotification;

    public double Mass { get; }
    public double Diameter { get; }
    public int BallID { get; }

    #endregion IBall

    #region private

    private void RaisePositionChangeEvent(object? sender, Data.IVector e) {
      NewPositionNotification?.Invoke(this, new Position(e.x, e.y));
    }

    #endregion private
  }
}