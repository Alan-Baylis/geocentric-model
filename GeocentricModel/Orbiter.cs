using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GeocentricModel
{
	public class Orbiter
	{
		//Member variables
		public Point Center
		{
			get { return GetPosition(); }
			private set { SetPosition(value); }
		}
		public double Radius { get; private set; }
		public double OrbitRadius { get; private set; }
		public Brush OrbitColour { get; private set; }
		public Point RotationCenter { get; private set; }

		protected DispatcherTimer Timer;
		protected double ThetaDecrementer = 2.0;

		private double _theta = 360.0;
		private readonly Canvas _canvas;
		private readonly Ellipse _circle;
		private const int MAX_DOTS = 5000;


		//Constructor
		public Orbiter(Dispatcher dispatcher, Canvas canvas, Ellipse ellipse, Point rotationCenter, double orbitRadius)
		{
			Debug.Assert(ellipse.Width == ellipse.Height, "Input Ellipse is not a circle!");
			
			//Configure the orbiter
			_circle = ellipse;
			Radius = _circle.Width / 2;	
			RotationCenter = rotationCenter;
			OrbitRadius = orbitRadius;
			OrbitColour = ellipse.Stroke;

			//Configure the timer
			Timer = new DispatcherTimer(DispatcherPriority.Normal, dispatcher)
			{
				Interval = new TimeSpan(0, 0, 0, 0, 16) //Timer ticks every 16 ms
			};
			Timer.Tick += Timer_Tick;

			//Set the canvas to draw on
			_canvas = canvas;
		}

		public void StartOrbiting()
		{
			Timer.Start();
		}

		Point GetPosition()
		{
			return new Point(_circle.Margin.Left + Radius, _circle.Margin.Top + Radius);
		}

		protected virtual void SetPosition(Point point)
		{
			_circle.Margin = new Thickness(point.X - Radius, point.Y - Radius, 0, 0);
		}

		protected virtual void Timer_Tick(object sender, EventArgs e)
		{
			//Prevent Theta from getting too small
			if (_theta < 0) 
				_theta += 360;

			//Set the new position of the orbiter
			double radians = Math.PI * _theta / 180; //Calculate radians from degress
			Center = new Point(RotationCenter.X + Math.Cos(radians) * OrbitRadius, RotationCenter.Y + Math.Sin(radians) * OrbitRadius);

			_theta -= ThetaDecrementer; //Decrement the angle theta
			DrawDot(); //Draw a new dot to track the orbiter's progress
		}

		private void DrawDot()
		{
			var dot = new Ellipse
			{
				Width = 1,
				Height = 1,
				Margin = new Thickness(Center.X, Center.Y, 0, 0),
				Fill = OrbitColour
			};

			//Only add the dot if there is currently less than the maximum number of dots already drawn
			if (_canvas.Children.Count < MAX_DOTS)
				_canvas.Children.Add(dot);
		}


		#region OrbiterWithChild
		/// <summary>
		/// A class for an Orbiter with another Orbiter orbiting around it
		/// </summary>
		public class OrbiterWithChild : Orbiter
		{
			private readonly Orbiter _child;
			private readonly Point _centerReference = new Point();

			/// <summary>
			/// Constructor for an OrbiterWithOrbiter
			/// </summary>
			/// <param name="dispatcher">The Dispatcher that will dispatch timer events</param>
			/// <param name="canvas">The Canvas to draw on</param>
			/// <param name="parent">The ellipse representing the parent Orbiter</param>
			/// <param name="rotationCenter">The Point that the parent Orbiter rotates around</param>
			/// <param name="orbitRadius">The radius of the parent Orbiter</param>
			/// <param name="child">The ellipse represeting the child Orbiter</param>
			/// <param name="speed">Indicates how many times faster the child orbits the parent than the parent orbits its center</param>
			public OrbiterWithChild(Dispatcher dispatcher, Canvas canvas, Ellipse parent, Point rotationCenter, double orbitRadius, Ellipse child, int speed)
				: base(dispatcher, canvas, parent, rotationCenter, orbitRadius)
			{
				_child = new Orbiter(dispatcher, canvas, child, _centerReference, Radius);
				ThetaDecrementer = ThetaDecrementer/speed;
			}

			protected override void SetPosition(Point point)
			{
				base.SetPosition(point);
				//Update this (parent) Orbiter's center reference for the child to orbit around
				_centerReference.X = Center.X;
				_centerReference.Y = Center.Y;
			}

			protected override void Timer_Tick(object sender, EventArgs e)
			{
				base.Timer_Tick(sender, e); //Make this (parent) Orbiter orbit
				_child.Timer_Tick(sender, e); //Make the child orbit
			}

		}
		#endregion //OrbiterWithChild

	} //End of Orbiter
}
