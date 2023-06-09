export function request(_options) {
 
  let options = Object.assign({}, _options)
 
  if (!options.url) {
    throw new Error('[request] options.url is required')
  }
  if (options.transformRequest) {
    options = options.transformRequest(_options)
  }
  if (!options.responseType) {
    options.responseType = 'json'
  } 
  if (!options.method) {
    options.method = 'GET'
  }
  let url = options.url
  let body
  if (options.params) {
    if (options.method === 'GET') {
      let params = new URLSearchParams()
      for (let key in options.params) {
        params.append(key, options.params[key])
      }
      url = options.url + '?' + params.toString()
    } else {
      body = JSON.stringify(options.params)
    }
  }

  let headers = {
		Accept: 'application/json',
		'Content-Type': 'application/json; charset=utf-8',
		'X-Frappe-Site-Name': window.location.hostname
	};
	
	if (window.csrf_token && window.csrf_token !== '{{ csrf_token }}') {
		headers['X-Frappe-CSRF-Token'] = window.csrf_token;
	}


  return fetch(url, {
    method: options.method || 'GET',
    headers: headers,
    body,
  }).then((response) => {
    if (options.transformResponse) {
      return options.transformResponse(response, options)
    }
    if (response.status >= 200 && response.status < 300) {
      if (options.responseType === 'json') {
        return response.json()
      }
      return response
    } else {

      // let error = new Error(response)
      // error.response = response
      // throw error
      // console.log(response)
      return false

    }
  })
}
