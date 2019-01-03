﻿using Coffeeland.Database;
using Coffeeland.Messaging.Commands.Commands;
using Coffeeland.Messaging.Dtos;
using Coffeeland.Messaging.Shared;
using System;
using Coffeeland.MailService;
using System.Threading;

namespace Coffeeland.Messaging.Commands.Handlers
{
    public class RegisterNewClientCommandHandler : ICommandHandler<RegisterNewClientCommand>
    {
        public IResult Handle(RegisterNewClientCommand command)
        {
            if (!InputChecker.isValidEmail(command.email) ||
                !InputChecker.isValidName(command.firstName) ||
                !InputChecker.isValidName(command.lastName) ||
                (command.receiveNewsletterEmail && 
                !InputChecker.isValidEmail(command.newsletterEmail)))
            {
                throw new Exception();
            }

            var clients = DatabaseQueryProcessor.GetClients();
            var foundClients = clients.FindAll(c => c.email == command.email);

            if (foundClients.Count != 0)
                throw new Exception();

            DatabaseQueryProcessor.CreateNewClient(
                command.email,
                command.firstName,
                command.lastName,
                PasswordEncryptor.encryptSha256(command.password),
                command.receiveNewsletterEmail ? command.newsletterEmail : ""
                );

            //ThreadPool.QueueUserWorkItem(
            //    o => MailSender.SendRegistrationEmail(command.email,command.firstName)
            //   );


            return new SuccessInfoDto()
            {
                isSuccess = true
            };
        }
    }
}