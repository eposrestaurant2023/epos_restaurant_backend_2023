using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using eAdmin;
using eAdmin.Shared;
using MudBlazor;
using eModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using eAdmin.Services;

namespace eAdmin.Shared.Users
{
    public partial class ComUserDisplay
    {
        [Parameter]
        public string Class { get; set; } = "";

        [Parameter]
        public string Style { get; set; } = "";

        [Inject] public IHttpService    http { get; set; }
        [CascadingParameter] public AppState state { get; set; }

    }
}