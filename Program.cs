namespace ProductionSystem
{
	public class Program
	{
		static void Main(string[] args)
		{
			
		}
	}

	public abstract class ProductionSystemFactory
	{
		public abstract Robot CreateRobot();
		public abstract Conveyor CreateConveyor();
		public abstract Sensor CreateSensor();
	}

	public class HighSpeedProductionSystemFactory : ProductionSystemFactory
	{
		public override Robot CreateRobot() { }
		public override Conveyor CreateConveyor() { }
		public override Sensor CreateSensor() { }
	}

	public class LowSpeedProductionSystemFactory : ProductionSystemFactory
	{
		public override Robot CreateRobot() { }
		public override Conveyor CreateConveyor() { }
		public override Sensor CreateSensor() { }
	}

	public class ProductionSystem
	{
		private Robot robot;
		private Conveyor conveyor;
		private Sensor sensor;
		private Bridge bridge;

		public ProductionSystem()
		{
			var productionSystemFactory = new HighSpeedProductionSystemFactory();
			robot = productionSystemFactory.CreateRobot();
			conveyor = productionSystemFactory.CreateConveyor();
			sensor = productionSystemFactory.CreateSensor();
			bridge = new Bridge(robot, conveyor);
		}

		public void StartProduction()
		{
			bridge.Connect();
		}

		public void StopProduction()
		{
			bridge.Disconnect();
		}
	}

	public interface IObserver
	{
		void Update(ISubject subject);
	}

	public interface ISubject
	{
		void Attach(IObserver observer);
		void Detach(IObserver observer);
		void Notify();
	}

	public class Bridge : ISubject
	{
		private bool isWorking;
		private Robot robot;
		private Conveyor conveyor;

		public Bridge(Robot robot, Conveyor conveyor)
		{
			this.robot = robot;
			this.conveyor = conveyor;
		}

		public bool IsWorking
		{
			get { return isWorking; }
			set
			{
				isWorking = value;
				Notify();
			}
		}

		private List<IObserver> observers = new List<IObserver>();

		public void Attach(IObserver observer) => observers.Add(observer);

		public void Detach(IObserver observer) => observers.Remove(observer);

		public void Notify()
		{
			foreach (var observer in observers)
			{
				observer.Update(this);
			}
		}

		public void Connect()
		{
			conveyor.Attach(robot);
			IsWorking = true;
		}

		public void Disconnect()
		{
			conveyor.Detach(robot);
			IsWorking = false;
		}

		// Операции для управления роботом. 
		public void MoveForward()
		{
		}

		public void MoveBackward()
		{
		}

		public void Fail()
		{
			IsWorking = false;
			Notify();
		}
	}

	public class Conveyor : ISubject
	{
		private bool _isWorking;

		public bool IsWorking
		{
			get { return _isWorking; }
			set
			{
				_isWorking = value;
				Notify();
			}
		}

		private List<IObserver> observers = new List<IObserver>();

		public void Attach(IObserver observer) => observers.Add(observer);

		public void Detach(IObserver observer) => observers.Remove(observer);

		public void Notify()
		{
			foreach (var observer in observers)
			{
				observer.Update(this);
			}
		}

		public void Fail()
		{
			IsWorking = false;
			Notify();
		}
	}

	public class Sensor : ISubject
	{
		private List<IObserver> observers = new List<IObserver>();
		private bool isWorking;

		public bool IsWorking
		{
			get { return isWorking; }
			set
			{
				isWorking = value;
				Notify();
			}
		}

		public void Attach(IObserver observer) => observers.Add(observer);

		public void Detach(IObserver observer) => observers.Remove(observer);

		public void Notify()
		{
			foreach (var observer in observers)
			{
				observer.Update(this);
			}
		}
	}

	public class Robot : IObserver
	{
		private bool isWorking;

		public void Update(ISubject subject)
		{
			if (subject is Sensor)
			{
				var sensor = (Sensor)subject;
				isWorking = sensor.IsWorking;
			}
		}
	}
}