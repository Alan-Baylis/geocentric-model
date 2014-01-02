using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Timers;

namespace GeocentricModel
{
	public class Orbiter
	{
		//Member variables
		public Point Center { get { return getPosition(); } private set { setPosition(value); } }
		public double Radius { get; private set; }
		public double OrbitRadius { get; private set; }
		public Brush OrbitColour { get; private set; }
		public Point RotationCenter { get; private set; }

		protected DispatcherTimer Timer;
		protected double THETA_DECREMENTER;
		
		Ellipse Circle;
		double Theta = 360;
		Canvas Canvas;


		//Constructor
		public Orbiter(Canvas canvas, Ellipse ellipse, Point rotationCenter, double orbitRadius, long ticks)
		{
			Debug.Assert(ellipse.Width == ellipse.Height, "Input Ellipse is not a circle");
			if (ellipse.Width != ellipse.Height) return;

			Circle = ellipse;
			Radius = (int) Circle.Width / 2;	

			RotationCenter = rotationCenter;
			OrbitRadius = orbitRadius;

			OrbitColour = ellipse.Stroke;

			//Configure the timer
			Timer = new DispatcherTimer(DispatcherPriority.Normal, MainWindow.s_Dispatcher);
			Timer.Interval = new TimeSpan(ticks);
			Timer.Tick += Timer_Tick;

			THETA_DECREMENTER = 1;

			Canvas = canvas;
		}

		//===================================================
		// Member functions
		//===================================================

		public void StartOrbiting()
		{
			Timer.Start();
		}

		Point getPosition()
		{
			return new Point(Circle.Margin.Left + Radius, Circle.Margin.Top + Radius);
		}

		protected virtual void setPosition(Point point)
		{
			Circle.Margin = new Thickness(point.X - Radius, point.Y - Radius, 0, 0);
		}

		protected virtual void Timer_Tick(object sender, EventArgs e)
		{
			if (Theta == 0) 
				Theta = 360; //To prevent Theta from getting too small

			double Radians = Math.PI * Theta / 180; //Calculate radians from degress
			//Set the new position of the orbiter
			Center = new Point(RotationCenter.X + Math.Cos(Radians) * OrbitRadius, RotationCenter.Y + Math.Sin(Radians) * OrbitRadius);
			Theta -= THETA_DECREMENTER; //Decrement the angle theta

			DrawPoint();
		}

		void DrawPoint()
		{
			Ellipse dot = new Ellipse();
			dot.Width = 1;
			dot.Height = 1;
			dot.Margin = new Thickness(Center.X, Center.Y, 0, 0);
			dot.Fill = OrbitColour;

			if (Canvas.Children.Count < 5000)
			{
				//Only add the dot if there are currently less than 5000 dots already drawn
				Canvas.Children.Add(dot);
			}
		}


		#region OrbiterWithChild
		public class OrbiterWithChild : Orbiter
		{
			Orbiter Child;
			int TickCounter = 2;
			Point CenterReference = new Point();

			public OrbiterWithChild(Canvas canvas, Ellipse ellipse, Point rotationCenter, double orbitRadius, long ticks, Canvas childCanvas, Ellipse child)
				: base(canvas, ellipse, rotationCenter, orbitRadius, ticks)
			{
				Child = new Orbiter(childCanvas, child, CenterReference, this.Radius, ticks / 3);
				THETA_DECREMENTER = 0.5;
			}

			protected override void setPosition(Point point)
			{
				base.setPosition(point);
				CenterReference.X = this.Center.X;
				CenterReference.Y = this.Center.Y;
			}

			protected override void Timer_Tick(object sender, EventArgs e)
			{
				Timer.Stop();
				TickCounter++;
				//Only orbit this orbiter every 2 ticks
				if (TickCounter > 2)
				{
					base.Timer_Tick(sender, e);
					TickCounter = 0;
				}
				Child.Timer_Tick(sender, e); //Make the child orbit
				Timer.Start();
			}

		}
		#endregion //OrbiterWithChild

	} //End of Orbiter
}
