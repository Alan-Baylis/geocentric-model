using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GeocentricModel
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static Dispatcher s_Dispatcher;

		//OrbiterWithChild orbiterEarth;
		Orbiter.OrbiterWithChild orbiterEarth;
		
		public MainWindow()
		{
			InitializeComponent();
			this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
			s_Dispatcher = this.Dispatcher;
		}

		void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			double dCenterCoordinate = (Canvas1.ActualHeight > Canvas1.ActualWidth ? Canvas1.ActualWidth / 2 : Canvas1.ActualHeight / 2);
			Point center = new Point(dCenterCoordinate, dCenterCoordinate);
			//orbiterEarth = new Orbiter(this, ellipseOrbit, center, dCenterCoordinate - ellipseOrbit.Width / 2 - 50, 350000);
			orbiterEarth = new Orbiter.OrbiterWithChild(Canvas1, ellipseOrbit, center, dCenterCoordinate - ellipseOrbit.Width / 2 - 50, 80000, Canvas2, ellipseOrbitOrbiter);
			orbiterEarth.StartOrbiting();
		}
	}
}
