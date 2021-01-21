﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
namespace eModels
{
    public abstract class UIComponentBase : ComponentBase
    {
        [Inject] public IJSRuntime JS { get; set; }
        [Parameter] public string ClassName { get; set; }
        [Parameter] public bool IsVisible { get; set; } = true;

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> CustomAttributes { get; set; }

        public string guid = System.Guid.NewGuid().ToString();
    }

    public enum LangName
    {
        kh, en
    }
}
