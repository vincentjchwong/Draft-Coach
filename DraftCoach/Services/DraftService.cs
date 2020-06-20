using DraftCoach.DataModels;
using DraftCoach.Helpers;
using System;
using System.Linq;

namespace DraftCoach.Services
{
    public class DraftService : IDraftService
    {
        Team BlueTeam;
        Team RedTeam;
        private string UsersGameMode;
        private string UsersTeamColour;

        public void StartDraftPhase()
        {
            BlueTeam = new Team(TeamColour.Blue);
            RedTeam = new Team(TeamColour.Red);
            UsersGameMode = string.Empty;
            UsersTeamColour = string.Empty;
            while (string.IsNullOrEmpty(UsersGameMode)) UsersGameMode = ParseGameModeFromUserInput();
            TextHelper.PrintInformationLine($"--- Running a {UsersGameMode} Draft ---\n");

            UsersTeamColour = ParseTeamColourFromUserInput();
            TextHelper.PrintInformationLine($"--- Selected {UsersTeamColour} Team ---\n");

            switch (UsersGameMode)
            {
                case GameMode.Ranked:
                    RunRankedDraft();
                    break;
                case GameMode.Clash:
                    RunClashDraft();
                    break;
                default:
                    throw new Exception("Could not resolve the selected game mode.");
            }

            TextHelper.PrintCyan("--- Good luck, have fun ---\n");
            Console.WriteLine();
        }

        #region Private methods - Draft logic

        private void RunRankedDraft()
        {
            // TODO: Needs refactoring; code is ugly and verbose

            // Ban Phase
            if (UsersTeamColour == TeamColour.Blue)
            {
                for (int i = 1; i < 6; i++)
                {
                    Console.Write($"Enter ban {i} for the ");
                    TextHelper.PrintBlue("Blue Team");
                    Console.Write(": ");
                    AddBan(BlueTeam);
                }

                for (int i = 1; i < 6; i++)
                {
                    Console.Write($"Enter ban {i} for the ");
                    TextHelper.PrintRed("Red Team");
                    Console.Write(": ");
                    AddBan(RedTeam);
                }
            }
            else
            {
                for (int i = 1; i < 6; i++)
                {
                    Console.Write($"Enter ban {i} for the Red Team: ");
                    AddBan(RedTeam);
                }

                for (int i = 1; i < 6; i++)
                {
                    Console.Write($"Enter ban {i} for the Blue Team: ");
                    AddBan(BlueTeam);
                }
            }

            // Pick Phase
            Console.Write($"Enter pick 1 for the Blue Team: ");
            AddPick(BlueTeam);

            Console.Write($"Enter pick 1 for the Red Team: ");
            AddPick(RedTeam);
            Console.Write($"Enter pick 2 for the Red Team: ");
            AddPick(RedTeam);

            Console.Write($"Enter pick 2 for the Blue Team: ");
            AddPick(BlueTeam);
            Console.Write($"Enter pick 3 for the Blue Team: ");
            AddPick(BlueTeam);

            Console.Write($"Enter pick 3 for the Red Team: ");
            AddPick(RedTeam);
            Console.Write($"Enter pick 4 for the Red Team: ");
            AddPick(RedTeam);

            Console.Write($"Enter pick 4 for the Blue Team: ");
            AddPick(BlueTeam);
            Console.Write($"Enter pick 5 for the Blue Team: ");
            AddPick(BlueTeam);

            Console.Write($"Enter pick 5 for the Red Team: ");
            AddPick(RedTeam);
        }

        private void RunClashDraft()
        {
            // TODO: Needs refactoring; code is ugly and verbose

            // Ban Phase 1
            Console.Write($"Enter ban 1 for the Blue Team: ");
            AddBan(BlueTeam);

            Console.Write($"Enter ban 1 for the Red Team: ");
            AddBan(RedTeam);

            Console.Write($"Enter ban 2 for the Blue Team: ");
            AddBan(BlueTeam);

            Console.Write($"Enter ban 2 for the Red Team: ");
            AddBan(RedTeam);

            Console.Write($"Enter ban 3 for the Blue Team: ");
            AddBan(BlueTeam);

            Console.Write($"Enter ban 3 for the Red Team: ");
            AddBan(RedTeam);

            // Pick Phase 1
            Console.Write($"Enter pick 1 for the Blue Team: ");
            AddPick(BlueTeam);

            Console.Write($"Enter pick 1 for the Red Team: ");
            AddPick(RedTeam);
            Console.Write($"Enter pick 2 for the Red Team: ");
            AddPick(RedTeam);

            Console.Write($"Enter pick 2 for the Blue Team: ");
            AddPick(BlueTeam);
            Console.Write($"Enter pick 3 for the Blue Team: ");
            AddPick(BlueTeam);

            Console.Write($"Enter pick 3 for the Red Team: ");
            AddPick(RedTeam);

            // Ban Phase 2
            Console.Write($"Enter ban 4 for the Red Team: ");
            AddBan(RedTeam);

            Console.Write($"Enter ban 4 for the Blue Team: ");
            AddBan(BlueTeam);

            Console.Write($"Enter ban 5 for the Red Team: ");
            AddBan(RedTeam);

            Console.Write($"Enter ban 5 for the Blue Team: ");
            AddBan(BlueTeam);

            // Pick Phase 2
            Console.Write($"Enter pick 4 for the Red Team: ");
            AddPick(RedTeam);

            Console.Write($"Enter pick 4 for the Blue Team: ");
            AddPick(BlueTeam);
            Console.Write($"Enter pick 5 for the Blue Team: ");
            AddPick(BlueTeam);

            Console.Write($"Enter pick 5 for the Red Team: ");
            AddPick(RedTeam);
        }

