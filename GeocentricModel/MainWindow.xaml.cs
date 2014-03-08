using System;
using System.Windows;
using System.Windows.Data;

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
			double orbitRadius = dCenterCoordinate - ParentOrbiter.Width/2 - Height*(5/83.0);
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

	public class PercentageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
