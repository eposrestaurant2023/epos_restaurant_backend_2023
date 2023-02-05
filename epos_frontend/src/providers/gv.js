import { authorizeDialog,noteDialog,createResource } from "@/plugin"
import { createToaster } from "@meforma/vue-toaster";

const toaster = createToaster({ position: "top" });
export default class Gv {
	constructor() {
		this.setting = {},
			this.customerMeta = null,
			this.countries = []

			 
	}

	
	getPrintReportPath(doctype,name,reportName, isPrint=false){
		let url = "";
		const serverUrl = window.location.protoco
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
									resolve({user:result.name, discount_codes:result.discount_codes,note:resultNote});
								}else{
									resolve(false);
								}
							}
							
						}else{
							resolve({user:result.name, discount_codes:result.discount_codes,note:""});	
						}
					}else{
						
						resolve({user:result.name, discount_codes:result.discount_codes,note:""});
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
						
							const resultNote = await noteDialog({name:categoryNoteName,data:{product_code:""}}) ;
							if(resultNote){
								resolve({user:currentUser.full_name, discount_codes:currentUser.permission.discount_codes,note:resultNote});
							}else{
								toaster.warning("You don't have permission to perform this action.")
								resolve(false);
							}
						}else{
						 
							resolve({user:currentUser.full_name, discount_codes:currentUser.permission.discount_codes,note:""});
						}
					}else{
					 
						resolve({user:currentUser.full_name, discount_codes:currentUser.permission.discount_codes,note:""});

					}
					
					
				} else {
					 
					toaster.warning("You don't have permission to perform this action.")
					resolve(false);
				}
			}
		})

	}
}