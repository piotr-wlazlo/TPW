//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.BusinessLogic.Test {
    [TestClass]
    public class BallUnitTest {
        #region testing instrumentation

        private class DataBallFixture : Data.IBall {
            public Data.IVector Velocity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public Data.IVector Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public Data.IBall Ball { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public double Mass { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public double Diameter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public int BallID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public event EventHandler<Data.IVector>? NewPositionNotification;
        }

        private class VectorFixture : Data.IVector {
            internal VectorFixture(double newX, double newY) {
                x = newX; y = newY;
            }

            public double x { get; init; }
            public double y { get; init; }
        }

        #endregion testing instrumentation
    }
}