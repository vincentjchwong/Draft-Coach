using System;
using DraftCoach.Helpers;
using System.Data;
using DraftCoach.DataMappers;
using System.Linq;
using System.Collections.Generic;
using DraftCoach.DataModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DraftCoach.Services;

namespace DraftCoach
{
    public class HostedService : IHostedService
    {
        IConfiguration _configuration;
        ILogger<HostedService> _logger;
        IDraftService _draftService;

        public HostedService(IConfiguration configuration, ILogger<HostedService> logger, IDraftService draftService)
        {
            _configuration = configuration;
            _logger = logger;
            _draftService = draftService;

            TextHelper.InitializeConsoleSettings();
            ChampionNameResolver.LoadAll(_configuration[DataLocations.Champions]);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => DraftCoach());
            
            return Task.CompletedTask;
        }

        public void DraftCoach()
        {
            _logger.LogInformation("Draft Coach Starting...");
            DisplayWelcomeMessage();

            while (true)
            {
                _draftService.StartDraftPhase();
                if (!ParseNewGameFromUserInput()) break;
                Console.WriteLine();
                TextHelper.PrintInformationLine($"--- Running another draft ---\n");
            }

            Console.WriteLine("Close the window to exit...");
        }

        #region Private methods - Composition data

        private IList<Composition> GetCompositions()
        {
            var compositionsDataTables = DataHelper.RetrieveDataTables(_configuration[DataLocations.Compositions]);
            return compositionsDataTables.ToCompositions();
        }

        #endregion

        #region Private methods - Text

        private void DisplayWelcomeMessage()
        {
            Console.WriteLine("################################");
            Console.WriteLine("#                              #");
            Console.WriteLine("#    Welcome to Draft Coach    #");
            Console.WriteLine("#                              #");
            Console.WriteLine("#  Placeholder How-To Message  #");
            Console.WriteLine("#                              #");
            Console.WriteLine("################################");
            Console.WriteLine();
        }

        private void DisplayComposition(Composition composition)
        {
            Console.WriteLine(composition.Name + ":");

            Console.WriteLine("Top    : " + string.Join(", ", composition.Top.Select(c => c.Name)));
            Console.WriteLine("Jungle : " + string.Join(", ", composition.Jungle.Select(c => c.Name)));
            Console.WriteLine("Mid    : " + string.Join(", ", composition.Mid.Select(c => c.Name)));
            Console.WriteLine("ADC    : " + string.Join(", ", composition.ADC.Select(c => c.Name)));
            Console.WriteLine("Support: " + string.Join(", ", composition.Support.Select(c => c.Name)));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region Private methods - User input

        private bool ParseNewGameFromUserInput()
        {
            Console.WriteLine("NEW GAME");
            Console.WriteLine("1) Yes");
            Console.WriteLine("2) No");
            Console.Write("Would you like to run another draft: ");

            while (true)
            {
                var userInput = Console.ReadLine().ToLower();

                if (userInput.ToLower().Contains("yes") || userInput == "1")
                { 
                    Console.WriteLine();
                    return true;
                }
                else if (userInput.ToLower().Contains("no") || userInput == "2")
                {
                    Console.WriteLine();
                    return false;
                }
                else
                {
                    TextHelper.PrintErrorLine("--- " + userInput + " was not recognised ---");
                    Console.Write("Would you like to run another draft: ");
                }
            }

        }

        #endregion
    }
}
