namespace MilanWebStore.Web.Areas.Administration.Controllers
{
    using MilanWebStore.Common;
    using MilanWebStore.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
