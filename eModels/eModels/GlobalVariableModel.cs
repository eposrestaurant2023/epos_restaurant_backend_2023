﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eModels
{
    public class GlobalVariableModel
    {
        public GlobalVariableModel() { }
        [Key]
        public int id { get; set; }

        public Guid empty_guid { get; set; } = Guid.Empty;
        public BusinessInformationModel business_info { get; set; } = new BusinessInformationModel();
        public List<CurrencyModel> currencies { get; set; }
        public List<PaymentTypeModel> payment_types { get; set; }
        public List<VendorModel> vendors { get; set; }
        public List<VendorGroupModel> vendor_groups { get; set; }
        public List<UnitModel> units { get; set; }
          
  
        public List<CountryModel> countries { get; set; }
 

        public List<SettingModel> settings { get; set; }
 
        public List<RoleModel> roles { get; set; }
        public List<ModuleViewModel> module_views { get; set; }
        public List<StockLocationModel> stock_locations { get; set; }
        public List<OutletModel> outlets { get; set; }
        public List<StockTransferModel> stock_stransfer { get; set; }
        public List<ProductTypeModel> product_types { get; set; }
        public List<ProductGroupModel> product_groups { get; set; }
        public List<ProductCategoryModel> product_categories { get; set; }
        public List<CustomerGroupModel> customer_groups { get; set; }
        public List<BusinessBranchModel> bussiness_branches { get; set; }
        public List<UnitCategoryModel> unit_categories { get; set; }
        public List<PrinterModel> printers { get; set; }
        public List<PriceRuleModel> price_rules{ get; set; }
        public List<CategoryNoteModel> category_notes { get; set; }
        public List<ProvinceModel> provinces { get; set; }

        public int current_outlet_id { get; set; }


        public List<BusinessBranchModel> business_branch_by_role
        {
            get
            {
                
                return current_login_user.role.business_branch_roles.Select(r => r.business_branch).ToList();
            }
        }

        public List<StockLocationModel> stock_location_by_role
        {
            get
            {
                return current_login_user.role.business_branch_roles.SelectMany(r => r.business_branch.stock_locations.ToList()).ToList();
            }
        }


        public string business_branch_ids_filter_1
        {
            get
            {
                string _data = "";
                foreach (var b in current_login_user.role.business_branch_roles.Select(r => r.business_branch).ToList())
                {
                    _data += $"{b.id},";
                } 
               if(!string.IsNullOrEmpty(_data))
                    _data = _data.Substring(0, _data.Length - 1);

                return _data;
            }
        }
        public string business_branch_ids_filter_2
        {
            get
            {
                string _data = "";
                foreach (var b in current_login_user.role.business_branch_roles.Select(r => r.business_branch).ToList())
                {
                    _data += $"{b.id},";
                }
                if (!string.IsNullOrEmpty(_data))
                    _data = _data.Substring(0, _data.Length - 1);

                return _data;
            }
        }
        public string outlet_ids_filter(string _business_branch_ids)
        {
            
            string _data = "";

            if (string.IsNullOrEmpty( _business_branch_ids ))
            { 
                foreach (var id in _business_branch_ids.Split(',').ToList())
                {
                    foreach (var o in outlets.Where(r => r.business_branch_id.ToString() == id && !r.is_deleted).ToList())
                    {
                        _data += $"{o.id},";
                    }
                } 
            }
            else
            { 
                foreach (var o in current_login_user.role.business_branch_roles.SelectMany(r=>r.business_branch.outlets.Where(x=>!x.is_deleted).ToList()).ToList())
                {
                    _data += $"{o.id},";
                }
            } 
            if(!string.IsNullOrEmpty(_data))
               _data = _data.Substring(0, _data.Length - 1); 

            return _data;
        }

        public string stock_location_ids_filter(string _business_branch_ids)
        {

            string _data = "";

            if (string.IsNullOrEmpty(_business_branch_ids))
            {
                foreach (var id in _business_branch_ids.Split(',').ToList())
                {
                    foreach (var o in stock_locations.Where(r => r.business_branch_id.ToString() == id).ToList())
                    {
                        _data += $"{o.id},";
                    }
                }
            }
            else
            {
                foreach (var o in current_login_user.role.business_branch_roles.SelectMany(r => r.business_branch.stock_locations.ToList()).ToList())
                {
                    _data += $"{o.id},";
                }
            }
            if (!string.IsNullOrEmpty(_data))
                _data = _data.Substring(0, _data.Length - 1);

            return _data;
        }

        //Note
        //============================
        private string get_setting_value(int id)
        {
            return settings.Where(r => r.id == id).FirstOrDefault()?.setting_description;
        }
        public string date_format
        {
            get
            {
                return get_setting_value(53);
            }
        }
        public string date_time_format
        {
            get
            {
                return get_setting_value(54);
            }
        }

        public string quantity_format
        {
            get
            {
                return get_setting_value(52);
            }
        }
        public string report_url
        {
            get
            {
                return get_setting_value(2);
            }
        }
        public string report_folder
        {
            get
            {
                return get_setting_value(3);
            }
        }


        private CurrencyModel get_currency(bool is_main = true)
        {
            return currencies.Where(r => r.is_main == is_main).FirstOrDefault();
        }
        public string main_currency_format
        {
            get
            {
                return "C2";
            }
        }
        public string currency_format
        {
            get
            {
                return get_setting_value(55);
            }
        }
        public string second_currency_format
        {
            get
            {
                return "R";
            }
        }

        public string GetRole(string option_name)
        {

            if (permission_options.Count() > 0)
            {
                var d = permission_options.Where(r => r.option_name == option_name && (r.roles ?? "") != "");
                if (d.Count() > 0)
                {
                    string role = d.FirstOrDefault().roles;
                    return role;
                }
            }
            return option_name;
        }

        public UserModel current_login_user { get; set; }
        public List<PermissionOptionModel> permission_options { get; set; }
        public List<PermissionOptionModel> MainMenu()
        {
            var data = permission_options.Where(r => r.parent_id == null && r.show_in_menu == true && r.roles != "").OrderBy(r => r.sort_order).ToList();
            return data;

        }
        public List<PermissionOptionModel> SubMenu(int parent_id)
        {
            var data = permission_options.Where(r => r.parent_id == parent_id && r.show_in_menu == true && r.roles != "").OrderBy(r => r.sort_order).ToList();
            return data;
        }
        public List<ModuleViewModel> GetModuleView(string view_name)
        {

            return module_views.Where(r => r.module_name == view_name).OrderBy(r => r.sort_order).ToList();

        }
        public ModuleViewModel GetDefaultModuleView(string view_name)
        {

            var d = module_views.Where(r => r.module_name == view_name && r.is_default == true);
            if (d.Count() > 0)
            {
                return d.FirstOrDefault();
            }
            return null;

        }
        public bool CheckPaging(PagerModel pager, int total_record)
        {
            if (total_record > 0)
            {
                int totalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(total_record) / Convert.ToDecimal(pager.per_page)));
                if (totalPage < pager.current_page)
                {
                    return true;
                }
            }
            return false;
        }

        public int default_payment_type_id
        {
            get
            {
                if (payment_types.Any())
                {
                    return payment_types.FirstOrDefault().id;
                }
                return 0;
            }
        }
    }

}


