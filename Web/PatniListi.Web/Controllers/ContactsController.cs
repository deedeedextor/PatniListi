namespace PatniListi.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Common;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Messaging;
    using PatniListi.Web.ViewModels.Models.Contacts;

    public class ContactsController : BaseController
    {
        private readonly IRepository<ContactFormEntry> contactsRepository;
        private readonly IEmailSender emailSender;

        public ContactsController(IRepository<ContactFormEntry> contactsRepository, IEmailSender emailSender)
        {
            this.contactsRepository = contactsRepository;
            this.emailSender = emailSender;
        }

        public IActionResult Contact()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Contact(ContactFormViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();

            var viewModel = new ContactFormEntry
            {
                FullName = input.FullName,
                Email = input.Email,
                Title = input.Title,
                Content = input.Content,
                Ip = ip,
            };

            await this.contactsRepository.AddAsync(viewModel);
            await this.contactsRepository.SaveChangesAsync();

            await this.emailSender.SendEmailAsync(
                input.Email, input.FullName, GlobalConstants.SystemEmail, input.Title, input.Content);

            return this.RedirectToAction("ThankYou");
        }

        [Authorize]
        public IActionResult ThankYou()
        {
            return this.View();
        }
    }
}
