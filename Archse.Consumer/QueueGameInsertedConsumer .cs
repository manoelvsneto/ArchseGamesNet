using System.Diagnostics;
using Archse.Application;
using Archse.Events;
using Archse.Models;
using AutoMapper;
using MassTransit;
using MassTransit.Metadata;
using Microsoft.Extensions.Logging;

namespace Archse.Consumer;

public class QueueGameInsertedConsumer : IConsumer<GameInsertedEvent>
{
    private readonly IGamesApplication _gamesApplication;
    private readonly IMapper _mapper;

    public QueueGameInsertedConsumer(IGamesApplication gamesApplication, IMapper mapper)
    {
        _gamesApplication = gamesApplication;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<GameInsertedEvent> context)
    {
        var timer = Stopwatch.StartNew();

        try
        {
            GameRequest gameData = _mapper.Map<GameRequest>(context.Message);
            _gamesApplication.Create(gameData, context.Message.Identificador);

            context.NotifyConsumed(timer.Elapsed, TypeMetadataCache<GameInsertedEvent>.ShortName);
            System.Console.WriteLine("Sucess QueueGameInsertedConsumer: " + context.Message.Identificador);
        }
        catch (System.Exception ex)
        {
            context.NotifyFaulted(timer.Elapsed, TypeMetadataCache<GameInsertedEvent>.ShortName, ex);
            System.Console.WriteLine("Exception QueueGameInsertedConsumer: " + context.Message.Identificador + " " + ex.Message);
        }
    }
}

public class QueueGameConsumerDefinition : ConsumerDefinition<QueueGameInsertedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueGameInsertedConsumer> consumerConfigurator)
    {
        consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
    }
}