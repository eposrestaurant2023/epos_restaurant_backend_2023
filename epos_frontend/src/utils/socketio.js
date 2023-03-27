

import io from 'socket.io-client';
import { socketio_port } from "../../../../../sites/common_site_config.json"

let host = window.location.hostname;
let port = ":" +  socketio_port;//window.location.port ? ':9004' : '';
 
let protocol = port ? 'http' : 'https';
let url = `${protocol}://${host}${port}`;
 
let socket = io(url);


export default socket;


