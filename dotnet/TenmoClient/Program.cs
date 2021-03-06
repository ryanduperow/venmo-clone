using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        private static readonly AccountService accountService = new AccountService();
        private static readonly TransferService transferService = new TransferService();
        //We initialized the new services we had created here so we could use them below

        static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            while(true)
            {
                int loginRegister = -1;
                while (loginRegister != 1 && loginRegister != 2)
                {
                    Console.WriteLine("Welcome to TEnmo!");
                    Console.WriteLine("1: Login");
                    Console.WriteLine("2: Register");
                    Console.Write("Please choose an option: ");

                    if (!int.TryParse(Console.ReadLine(), out loginRegister))
                    {
                        Console.WriteLine("Invalid input. Please enter only a number.");
                    }
                    else if (loginRegister == 1)
                    {
                        while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                        {
                            LoginUser loginUser = consoleService.PromptForLogin();
                            ApiUser user = authService.Login(loginUser);
                            if (user != null)
                            {
                                UserService.SetLogin(user);
                            }
                        }
                    }
                    else if (loginRegister == 2)
                    {
                        bool isRegistered = false;
                        while (!isRegistered) //will keep looping until user is registered
                        {
                            LoginUser registerUser = consoleService.PromptForLogin();
                            isRegistered = authService.Register(registerUser);
                            if (isRegistered)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Registration successful. You can now log in.");
                                loginRegister = -1; //reset outer loop to allow choice for login
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }

                MenuSelection();
            }
        }

        private static void MenuSelection()
        {
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View an idividual transfer");
                Console.WriteLine("4: Send TE bucks");
                //Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("5: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)
                {
                    Console.WriteLine($"Your account balance: ${accountService.GetBalance()}");
                }
                // Past transfers
                else if (menuSelection == 2)
                {
                    IList<Transfer> transfers = transferService.GetTransferList();

                    foreach (Transfer transfer in transfers)
                    {
                        Console.WriteLine(transfer.FormattedTransfer);
                    }
                }
                // Get individual transfer
                else if (menuSelection == 3)
                {
                        int tranferId = consoleService.PromptForTransferID("view");
                        Console.WriteLine(transferService.GetTransferById(tranferId));
                }
                //New transfer
                else if (menuSelection == 4)
                {
                    IList<ListUser> users = accountService.GetUsers();

                    foreach (ListUser user in users)
                    {
                        Console.WriteLine(user.FormattedUser());
                    }
                    //hypothetical future TODO: get user by ID from endpoint to validate that they exist
                    // before we allow the user to continue
                    int toAccountId = consoleService.PromptForRecipientID();
                    decimal amount = consoleService.PromptForAmount();

                    // Assemble a new transfer object to pass into our transferService.CreateNewTransfer
                    Transfer transfer = new Transfer
                    {
                        //The values here are hardcoded because when creating a new transfer,
                        // the status type and transfer status will always be 2 ('send' type and 'approved' status)
                        AccountFrom = UserService.GetAccountId(),
                        AccountTo = toAccountId,
                        TransferTypeID = 2,
                        TransferStatusID = 2,
                        Amount = amount
                    };

                    Console.WriteLine(transferService.CreateNewTransfer(transfer));
                    Console.ReadLine();
                }
                else if (menuSelection == 5)
                {
                    Console.WriteLine("");
                    UserService.SetLogin(new ApiUser()); //wipe out previous login info
                    Console.Clear();
                    menuSelection = 0;
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }
    }
}
