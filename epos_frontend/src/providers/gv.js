import { authorizeDialog,noteDialog,createResource,confirm } from "@/plugin"
import { createToaster } from "@meforma/vue-toaster";
import moment from '@/utils/moment.js';
const toaster = createToaster({ position: "top" });
export default class Gv {
	constructor() {
		this.setting = {},
		this.customerMeta = null,
		this.saleMeta = null,
		this.countries = [] 
		//use this variable to control state of open/close shift in drawer

		this.workingDay = "";
		this.cashierShift= "";
		this.isFullscreen = true
	}

	
	getPrintReportPath(doctype,name,reportName, isPrint=false){
		let url = "";
		let serverUrl = window.location.protoco
		l + "//" +  window.location.host;
		url  = serverUrl + "/printview?doctype=" + doctype + "&name=" + name + "&format="+ reportName +"&no_letterhead=0&letterhead=Defualt%20Letter%20Head&settings=%7B%7D&_lang=en&d=" + new Date()
		if(isPrint){
			serverUrl = serverUrl + "&trigger_print=" + triggerPrint
		}
	}

	async authorize(settingKey, permissionCode,requiredNoteKey="",categoryNoteName="", product_code = "", inlineNote = false) {
		 
		return new Promise(async (resolve,reject) => {

			if (this.setting.pos_setting[settingKey] == 1) {
				const result = await authorizeDialog({ permissionCode: permissionCode });
				
				if (result) {
					if(requiredNoteKey && categoryNoteName){
						
						//check if require note 
						if(this.setting.pos_setting[requiredNoteKey] == 1){
							
							if(inlineNote){
								resolve({category_note_name: categoryNoteName,discount_codes:result.discount_codes})
							}else{
								const resultNote = await noteDialog({name:categoryNoteName,data:{product_code:product_code}}) ;
								if(resultNote){
									resolve({user:result.name, discount_codes:result.discount_codes,note:resultNote,username:result.username});
								}else{
									resolve(false);
								}
							}
							
						}else{
						
							resolve({user:result.name, discount_codes:result.discount_codes,note:"",username:result.username});	
						}
					}else{
					
						resolve({user:result.name, discount_codes:result.discount_codes,note:"",username:result.username});
					}
					
				} else {
					resolve(false);
				}
			}
			else {
				
			 const currentUser = JSON.parse(localStorage.getItem("current_user"));
				
			 if (JSON.parse(localStorage.getItem("current_user")).permission[permissionCode] == 1) {
					
					if(requiredNoteKey && categoryNoteName){
						
						//check if require note 
						if(this.setting.pos_setting[requiredNoteKey] == 1){ 
							if(inlineNote){ 
								resolve({user:currentUser.full_name, discount_codes:currentUser.permission.discount_codes,note:'',username:currentUser.name,category_note_name: categoryNoteName})
							}else{
								const resultNote = await noteDialog({name:categoryNoteName,data:{product_code:""}}) ;
								if(resultNote){
									resolve({user:currentUser.full_name, discount_codes:currentUser.permission.discount_codes,note:resultNote,username:currentUser.name});
								}else{
									toaster.warning("You don't have permission to perform this action.")
									resolve(false);
								}
							}
						}else{
							 
							resolve({user:currentUser.full_name, discount_codes:currentUser.permission.discount_codes,note:"",show_confirm:1,username:currentUser.name});
						}
					}else{
					 
						resolve({user:currentUser.full_name, discount_codes:currentUser.permission.discount_codes,note:"",username:currentUser.name});

					}
					
					
				} else {
					 
					toaster.warning("You don't have permission to perform this action.")
					resolve(false);
				}
			}
		})

	}

	async confirm_close_working_day(working_day){
	 
		let check_date = "";
		if(this.setting.close_business_day_on=="Current Day"){
			check_date =  moment(working_day).format('yyyy-MM-DD') + " " + this.setting.alert_close_working_day_after;
		}else{ 
			check_date = moment(working_day).add(1, 'days').format('yyyy-MM-DD') + " " + this.setting.alert_close_working_day_after;
		}
 
		if(new Date() > new Date(check_date)){
		 await confirm({title:'Close Working Day', text:'Your working day is to long. Please close your working day.',hide_cancel:true});			
			 
		}
	}

	getCurrentUser(){
		if(this.checkCookie('system_user') == 'yes'){
			return this.checkCookie('user_id')
		}
		return ''
	}

	checkCookie(name) {
		var nameEQ = name + "=";
		var ca = document.cookie.split(';');
		for(var i=0;i < ca.length;i++) {
		  var c = ca[i];
		  while (c.charAt(0)==' ') c = c.substring(1,c.length);
		  if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
		}
		return null;
	}
}