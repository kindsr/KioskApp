using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace iBeautyNail.Extensions.Behaviour
{
    public static class ScrollBarCommandsCanExecuteFixBehavior
    {
        #region Nested Types

        public class CommandCanExecuteMonitor<T> where T : UIElement
        {
            protected T Target { get; private set; }

            protected CommandCanExecuteMonitor(T target, RoutedCommand command)
            {
                Target = target;

                var binding = new CommandBinding(command);

                binding.CanExecute += OnCanExecute;

                target.CommandBindings.Add(binding);
            }

            protected virtual void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
            {

            }
        }

        public class PageUpCanExecuteMonitor : CommandCanExecuteMonitor<ScrollViewer>
        {
            public PageUpCanExecuteMonitor(ScrollViewer scrollViewer)
                : base(scrollViewer, ScrollBar.PageUpCommand)
            {
            }

            protected override void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                if (e.Handled)
                {
                    return;
                }

                if (Equals(Target.VerticalOffset, 0.0))
                {
                    e.CanExecute = false;
                    e.Handled = true;
                }
            }
        }

        public class PageDownCanExecuteMonitor : CommandCanExecuteMonitor<ScrollViewer>
        {
            public PageDownCanExecuteMonitor(ScrollViewer scrollViewer)
                : base(scrollViewer, ScrollBar.PageDownCommand)
            {
            }

            protected override void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                if (e.Handled)
                {
                    return;
                }

                if (Equals(Target.VerticalOffset, Target.ScrollableHeight))
                {
                    e.CanExecute = false;
                    e.Handled = true;
                }
            }
        }

        public class PageLeftCanExecuteMonitor : CommandCanExecuteMonitor<ScrollViewer>
        {
            public PageLeftCanExecuteMonitor(ScrollViewer scrollViewer)
                : base(scrollViewer, ScrollBar.PageLeftCommand)
            {
            }

            protected override void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                if (e.Handled)
                {
                    return;
                }

                if (Equals(Target.HorizontalOffset, 0.0))
                {
                    e.CanExecute = false;
                    e.Handled = true;
                }
            }
        }

        public class PageRightCanExecuteMonitor : CommandCanExecuteMonitor<ScrollViewer>
        {
            public PageRightCanExecuteMonitor(ScrollViewer scrollViewer)
                : base(scrollViewer, ScrollBar.PageRightCommand)
            {
            }

            protected override void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                if (e.Handled)
                {
                    return;
                }

                if (Equals(Target.HorizontalOffset, Target.ScrollableWidth))
                {
                    e.CanExecute = false;
                    e.Handled = true;
                }
            }
        }

        #endregion

        #region IsEnabled Attached Property

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ScrollBarCommandsCanExecuteFixBehavior), new PropertyMetadata(false, OnIsEnabledChanged));

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                var scrollViewer = d as ScrollViewer;

                if (scrollViewer != null)
                {
                    OnAttached(scrollViewer);
                }
                else
                {
                    throw new NotSupportedException("This behavior only supports ScrollViewer instances.");
                }
            }
        }

        private static void OnAttached(ScrollViewer target)
        {
            SetPageUpCanExecuteMonitor(target, new PageUpCanExecuteMonitor(target));
            SetPageDownCanExecuteMonitor(target, new PageDownCanExecuteMonitor(target));
            SetPageLeftCanExecuteMonitor(target, new PageLeftCanExecuteMonitor(target));
            SetPageRightCanExecuteMonitor(target, new PageRightCanExecuteMonitor(target));
        }

        #endregion

        #region PageUpCanExecuteMonitor Attached Property

        private static void SetPageUpCanExecuteMonitor(DependencyObject obj, PageUpCanExecuteMonitor value)
        {
            obj.SetValue(PageUpCanExecuteMonitorProperty, value);
        }

        private static readonly DependencyProperty PageUpCanExecuteMonitorProperty =
            DependencyProperty.RegisterAttached("PageUpCanExecuteMonitor", typeof(PageUpCanExecuteMonitor), typeof(ScrollBarCommandsCanExecuteFixBehavior), new PropertyMetadata(null));

        #endregion

        #region PageDownCanExecuteMonitor Attached Property

        private static void SetPageDownCanExecuteMonitor(DependencyObject obj, PageDownCanExecuteMonitor value)
        {
            obj.SetValue(PageDownCanExecuteMonitorProperty, value);
        }

        private static readonly DependencyProperty PageDownCanExecuteMonitorProperty =
            DependencyProperty.RegisterAttached("PageDownCanExecuteMonitor", typeof(PageDownCanExecuteMonitor), typeof(ScrollBarCommandsCanExecuteFixBehavior), new PropertyMetadata(null));

        #endregion

        #region PageLeftCanExecuteMonitor Attached Property

        private static void SetPageLeftCanExecuteMonitor(DependencyObject obj, PageLeftCanExecuteMonitor value)
        {
            obj.SetValue(PageLeftCanExecuteMonitorProperty, value);
        }

        private static readonly DependencyProperty PageLeftCanExecuteMonitorProperty =
            DependencyProperty.RegisterAttached("PageLeftCanExecuteMonitor", typeof(PageLeftCanExecuteMonitor), typeof(ScrollBarCommandsCanExecuteFixBehavior), new PropertyMetadata(null));

        #endregion

        #region PageDownCanExecuteMonitor Attached Property

        private static void SetPageRightCanExecuteMonitor(DependencyObject obj, PageRightCanExecuteMonitor value)
        {
            obj.SetValue(PageRightCanExecuteMonitorProperty, value);
        }

        private static readonly DependencyProperty PageRightCanExecuteMonitorProperty =
            DependencyProperty.RegisterAttached("PageRightCanExecuteMonitor", typeof(PageRightCanExecuteMonitor), typeof(ScrollBarCommandsCanExecuteFixBehavior), new PropertyMetadata(null));

        #endregion
    }
}
