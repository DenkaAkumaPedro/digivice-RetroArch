using Backend.Application;
using Backend.Application.Loaders;
using Backend.Application.Loaders.Interfaces;
using Backend.Application.Providers;
using Backend.Application.Providers.Interfaces;
using Backend.Diagnostics;
using Backend.Events.Services;
using Backend.Events.States;
using Backend.Infrastructure.Duckstation;
using Backend.Infrastructure.Emulator;
using Backend.Infrastructure.Memory;
using Backend.Infrastructure.Processes;
using Backend.Infrastructure.RetroArch;
using Backend.Memory.Readers;
using Backend.Memory.Readers.Interfaces;
using Backend.Memory.Repositories;

namespace Backend.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBackendServices(this IServiceCollection services, string basePath)
        {
            var emulatorType = GetEmulatorType(services);

            services.AddSingleton<IProcessService, WindowsProcessProvider>();
            services.AddSingleton<IMemoryProvider, WindowsMemoryProvider>();
            services.AddSingleton<EmulatorSession>();
            services.AddSingleton<IEmulatorSession>(provider => provider.GetRequiredService<EmulatorSession>());
            services.AddSingleton<IMemoryReader, MemoryReader>();

            switch (emulatorType)
            {
                case EmulatorType.RetroArch:
                    services.AddSingleton<IEmulatorConnector, RetroArchConnector>();
                    services.AddSingleton<IRetroArchNetworkClient, RetroArchNetworkClient>();
                    break;
                default:
                    services.AddSingleton<IEmulatorConnector, DuckstationConnector>();
                    break;
            }

            string memoryDefinitionsDirectory = Path.Combine(basePath, "Memory", "Definitions");
            services.AddSingleton<IAddressesRepository>(new AddressesRepository(memoryDefinitionsDirectory));

            services.AddSingleton<IDigimonReader, DigimonReader>();
            services.AddSingleton<IDigimonSlotReader, DigimonSlotReader>();
            services.AddSingleton<IDigievolutionReader, DigievolutionReader>();
            services.AddSingleton<IStoredDigievolutionReader, StoredDigievolutionReader>();
            services.AddSingleton<IDigievolutionSlotReader, DigievolutionSlotReader>();
            services.AddSingleton<IPartyReader, PartyReader>();
            services.AddSingleton<IInBattleReader, InBattleReader>();
            services.AddSingleton<IEnemyReader, EnemyReader>();
            services.AddSingleton<IDigimonBattleReader, DigimonBattleReader>();
            services.AddSingleton<IRequisiteReader, RequisiteReader>();
            services.AddSingleton<IStepReader, StepReader>();
            services.AddSingleton<IPlayerReader, PlayerReader>();
            services.AddSingleton<ICardBattleReader, CardBattleReader>();
            services.AddSingleton<IImportantItemsReader, ImportantItemsReader>();
            services.AddSingleton<IAuctionsReader, AuctionsReader>();
            services.AddSingleton<IQuestReader, QuestReader>();
            services.AddSingleton<INpcsReader, NpcsReader>();

            services.AddSingleton<IPlayerLoader, PlayerLoader>();
            services.AddSingleton<IImportantItemsLoader, ImportantItemsLoader>();
            services.AddSingleton<IAuctionsLoader, AuctionsLoader>();
            services.AddSingleton<INpcsLoader, NpcsLoader>();
            services.AddSingleton<IQuestLoader, QuestLoader>();
            services.AddSingleton<IJournalLoader, JournalLoader>();
            services.AddSingleton<IPartyLoader, PartyLoader>();
            services.AddSingleton<IDigimonLoader, DigimonLoader>();
            services.AddSingleton<IDigimonBattleLoader, DigimonBattleLoader>();
            services.AddSingleton<ICardBattleLoader, CardBattleLoader>();

            services.AddSingleton<IPlayerProvider, PlayerProvider>();
            services.AddSingleton<IImportantItemsProvider, ImportantItemsProvider>();
            services.AddSingleton<IPartyProvider, PartyProvider>();
            services.AddSingleton<IDigimonBattleProvider, DigimonBattleProvider>();
            services.AddSingleton<ICardBattleProvider, CardBattleProvider>();
            services.AddSingleton<IAuctionsProvider, AuctionsProvider>();
            services.AddSingleton<INpcsProvider, NpcsProvider>();
            services.AddSingleton<IJournalProvider, JournalProvider>();

            services.AddSingleton<StateComposer>();
            services.AddSingleton<DebugConsoleRenderer>();

            services.AddSingleton<IGameStateStore, GameStateStore>();
            services.AddSingleton<IEventDispatcherService, EventDispatcherService>();

            services.AddHostedService<GameLoopService>();

            return services;
        }

        private static EmulatorType GetEmulatorType(IServiceCollection services)
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var emulatorTypeString = configuration.GetValue<string>("Emulator:Type");

            if (Enum.TryParse<EmulatorType>(emulatorTypeString, ignoreCase: true, out var emulatorType))
            {
                return emulatorType;
            }

            return EmulatorType.Duckstation;
        }
    }
}
