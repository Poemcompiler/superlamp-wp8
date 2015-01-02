using Superlamp.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Superlamp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        MapViewModel vm;
        public MapPage()
        {
            this.InitializeComponent();
            vm = this.DataContext as MapViewModel;
            vm.PropertyChanged += vm_PropertyChanged;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        protected void vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MyPoint")
            {
                xMap.Center = vm.MyPoint;

                Ellipse border = new Ellipse
                {
                    Fill = App.Current.Resources["AppColorFeatured"] as SolidColorBrush,
                    Height = 10,
                    Width = 10,
                    Opacity = 0.5,
                };

                Ellipse myCircle = new Ellipse
                {
                    Height = 20,
                    Width = 20,
                    Opacity = 100,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Fill = App.Current.Resources["AppColorFeatured"] as SolidColorBrush
                };

                Grid grid = new Grid { Height = 80, Width = 80 };
                grid.Children.Add(border);
                grid.Children.Add(myCircle);

                DoubleAnimation heightAnimation = new DoubleAnimation
                {
                    From = 10,
                    To = 80,
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = true
                };

                DoubleAnimation widthAnimation = new DoubleAnimation
                {
                    From = 10,
                    To = 80,
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = true
                };

                DoubleAnimation opacityAnimation = new DoubleAnimation
                {
                    From = .5,
                    To = .1,
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = true
                };

                Storyboard story = new Storyboard();
                Storyboard.SetTarget(opacityAnimation, border);
                Storyboard.SetTarget(heightAnimation, border);
                Storyboard.SetTarget(widthAnimation, border);
                //Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(Ellipse.OpacityProperty));
                //Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(Ellipse.HeightProperty));
                //Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Ellipse.WidthProperty));
                story.Children.Add(opacityAnimation);
                story.Children.Add(heightAnimation);
                story.Children.Add(widthAnimation);
                story.Begin();


                MapIcon mapIcon = new MapIcon { Location = vm.MyPoint, Title = "You" };
                xMap.MapElements.Add(mapIcon);

                //// Create a MapOverlay to contain the circle.
                //MapOverlay myLocationOverlay = new MapOverlay();
                //myLocationOverlay.Content = grid;
                //myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
                //myLocationOverlay.GeoCoordinate = new System.Device.Location.GeoCoordinate((vm as MapViewModel).Latitude, (vm as MapViewModel).Longitude);

                //// Create a MapLayer to contain the MapOverlay.
                //MapLayer myLocationLayer = new MapLayer();
                //myLocationLayer.Add(myLocationOverlay);

                //// Add the MapLayer to the Map.
                //xMap.Layers.Add(myLocationLayer);
            }
        }
    }
}
