using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Xamarin.Forms.Platform.UWP
{
	internal class TitleViewManager
	{
		readonly ITitleViewRendererController _titleViewRendererController;

		View TitleView => _titleViewRendererController.TitleView;
		CommandBar CommandBar => _titleViewRendererController.CommandBar;
		FrameworkElement TitleViewPresenter => _titleViewRendererController.TitleViewPresenter;

		public TitleViewManager(ITitleViewRendererController titleViewRendererController)
		{
			_titleViewRendererController = titleViewRendererController;

			if (TitleViewPresenter != null)
			{
				TitleViewPresenter.Loaded += OnTitleViewPresenterLoaded;
			}

			if (CommandBar != null)
			{
				CommandBar.LayoutUpdated += commandLayoutUpdated;
				CommandBar.Unloaded += commandBarUnloaded;
			}
		}

		internal void OnTitleViewPropertyChanged()
		{
			UpdateTitleViewWidth();
		}

		void OnTitleViewPresenterLoaded(object sender, RoutedEventArgs e)
		{
			UpdateTitleViewWidth();
			if (TitleViewPresenter != null)
			{
				TitleViewPresenter.Loaded -= OnTitleViewPresenterLoaded;
			}
		}

		void commandBarUnloaded(object sender, RoutedEventArgs e)
		{
			if (CommandBar != null)
			{
				CommandBar.LayoutUpdated -= commandLayoutUpdated;
				CommandBar.Unloaded -= commandBarUnloaded;
				CommandBar.Loaded += CommandBar_Loaded;
			}
		}

		private void CommandBar_Loaded(object sender, RoutedEventArgs e)
		{
			if (CommandBar != null)
			{
				CommandBar.LayoutUpdated += commandLayoutUpdated;
				CommandBar.Unloaded += commandBarUnloaded;
				CommandBar.Loaded -= CommandBar_Loaded;
			}
		}

		void commandLayoutUpdated(object sender, object e)
		{
			UpdateTitleViewWidth();
		}

		void UpdateTitleViewWidth()
		{
			if (TitleView == null || TitleViewPresenter == null || CommandBar == null)
				return;

			if (CommandBar.ActualWidth <= 0)
				return;

			double buttonWidth = 0;
			foreach (var item in CommandBar.GetDescendantsByName<Windows.UI.Xaml.Controls.Button>("MoreButton"))
				if (item.Visibility == Visibility.Visible)
					buttonWidth += item.ActualWidth;

			if (!CommandBar.IsDynamicOverflowEnabled)
				foreach (var item in CommandBar.GetDescendantsByName<ItemsControl>("PrimaryItemsControl"))
					buttonWidth += item.ActualWidth;

			TitleViewPresenter.Width = CommandBar.ActualWidth - buttonWidth;
			UpdateVisibility();
		}

		void UpdateVisibility()
		{
			if (TitleView == null)
				_titleViewRendererController.TitleViewVisibility = Visibility.Collapsed;
			else
				_titleViewRendererController.TitleViewVisibility = Visibility.Visible;
		}
	}
}
