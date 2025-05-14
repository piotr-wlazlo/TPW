//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data.Test
{
  [TestClass]
  public class BallUnitTest
  {
    [TestMethod]
    public void ConstructorTestMethod()
    {
            double massValue = 1.0;
            double anotherValue = 0.0;

            Vector testinVector = new Vector(0.0, 0.0);
            Ball newInstance = new(testinVector, testinVector, massValue, anotherValue);
        }

    [TestMethod]
    public void MoveTestMethod()
    {
            double massValue = 1.0;
            double anotherValue = 0.0;
            
            Vector initialPosition = new(10.0, 10.0);
            Ball newInstance = new(initialPosition, new Vector(0.0, 0.0), massValue, anotherValue); IVector curentPosition = new Vector(0.0, 0.0);
      int numberOfCallBackCalled = 0;
      newInstance.NewPositionNotification += (sender, position) => { Assert.IsNotNull(sender); curentPosition = position; numberOfCallBackCalled++; };
      newInstance.Move(new Vector(0.0, 0.0));
      Assert.AreEqual<int>(1, numberOfCallBackCalled);
      Assert.AreEqual<IVector>(initialPosition, curentPosition);
    }
  }
}