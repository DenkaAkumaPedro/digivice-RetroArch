using Backend.Diagnostics;
using Backend.Events.Factory;
using Backend.Events.Services;
using Backend.Events.States;
using Backend.Infrastructure.Emulator;
using Backend.Memory;

namespace Backend.Application
{
    public class GameLoopService
    (
        IEmulatorConnector emulatorConnector,
        StateComposer stateComposer,
        IEventDispatcherService eventDispatcherService,
        IGameStateStore gameStateStore,
        DebugConsoleRenderer debugConsoleRenderer,
        IConfiguration configuration
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Serilog.Log.Information("Starting GameLoopService...");

            var pollingIntervalMs = configuration.GetValue<int?>("GameLoop:PollingIntervalMs") ?? 1000;
            var isDebuggingEnabled = configuration.GetValue<bool>("Features:Debugging");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var connectionAttemptResult = emulatorConnector.EnsureConnection();
                    if (!connectionAttemptResult.IsSuccess)
                    {
                        eventDispatcherService.DispatchEvents(
                            ConnectionEventFactory.CreateError(
                                gameStateStore,
                                connectionAttemptResult.ErrorCode!,
                                connectionAttemptResult.ErrorDetail));

                        if (isDebuggingEnabled && !Console.IsOutputRedirected)
                        {
                            debugConsoleRenderer.Render(null);
                        }

                        await Task.Delay(pollingIntervalMs, stoppingToken);
                        continue;
                    }

                    eventDispatcherService.DispatchEvents(
                    ConnectionEventFactory.CreateSuccess(gameStateStore));

                    try
                    {
                        var newState = stateComposer.Compose();
                        var previousState = gameStateStore.CurrentState;

                        var events = StateEventFactory.Create(previousState, newState);
                        eventDispatcherService.DispatchEvents(events);

                        gameStateStore.UpdateState(newState);

                        Serilog.Log.Information("State read OK - Player: {MapId}, Bits: {Bits}",
                            newState.Player?.MapId ?? "?", newState.Player?.Bits ?? 0);

                        if (isDebuggingEnabled && !Console.IsOutputRedirected)
                        {
                            debugConsoleRenderer.Render(newState);
                        }
                    }
                    catch (MemoryReadException ex)
                    {
                        emulatorConnector.ClearSession();
                        eventDispatcherService.DispatchEvents(
                            ConnectionEventFactory.CreateError(
                                gameStateStore,
                                EmulatorConnectionErrorCodes.MemoryReadFailed,
                                ex.Message));

                        if (isDebuggingEnabled && !Console.IsOutputRedirected)
                        {
                            debugConsoleRenderer.Render(null);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        Serilog.Log.Error(ex, "Error processing game state in GameLoopService.");
                        emulatorConnector.ClearSession();
                        eventDispatcherService.DispatchEvents(
                            ConnectionEventFactory.CreateError(
                                gameStateStore,
                                EmulatorConnectionErrorCodes.StateComposeFailed,
                                ex.Message));

                        if (isDebuggingEnabled && !Console.IsOutputRedirected)
                        {
                            debugConsoleRenderer.Render(null);
                        }
                    }

                    try
                    {
                        await Task.Delay(pollingIntervalMs, stoppingToken);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                }
            }
            finally
            {
                emulatorConnector.ClearSession();
                Serilog.Log.Information("GameLoopService shutting down gracefully.");
            }
        }
    }
}