        #endregion

        #region Private methods - Pick/Ban

        private void AddBan(Team team)
        {
            Champion championToBan = null;
            while (championToBan == null)
            {
                championToBan = ChampionNameResolver.ParseChampionFromUserInput();

                if (championToBan == null || ChampionHasBeenPickedOrBanned(championToBan))
                {
                    championToBan = null;
                    Console.Write("Enter a champion: ");
                }
            }
            Console.WriteLine();

            team.BannedChampions.Add(championToBan);
            DisplayPicksAndBans();
        }

        private void AddPick(Team team)
        {
            Champion championToPick = null;
            while (championToPick == null)
            {
                championToPick = ChampionNameResolver.ParseChampionFromUserInput();

                if (championToPick == null || ChampionHasBeenPickedOrBanned(championToPick))
                {
                    championToPick = null;
                    Console.Write("Enter a champion: ");
                }
            }
            Console.WriteLine();

            team.PickedChampions.Add(championToPick);
            DisplayPicksAndBans();
        }

        private bool ChampionHasBeenPickedOrBanned(Champion champion)
        {
            if (BlueTeam.PickedChampions.Contains(champion) || RedTeam.PickedChampions.Contains(champion))
            {
                TextHelper.PrintErrorLine($"{champion.Name} has already been picked.");
                return true;
            }

            if (BlueTeam.BannedChampions.Contains(champion) || RedTeam.BannedChampions.Contains(champion))
            {
                TextHelper.PrintErrorLine($"{champion.Name} has already been banned.");
                return true;
            }

            return false;
        }

        #endregion

        #region Private methods - User input

        private string ParseGameModeFromUserInput()
        {
            Console.WriteLine("GAME MODE");
            Console.WriteLine("1) Ranked");
            Console.WriteLine("2) Clash");
            Console.Write("Which game mode are you playing: ");

            var gameMode = string.Empty;

            while (string.IsNullOrEmpty(gameMode))
            {
                var userInput = Console.ReadLine().ToLower();

                if (userInput.ToLower() == GameMode.Ranked.ToLower() || userInput == "1")
                {
                    gameMode = GameMode.Ranked;
                }
                else if (userInput.ToLower() == GameMode.Clash.ToLower() || userInput == "2")
                {
                    gameMode = GameMode.Clash;
                }
                else
                {
                    TextHelper.PrintErrorLine("--- " + userInput + " is not a game mode ---");
                    Console.Write("Please enter a game mode or number: ");
                }
            }

            Console.WriteLine();

            return gameMode;
        }

        private string ParseTeamColourFromUserInput()
        {
            Console.WriteLine("TEAM COLOUR");
            Console.WriteLine("1) Blue team");
            Console.WriteLine("2) Red team");
            Console.Write("Which team are you: ");
            
            var teamColour = string.Empty;

            while (string.IsNullOrEmpty(teamColour))
            {
                var userInput = Console.ReadLine().ToLower();

                if (userInput.ToLower().Contains(TeamColour.Blue.ToLower()) || userInput == "1")
                {
                    teamColour = TeamColour.Blue;
                }
                else if (userInput.ToLower().Contains(TeamColour.Red.ToLower()) || userInput == "2")
                {
                    teamColour = TeamColour.Red;
                }
                else
                {
                    TextHelper.PrintErrorLine("--- " + userInput + " is not a team colour ---");
                    Console.Write("Please enter a team colour or number: ");
                }
            }

            Console.WriteLine();

            return teamColour;
        }

        #endregion

        #region Private methods - Text

        private void DisplayPicksAndBans()
        {
            TextHelper.PrintHorizontalBorderLine();
            Console.WriteLine();

            // TODO: This should display the team picks and bans based on the player's team
            // i.e. if the player is Red Team then their picks and bans should show up on the left
            TextHelper.PrintBlue(TextHelper.PadTextRightHalf("Blue Team"));
            TextHelper.PrintRed(TextHelper.PadTextLeftHalf("Red Team"));
            Console.WriteLine();

            Console.Write(TextHelper.PadTextRightHalf("Bans: " + string.Join(", ", BlueTeam.BannedChampions.Select(c => c.Name))));
            Console.WriteLine(TextHelper.PadTextLeftHalf(string.Join(", ", RedTeam.BannedChampions.Select(c => c.Name))));
            Console.WriteLine();

            TextHelper.PrintCyan("First Pick\n");
            for (int i = 0; i < 5; i++)
            {
                if (i < BlueTeam.PickedChampions.Count)
                {
                    Console.Write(TextHelper.PadTextRightHalf(BlueTeam.PickedChampions[i].Name));
                }
                else
                {
                    Console.Write(TextHelper.PadTextRightHalf($"Summoner {i+1}"));
                }

                if (i < RedTeam.PickedChampions.Count) {
                    Console.Write(TextHelper.PadTextLeftHalf(RedTeam.PickedChampions[i].Name));
                }
                else
                {
                    Console.Write(TextHelper.PadTextLeftHalf($"Summoner {i+1}"));
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            TextHelper.PrintHorizontalBorderLine();
            Console.WriteLine();
        }

        #endregion
    }
}
