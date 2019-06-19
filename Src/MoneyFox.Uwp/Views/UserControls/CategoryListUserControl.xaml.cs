﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.ServiceLayer.ViewModels;

namespace MoneyFox.Uwp.Views.UserControls
{
    public partial class CategoryListUserControl
    {
        public CategoryListUserControl()
        {
            InitializeComponent();
        }

        private void CategoryListRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

            flyoutBase?.ShowAt(senderElement, e.GetPosition(senderElement));
        }

        private void EditCategory(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var category = element.DataContext as CategoryViewModel;
            if (category == null)
            {
                return;
            }

            ((AbstractCategoryListViewModel) DataContext).EditCategoryCommand.Execute(category);
        }

        private void DeleteCategory(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            var category = element.DataContext as CategoryViewModel;
            if (category == null)
            {
                return;
            }

            ((AbstractCategoryListViewModel) DataContext).DeleteCategoryCommand.Execute(category);
        }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ((AbstractCategoryListViewModel) DataContext).SearchCommand.Execute(SearchTextBox.Text);
        }
    }
}