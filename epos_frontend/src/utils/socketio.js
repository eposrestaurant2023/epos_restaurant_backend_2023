// import { io } from 'socket.io-client/dist/socket.io'

// export default function initSocket(options = {}) {
//   let host = window.location.hostname
//   let socketio_port = options.port || 5566
//   let port = window.location.port ? `:${socketio_port}` : ''
//   let protocol = port ? 'http' : 'https'
//   let url = `${protocol}://${host}${port}`
//   let socket = io(url, { 
//     cors: {
//       origin: ["http://192.168.10.114:5566"],
//       allowedHeaders: ['Content-Type', 'Authorization'],
//       credentials: true
      
//     }
//    })
//   return socket
// }


// import io from 'socket.io-client/dist/socket.io';

// let host = window.location.hostname;
// let port = window.location.port ? ':1217' : '';
// let protocol = port ? 'http' : 'https';
// let url = `${protocol}://${host}${port}`;
// let socket = io(url,{
//   cors: {
//     origin: ["http://192.168.10.114:1217"],
//     allowedHeaders: ['Content-Type', 'Authorization'],
//     credentials: true
    
//   }
// });

// export default socket;



// Authors: Faris Ansari <faris@frappe.io>

// import io from 'socket.io-client/dist/socket.io.slim';

// let host = window.location.hostname;
// let port = window.location.port;// ? ':9000' : '';
// let protocol = port ? 'http' : 'https';
// let url = `${protocol}://${host}${port}`;
// let socket = io(url);



// export default socket;

import io from 'socket.io-client';

let host = window.location.hostname;
let port = ":3000";//window.location.port ? ':9004' : '';
let protocol = port ? 'http' : 'https';
let url = `${protocol}://${host}${port}`;
 
let socket = io(url);


export default socket;


