export default async function api(method, args, _headers) { 
    if (!args) {
      args = {}
    }
  
    let headers = Object.assign(
      {
        Accept: 'application/json',
        'Content-Type': 'application/json; charset=utf-8',
        'X-Frappe-Site-Name': window.location.hostname,
      },
      _headers
    )
  
    if (window.csrf_token && window.csrf_token !== '{{ csrf_token }}') {
      headers['X-Frappe-CSRF-Token'] = window.csrf_token
    }
    // create & update
    const res = await fetch(`/api/resource/${args.name ? method + '/' + args.name : method}`, {
      method: args.name ? 'PUT' : 'POST',
      headers,
      body: JSON.stringify(args),
    })
    return new Promise((resolve, reject)=>{
      if (res.ok) {
        resolve(res.json())
      } else {
        let response = res.text()
        reject(JSON.parse(response))
      }
    })
    
  }
  