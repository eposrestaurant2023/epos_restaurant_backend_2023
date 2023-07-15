import { request } from './request'
import { createToaster } from "@meforma/vue-toaster";
import {createRouter, useRouter} from 'vue-router';
import { i18n } from "@/plugin";


export function frappeRequest(options) { 

  return request({
    ...options,
    transformRequest: (options = {}) => {
      if (!options.url) {
        throw new Error('[frappeRequest] options.url is required')
      }
      let headers = Object.assign(
        {
          Accept: 'application/json',
          'Content-Type': 'application/json; charset=utf-8',
          'X-Frappe-Site-Name': window.location.hostname,
        },
        options.headers || {}
      )
      if (window.csrf_token && window.csrf_token !== '{{ csrf_token }}') {
        headers['X-Frappe-CSRF-Token'] = window.csrf_token
      }
      if (!options.url.startsWith('/') && !options.url.startsWith('http')) {
        options.url = '/api/method/' + options.url
      }
      return {
        ...options,
        method: options.method || 'POST',
        headers,
      }
    },
    transformResponse: async (response, options) => {
      const toaster = createToaster({position:"top"});
      let url = options.url
      if (response.ok) {
        try{
        const data = await response.json()
        if (data.docs || url === 'login') {
          return data
        }
        if (data.exc) {
          try {
            console.groupCollapsed(url)
            let warning = JSON.parse(data.exc)
            for (let text of warning) {
              console.log(text)
            }
            console.groupEnd()
          } catch (e) {
            console.warn('Error printing debug messages', e)
          }
        }
        return data.message
        } catch(r){  
          
          let currentUrl = window.location.href
          let lastUrl = currentUrl.substr(currentUrl.lastIndexOf('/') + 1);
   
          if(lastUrl != 'server-error' && lastUrl != 'startup-config'){
            window.location.replace('server-error')
          }
          else if(lastUrl == 'startup-config'){
            toaster.error('Internal Server Error',{position:"top"});
          }
          throw {
            status: 500,
            message: 'Internal Server Error',
            error_text: ['Internal Server Error'],
            data: null
          }
        }
      } else {
        let errorResponse = await response.text()
        let error, exception
        try {
          error = JSON.parse(errorResponse)
          // eslint-disable-next-line no-empty
        } catch (e) {}
        let errorParts = [
          [options.url, error.exc_type, error._error_message]
            .filter(Boolean)
            .join(' '),
        ]
        if (error.exc) {
          exception = error.exc
          try {
            exception = JSON.parse(exception)[0]
           
            // eslint-disable-next-line no-empty
          } catch (e) {}
        } 
        let e = new Error(errorParts.join('\n'))
        e.exc_type = error.exc_type
        e.exc = exception
        e.status = errorResponse.status
        e.messages = error._server_messages
          ? JSON.parse(error._server_messages)
          : []
      
       
 
        e.messages = e.messages.concat(error.message)
        e.messages = e.messages.map((m) => {
          try {
            return JSON.parse(m).message
          } catch (error) {
            return m
          }
        })
    
        e.messages = e.messages.filter(Boolean)
        if (!e.messages.length) {
          e.messages = error._error_message
            ? [error._error_message]
            : ['Internal Server Error']
        } 
        
        let error_text =  e.messages;
        

        if (options.onError || error_text) {
          if(error_text[0]=="Not permitted"){           
            localStorage.removeItem("current_user");
            location.reload("/epos_frontend/login");
          } 
          // else if(error_text[0] == 'Internal Server Error'){
          //  window.open("/epos_frontend/server-error","_self")
          // }
          
          if(Array.isArray(error_text)){
            error_text.forEach(r => {
              const msg = r.split(':')
              if(msg.length == 3 && msg[0] == 'Error' && msg[1].search('Value missing for')){
                toaster.warning(`Invalid ${msg[2]}`);
              }else{
                let _msg ="";
                let _msg_type ="warning";
                let _tran =true;

                const { t: $t } = i18n.global;
                switch(r){
                  case 'Invalid PIN Code':
                    break;
                  case 'Not permitted':
                    break;

                  case 'Please start working day first' :
                    _msg = r; 
                    break;
                  case 'Please start shift first' :                  
                    _msg = r;     
                    break;
                  case 'discount percent cannot greater than 100 percent' :                  
                    _msg = r;     
                    break; 
                  case 'Working day was closed' :                  
                    _msg = r;     
                    break;
                  case 'Cashier shift was closed' :                  
                    _msg = r;     
                    break;
                  case 'Discount amount cannot greater than discountable amount' :                  
                    _msg = r;     
                    break;
                  case 'Cannot set price becouse this product is free' :                  
                    _msg = r;     
                    break;
                  default: 
                    _msg_type = "error";    
                    _msg = r;      
                    _tran = false;          
                  break;
                } 
                               
                
                if(_msg !=""){
                  if(_msg_type=="error"){
                    toaster.error($t(_tran?`msg.${_msg}`: `${_msg}`));
                  }else if(_msg_type=='warning'){
                    toaster.warning($t(`msg.${r}`));
                  }
                }
                
              }
            });
          }
            // options.onError({
            //   error_text:error_text,
            //   response: errorResponse,
            //   status: errorResponse.status,
            //   error: e,
            // })
        }
        throw e
      }
    },
  })
}
