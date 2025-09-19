using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;

namespace console_database
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            while (true) // Startmenu loop
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("------------- Start Menu ------------");

                string dbStatus;
                using (var db = new AppDbContext())
                {
                    if (db.Database.CanConnect())
                    {
                        dbStatus = "Connected to Database";
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        dbStatus = "Not Connected to Database";
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.WriteLine($"Database Status: {dbStatus}");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-------------------------------------");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("--------------- Admin Panel ---------");
                Console.WriteLine("4. Create (Reset) Database");
                Console.WriteLine("5. Delete Database");
                Console.WriteLine("6. Update Database");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-------------------------------------");
                Console.ForegroundColor = ConsoleColor.White;

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Login
                        HandleLogin();
                        break;

                    case "2": // Register
                        HandleRegister();
                        break;

                    case "3": // Exit
                        Environment.Exit(0);
                        break;

                    case "4": // Create/Reset Database
                        using (var db = new AppDbContext())
                        {
                            Console.Clear();
                            Console.WriteLine("Resetting database...");
                            db.Database.EnsureDeleted();
                            db.Database.EnsureCreated();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Database reset successfully!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;

                    case "5": // Delete Database
                        using (var db = new AppDbContext())
                        {
                            Console.Clear();
                            Console.WriteLine("Deleting database...");
                            db.Database.EnsureDeleted();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Database deleted successfully!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;

                    case "6": // Update Database
                        using (var db = new AppDbContext())
                        {
                            Console.Clear();
                            Console.WriteLine("Updating database...");
                            db.Database.EnsureCreated();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Database updated successfully!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;

                   


                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void HandleLogin()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Email:");
            Console.ForegroundColor = ConsoleColor.White;
            string loginEmail = Console.ReadLine();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Password:");
            Console.ForegroundColor = ConsoleColor.White;
            string loginPassword = Console.ReadLine();

            Console.WriteLine("\nChecking credentials...");

            using (var db = new AppDbContext())
            {
                if (db.Database.CanConnect())
                {
                    var dbUser = db.Users.FirstOrDefault(u => u.Email == loginEmail);

                    if (dbUser == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Email niet gevonden!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        string hashedInput = PasswordHelper.HashPassword(loginPassword);

                        if (dbUser.Password == hashedInput)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Login succesvol! Welkom, {dbUser.Name}");
                            Console.ForegroundColor = ConsoleColor.White;

                            ShowLoggedInMenu(dbUser.Name);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error: Onjuist wachtwoord!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Kan geen verbinding maken met de database!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void ShowLoggedInMenu(string loggedInUser)
        {
            bool stayLoggedIn = true;

            while (stayLoggedIn)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"------------- Welcome {loggedInUser} ------------");

                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine("1. Send Message");
                Console.WriteLine("2. View Messages");
                Console.WriteLine("3. Profile");
                Console.WriteLine("4. Logout");
                Console.WriteLine("5. Exit");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-------------------------------------");
                Console.ForegroundColor = ConsoleColor.White;

                string loggedInChoice = Console.ReadLine();

                switch (loggedInChoice)
                {
                    case "1": // Send Message
                        Console.Clear();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Enter your message:");

                        Console.ForegroundColor = ConsoleColor.White;
                        string messageContent = Console.ReadLine();
                        using (var db = new AppDbContext())
                        {
                            var user = db.Users.FirstOrDefault(u => u.Name == loggedInUser);
                            if (user != null)
                            {
                                var message = new Message
                                {
                                    Content = messageContent,
                                    CreatedAt = DateTime.Now,
                                    UserId = user.Id
                                };
                                db.Messages.Add(message);
                                db.SaveChanges();


                                Console.WriteLine("Sending message...");
                                Thread.Sleep(2000); 

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Message sent successfully!");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        Console.WriteLine("Press any key to go back...");
                        Console.ReadKey();
                        break;

                    

                    case "2": 
                        Console.Clear();


                        using (var db = new AppDbContext())
                        {
                            var messages = db.Messages
                                .Include(m => m.User)
                                .OrderByDescending(m => m.CreatedAt)
                                .ToList();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("------------- Messages ---------------");
                            Console.ForegroundColor = ConsoleColor.White;
                            foreach (var msg in messages)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"[{msg.CreatedAt}] {msg.User.Name}:");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(msg.Content);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("-------------------------------------");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        Console.WriteLine("Press any key to go back...");
                        Console.ReadKey();
                        break;




                    case "3":
                        Console.Clear();
                        using (var db = new AppDbContext())
                        {
                            var user = db.Users.FirstOrDefault(u => u.Name == loggedInUser);
                            if (user != null)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("------------- Profile ---------------");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Name: {user.Name}");
                                Console.WriteLine($"Email: {user.Email}");
                                Console.WriteLine($"Hashed Password: {user.Password}");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("-------------------------------------");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        Console.WriteLine("Press any key to go back...");
                        Console.ReadKey();
                        break;

                    case "4": // Logout
                        stayLoggedIn = false;
                        break;

                    case "5": // Exit
                        Environment.Exit(0);
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void HandleRegister()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Name:");
            Console.ForegroundColor = ConsoleColor.White;
            string name = Console.ReadLine();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Email:");
            Console.ForegroundColor = ConsoleColor.White;
            string email = Console.ReadLine();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Password:");
            Console.ForegroundColor = ConsoleColor.White;
            string password = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Creating user...");

            using (var db = new AppDbContext())
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Email == email);
                if (existingUser != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Dit emailadres is al in gebruik!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }

                var user = new User
                {
                    Name = name,
                    Email = email,
                    Password = PasswordHelper.HashPassword(password)
                };

                db.Users.Add(user);
                db.SaveChanges();
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("User Created!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
