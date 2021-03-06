﻿using AutoMapper;
using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Uwp.Services;

namespace MoneyFox.Uwp.ViewModels
{
    /// <inheritdoc cref="ISelectCategoryListViewModel" />
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel, ISelectCategoryListViewModel
    {
        private CategoryViewModel selectedCategory;

        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a CategoryViewModel selection.
        /// </summary>
        public SelectCategoryListViewModel(IMediator mediator,
                                           IMapper mapper,
                                           IDialogService dialogService,
                                           NavigationService navigationService)
            : base(mediator, mapper, dialogService, navigationService)
        { }

        /// <summary>
        ///     CategoryViewModel currently selected in the view.
        /// </summary>
        public CategoryViewModel SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if (selectedCategory == value) return;
                selectedCategory = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Post selected CategoryViewModel to message hub
        /// </summary>
        protected override void ItemClick(CategoryViewModel category)
        {
            MessengerInstance.Send(new CategorySelectedMessage(this, category.Id));
            NavigationService.GoBack();
        }
    }
}
