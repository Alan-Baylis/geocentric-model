using System.Windows;

namespace GeocentricModel
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		Orbiter.OrbiterWithChild _orbiterEarth;
		
		public MainWindow()
		{
			InitializeComponent();
		}

		void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			double dCenterCoordinate = Canvas.ActualHeight > Canvas.ActualWidth ? Canvas.ActualWidth/2 : Canvas.ActualHeight/2;
			var center = new Point(dCenterCoordinate, dCenterCoordinate);
			double orbitRadius = dCenterCoordinate - ParentOrbiter.Width/2 - 50;
			_orbiterEarth = new Orbiter.OrbiterWithChild(
				Dispatcher,
				Canvas,
				ParentOrbiter,
				center,
				orbitRadius,
				ChildOrbiter,
				6);
			_orbiterEarth.StartOrbiting();
		}
	}
}
