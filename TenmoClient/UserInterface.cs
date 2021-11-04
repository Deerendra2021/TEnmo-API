using System;
using System.Collections.Generic;
using TenmoClient.APIClients;
using TenmoClient.Data;
using System.Linq;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();
        private readonly TenmoApiClient tenmoApi = new TenmoApiClient();
        

        private bool quitRequested = false;

        public void Start()
        {
            while (!quitRequested)
            {
                while (!UserService.IsLoggedIn)
                {
                    ShowLogInMenu();
                }

                // If we got here, then the user is logged in. Go ahead and show the main menu
                ShowMainMenu();
            }
        }

        private void ShowLogInMenu()
        {
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.Write("Please choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int loginRegister))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (loginRegister == 1)
            {
                HandleUserLogin();
            }
            else if (loginRegister == 2)
            {
                HandleUserRegister();
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void ShowMainMenu()
        {
            int menuSelection;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else
                {
                    switch (menuSelection)
                    {
                        case 1: // View Balance
                            Console.WriteLine(); 
                            GetAccounBalance();
                            break;

                        case 2: // View Past Transfers
                            DisplayUserTransfers();
                            break;

                        case 3: // View Pending Requests
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 4: // Send TE Bucks
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 5: // Request TE Bucks
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 6: // Log in as someone else
                            Console.WriteLine();
                            UserService.ClearLoggedInUser(); //wipe out previous login info
                            return; // Leaves the menu and should return as someone else

                        case 0: // Quit
                            Console.WriteLine("Goodbye!");
                            quitRequested = true;
                            return;

                        default:
                            Console.WriteLine("That doesn't seem like a valid choice.");
                            break;
                    }
                }
            } while (menuSelection != 0);
        }

        private void GetAccounBalance()
        {
            Account balance = tenmoApi.GetAccountBalance();

            Console.WriteLine("Your currrent balance is: " + balance.account_Balance.ToString("C"));
        }

        private void DisplayUserTransfers()
        {
            Console.WriteLine($"Transfers");
            Console.WriteLine("ID   From/To         Amount");
            Console.WriteLine("----------------------------");

            List<Transfer> userTransfers = tenmoApi.GetUserTransfers();

            foreach (Transfer transfer in userTransfers)
            {
                if (transfer.accountFromUserId == UserService.UserId )
                {
                    Console.WriteLine($"{transfer.transfer_Id}      {transfer.accountToUserName}      {transfer.amount}");
                }
                else if (transfer.accountFromUserId != UserService.UserId)
                {
                    Console.WriteLine($"{transfer.transfer_Id}      {transfer.accountFromUserName}      {transfer.amount}");
                }
            }
            Console.Write("Please enter transfer ID to view details (0 to cancel): ");
            int input;
            int.TryParse(Console.ReadLine(), out input);
        }
        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = consoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
        }

        private void HandleUserLogin()
        {
            while (!UserService.IsLoggedIn) //will keep looping until user is logged in
            {
                LoginUser loginUser = consoleService.PromptForLogin();
                authService.Login(loginUser);
            }
        }
    }
}
