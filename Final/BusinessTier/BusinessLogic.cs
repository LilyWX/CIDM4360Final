namespace Final;
using System.Data;
using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Azure;
using Azure.Communication.Email;
using Azure.Communication.Email.Models;
class BusinessLogic
{

    static async Task Main(string[] args)
    {
        bool _continue = true;
        User user = new User();
        GuiTier appGUI = new GuiTier();
        DataTier database = new DataTier();

        // start GUI
        user = appGUI.Login();


        if (database.LoginCheck(user))
        {

            while (_continue)
            {
                int option = appGUI.Dashboard(user);
                switch (option)
                {
                    // Add a package
                    case 1:
                        while (_continue)
                        {
                            Console.WriteLine("Please input the owner name");
                            string full_name = Console.ReadLine();
                            Console.WriteLine("Please input the unit numbrt(type 0 if no unit)");
                            int unit_number = Convert.ToInt16(Console.ReadLine());
                            if (database.PackageCheck(full_name, unit_number))
                            {
                                Console.WriteLine("---------Resident Match, package will be added to Pending Area.----------");
                                Console.WriteLine("Please input the agency of posting service(FedEx, USPS, UPS, etc).");
                                string post_agency = Console.ReadLine();
                                database.AddToPendingArea(unit_number, full_name, post_agency);
                                database.AddToHistory(unit_number, full_name, post_agency);
                                Console.WriteLine("--------------------Sucessful Added!----------------");
                                DataTable tableAllPending = database.ShowAllPending(user);
                                if (tableAllPending != null)
                                {
                                    appGUI.DisplayRecords(tableAllPending);
                                }
                                // send auto emial
                                string serviceConnectionString = "endpoint=https://xwangweek10communicationservice.communication.azure.com/;accesskey=N0v0izSAL3mLW0R2F175FKy5oOZ1cMD4uDpqsbRoP2s1vd5vtC07bsURaHesUyY0oHQxArlRQrzG55EdOLxfKw==";
                                EmailClient emailClient = new EmailClient(serviceConnectionString);
                                var subject = "Package Delieved";
                                var emailContent = new EmailContent(subject);
                                // use Multiline String @ to design html content
                                emailContent.Html = @"
                                        <html>
                                             <body>
                                            <h1 style=color:red>Pick your package at office.</h1>
                                            <h4>You have a package delieved at apartment office today. Please pick it.</h4>
                                            
                                            </body>
                                         </html>";
                                // mailfrom domain of your email service on Azure
                                var sender = "DoNotReply@bc22eff7-49df-4694-90ff-a77d4d8e3240.azurecomm.net";
                                string inputEmail = database.ResidentEmail(unit_number, full_name);
                                var emailRecipients = new EmailRecipients(new List<EmailAddress> {
                                    new EmailAddress(inputEmail) { DisplayName = "Testing" },
                                });

                                var emailMessage = new EmailMessage(sender, emailContent, emailRecipients);

                                try
                                {
                                    SendEmailResult sendEmailResult = emailClient.Send(emailMessage);

                                    string messageId = sendEmailResult.MessageId;
                                    if (!string.IsNullOrEmpty(messageId))
                                    {
                                        Console.WriteLine($"Email sent, MessageId = {messageId}");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Failed to send email.");
                                        return;
                                    }

                                    // wait max 2 minutes to check the send status for mail.
                                    var cancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(2));
                                    do
                                    {
                                        SendStatusResult sendStatus = emailClient.GetSendStatus(messageId);
                                        Console.WriteLine($"Send mail status for MessageId : <{messageId}>, Status: [{sendStatus.Status}]");

                                        if (sendStatus.Status != SendStatus.Queued)
                                        {
                                            break;
                                        }
                                        await Task.Delay(TimeSpan.FromSeconds(10));

                                    } while (!cancellationToken.IsCancellationRequested);

                                    if (cancellationToken.IsCancellationRequested)
                                    {
                                        Console.WriteLine($"Looks like we timed out for email");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error in sending email, {ex}");
                                }




                            }
                            else
                            {
                                Console.WriteLine("-----------Do not find the owner, package will be added to Unknow Area.-----------");
                                Console.WriteLine("Please put the Owner name: ");
                                string packageOwner = Console.ReadLine();
                                Console.WriteLine("Please input the Post agency");
                                string postServiceAgency = Console.ReadLine();
                                Console.WriteLine("Please input delivery date:");
                                string deliveryDate = Console.ReadLine();
                                database.AddToUnknow(packageOwner,postServiceAgency,deliveryDate);
                                DataTable showUnknow = database.ShowUnknow(user);
                                if(showUnknow != null){
                                    appGUI.DisplayUnknow(showUnknow);
                                }
                            }
                            Console.WriteLine("Please input T if you want to add more packages (T or F):");
                            string answer = Console.ReadLine();
                            if (answer != "T")
                            {
                                break;
                            }
                        }
                        break;
                    // pick a package
                    case 2:
                        while (_continue)
                        {
                            Console.WriteLine("Please input the owner name");
                            string full_name = Console.ReadLine();
                            Console.WriteLine("Please input the unit numbrt");
                            int unit_number = Convert.ToInt16(Console.ReadLine());
                            if (database.PackageCheck(full_name, unit_number))
                            {
                                Console.WriteLine("-----------------This is your Pending Package Table----------------");
                                DataTable tablePending = database.ShowPending(user, unit_number, full_name);
                                if (tablePending != null)
                                {
                                    appGUI.DisplayRecords(tablePending);
                                }
                                database.DeleteFromPending(unit_number, full_name);
                                Console.WriteLine("-------------Pending Table after pick the package-----------");
                            }
                            else
                            {
                                Console.WriteLine("Wrong information. Please try again.");
                            }
                            Console.WriteLine("Please input T if you want to pick more packages (T or F):");
                            string answer = Console.ReadLine();
                            if (answer != "T")
                            {
                                break;
                            }
                        }
                        break;
                    // retrieve package records history
                    case 3:
                        Console.WriteLine("-----------------Package Records History----------------");

                        DataTable tableRecords = database.ShowHistory(user);
                        if (tableRecords != null)
                        {
                            appGUI.DisplayRecords(tableRecords);
                        }


                        break;
                    // Return to post office
                    case 4:
                        Console.WriteLine("-----------------Unknown Packages Records before return----------------");

                        DataTable tableUnkown = database.ShowUnknow(user);
                        if (tableUnkown != null)
                        {
                            appGUI.DisplayUnknow(tableUnkown);
                        }

                        database.DeleteUnknow(user);
                        Console.WriteLine("--------------Successful Return--------------");

                        break;
                    // log out
                    case 5:
                        _continue = false;
                        Console.WriteLine("Log out, Goodbye.");
                        break;
                    // test
                   
                    // default: wrong input
                    default:
                        Console.WriteLine("Wrong Input");
                        break;
                }

            }
        }
        else
        {
            Console.WriteLine("Login Failed, Goodbye.");
        }
    }
}
