﻿using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.ViewModels;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class AddAccountViewModelTests
    {
        [Fact]
        public void Ctor_CategoryCreated()
        {
            // Arrange
            // Act
            var mediator = new Mock<IMediator>();

            var addAccountVm = new AddAccountViewModel(mediator.Object, null, null, null, null, null);


            // Assert
            addAccountVm.SelectedAccount.ShouldNotBeNull();
        }

        [Fact]
        public void Ctor_Title_Set()
        {
            // Arrange
            // Act
            var mediator = new Mock<IMediator>();

            var addAccountVm = new AddAccountViewModel(mediator.Object, null, null, null, null, null);

            // Assert
            addAccountVm.Title.ShouldEqual(Strings.AddAccountTitle);
        }

        [Fact]
        public async Task AmountStringSetOnInit()
        {
            // Arrange            
            var mediator = new Mock<IMediator>();
            var addAccountVm = new AddAccountViewModel(mediator.Object, null, null, null, null, null);

            // Act
            await addAccountVm.InitializeCommand.ExecuteAsync();

            // Assert
            addAccountVm.AmountString.ShouldEqual("0.00");
        }

        [Theory]
        [InlineData("de-CH", "12.20", 12.20)]
        [InlineData("de-DE", "12,20", 12.20)]
        [InlineData("en-US", "12.20", 12.20)]
        [InlineData("ru-RU", "12,20", 12.20)]
        [InlineData("de-CH", "-12.20", -12.20)]
        [InlineData("de-DE", "-12,20", -12.20)]
        [InlineData("en-US", "-12.20", -12.20)]
        [InlineData("ru-RU", "-12,20", -12.20)]
        public async Task AmountCorrectlyFormattedOnSave(string cultureString, string amountString, decimal expectedAmount)
        {
            // Arrange
            var cultureInfo = new CultureInfo(cultureString);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var mediatorMock = new Mock<IMediator>();
            var mapperMock = new Mock<IMapper>();
            var settingsFacadeMock = new Mock<ISettingsFacade>();
            var navigationServiceMock = new Mock<INavigationService>();

            var addAccountVm = new AddAccountViewModel(mediatorMock.Object, mapperMock.Object, settingsFacadeMock.Object, null, null,
                                                       navigationServiceMock.Object);
            addAccountVm.SelectedAccount.Name = "Foo";

            // Act
            addAccountVm.AmountString = amountString;
            await addAccountVm.SaveCommand.ExecuteAsync();

            // Assert
            addAccountVm.SelectedAccount.CurrentBalance.ShouldEqual(expectedAmount);
        }
    }
}
