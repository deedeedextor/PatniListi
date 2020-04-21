namespace PatniListi.Web.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Services.Data;

    public class ValidationController : Controller
    {
        private readonly ICarsService carsService;
        private readonly IUsersService usersService;
        private readonly IInvoicesService invoicesService;

        public ValidationController(ICarsService carsService, IUsersService usersService, IInvoicesService invoicesService)
        {
            this.carsService = carsService;
            this.usersService = usersService;
            this.invoicesService = invoicesService;
        }

        public IActionResult ValidateLicensePlate(string licensePlate, string id)
        {
            bool exists = this.carsService.IsLicensePlateExist(licensePlate);

            if (exists && id == null)
            {
                return this.Json(data: "Регистрационният номер е зает.");
            }
            else if (exists && id != null)
            {
                if (licensePlate == this.carsService.GetLicensePlateById(id))
                {
                    return this.Json(data: true);
                }
                else
                {
                    return this.Json(data: "Регистрационният номер е зает.");
                }
            }

            return this.Json(data: true);
        }

        public IActionResult ValidateUsername(string username, string id)
        {
            bool exists = this.usersService.IsUsernameInUse(username);

            if (exists && id == null)
            {
                return this.Json(data: "Потребителското име е заето.");
            }
            else if (exists && id != null)
            {
                if (username == this.usersService.GetUsernameById(id))
                {
                    return this.Json(data: true);
                }
                else
                {
                    return this.Json(data: "Номерът на фактурата е зает.");
                }
            }

            return this.Json(data: true);
        }

        public IActionResult ValidateEmail(string email, string id)
        {
            bool exists = this.usersService.IsEmailInUse(email);

            if (exists && id == null)
            {
                return this.Json(data: "Имейл адресът е зает.");
            }
            else if (exists && id != null)
            {
                if (email == this.usersService.GetEmailById(id))
                {
                    return this.Json(data: true);
                }
                else
                {
                    return this.Json(data: "Имейл адресът е зает.");
                }
            }

            return this.Json(data: true);
        }

        public IActionResult ValidateQuantity(double quantity, double currentLiters, int carTankCapacity)
        {
            if ((int)(currentLiters + quantity) > carTankCapacity)
            {
                return this.Json(data: "Наличното и заредено количество гориво не трябва да надвишават капацитета на резервоара");
            }

            return this.Json(data: true);
        }

        public IActionResult ValidateNumber(string number, string id)
        {
            bool exists = this.invoicesService.IsNumberExist(number);

            if (exists && id == null)
            {
                return this.Json(data: "Номерът на фактурата е зает.");
            }
            else if (exists && id != null)
            {
                if (number == this.invoicesService.GetInvoiceNumberById(id))
                {
                    return this.Json(data: true);
                }
                else
                {
                    return this.Json(data: "Номерът на фактурата е зает.");
                }
            }

            return this.Json(data: true);
        }

        public IActionResult ValidatePeriodBetweenDates(DateTime from, DateTime to)
        {
            var daysBetween = (to - from).TotalDays;

            if (daysBetween > 31)
            {
                return this.Json(data: "Избраният период не може да бъде по-голям от месец.");
            }

            return this.Json(data: true);
        }
    }
}
