//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using TP.ConcurrentProgramming.BusinessLogic;

namespace TP.ConcurrentProgramming.Presentation.Model.Test
{
  [TestClass]
  public class ModelBallUnitTest
  {
    #region testing instrumentation

    private class BusinessLogicIBallFixture : BusinessLogic.IBall
    {

      public event EventHandler<IPosition>? NewPositionNotification;

      public void Dispose()
      {
        throw new NotImplementedException();
      }

            public double Mass { get => throw new NotImplementedException(); } 
            
            public double Diameter { get => throw new NotImplementedException(); }

            public int BallID { get => throw new NotImplementedException(); }
        }

        
    #endregion testing instrumentation
  }
}