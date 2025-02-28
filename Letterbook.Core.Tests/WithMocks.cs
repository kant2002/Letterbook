﻿using Letterbook.Core.Adapters;
using Microsoft.Extensions.Options;
using Moq;

namespace Letterbook.Core.Tests;

public abstract class WithMocks
{
    protected Mock<IActivityAdapter> ActivityAdapterMock;
    protected Mock<IAccountProfileAdapter> AccountProfileMock;
    protected Mock<IShareAdapter> ShareAdapter;
    protected Mock<IMessageBusAdapter> MessageBusAdapterMock;
    protected Mock<IAccountEventService> AccountEventServiceMock;
    protected IOptions<CoreOptions> CoreOptionsMock;

    protected WithMocks()
    {
        ActivityAdapterMock = new Mock<IActivityAdapter>();
        AccountProfileMock = new Mock<IAccountProfileAdapter>();
        ShareAdapter = new Mock<IShareAdapter>();
        MessageBusAdapterMock = new Mock<IMessageBusAdapter>();
        AccountEventServiceMock = new Mock<IAccountEventService>();
        var mockOptions = new CoreOptions
        {
            DomainName = "letterbook.example",
            Port = "80",
            Scheme = "http"
        };
        CoreOptionsMock = Options.Create(mockOptions);
    }
}