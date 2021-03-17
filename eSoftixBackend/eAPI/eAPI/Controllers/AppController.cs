﻿using eModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eAPI.Controllers
{
    [ApiController,Route("api")]
    public class AppController:ControllerBase
    {

        private readonly ApplicationDbContext db;
        public AppController(ApplicationDbContext _db)
        {
            db = _db;
        }
        [Route("GlobalVariable"),EnableQuery]
        public ActionResult GetGlobalVariable()
        {
            GlobalVariableModel gv = new GlobalVariableModel();
            gv.permission_options = db.PermissionOption.ToList();
            gv.module_views = db.ModuleViews.ToList();
            gv.outlets = db.Outlets.ToList();
            gv.countries = db.Countries.ToList();
            gv.customer_groups = db.CustomerGroups.ToList();
            gv.project_type = db.ProjectType.ToList();
            gv.customer = db.Customers.ToList();
            gv.roles = db.Roles.ToList();

            return Ok(gv);
        }
        [HttpPost]
        [Route("Contact/delete/{contact_id}")]
        public async Task<ActionResult<ContactModel>> DeleteContact(int contact_id)
        {
            var contact = await db.Contacts.FindAsync(contact_id);
            contact.is_deleted = !contact.is_deleted;
            db.Contacts.Update(contact);

            await SaveChange.SaveAsync(db, Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Ok(contact);
        }

        [HttpPost]
        [Route("GetData")]
        public string GetData([FromBody] FilterModel f)
        {
            var d = db.StoreProcedureResult.FromSqlRaw(string.Format("exec {0} {1}", f.procedure_name, f.procedure_parameter)).ToList().FirstOrDefault();
            string r = d.result.Replace("\\", "").Replace("\"[", "[").Replace("]\"", "]").ToString();
            return r;
        }



    }
}
