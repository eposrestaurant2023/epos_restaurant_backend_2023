using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace eKnowledgebase
{
    public partial class AppState
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject] protected ILocalStorageService localStorage { get; set; }
        [Inject] protected IConfiguration config { get; set; }

        private bool _is_loading;

        public bool is_loading
        {
            get { return _is_loading; }
            set { _is_loading = value;
                StateHasChanged();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            string lang = await localStorage.GetItemAsStringAsync("lang");

            current_language = string.IsNullOrEmpty(lang) ? "en" : lang;

        }


        private string _current_language;
        public string current_language
        {
            get { return string.IsNullOrEmpty( _current_language)?"en":_current_language; }
            set { _current_language = value; StateHasChanged(); }
        }

        public async Task SetCurrentLanguage(string lang_code)
        {
            await localStorage.SetItemAsStringAsync("lang", lang_code);
        }

        public string GetTitleText(eKnowledgebase.Models.eKnowledgebaseModel model)
        {
            return current_language == "en" ? model.title_en : model.title_kh;
        }
       
        public string GetImageUrl(eKnowledgebase.Models.eKnowledgebaseModel model)
        {
            string base_url = config.GetValue<string>("baseUrl");
            if(string.IsNullOrEmpty(model.photo_en) && string.IsNullOrEmpty(model.photo_kh))
            {
                return "";
            }else
            {
                return $"{base_url}upload/"+ (current_language == "en" ? model?.photo_en : model?.photo_kh);
            } 
            

        }

        public string GetDescriptText(eKnowledgebase.Models.eKnowledgebaseModel model)
        {
            return current_language == "en" ? model.description_en : model.description_kh;

        }


    }
}
